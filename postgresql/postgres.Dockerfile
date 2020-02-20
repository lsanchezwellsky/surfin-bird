FROM postgres:latest

# Custom initialization scripts
COPY ./create_db.sh /docker-entrypoint-initdb.d/create_db.sh
COPY ./db/wellsky.backup /wellsky.backup

RUN chmod +x /docker-entrypoint-initdb.d/create_db.sh
