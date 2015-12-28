from .user import User
from .message import Message 

models = [User, Message]
models = dict(zip(map(lambda x: x.__name__, models), models))

def fakedata(db):
    print 'dropping ...'
    db.drop_all()
    print 'creating ...'
    db.create_all()

    u1 = User(username='admin', password='123456')
    u2 = User(username='test', password='123456')
    db.session.add_all([u1, u2])
    db.session.commit()

    msg1 = Message(u1, u2, 'hi')
    msg2 = Message(u2, u1, 'hi you')
    msg3 = Message(u1, u2, 'got to go')
    db.session.add_all([msg1, msg2, msg3])
    db.session.commit()

    print 'done.'
