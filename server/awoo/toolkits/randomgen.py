import random, string

def randomstr(len=32):
    return ''.join([random.choice(string.ascii_letters + string.digits) for n in xrange(len)])
