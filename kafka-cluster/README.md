
Goals:

* Understand SSH
* Create Compose file
* Get Compose file onto server
* Run Compose file 
* Try and connect to kafka
* Remotely install Zookeeper then Kafka
* Start Zookeeper then Kafka
* Get code on machine
* Start up Producer
* Start Consumer
* Send messages
* Look at `docker-machine`



* Create a producer and consumer (first in Node)
* Get NodeJs installed
* Upload code to vm 
* Create a container out of it
* Start it and validate it runs



* How do we transfer a container into VM and start it
https://www.fir3net.com/Containers/Docker/the-essential-guide-in-transporting-your-docker-containers.html


Next: .NET?


## Standing Up a Container Environment

Install Docker Machine

Linux
```
base=https://github.com/docker/machine/releases/download/v0.14.0 &&
  curl -L $base/docker-machine-$(uname -s)-$(uname -m) >/tmp/docker-machine &&
  sudo install /tmp/docker-machine /usr/local/bin/docker-machine
```

Create a new VM
```
docker-machine create vm1 -d "virtualbox"
```

SSH into the machine
```
docker-machine ssh vm1
```

Install Compose
```
sudo curl -L "https://github.com/docker/compose/releases/download/1.22.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
docker-compose --version
```

Add a compose file
```
docker-machine scp -r ./docker-compose.yml vm1:/home/docker
```

Start the composed service
```
docker-compose up
```

## Docker-Machine Building, Starting Container

This recipe:
* creates a VM
* Adds files
* Builds a container
* Starts container
* Checks service is working


```
docker-machine create -d virtualbox vm1
```

Copy app files
```
docker-machine scp -r . vm1:~/producer-node
```

SSH into the machine, then build Node app
```
docker build -t gregberns/node-web-app .
```

Start Container
```
docker run -p 49160:8080 -d gregberns/node-web-app
```

See if the service is up
```
curl http://localhost:49160
```


## Docker-Compose Run Multiple Containers

Add a compose file
```
docker-machine scp -r ./docker-compose.yml vm1:/home/docker
```


```
docker-compose up
```




## Kafka Quick Start

Start Zookeeper

```
zookeeper-server-start.sh config/zookeeper.properties
```

Start Kafka

```
kafka-server-start.sh config/server.properties
```

Create a Topic

```
kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic test
```

error An unexpected error occurred: "https://registry.yarnpkg.com/express: unable to get local issuer certificate".



## Clear Topic by Changing Retention


```
kafka-configs.sh --zookeeper <zkhost>:2181 --alter --entity-type topics --entity-name <topic name> --add-config retention.ms=1000
```

kafka-configs.sh --zookeeper localhost:2181 --alter --entity-type topics --entity-name test --add-config retention.ms=1000


[2018-10-01 16:44:08,416] WARN [Log partition=test-0, dir=C:\tmp\kafka-logs] ret
ention.ms for topic test is set to 600000. It is smaller than message.timestamp.
difference.max.ms's value 9223372036854775807. This may result in frequent log r
olling. (kafka.log.Log)






## Configuring and Running Kafka

`spotify/kafka/`
https://github.com/spotify/docker-kafka

If using just docker

```
docker run -p 2181:2181 -p 9092:9092 --env ADVERTISED_PORT=9092 spotify/kafka
```

Or if youre using docker machine (not needed in a simple scenario)

```
docker run -p 2181:2181 -p 9092:9092 --env ADVERTISED_HOST=`docker-machine ip \`docker-machine active\`` --env ADVERTISED_PORT=9092 spotify/kafka
```

```
export KAFKA=`docker-machine ip \`docker-machine active\``:9092
kafka-console-producer.sh --broker-list $KAFKA --topic test
```

```
export ZOOKEEPER=`docker-machine ip \`docker-machine active\``:2181
kafka-console-consumer.sh --zookeeper $ZOOKEEPER --topic test
```



Copy file from local to VM:

```
scp /Users/huck/gitlab/concord-revolution/kafka-cluster/docker-compose.yml vm1@192.168.0.107:/home/vm1/tmp/
```

## Errors


```
docker: Got permission denied while trying to connect to the Docker daemon socket at unix:///var/run/docker.sock: Post http://%2Fvar%2Frun%2Fdocker.sock/v1.38/containers/create: dial unix /var/run/docker.sock: connect: permission denied.
See 'docker run --help'.
```

https://techoverflow.net/2017/03/01/solving-docker-permission-denied-while-trying-to-connect-to-the-docker-daemon-socket/

```
sudo usermod -a -G docker $USER
```


