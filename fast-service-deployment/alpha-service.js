'use strict';

const http = require('http')
const port = process.env.PORT || 3000

const requestHandler = (request, response) => {
  console.log(`${new Date().toLocaleTimeString()} ${request.url}`)
  if (request.url === '/ping') {
    response.end(`ping. port: ${port}`)
    return;
  } else if (request.url === '/health'){
    response.end('good')
    return;
  } else {
    response.end('Hello Node.js Server!')
    return;
  }
}

const server = http.createServer(requestHandler)

server.listen(port, (err) => {
  if (err) {
    return console.log('something bad happened', err)
  }

  console.log(`server is listening on ${port}`)
})
