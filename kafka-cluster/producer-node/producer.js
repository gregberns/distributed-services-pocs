'use strict'

const kafka = require('kafka-node');

//start up the consumer
require('./consumer')

const Producer = kafka.Producer;
const KeyedMessage = kafka.KeyedMessage;

const KAFKA_HOST = "kafka"

const client = new kafka.KafkaClient({kafkaHost: `${KAFKA_HOST}:9092`});
var producer = new Producer(client, { requireAcks: 1 });

const topic = "test"
const partition = 0
const attributes = 0



//ready: this event is emitted when producer is ready to send messages.
console.log('Kafka Producer - Connect')
producer.on('ready', function () {
  console.log('Kafka Producer - Ready')
  var message = 'a message' + (new Date()).toString();
  var keyedMessage = new KeyedMessage('keyed', 'a keyed message');

  producer.send([
    { topic: topic, 
      partition: partition, 
      messages: [
        message,
        keyedMessage
      ], 
      attributes: attributes }
  ], function (err, result) {
    console.log('Kafka Producer - Message Sent', err || result)
    //process.exit();
  });
});

//error: this is the error event propagates from internal client, producer should always listen it.
producer.on('error', function (err) {
  console.log('error', err);
});