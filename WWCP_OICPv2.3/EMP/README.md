# OICP EMP

This is the implementation of the OICP **E-Mobility Provider (EMP)** functionalities.

## Normal EMP Operation

### EMP Client Feature Support

When you connect as an EMP to Hubject.

| Feature                    | Implementation Status    | Test Status                        |
| -------------------------- | ------------------------ | ---------------------------------- |
| PullEVSEData               | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PullEVSEStatus             | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PullEVSEStatusById         | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PullEVSEStatusByOperatorId | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| PullPricingProductData     | :heavy_check_mark: ready | :broken_heart: more tests required |
| PullEVSEPricing            | :heavy_check_mark: ready | :broken_heart: more tests required |
| PushAuthenticationData     | :heavy_check_mark: ready | :broken_heart: more tests required |
| RemoteReservationStart     | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| RemoteReservationStop      | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| RemoteStart                | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| RemoteStop                 | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| GetChargeDetailRecords     | :heavy_check_mark: ready | :heavy_check_mark: passing         |


### EMP Server API Feature Support

When you as an EMP receive requests from Hubject.

| Feature                       | Implementation Status    | Test Status                        |
| ----------------------------- | ------------------------ | ---------------------------------- |
| AuthorizeStart                | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| AuthorizeStop                 | :heavy_check_mark: ready | :heavy_check_mark: passing         |
| ChargingStartNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingProgressNotification  | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingEndNotification       | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargingErrorNotification     | :heavy_check_mark: ready | :broken_heart: more tests required |
| ChargeDetailRecord            | :heavy_check_mark: ready | :heavy_check_mark: passing         |




## Reverse Operation

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

