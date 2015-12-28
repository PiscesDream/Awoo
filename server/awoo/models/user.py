from .. import db
from .. import login_manager

from flask import current_app
from flask.ext.login import UserMixin
from werkzeug.security import generate_password_hash, check_password_hash
from ..toolkits.randomgen import randomstr
from datetime import datetime

class Friendship(db.Model):
    id = db.Column(db.Integer, primary_key=True)  

    first_id = db.Column(db.Integer, db.ForeignKey('user.id'))
    second_id = db.Column(db.Integer, db.ForeignKey('user.id'))

    def __repr__(self):
        return '<Friendship %r with %r>'\
                % (User.query.get(self.first_id).username,
                   User.query.get(self.second_id).username)


class User(UserMixin, db.Model):
    """ User Model """
    STATUS_OFFLINE = 0
    STATUS_ONLINE = 1
    STATUS_OCCUPIED = 2

    id = db.Column(db.Integer, primary_key=True)  

    avatar = db.Column(db.Text) # base64 pic
    intro = db.Column(db.Text, default='')
    status = db.Column(db.Integer, default=0)
    username = db.Column(db.String(128), unique=True)
    email = db.Column(db.String(128), unique=True)
    lastlogin = db.Column(db.DateTime, default=datetime.now)

    token = db.Column(db.String(128), default='') 
    password_hash = db.Column(db.String(32)) 
    salt = db.Column(db.String(32))

    def jsonify(self):
        return \
            {
                'id': self.id,
                'avatar': self.avatar,
                'intro': self.intro,
                'status': self.status,
                'username': self.username,
                'email': self.email,
                'lastlogin': self.lastlogin,
            }

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
        if not kwargs.get('avatar', ''): 
            kwargs['avatar'] = current_app.config['DEFAULT_AVATAR']
        super(User, self).__init__(**kwargs)
       #if self.username != "admin":
       #    self.send_email(subject='Welcome to FLAME', template='email/greeting',
       #                    user=self, async=async_mail)

    def __repr__(self):
        return '<User %r>' % (self.username)


    # relationship
    @property
    def friends(self):
        return User.query.join(Friendship,
                    self.id==Friendship.first_id).\
                filter(User.id==Friendship.second_id).all() + \
               User.query.join(Friendship,
                    self.id==Friendship.second_id).\
                filter(User.id==Friendship.first_id).all()

    def makeFriend(self, user):
        if user in self.friends: return
        f1 = Friendship(first_id=self.id, second_id=user.id)
#        f2 = Friendship(first_id=user.id, second_id=self.id)
        db.session.add(f1)
        db.session.commit()


