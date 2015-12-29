import json
import urllib2

def sendjson(host, data):
    req = urllib2.Request(host)
    req.add_header('Content-Type', 'application/json')
    response = urllib2.urlopen(req, json.dumps(data))
    return response

def sendrecvjson(url, data):
    res = sendjson(url, data)
    return json.loads(''.join(res.readlines()))

def login(host, username, password):
    res = sendrecvjson(
            host+'/api/login',
            {'username': username, 'password': password }
            )
    return res['token']

def fetchmsg(host, username, token):
    res = sendrecvjson(
            host+'/api/msg/fetch',
            {'username': username, 'token': token}
            )
    return res

def sendmsg(host, username, token, receiver, message):
    res = sendrecvjson(
            host+'/api/msg/send',
            {'username': username, 'token': token,
             'receiver': receiver, 'message': message})
    return res

if __name__ == '__main__':
    host = 'http://localhost:5000'
    pass
