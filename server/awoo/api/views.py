from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User
from ..toolkits.randomgen import randomstr

@api.route('/test', methods=["GET"])
def test():
    return jsonify(teststr="testing")

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
    db.session.add(user)
    db.session.commit()
    return jsonify(reply="succeed", token=user.token)
