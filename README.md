# WWCP OICP

This software will allow the communication between World Wide Charging Protocol
(WWCP) entities and entities implementing the [Open InterCharge Protocol (OICP)](https://github.com/hubject/oicp/),
which is defined and used by [Hubject GmbH](http://www.hubject.com). The focus
of this protocol are the communication aspects between a central clearing house,
charge point operators (CPOs) and e-mobility providers (EMPs) in electric mobility.


### Scope

This implementation aims to become one of the OICP reference implementations. Therefore you will find a lot of inline documentation, many test cases and mockups. Beside the obvious implementation of the [Charge Point Operator (CPO)](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/CPO) and [E-Mobility Provider (EMP)](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/EMP) functionalities this software also provides basic functionality of the [Central Service](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/CentralService) e.g. for your automated continous integration tests.


### Versions

- [OICP v2.3 .NET7](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3) is fully maintained and should be used for new deployments


### Requirements & Configuration

#### OICP Classic

1. You will need .NET7+
2. Tested and running in production since 2014 on *Debian GNU/Linux* servers
3. The mutual authentication (TLS client certificates) with Hubject must be set up.
4. Your server(s) must be registered within the Hubject firewalls.

You can of course skip *3.* and *4.* when you use this software for your (internal) testing purposes only. 

#### OICP Peer-to-Peer

OICO P2P is a reference implementation of the Open InterCharge Protocol reassembled to support peer-to-peer
operation instead of using a central EV roaming hub. This software is experimental, but should be as stable
as the normal protocol implementation.

1. You will need .NET7+
2. Tested and running in production since 2022 on *Debian GNU/Linux* servers
3. You can use OICP P2P with or without security. You're the admin ;)


### Documentation

- Overview on the [OICP v2.3 implementation](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3)


### Tools & Extensions

- [OICP Tool](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/OICPTool) is a small tool using this OICP library allowing you to test requests and responses easily on the command line. It can also be used within (bash/zsh/...) scripts for automated testing or automated processes like e.g. downloading new *Charge Detail Records* every night.


### Your participation

This software is **Free Open Source** under the [Apache 2.0 license](https://github.com/OpenChargingCloud/WWCP_OICP/blob/master/LICENSE). We appreciate your participation in this ongoing project, and your help to improve it and the e-mobility ICT in general. If you find bugs, want to request a feature or send us a pull request, feel free to use the normal GitHub features to do so. For this please read the [Contributor License Agreement](https://github.com/OpenChargingCloud/WWCP_OICP/blob/master/Contributor%20License%20Agreement.txt) carefully and send us a signed copy or use a similar free and open license.
