'use strict'

const kafka = require('kafka-node');

//start up the consumer
require('./consumer')

const Producer = kafka.Producer;
const KeyedMessage = kafka.KeyedMessage;

const KAFKA_HOST = "localhost"

const client = new kafka.KafkaClient({kafkaHost: `${KAFKA_HOST}:9092`});

const topic = "test"
const partition = 0
const attributes = 0

var topics = [{ topic: topic, partition: partition }];
var options = { autoCommit: false, fetchMaxWaitMs: 1000, fetchMaxBytes: 1024 * 1024 };

var consumer = new Consumer(client, topics, options);
var offset = new Offset(client);

consumer.on('message', function (message) {
  console.log('Kafka Consumer - Message Recieved', message)
});

consumer.on('error', function (err) {
  console.log('Kafka Consumer - Error Recieved', err)
});
