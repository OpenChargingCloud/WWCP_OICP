WWCP OICP
=========

This software will allow the communication between World Wide Charging Protocol (WWCP) entities and entities implementing the _Open InterCharge Protocol (OICP)_, which is defined and used by [Hubject GmbH](http://www.hubject.com). The focus of this protocol are the communication aspects between a central clearing house, charge point operators (CPOs) and e-mobility providers (EMPs). For more details on this protocol please visit http://www.intercharge.eu.

This software is developed by [GraphDefined GmbH](http://www.graphdefined.com). We appreciate your participation in this ongoing project, and your help to improve it. If you find bugs, want to request a feature or send us a pull request, feel free to use the normal GitHub features to do so. For this please read the [Contributor License Agreement](Contributor%20License%20Agreement.txt) carefully and send us a signed copy.

### Requirements & Configuration

1. You need .NET 4.6+
2. The mutual authentication (TLS client certificates) with Hubject works best using stunnel/openssl (but a native .NET version is coming soon...)
3. Your server must be registered within the Hubject firewalls
