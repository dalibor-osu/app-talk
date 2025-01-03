SELECT 'CREATE DATABASE app_talk' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'app_talk')\gexec

DO $$ BEGIN
    IF NOT EXISTS (SELECT * FROM pg_user WHERE usename = 'app_talk') THEN
CREATE ROLE app_talk LOGIN SUPERUSER password 'HteVuDclI4pMTiUMRp8fQN3wXfqMRf';
END IF;
END $$;