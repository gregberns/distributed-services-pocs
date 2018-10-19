


## Logspout

https://github.com/gliderlabs/logspout#inspect-log-streams-using-curl
```

$ docker run -d gberns/simple-stout-activity-generator


$ docker run -d -e "RAW_FORMAT={{ toJSON .Data}}\n" --volume=/var/run/docker.sock:/var/run/docker.sock --publish=127.0.0.1:8000:80 gliderlabs/logspout


$ curl http://127.0.0.1:8000/logs

```


## Docker Logging Drivers

To change the logging driver change `/etc/docker/daemon.json`

Fluentd example:

```
{
    "log-driver": "fluentd",
    "log-opts": {
        "fluentd-address": "fluentdhost:24224"
    }
}
```

Then restart the daemon

```
sudo systemctl restart docker
```

## Run Fluentd


```
docker run -d -p 24224:24224 -p 24224:24224/udp -v /data:/fluentd/log fluent/fluentd
```

https://docs.fluentd.org/v0.12/articles/docker-logging-efk-compose
```


version: '2'
services:  
  fluentd:
    build: ./fluentd
    volumes:
      - ./fluentd/conf:/fluentd/etc
    links:
      - "elasticsearch"
    ports:
      - "24224:24224"
      - "24224:24224/udp"

```

```


# fluentd/Dockerfile
FROM fluent/fluentd:v0.12-debian
RUN ["gem", "install", "fluent-plugin-elasticsearch", "--no-rdoc", "--no-ri", "--version", "1.9.2"]
```