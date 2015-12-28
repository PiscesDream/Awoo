from . import api 
from .. import db
from flask import render_template, jsonify, request
from ..models import User
from ..toolkits.randomgen import randomstr

@api.route('/test', methods=["GET"])
def test():
    return jsonify(teststr="testing")

