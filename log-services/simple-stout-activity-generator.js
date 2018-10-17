'use strict';

const os = require('os')

function sendStdOutMsg() {
  console.log(`Message Generated at ${new Date().toISOString()}`)
}

function sendStdErrMsg() {
  console.error(`Error Message Generated at ${new Date().toISOString()}`, {date: new Date(), host: os.hostname()})
}

setInterval(sendStdOutMsg, 5*1000);
setInterval(sendStdErrMsg, 15*1000);


console.log('Service Started: simple-stdout-activity-generator')
sendStdOutMsg()
sendStdErrMsg()
