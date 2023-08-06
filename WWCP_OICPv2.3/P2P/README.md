# OICP Peer-2-Peer Services

This software implements the _Open InterCharge Protocol (OICP)_, which is defined and used by Hubject GmbH, but instead of sending all data through a central clearing house this implementation relies on **Peer-2-Peer** communication between charge point operators (CPOs) and e-mobility providers (EMPs) in electric mobility.


## Benefits of p2p

There are many benefits of having a direct communication between CPOs and EMPs:

- The evolution of OICP in the last 10 years had been very slow and OICP had always been a bottleneck of reliability and innovation. You can now be responsible for your own speed or slowness of innovation.
- Exchange of EVSE data and status information is far to slow via the traditional OICP. What you really want is to push all EVSE data and status updates directly to all EMPs and not waiting until an EMP will pull them from the central clearing house.
- OICP is broken when it comes to timeout handling of remote starts. It could happen, that there is a running charging session at an EVSE, but the central clearing house assumes, that there was a timeout of the remote start request. Because of this the session identification is unkown at the central clearing house and thus the CPO will no longer be able to send a charge detail record (CDR) to the EMP.
- Until now every OICP implementation works a little bit different and there is no real open source reference implementation for verifying your implementation. Because of this misunderstandings can happen easily. In a p2p system on the other hand you could adapt to a different behaviour of a peer easily.
- There is no need having *either p2p... or the traditional communication*. You can still use the central clearing house as one of your peers for some requests or for some communication partners. It is working together nicely.


## Drawbacks of p2p

There are also some drawbacks of having a direct communication between CPOs and EMPs:

- As already mentioned every OICP implementation is a little bit different because of a missing open source reference implementation. So it is often helpful, that you can at least argue, that the request, response, CDR, ... is at least available at the central clearing house and thus it is not your fault, that some process did not work ;)
- Reaching all your peers, checking firewall settings, adapt to changes is now your own task. Maybe you love to do this, because you have a great IT department, but do not underestimate the amount of additional work to maintain a p2p network.


## Setup of your first p2p network

There are a lot of [unit tests](https://github.com/OpenChargingCloud/WWCP_OICP/blob/master/WWCP_OICPv2.3_Tests/P2P/AP2PTests.cs) for p2p communication which will help you to understand what you can do with it.

First you have to create a CPO and an EMP peer...

```csharp
var cpoP2P_DEGEF = new CPOPeer(
                       HTTPServerPort:  IPPort.Parse(7001),
                       LoggingPath:    "DEGEF",
                       AutoStart:       true
                   );                   
                   
var empP2P_DEGDF = new EMPPeer(
                       HTTPServerPort:  IPPort.Parse(8001),
                       LoggingPath:    "DEGDF",
                       AutoStart:       true
                   );
```

...then for every peer you can register its remote peer.
```csharp
cpoP2P_DEGEF.RegisterProvider(Provider_Id.Parse("DE-GDF"),
                              new CPOClient(
                                  URL.Parse("http://127.0.0.1:8001"),
                                  RequestTimeout: TimeSpan.FromSeconds(10)
                              ));

empP2P_DEGDF.RegisterOperator(Operator_Id.Parse("DE*GEF"),
                              new EMPClient(
                                  URL.Parse("http://127.0.0.1:7001"),
                                  RequestTimeout: TimeSpan.FromSeconds(10)
                              ));                   
```

That's it! Everything else will be just the same except, that for most requests you have to add the *providerId* or *operatorId* of the remote peer. Some requests, e.g. the remote start request already comes with the required *operatorId* and thus it is not required again.
