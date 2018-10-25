FROM docker.elastic.co/logstash/logstash:6.4.2

COPY ./sqljdbc_7.0/enu/mssql-jdbc-7.0.0.jre8.jar /opt/logstash/vendor/jar/jdbc/
COPY ./sqljdbc_7.0/enu/mssql-jdbc-7.0.0.jre10.jar /opt/logstash/vendor/jar/jdbc/


RUN rm -f /usr/share/logstash/pipeline/logstash.conf
ADD pipeline/ /usr/share/logstash/pipeline/
ADD config/ /usr/share/logstash/config/

# Install JDBC Plugin
RUN /opt/logstash/bin/logstash-plugin install logstash-input-jdbc
