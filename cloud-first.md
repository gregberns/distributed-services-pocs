# Cloud First

## Dream

Docker > K8s > Service Mesh

* Contanerize applications
* Push individual service changes into Prod


But...

We can barely do Docker... wait nope, can't even do Docker.

Can we even do .Net Core?

## Why the Dream

Lean Mfg says reducing cycle time 

* Improves time to market
* Reduced cost
* 


### Current Strategy Issues

Current strategy requires:

* all code to be deployed at once
* one monolithic repository


Prevents:

* Small, single service code deploys

Results in:

* Being bound to 'monolithic' 1-month process
* Large change sets
* High risk changes


### Proposed Strategy 

Primary 'ship' continues on at 1 month cycle times

Deploy a single 'speed boat' to move at 2-3 day cycle times ('commit to master' to 'in prod')

## Tactics

### Initial

Create POC service to validate initial system works

Initial service simply tracks server CPU, memory, etc.

#### Purpose

* Validate firewall rules, security, traffic allocation are in place
* Validate deployment, updating, rollback works
* Validate logging, metrics, alerting works

### Secondary

Find **most volitile** and **business value** parts of code.

[Find the most changed files in TFS](https://stackoverflow.com/questions/36287349/how-to-find-top-10-files-changed-most-frequently-in-a-period-from-tfs-2015-versi)



**Target:** 

* Most volitile parts of .NET code
* Segment (separate) services into self deployable units
* Add **automated testing:** unit, integration, system test
* Add **monitoring:** logging, metrics, alerting
* Allow deploys to occur **anytime**

### Benefits

* Get changes into production very quickly
* Work faster thatn current process
* Proactively identify issues
* Fix issues quickly
* Forces team to quickly identify and improve wasteful activities


## Team

Must include: Dev, DevOps, ITOps

Without each, this will fail.

From current experience, without ITOps integration, change in PROD will **not** happen.

**Proposal:** Integrate one or two ITOps engineers into Black Mesa.


[Cloud First Trailmap](https://raw.githubusercontent.com/cncf/landscape/master/trail_map/CNCF_TrailMap_latest.png)

[Popular Docker Containers/Technology](https://www.datadoghq.com/docker-adoption/#6)

[Docker for windows](https://glennsarti.github.io/blog/getting-started-with-windows-containers/)

## Parts of the Process

Install

Start

Run
Priorities
Monitor, log, metrics

Update

Rollback

Uninstall