@echo off
set TOPICS=TEST
set GROUPID=test-entity-producer-consumers
set CLIENTID=test-entity-producer-consumer

set PATHTOMONITOR=C:\temp\MessageSource\Messages
set ARCHIVEFOLDERPATH=C:\temp\MessageSource\Archive
set ERRORFOLDERPATH=C:\temp\MessageSource\Errors

set BOOTSTRAPSERVERS=localhost:9092

start C:\TFS\Sprint19\Services\ClientDataProducer\bin\Debug