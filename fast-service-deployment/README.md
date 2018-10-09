# Service Deployment Investigation

## Objectives

## Scope

In priority order

* Dynamic Reverse proxy
    * Have multiple instances
* Spin up and tear down services
* Detect if services are available, notify if not
* Logging
* Service Discovery
* Metrics




## Phase 1

**Objective:** Have proxy dynamically switch between two different running services, with no interuption.

* Start two instances of server on different ports
* Client starts pinging service, instance1 will console.out for demo purposes
* Switch proxy to point to instance2
* Requests are directed to instance2

## Phase 2

Support multiple instances running at the same time and round-robin requests.

```
[backends.backend1.servers.server1]
url = "http://localhost:3000"
weight = 1
[backends.backend1.servers.server2]
url = "http://localhost:3001"
weight = 1
```





#### Docker Helper Commands

```
# scale the instances
docker-compose scale <service name in the compose file>=2

# see all local images
docker images

# See running docker images
docker container ls

# to build the docker image
docker build -t gregberns/alpha-service .

# to run the image
docker run -p 49160:8080 -d gregberns/alpha-service

```


```
# start the reverse proxy
docker-compose up -d reverse-proxy

# start the service
docker-compose up -d alpha-service

# ping the service
curl -H Host:alpha-service.docker.localhost http://127.0.0.1/ping
```



## Consul

```
./consul agent -dev -config-dir=./consul.d/web.json
```

Health check
`curl 'http://localhost:8500/v1/health/service/alpha?passing'`

List of services
`curl http://localhost:8500/v1/catalog/service/alpha`

https://www.consul.io/intro/getting-started/services.html#updating-services











* Host 3 services
* Create fake client to create traffic
    * some requests take longer than others
    * Use Apache jMeter to load up the services


## Things to look at

* Use Octopus to do deploys
* --How can we get approvals before swaps--
* Metrics - promethius
    * has ASP middleware
    * has standalone metrics server
* Kestrel - to host a ASP Core service
* Test some logging services to send messages to
    * trace, log, warn, error
* Service Registry - etcd, consul

## Services

* Should have a '/health' endpoint that returns a 2** or 3** status


## Tools

* Traefik.io - reverse proxy that has dynamic configuration






# Service Discovery

(https://www.consul.io/discovery.html)

> Load balancers are often used to front a service tier and provide a static IP. These load balancers add cost, increase latency, introduce single points of failure, and must be updated as services scale up/down.

> Instead of load balancers, connectivity in dynamic infrastructure is best solved with service discovery. Service discovery uses a registry to keep a real-time list of services, their location, and their health. Services query the registry to discover the location of upstream services and then connect directly. This allows services to scale up/down and gracefully handle failure without a load balancer intermediary.

SICK~!~
What if we deploy and start up services, and they get registered. Then once registered, they show up on a dashboard, and we can toggle between the versions. We can pucblish tags with the name of the service plus version+build number.
https://thomashunter.name/presentations/node-consul-v1/#/6












