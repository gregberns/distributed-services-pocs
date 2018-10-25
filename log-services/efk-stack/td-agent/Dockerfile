FROM ubuntu:16.04
MAINTAINER Your Name <your@email.com>

RUN apt-get update &&
    apt-get upgrade -y &&
    curl -L https://toolbelt.treasuredata.com/sh/install-ubuntu-bionic-td-agent3.sh | sh &&
    systemctl start td-agent.service


