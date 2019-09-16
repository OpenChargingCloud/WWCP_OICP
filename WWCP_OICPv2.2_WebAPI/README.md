WWCP OICP WebAPI v2.1
=====================

This software will allow the communication between WWCP entities and
entities implementing the _Open InterCharge Protocol (OICP)_, which is
defined and used by Hubject GmbH. The focus of this protocol are the
communication aspects of a central clearing house. For more details on
this protocol please visit http://www.intercharge.eu.

The OICP WebAPI helps clients of Hubject to share OICP data sets also
via plain HTTP, instead of HTTP/SOAP. In contrast to the Hubject APIs
the WebAPI allows you to extend the XML with additional data sets
which are currently not available in the Open InterCharge Protocol
and can not be shared via Hubject because of their inbound (X)ML
schema validation.

### Usage

```csharp

var OICPWebAPI = new OICPWebAPI(HTTPServer:            <A HTTPServer>,
                                URIPrefix:             "/ext/OICPPlus",
                                HTTPRealm:             "OICP+ WebAPI",
                                HTTPLogins:            Enumeration.Create(
                                                           new KeyValuePair<String, String>("hello", "world")
                                                       ),
                                XMLNamespaces:         <Additional XML namespaces>,
                                EVSE2EVSEDataRecord:   <An optional delegate to transform WWCP EVSEs into OICP EVSE data records>,
                                EVSEDataRecord2XML:    <An optional delegate to serialize OICP EVSE data record into XML>,
                                EVSEStatusRecord2XML:  <An optional delegate to serialize OICP EVSE status record into XML>,);

```

### Your participation

This software is Open Source under the Apache 2.0 license. We appreciate
your participation in this ongoing project, and your help to improve it
and the e-mobility ICT in general. If you find bugs, want to request a
feature or send us a pull request, feel free to use the normal GitHub
features to do so. For this please read the Contributor License Agreement
carefully and send us a signed copy or use a similar free and open license.
