
Goals:

https://logz.io/blog/docker-logging/

* Git pull ELK stack image - Complete
* Get ELK stack up and data flowing through - Complete
* Logstash and/or Fluentd - Retrieve data from RabbitMQ
* Logstash and/or Fluentd - Retrieve data from SQL Server


## LogStash

### Extract Logs from SQL Server

[Logstash - Extract Logs from SQL Server DB](https://www.elastic.co/guide/en/logstash/current/plugins-inputs-jdbc.html)

[Jdbc input plugin](https://www.elastic.co/guide/en/logstash/current/plugins-inputs-jdbc.html)

[Download Microsoft JDBC Driver for SQL Server](https://docs.microsoft.com/en-us/sql/connect/jdbc/download-microsoft-jdbc-driver-for-sql-server?view=sql-server-2017)

```
input {
  jdbc {
    jdbc_driver_library => "mysql-connector-java-5.1.36-bin.jar"
    jdbc_driver_class => "com.mysql.jdbc.Driver"
    jdbc_connection_string => "jdbc:mysql://localhost:3306/mydb"
    jdbc_user => "mysql"
    --parameters => { "favorite_artist" => "Beethoven" }
    schedule => "* * * * *"
    statement => "SELECT id, mycolumn1, mycolumn2 FROM my_table WHERE id > :sql_last_value"
    use_column_value => true
    tracking_column => "id"
  }
}

```

> sql_last_value:  The value used to calculate which rows to query. Before any query is run, this is set to Thursday, 1 January 1970, or 0 if use_column_value is true and tracking_column is set. It is updated accordingly after subsequent queries are run.







SSH into the machine
```
docker-machine ssh vm1
```

Install FluentD on Windows
https://docs.fluentd.org/v1.0/articles/install-by-msi



## Items to Look At


[Moving Data from SQL Server to Elastic](https://codeshare.co.uk/blog/how-to-copy-sql-server-data-to-elasticsearch-using-logstash/)

[Loading large datasets into SQL Server](https://instarea.com/heavy-load-ms-sql-elasticsearch/)