# WWCP OICP

This software will allow the communication between World Wide Charging Protocol
(WWCP) entities and entities implementing the [Open InterCharge Protocol (OICP)](https://github.com/hubject/oicp/),
which is defined and used by [Hubject GmbH](http://www.hubject.com). The focus
of this protocol are the communication aspects between a central clearing house,
charge point operators (CPOs) and e-mobility providers (EMPs) in electric mobility.

### Scope

This implementation aims to become one of the OICP reference implementations. Therefore you will find a lot of inline documentation, many test cases and mockups. Beside the obvious implementation of the [Charge Point Operator (CPO)](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/CPO) and [E-Mobility Provider (EMP)](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/EMP) functionalities this software also provides basic functionality of the [Central Service](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3/CentralService) e.g. for your automated continous integration tests.

### Requirements & Configuration

1. You need .NET6+
2. The mutual authentication (TLS client certificates) with Hubject must be set up.
3. Your server(s) must be registered within the Hubject firewalls.

You can of course skip *2.* and *3.* when you use this software for your (internal) testing purposes only. 

### Versions

- [OICP v2.2](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/dotNetFramework/WWCP_OICPv2.2) is deprecated and only left for reference
- [OICP v2.3](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/dotNetFramework/WWCP_OICPv2.3) is deprecated and only left for reference
- [OICP v2.3 NET6](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3) is fully maintained and should be used for new deployments

### Documentation

- Overview on the [OICP v2.3 implementation](https://github.com/OpenChargingCloud/WWCP_OICP/tree/master/WWCP_OICPv2.3)

### Your participation

This software is **Open Source** under the [Apache 2.0 license](https://github.com/OpenChargingCloud/WWCP_OICP/blob/master/LICENSE).
We appreciate your participation in this ongoing project, and your help to
improve it and the e-mobility ICT in general. If you find bugs, want to request
a feature or send us a pull request, feel free to use the normal GitHub
features to do so. For this please read the Contributor License Agreement
carefully and send us a signed copy or use a similar free and open license.
