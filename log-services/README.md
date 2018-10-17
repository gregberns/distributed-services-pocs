# Log Services


**Objectives**
* Need to do the SQL stmts
* Research workign with STDOUT/STDERR
* What are *NIX best practices around logging
* How to move logs to a particular folder 
  * Could we use a log aggregator to move the files? Is it the right thing to do
* Research Alerting
* Research health monitoring of services - tools to use


## TODO

Use [JSON appsettings.json configuration](https://github.com/serilog/serilog-sinks-file#json-appsettingsjson-configuration) to output JSON formatted logs

##### JSON event formatting
To write events to the file in an alternative format such as JSON, pass an ITextFormatter as the first argument:

```
.WriteTo.File(new CompactJsonFormatter(), "log.txt")
```

##### Shared log files

To enable multi-process shared log files, set shared to true:

```
    .WriteTo.File("log.txt", shared: true)
```


### Reference


Good article on logging best practices
https://logmatic.io/blog/beyond-application-monitoring-discover-logging-best-practices/

### Further Research

#### Monitoring

[USE FILE-BASED SERVICE DISCOVERY TO DISCOVER SCRAPE TARGETS](https://prometheus.io/docs/guides/file-sd/)

Can use [node_exporter](https://github.com/prometheus/node_exporter) to monitor *NIX hardware and OS metrics.


### Ideas

#### Configuration Exposed through URL

Can we expose configuration through an exposed http url endpoint?!?!

See: `kubectl create -f https://k8s.io/examples/pods/security/security-context.yaml`

https://kubernetes.io/docs/tasks/configure-pod-container/security-context/#set-the-security-context-for-a-pod

#### Microsoft.Extensions.Logging

Look at `Microsoft.Extensions.Logging`


## Log Mgmt in *NIX

Could use `Rsyslog` to move logs from local to remote server
https://www.loggly.com/ultimate-guide/managing-linux-logs/

> Many applications add some sort of date time stamp in them. This makes it much more difficult to find the latest file and to setup file monitoring by rsyslog. A better approach is to add timestamps to older log files using logrotate. This makes them easier to archive and search historically.

> Linux best practice usually suggests mounting the /var directory to a separate file system. This is because of the high number of I/Os associated with this directory. We would recommend mounting /var/log directory under a separate disk system. This can save I/O contention with the main application’s data. Also, if the number of log files becomes too large or the single log file becomes too big, it doesn’t fill up the entire disk.

> When logrotate copies a file, the new file has a new inode, which can interfere with rsyslog’s ability to monitor the new file. You can alleviate this issue by adding the copytruncate parameter to your logrotate cron job. This parameter copies existing log file contents to a new file and truncates these contents from the existing file. The inode never changes because the log file itself remains the same; its contents are in a new file.
> The logrotate utility uses the main configuration file at /etc/logrotate.conf and application-specific settings in the directory /etc/logrotate.d/. DigitalOcean has a detailed tutorial on logrotate.


## Setting Up a Logging Framework

**Objective:** What are the pieces to a modern logging framework. Will focus on getting logs forwarded to a centralized platform.


**Goals:**

* Git pull ELK stack image - Complete
* Get ELK stack up and data flowing through - Complete
* Logstash and/or Fluentd - Retrieve data from SQL Server
* Logstash and/or Fluentd - Retrieve data from RabbitMQ


[Setting up a ELK stack in Docker](https://logz.io/blog/docker-logging/)


### Extract Data from SQL Log Stores

[Run SQL Server container images with Docker](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-2017)

Pull the SQL Server Docker Instance

```
docker pull mcr.microsoft.com/mssql/server:2017-latest
```

Start the SQL Server instance

```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Solution1' \
  -p 1433:1433 --name sql1 \
  -d mcr.microsoft.com/mssql/server:2017-latest
```

Check to see if the instance is up.

```
docker ps
```

### Get sqlcmd Working

Either use the [docker container](https://hub.docker.com/r/microsoft/mssql-tools/)

```
docker pull mcr.microsoft.com/mssql-tools
docker run -it mcr.microsoft.com/mssql-tools
```

Or on Mac

```
# brew untap microsoft/mssql-preview if you installed the preview version
brew tap microsoft/mssql-release https://github.com/Microsoft/homebrew-mssql-release
brew update
brew install --no-sandbox mssql-tools
#for silent install:
#ACCEPT_EULA=y brew install --no-sandbox mssql-tools
```

#### Interact with `sqlcmd`, Create Log Database

Connect to the SQL instance

```
# sqlcmd -U sa -P Solution1 -S localhost
> select @@version
> go
```

List Databases in instance

```
select name, database_id from sys.databases
```

```
CREATE DATABASE logs
go

ALTER LOGIN sa with default_database = logs
go

CREATE TABLE log (ID int IDENTITY(1,1) PRIMARY KEY, loglevel varchar(255), message varchar(255), stacktrace varchar(255))
go

--check table was created
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'
go

SELECT * FROM dbo.log
go

INSERT INTO log (loglevel, message, stacktrace) VALUES ('ERR', 'This Broke', 'Method Line:123')
go

INSERT INTO log (loglevel, message, stacktrace) VALUES ('WRN', 'Just a warning', 'Method Line:345')

```

#### JDBC ODBC Connection to SQL Server

Validata Java is installed (if not, figure out how to get it installed)

```
java -version
```


[Download Microsoft JDBC Driver for SQL Server](https://docs.microsoft.com/en-us/sql/connect/jdbc/download-microsoft-jdbc-driver-for-sql-server?view=sql-server-2017)

[Jdbc input plugin](https://www.elastic.co/guide/en/logstash/current/plugins-inputs-jdbc.html)


### Logstash

#### Install LogStash

[Directions to install Logstash](https://www.elastic.co/guide/en/logstash/current/installing-logstash.html)

[Download Logstash](https://www.elastic.co/downloads/logstash)

##### Send a Message

[Send first Logstash event](https://www.elastic.co/guide/en/logstash/current/first-event.html)

Run this command: (Note: it may take a moment to spin up.)

```
./../bin/logstash-6.4.2/bin/logstash -e 'input { stdin { } } output { stdout {} }'
```

Once you see `Successfully started Logstash API endpoint` you can type an input, press ENTER, and the message will be output in a structured format.

#### Extract Logs from SQL Server

We'll use a plugin to pull data from SQL Server: [Logstash - Extract Logs from SQL Server DB](https://www.elastic.co/guide/en/logstash/current/plugins-inputs-jdbc.html)

Use the `-f` command to define the location of the config file. Example: `-f logstash-filter.conf`

Configuration Example:

```
input {
 jdbc {
   jdbc_driver_library => "./../bin/sqljdbc_7.0/enu/mssql-jdbc-7.0.0.jre8.jar"
   jdbc_driver_class => "com.microsoft.sqlserver com.sqlserver.jdbc.Driver"
   jdbc_connection_string => "jdbc:sqlserver://localhost:1433;databaseName=master;user=sa;password=Solution1"
   jdbc_user => "sa"
   schedule => "* * * * *"
   statement => "SELECT id, loglevel, message, stacktrace FROM log WHERE id > :sql_last_value"
   use_column_value => true
   tracking_column => "id"
   last_run_metadata_path => "./last_run_metadata_path"
 }
}

output {
 stdout {}
}
```

> sql_last_value:  The value used to calculate which rows to query. Before any query is run, this is set to Thursday, 1 January 1970, or 0 if use_column_value is true and tracking_column is set. It is updated accordingly after subsequent queries are run.

Command to run:

```
./../bin/logstash-6.4.2/bin/logstash -f logstash-sqlserver.config
```


### Fluentd

Install FluentD on Windows
https://docs.fluentd.org/v1.0/articles/install-by-msi



### Items to Look At

[Moving Data from SQL Server to Elastic](https://codeshare.co.uk/blog/how-to-copy-sql-server-data-to-elasticsearch-using-logstash/)

[Loading large datasets into SQL Server](https://instarea.com/heavy-load-ms-sql-elasticsearch/)


