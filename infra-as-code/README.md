# Infrastructure As Code

## Ansible


### Setting up Inventory (Hosts))

In `/etc/ansible/hosts`, is something like:

```
mail.example.com

[webservers]
foo.example.com
bar.example.com

[dbservers]
one.example.com
two.example.com
three.example.com
```

To have an alias for a server/IP:

```
jumper ansible_port=5555 ansible_host=192.0.2.50
```

To add several hosts following a similar pattern:

```
[webservers]
www[01:50].example.com
# Or
docker[01:06]
```

#### Adding Variables to a Group

```
[atlanta:vars]
ntp_server=ntp.atlanta.example.com
proxy=proxy.atlanta.example.com
```

#### Adding Children

```
[atlanta]
host1
host2

[raleigh]
host2
host3

[southeast:children]
atlanta
raleigh

[southeast:vars]
some_server=foo.southeast.example.com
halon_system_timeout=30

[usa:children]
southeast
northeast
```