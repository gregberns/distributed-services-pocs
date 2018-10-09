const consul = require('consul')()
const fetch = require("node-fetch");

let services = {}
let clientList = []

function getServices() {
  consul.catalog.service.list(function(err, res) {
    if (err) throw err;
    console.log('getServices response', res)
    services = res
    //{ beta: [ 'beta', 'v1.0.0' ], consul: [] }

  });
}

function getNodes() {
  const options = { service: 'beta' }
  consul.catalog.service.nodes('beta', function(err, res) {
    if (err) throw err;
    console.log('getNodes response', res)
    // [ { ID: '4a0b4ec3-0ab4-e50e-7b24-caeae5169b65',
    // Node: 'Gregs-MacBook-Pro.local',
    // Address: '127.0.0.1',
    // Datacenter: 'dc1',
    // TaggedAddresses: { lan: '127.0.0.1', wan: '127.0.0.1' },
    // NodeMeta: { 'consul-network-segment': '' },
    // ServiceKind: '',
    // ServiceID: '036901b4-8492-4eb8-9d5f-92644ffc1140',
    // ServiceName: 'beta',
    // ServiceTags: [ 'beta', 'v1.0.0' ],
    // ServiceAddress: 'localhost',
    // ServiceWeights: { Passing: 1, Warning: 1 },
    // ServiceMeta: {},
    // ServicePort: 3000,
    // ServiceEnableTagOverride: false,
    // ServiceProxyDestination: '',
    // ServiceConnect: { Native: false, Proxy: null },
    // CreateIndex: 95,
    // ModifyIndex: 108 } ]
  });
}

getServices()
setInterval(getNodes, 3*1000)
