# OICP CPO

This is the implementation of the OICP **Charge Point Operator (CPO)** functionalities.

## Normal CPO Operation

### CPO Client Feature Support

When you connect as an CPO to Hubject.

| Feature                           | Implementation Status    | Test Status                        |
| --------------------------------- | ------------------------ | ---------------------------------- |
| PushEVSEData                      | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PushEVSEStatus                    | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PushPricingProductData            | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushEVSEPricing                   | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullAuthenticationData (obsolete) | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStart                    | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| AuthorizeStop                     | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| SendChargingStartNotification     | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| SendChargingProgressNotification  | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| SendChargingEndNotification       | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| SendChargingErrorNotification     | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| SendChargeDetailRecord            | :heavy_check_mark: ready | :heavy_check_mark: passing         |


### CPO Server API Feature Support

When you as an CPO receive requests from Hubject.

| Feature                         | Implementation Status    | Test Status                        |
| ------------------------------- | ------------------------ | ---------------------------------- |
| AuthorizeRemoteReservationStart | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| AuthorizeRemoteReservationStop  | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| AuthorizeRemoteStart            | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| AuthorizeRemoteStop             | :heavy_check_mark: ready | :heavy_check_mark: passing         |




## Reverse Operation

### CPO Client API Feature Support

When you act as Hubject and receive CPO Client requests.

| Feature                           | Implementation Status    | Test Status                        |
| --------------------------------- | ------------------------ | ---------------------------------- |
| PushEVSEData                      | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushEVSEStatus                    | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushPricingProductData            | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushEVSEPricing                   | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullAuthenticationData (obsolete) | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStart                    | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStop                     | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingStartNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingProgressNotification  | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingEndNotification       | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingErrorNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargeDetailRecord            | :heavy_check_mark: ready | :broken_heart: more tests required |


### CPO Server API Client Feature Support

When you act as Hubject and send requests to an CPO.

| Feature                         | Implementation Status    | Test Status                        |
| ------------------------------- | ------------------------ | ---------------------------------- |
| AuthorizeRemoteReservationStart | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteReservationStop  | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteStart            | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteStop             | :heavy_check_mark: ready | :broken_heart: more tests required |

