from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User
from ..toolkits.randomgen import randomstr

@api.route('/logout', methods=["POST"])
def logout():
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

    user.token = '' 
    user.status = User.STATUS_OFFLINE
    db.session.add(user); db.session.commit()
    return jsonify(reply="logged out")

@api.route('/login', methods=["POST"])
def login():
    data = request.get_json()
    username = data.get('username', '')
    password = data.get('password', '')
    
    user = User.query.filter_by(username=username).first()
    if not user:
        return jsonify(reply="invalid username")
    if not user.verify_password(password):
        return jsonify(reply="incorrect password")

    user.token = randomstr(128)
    user.status = User.STATUS_ONLINE
    db.session.add(user)
    db.session.commit()
    return jsonify(reply="logged in", token=user.token)
