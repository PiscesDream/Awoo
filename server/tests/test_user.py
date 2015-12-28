from . import AwooTestCase

from toolkits import sendrecvjson
import json

class TestUser(AwooTestCase):
    def test_login(self):
        host = self.get_server_url()+'/api/login'
        data = {
            'username':'asdf',
            'password':'asdf'
            }

        assert sendrecvjson(host, data).get('reply', '') == 'invalid username'
        data['username'] = 'admin'
        assert sendrecvjson(host, data).get('reply', '') == 'incorrect password'
        data['password'] = '123456'
        assert sendrecvjson(host, data).get('reply', '') == 'logged in'

