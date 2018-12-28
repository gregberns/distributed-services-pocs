FROM docker.elastic.co/logstash/logstash:6.4.2

COPY ./sqljdbc_7.0/enu/mssql-jdbc-7.0.0.jre8.jar /opt/logstash/vendor/jar/jdbc/
COPY ./sqljdbc_7.0/enu/mssql-jdbc-7.0.0.jre10.jar /opt/logstash/vendor/jar/jdbc/

# This apparently is used on windows boxes
# https://discuss.elastic.co/t/does-logstash-jdbc-connection-work-with-windows-authentication/141562/10
#COPY ./sqljdbc_7.0/enu/auth/x64/sqljdbc_auth.dll /opt/logstash/vendor/jar/jdbc/


#RUN rm -f /usr/share/logstash/pipeline/logstash.conf
#ADD pipeline/ /usr/share/logstash/pipeline/
#ADD config/ /usr/share/logstash/config/

#RUN chown /usr/share/logstash/config/
#RUN chmod 755 /usr/share/logstash/config/

# Install JDBC Plugin
RUN /opt/logstash/bin/logstash-plugin install logstash-input-jdbc
