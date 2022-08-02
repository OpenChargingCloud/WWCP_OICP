# OICP Central Service

This is the implementation of the OICP **Central Service** functionalities.

You can e.g. use this for creating automatic continous integration tests of your own software.

### CPO Client API Feature Support

When you act as Hubject and receive CPO Client requests.

| Feature                          | Implementation Status    | Test Status                        |
| -------------------------------- | ------------------------ | ---------------------------------- |
| PushEVSEData                     | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushEVSEStatus                   | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushPricingProductData           | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushEVSEPricing                  | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullAuthenticationData           | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStart                   | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStop                    | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingStartNotification    | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingProgressNotification | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingEndNotification      | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargingErrorNotification    | :heavy_check_mark: ready | :broken_heart: more tests required |
| SendChargeDetailRecord           | :heavy_check_mark: ready | :broken_heart: more tests required |


### CPO Server API Client Feature Support

When you act as Hubject and send requests to an CPO.

| Feature                         | Implementation Status    | Test Status                        |
| ------------------------------- | ------------------------ | ---------------------------------- |
| AuthorizeRemoteReservationStart | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteReservationStop  | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteStart            | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeRemoteStop             | :heavy_check_mark: ready | :broken_heart: more tests required |


### EMP Client API Feature Support

When you act as Hubject and receive EMP Client requests.

| Feature                    | Implementation Status    | Test Status                        |
| -------------------------- | ------------------------ | ---------------------------------- |
| PullEVSEData               | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullEVSEStatus             | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullEVSEStatusById         | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullEVSEStatusByOperatorId | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullPricingProductData     | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullEVSEPricing            | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushAuthenticationData     | :heavy_check_mark: ready | :broken_heart: more tests required |
| RemoteReservationStart     | :heavy_check_mark: ready | :broken_heart: more tests required |
| RemoteReservationStop      | :heavy_check_mark: ready | :broken_heart: more tests required |
| RemoteStart                | :heavy_check_mark: ready | :broken_heart: more tests required |
| RemoteStop                 | :heavy_check_mark: ready | :broken_heart: more tests required |
| GetChargeDetailRecords     | :heavy_check_mark: ready | :broken_heart: more tests required |


### EMP Server API Client Feature Support

When you act as Hubject and send requests to an EMP.

| Feature                       | Implementation Status    | Test Status                        |
| ----------------------------- | ------------------------ | ---------------------------------- |
| AuthorizeStart                | :heavy_check_mark: ready | :broken_heart: more tests required |
| AuthorizeStop                 | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingStartNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingProgressNotification  | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingEndNotification       | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingErrorNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargeDetailRecord            | :heavy_check_mark: ready | :broken_heart: more tests required |

