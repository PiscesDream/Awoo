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
    u3 = User(username='user3', password='123456')
    u4 = User(username='user4', password='123456')
    u5 = User(username='user5', password='123456')
    u6 = User(username='user6', password='123456')
    u7 = User(username='user7', password='123456')
    u8 = User(username='user8', password='123456')
    u9 = User(username='user9', password='123456')
    db.session.add_all([u1, u2, u3, u4, u5, u6, u7, u8, u9])
    u1.makeFriend(u2)
    u2.makeFriend(u3)
    u4.makeFriend(u3)
    u3.makeFriend(u1)
    u4.makeFriend(u2)
    u5.makeFriend(u2)
    u6.makeFriend(u2)
    u7.makeFriend(u2)
    u8.makeFriend(u2)
    u9.makeFriend(u2)
    db.session.add_all([u1, u2, u3, u4, u5])
    db.session.commit()

    msg1 = Message(u1, u2, 'hi')
    msg2 = Message(u2, u1, 'hi you')
    msg3 = Message(u2, u3, 'got to go')
    msg4 = Message(u2, u4, 'got to go')
    msg5 = Message(u2, u5, 'got to go')
    msg6 = Message(u2, u6, 'got to go')
    msg7 = Message(u2, u7, 'got to go')
    msg8 = Message(u2, u8, 'got to go')
    db.session.add_all([msg1, msg2, msg3, msg4,
        msg5, msg6, msg7, msg8])
    db.session.commit()

    print 'done.'
