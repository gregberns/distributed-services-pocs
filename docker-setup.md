# Docker Setup and Helpful Commands

## Restart Docker Daemon

If a `docker-compose up` fails with `retrying in 1 second`, then restarting the Docker daemon can help.

```
sudo systemctl restart docker
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
