from . import main
from flask import render_template

@main.route('/')
def index():
    return render_template('empty.html')

@main.route('/login')
def login():
    return jsonify(reply='Please login through /api/login first')
