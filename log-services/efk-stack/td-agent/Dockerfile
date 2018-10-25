FROM ubuntu:16.04
MAINTAINER Your Name <your@email.com>

RUN apt-get update &&
    apt-get upgrade -y &&
    curl -L https://toolbelt.treasuredata.com/sh/install-ubuntu-bionic-td-agent3.sh | sh &&
    systemctl start td-agent.service


#     docker run -p 24224:24224 -p 24224:24224/udp -v /data/fluentd:/fluentd/log 
#     -v /home/sandbox/gberns/efk-stack/fluentd/conf:/fluentd/etc 
#     -e FLUENTD_CONF=fluent.conf fluentd1

# /etc/td-agent/td-agent.conf

# -v /home/sandbox/gberns/efk-stack/td-agent/conf:/etc/td-agent

#It’s HIGHLY recommended that you set up ntpd on the node to prevent invalid timestamps in your logs. Please check the Preinstallation Guide.