## Charge Point Operator Methods

##### Setting up a CPO client...
```csharp
CPOClient HubjectCPO  = new CPOClient("api.playground.hubject.com");
```

##### Sending static EVSE data...
```csharp
var EVSEDataRecords  = Enumeration.Create(

    new EVSEDataRecord(
        EVSEId:               EVSE_Id.           Parse("DE*GEF*E123456789*1"),
        ChargingStationId:    ChargingStation_Id.Parse("DE*GEF*S123456789"),
        ChargingStationName:  I18NString.        Create(Languages.de, "Testbox 1").
                                                    Add(Languages.en, "Testbox One"),

        Address:              Address.Create(
                                  Country.Germany,
                                  "07749", "Jena",
                                  "Biberweg", "18"
                              ),

        GeoCoordinate:        GeoCoordinate.Create(
                                  Latitude. Parse(49.731102),
                                  Longitude.Parse(10.142530)
                              ),

        Plugs:                Enumeration.Create(
                                  PlugTypes.TypeFSchuko,
                                  PlugTypes.Type2Outlet
                              ),

        AuthenticationModes:  Enumeration.Create(
                                  AuthenticationModes.NFC_RFID_Classic,
                                  AuthenticationModes.NFC_RFID_DESFire,
                                  AuthenticationModes.REMOTE
                              ),

        PaymentOptions:       Enumeration.Create(
                                  PaymentOptions.Contract,
                                  PaymentOptions.Direct
                              ),

        Accessibility:        AccessibilityTypes.Paying_publicly_accessible,

        HotlinePhoneNumber:   "+4955512345678",

        IsHubjectCompatible:  true,
        DynamicInfoAvailable: true,
        IsOpen24Hours:        true
    )

);


var req = HubjectCPO.

              PushEVSEData(EVSEDataRecords,
                           ActionType.insert,
                           IncludeEVSEs: evse => evse.EVSEId.ToString().StartsWith("DE")).

              ContinueWith(task => {

                  var Acknowledgement = task.Result.Content;

                  if (!Acknowledgement.Result)
                  {
                      Console.WriteLine(task.Result.Content.Code);
                      Console.WriteLine(task.Result.Content.Description);
                      Console.WriteLine(task.Result.Content.AdditionalInfo);
                  }

              });

req.Wait();              
```

##### Sending dynamic EVSE status updates...
```csharp
var EVSEStatus = new Dictionary<EVSE_Id, OICPEVSEStatus>();
EVSEStatus.Add(EVSE_Id.Parse("DE*GEF*E123456789*1"), OICPEVSEStatus.Available);
EVSEStatus.Add(EVSE_Id.Parse("DE*GEF*E123456789*2"), OICPEVSEStatus.Occupied);

var req = new CPOClient("api.playground.hubject.com").

              PushEVSEStatus(EVSEStatus,
                             ActionType.insert,
                             OperatorId:    EVSEOperator_Id.Parse("DE*GEF"),
                             OperatorName:  "Test CPO 1",
                             QueryTimeout:  TimeSpan.FromSeconds(120))

              ContinueWith(task => {

                  var Acknowledgement = task.Result.Content;

                  if (Acknowledgement.Result)
                      Console.WriteLine("success!");

                  else
                  {
                      Console.WriteLine(task.Result.Content.Code);
                      Console.WriteLine(task.Result.Content.Description);
                      Console.WriteLine(task.Result.Content.AdditionalInfo);
                  }

              });
```
