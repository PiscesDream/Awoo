from .user import User

models = [User]
models = dict(zip(map(lambda x: x.__name__, models), models))

def fakedata(db):
    u = User(username='admin', password='123456')
    db.session.add(u)
    db.session.commit()
