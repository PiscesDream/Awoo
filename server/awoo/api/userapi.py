from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User
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

    user.makeFriend(quser)
    return jsonify(reply="succeed")


