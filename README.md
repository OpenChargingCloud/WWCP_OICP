WWCP OICP
=========

This software will allow the communication between World Wide Charging Protocol
(WWCP) entities and entities implementing the _Open InterCharge Protocol (OICP)_,
which is defined and used by [Hubject GmbH](http://www.hubject.com). The focus
of this protocol are the communication aspects between a central clearing house,
charge point operators (CPOs) and e-mobility providers (EMPs). For more details
on this protocol please visit http://www.intercharge.eu.

### Requirements & Configuration

1. You must be a client of Hubject GmbH.
2. You need .NET 4.7.2+
3. The mutual authentication (TLS client certificates) with Hubject works best using stunnel/openssl (but a native .NET version is coming soon...)
4. Your server must be registered within the Hubject firewalls

### Your participation

This software is Open Source under the Apache 2.0 license. We appreciate
your participation in this ongoing project, and your help to improve it
and the e-mobility ICT in general. If you find bugs, want to request a
feature or send us a pull request, feel free to use the normal GitHub
features to do so. For this please read the Contributor License Agreement
carefully and send us a signed copy or use a similar free and open license.
