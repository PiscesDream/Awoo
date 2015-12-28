import json
import urllib2

def sendjson(host, data):
    req = urllib2.Request(host)
    req.add_header('Content-Type', 'application/json')
    response = urllib2.urlopen(req, json.dumps(data))
    return response

if __name__ == '__main__':
    host = 'http://127.0.0.1:5000/api/login'
    data = {
        'username':'asdf',
        'password':'asdf'
        }
    res = sendjson(host, data)
