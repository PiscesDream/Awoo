class Config:
    SECRET_KEY = "TheAwooIsSimpleSoDon'tMockMe"
    
    
    #SQLalchemy
    SQLALCHEMY_DATABASE_URI = "sqlite:///Awoo.db"
    SQLALCHEMY_COMMIT_ON_TEARDOWN = True
    SQLALCHEMY_RECORD_QUERIES = True
    #db query time
    FLASKY_SLOW_DB_QUERY_TIME=0.5

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

