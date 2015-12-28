class Config:
    SECRET_KEY = "AwooIsNaiveSoDoNotMockAtMe"
     
    #SQLalchemy
    SQLALCHEMY_DATABASE_URI = "sqlite:///Awoo.db"
    SQLALCHEMY_COMMIT_ON_TEARDOWN = True
    SQLALCHEMY_RECORD_QUERIES = True
    
    #db query time
    FLASKY_SLOW_DB_QUERY_TIME=0.5

    #email
    MAIL_SERVER = 'smtp-mail.outlook.com'
    MAIL_PORT = 25 
#    MAIL_USE_SSL = True
    MAIL_USE_TLS = True
    MAIL_USERNAME = 'Awoo.hapd@outlook.com'
    MAIL_PASSWORD = 'Awoo@hapd' 
    FLASKY_MAIL_SUBJECT_PREFIX = 'Awoo@Hapd'
    FLASKY_MAIL_SENDER = 'Awoo.hapd <Awoo.hapd@outlook.com>'

    @staticmethod
    def init_app(app):
        pass

class DevelopmentConfig(Config):
    DEBUG = True

class ProductionConfig(Config):
    DEBUG = False

config = {
    'development': DevelopmentConfig,
    'production': ProductionConfig,

    'default': DevelopmentConfig
}
