//https://thomashunter.name/presentations/node-consul-v1/#/6
const uuid = require('uuid')
const http = require('http')
const consul = require('consul')()

const host = 'localhost'
const port = process.env.PORT || 3000



const requestHandler = (request, response) => {
  console.log(request.url)
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


const CONSUL_ID = uuid.v4();

function register() {
  
  let details = {
    name: 'beta',
    tags: ['beta', 'v1.0.0'],
    address: host,
    port: port,
    id: CONSUL_ID,
    check: {
      ttl: '10s',
      deregister_critical_service_after: '1m'
    }
  };
  consul.agent.service.register(details, err => {
    // schedule heartbeat
    console.log('consul register callback', err)
  });
}

function startHearbeat() {
  setInterval(() => {
    consul.agent.check.pass({id:`service:${CONSUL_ID}`}, err => {
      if (err) throw new Error(err);
      console.log('told Consul that we are healthy');
    });
  }, 5 * 1000);
}

register()
startHearbeat()

process.on('SIGINT', () => {
  console.log('SIGINT. De-Registering...');
  let details = {id: CONSUL_ID};

  consul.agent.service.deregister(details, (err) => {
    console.log('de-registered.', err);
    process.exit();
  });
});
