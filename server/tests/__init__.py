from flask.ext.testing import LiveServerTestCase
#from flask.ext.testing import TestCase

from awoo import create_app, db
from awoo.models import User 
from time import sleep

class AwooTestCase(LiveServerTestCase):
    def create_app(self):
        return create_app('test')

    def setUp(self):
        with self.app.app_context():
            db.create_all()

            # administrator
            u = User(username='admin', email='admin@example.com', password='123456')
            db.session.add(u)
            db.session.commit()

    def tearDown(self):
        with self.app.app_context():
            db.session.remove()
            db.drop_all()

