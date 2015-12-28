from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User, Message
from ..toolkits.randomgen import randomstr

@api.route('/msg/fetch', methods=["POST"])
def fetchmsg():
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

    msgs = []
    queries = Message.query.filter_by(recver_id=user.id, read=False).all()
    for msg in queries:
        sender = User.query.get(msg.sender_id)
        recver = User.query.get(msg.recver_id)
        msgs.append({
            'sender_id': sender.id,
            'sender': sender.username,
            'timestamp': msg.timestamp,
            'content': msg.content
        }) 
        msg.read = True
    db.session.add_all(queries)
#    db.session.delete(queries)
    db.session.commit()

    return jsonify(reply="succeed", messages=msgs)


@api.route('/msg/send', methods=["POST"])
def sendmsg():
    data = request.get_json()
    username = data.get('username', '')
    token = data.get('token', '')
    receiver = data.get('receiver', '')
    message = data.get('message', '')
    
    user = User.query.filter_by(username=username).first()
    recver = User.query.filter_by(username=receiver).first()
    if not user:
        return jsonify(reply="invalid username")
    if not recver:
        return jsonify(reply="invalid receiver")
    if user.token == '':
        return jsonify(reply="user haven't logged in yet")
    if user.token != token:
        return jsonify(reply="incorrect token")

    m = Message(user, recver, message)
    db.session.add(m)
    db.session.commit()
    return jsonify(reply="succeed")


