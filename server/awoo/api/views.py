from . import api 
from flask import render_template, jsonify

@api.route('/test', methods=["GET"])
def test():
    return jsonify(teststr="testing")
