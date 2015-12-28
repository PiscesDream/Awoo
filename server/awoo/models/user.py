from .. import db
from .. import login_manager

from flask.ext.login import UserMixin
from werkzeug.security import generate_password_hash, check_password_hash
from ..toolkits.randomgen import randomstr
from datetime import datetime

class User(UserMixin, db.Model):
    """ User Model """

    id = db.Column(db.Integer, primary_key=True)  

    username = db.Column(db.String(128), nullable=False, unique=True)
    email = db.Column(db.String(128), unique=True)
    avatar = db.Column(db.Text) # base64 pic

    token = db.Column(db.String(128), default='') 
    lastlogin = db.Column(db.DateTime, default=datetime.now)

    password_hash = db.Column(db.String(32)) 
    salt = db.Column(db.String(32))
    @property
    def password(self):
        return "You should not see it"

    @password.setter
    def password(self, value):
        self.salt = randomstr(32) 
        self.password_hash = generate_password_hash(value+self.salt)

    def verify_password(self, password):
        return check_password_hash(self.password_hash, password+self.salt)

    def __init__(self, **kwargs):
        kwargs['username'] = kwargs['username'][:128]
        super(User, self).__init__(**kwargs)
       #if self.username != "admin":
       #    self.send_email(subject='Welcome to FLAME', template='email/greeting',
       #                    user=self, async=async_mail)


