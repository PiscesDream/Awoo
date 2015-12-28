from awoo import create_app, db
from flask.ext.script import Manager, Shell
from awoo.models import models, fakedata

app = create_app('default')
manager = Manager(app)

def make_shell_context():
    return dict(app=app, db=db, 
            fakedata=fakedata, **models)

manager.add_command("shell", Shell(make_context=make_shell_context))

if __name__ == '__main__':
    manager.run()
