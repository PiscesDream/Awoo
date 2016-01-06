from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User, Friendship, Message
from ..toolkits.randomgen import randomstr

@api.route('/user/all', methods=["POST"])
def userall():
    data = request.get_json()
    username = data.get('username', '')
    token = data.get('token', '')
    
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify(reply="invalid username")
    if user.token == '':
        return jsonify(reply="user haven't logged in yet")
    if user.token != token:
        return jsonify(reply="incorrect token")

    users = map(lambda x: {'username':x.username, 'id':x.id}, User.query.all())
    return jsonify(reply="succeed", users=users)

@api.route('/user/query', methods=["POST"])
def userquery():
    data = request.get_json()
    username = data.get('username', '')
    token = data.get('token', '')
    qusername = data.get('query', '')
    
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify(reply="invalid username")
    if user.token == '':
        return jsonify(reply="user haven't logged in yet")
    if user.token != token:
        return jsonify(reply="incorrect token")
    quser = User.query.filter(User.username.like('%'+qusername+'%')).all()
    quser = map(lambda x: x.username, quser)

    return jsonify(reply="succeed", usernames=quser)




@api.route('/user/makefriend', methods=["POST"])
def usermakefriend():
    data = request.get_json()
    username = data.get('username', '')
    token = data.get('token', '')
    qusername = data.get('query', '')
    
    user = User.query.filter_by(username=username).first()
    quser = User.query.filter_by(username=qusername).first()
    if not user:
        return jsonify(reply="invalid username")
    if not quser:
        return jsonify(reply="not found")
    if user.token == '':
        return jsonify(reply="user haven't logged in yet")
    if user.token != token:
        return jsonify(reply="incorrect token")

    m = Message(user, quser, user.username+' want to be friend with you !')
    db.session.add(m)
    user.makeFriend(quser)
    return jsonify(reply="succeed")

@api.route('/user/deletefriend', methods=["POST"])
def userdeletefriend():
    data = request.get_json()
    username = data.get('username', '')
    token = data.get('token', '')
    qusername = data.get('query', '')
    
    user = User.query.filter_by(username=username).first()
    quser = User.query.filter_by(username=qusername).first()
    if not user:
        return jsonify(reply="invalid username")
    if not quser:
        return jsonify(reply="not found")
    if user.token == '':
        return jsonify(reply="user haven't logged in yet")
    if user.token != token:
        return jsonify(reply="incorrect token")

    f = Friendship.query.filter_by(first_id=user.id, second_id=quser.id).first()
    if not f: f = Friendship.query.filter_by(second_id=user.id, first_id=quser.id).first()
    if not f:
        return jsonify(reply="you are not friends yet")

    db.session.delete(f)
    db.session.commit()
    return jsonify(reply="succeed")


