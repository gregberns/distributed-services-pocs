





## Restart Docker Daemon

```
sudo systemctl s tart docker
```


## Create and Run a Docker Image

```

# Builds an image based on the local `Dockerfile`
#  and stores it in the local store under the name provided
$ docker build -t gberns/simple-stout-activity-generator .

# -d runs the container in the background (detached)
$ docker run -d -t  gberns/simple-stout-activity-generator

# the command above will return a long string identifier, use it to replace 'XXXX'
$ docker logs XXXXXXXXXXXXXXXXXXXXXXXX



```




