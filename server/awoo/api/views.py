from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User
from ..toolkits.randomgen import randomstr

@api.route('/test', methods=["GET"])
def test():
    return jsonify(teststr="testing")

@api.route('/user/fetch/id', methods=["POST"])
def user_fetch_by_id():
    data = request.get_json()
    user = User.query.get(data.get('user_id', 0))

    if not user:
        return jsonify(reply="invalid user id")

    return jsonify(reply="succeed", info=user.jsonify())

@api.route('/user/fetch/username', methods=["POST"])
def user_fetch_by_username():
    data = request.get_json()
    user = User.query.filter_by(username=data.get('username', 0)).first()

    if not user:
        return jsonify(reply="invalid username")

    return jsonify(reply="succeed", **user.jsonify())




