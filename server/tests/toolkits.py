import json
import urllib2

def sendjson(host, data):
    req = urllib2.Request(host)
    req.add_header('Content-Type', 'application/json')
    response = urllib2.urlopen(req, json.dumps(data))
    return response

def sendrecvjson(host, data):
    res = sendjson(host, data)
    return json.loads(''.join(res.readlines()))

def login(username, password):
    res = sendrecvjson(
            'http://127.0.0.1:5000/api/login',
            {'username': username, 'password': password }
            )
    return res['token']

def fetchmsg(username, token):
    res = sendrecvjson(
            'http://127.0.0.1:5000/api/msg/fetch',
            {'username': username, 'token': token}
            )
    return res

def sendmsg(username, token, receiver, message):
    res = sendrecvjson(
            'http://127.0.0.1:5000/api/msg/send'
            {'username': username, 'token': token,
             'receiver': receiver, 'message': message}
            )
    return res

if __name__ == '__main__':
    pass
