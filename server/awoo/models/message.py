from .. import db
from datetime import datetime

class Message(db.Model):
    """ Message Model """

    id = db.Column(db.Integer, primary_key=True)  
    
    sender_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    recver_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    read = db.Column(db.Boolean, default=False)

    timestamp = db.Column(db.DateTime, index=True, default=datetime.now)
    content = db.Column(db.Text()) 

    def __init__(self, sender, recver, content):
        super(Message, self).\
                __init__(sender_id=sender.id, 
                         recver_id=recver.id, 
                         content=content)

    def __repr__(self):
        return '[{}]{}->{}: {}'.\
            format(
                self.timestamp.strftime('%d/%m/%Y %H:%M'),\
                self.sender_id,\
                self.recver_id,\
                self.content[:30])


