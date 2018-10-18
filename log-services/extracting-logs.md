




https://github.com/gliderlabs/logspout#inspect-log-streams-using-curl
```
$ docker run -d --name="logspout" --volume=/var/run/docker.sock:/var/run/docker.sock --publish=127.0.0.1:8000:80 gliderlabs/logspout

# This does not seem to work
$ curl http://127.0.0.1:8000/logs

```
