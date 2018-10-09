resource "virtualbox" "node" {
    count = 2
    name = "${format("node-%02d", count.index+1)}"

    image = "../../../VM_images/ubuntu-16.04.5-desktop-amd64.iso"
    cpus = 2
    memory = "1024mib"

    network_adapter {
        type = "nat"
    }

    network_adapter {
        type = "bridged"
        host_interface = "en0"
    }

    # optical_disks = ["../../../VM_images/ubuntu-16.04.5-desktop-amd64.iso"]
}

output "IPAddr" {
    # Get the IPv4 address of the bridged adapter (the 2nd one) on 'node-02'
    value = "${element(virtualbox.node.*.network_adapter.1.ipv4_address, 1)}"
}