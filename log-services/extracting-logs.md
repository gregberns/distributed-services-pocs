


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









https://opensource.com/article/18/3/efk-creating-open-source-stack



docker run --network=efk -d -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "cluster.name=docker-cluster" -e "bootstrap.memory_lock=true" -e "ES_JAVA_OPTS=-Xms512m -Xmx512m" --ulimit memlock=-1:-1 -v elasticdata:/usr/share/elasticsearch/data docker.elastic.co/elasticsearch/elasticsearch-oss:6.2.2



docker run --network=efk --name kibana -d -p 5601:5601 docker.elastic.co/kibana/kibana-oss:6.2.2


docker run -d --network efk --name fluentd -p 42185:42185/udp <Image ID>

docker run -d -p 24224:24224 -p 24224:24224/udp -v /data:/fluentd/log fluent/fluentd

docker run -d --network efk -c ./fluent/fluent.conf -p 24224:24224 -p 24224:24224/udp fluent/fluentd

docker run -d worker 



