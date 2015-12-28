from .user import User, Friendship
from .message import Message 

models = [User, Message, Friendship]
models = dict(zip(map(lambda x: x.__name__, models), models))

def fakedata(db):
    print 'dropping ...'
    db.drop_all()
    print 'creating ...'
    db.create_all()

    u1 = User(username='admin', password='123456')
    u2 = User(username='test', password='123456')
    u3 = User(username='user1', password='123456')
    u4 = User(username='user2', password='123456')
    u5 = User(username='user3', password='123456')
    db.session.add_all([u1, u2, u3, u4, u5])
    u1.makeFriend(u2)
    u2.makeFriend(u3)
    u4.makeFriend(u3)
    u3.makeFriend(u1)
    db.session.add_all([u1, u2, u3, u4, u5])
    db.session.commit()

    msg1 = Message(u1, u2, 'hi')
    msg2 = Message(u2, u1, 'hi you')
    msg3 = Message(u1, u2, 'got to go')
    db.session.add_all([msg1, msg2, msg3])
    db.session.commit()

    print 'done.'
