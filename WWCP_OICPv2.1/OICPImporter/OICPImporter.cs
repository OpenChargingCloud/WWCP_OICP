/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A service to import EVSE data (from Hubject) via OICP v2.0 into WWCP data structures.
    /// </summary>
    public class OICPImporter : OICPImporter<String>
    {

        #region Data

        private readonly RoamingNetwork _RoamingNetwork;

        #endregion

        #region Constructor(s)

        public OICPImporter(String                                                    Identification,
                            RoamingNetwork                                            RoamingNetwork,
                            String                                                    Hostname,
                            IPPort                                                    TCPPort,
                            String                                                    HTTPVirtualHost,
                            eMobilityProvider_Id                                      ProviderId,
                            DNSClient                                                 DNSClient                      = null,

                            TimeSpan?                                                 UpdateEVSEDataEvery            = null,
                            TimeSpan?                                                 UpdateEVSEDataTimeout          = null,
                            TimeSpan?                                                 UpdateEVSEStatusEvery          = null,
                            TimeSpan?                                                 UpdateEVSEStatusTimeout        = null,

                            Func<UInt64, StreamReader>                                LoadStaticDataFromStream       = null,
                            Func<UInt64, StreamReader>                                LoadDynamicDataFromStream      = null,

                            Action<String, DateTime, IEnumerable<XElement>>           EVSEDataXMLHandler             = null,
                            Action<String, DateTime, IEnumerable<OperatorEVSEData>>   EVSEDataHandler                = null,

                            Func  <String, DateTime, IEnumerable<EVSE_Id>>            GetEVSEIdsForStatusUpdate      = null,
                            Action<String, DateTime, XElement>                        EVSEStatusXMLHandler           = null,
                            Action<String, DateTime, EVSE_Id, EVSEStatusTypes>        EVSEStatusHandler              = null,

                            Func<EVSEStatusReport, ChargingStationStatusType>         EVSEStatusAggregationDelegate  = null

            )

            : base(Identification:            Identification,
                   Hostname:                  Hostname,
                   TCPPort:                   TCPPort,
                   HTTPVirtualHost:           HTTPVirtualHost,
                   ProviderId:                ProviderId,
                   DNSClient:                 DNSClient,

                   // Remove both lines for normal behaviour!
                   //LoadStaticDataFromStream:  Counter => File.OpenText("HubjectQATestdata-Static"  + Counter + ".xml"),
                   //LoadDynamicDataFromStream: Counter => File.OpenText("HubjectQATestdata-Dynamic" + Counter + ".xml"),

                   UpdateEVSEDataEvery:       UpdateEVSEDataEvery,
                   UpdateEVSEStatusEvery:     UpdateEVSEStatusEvery,

                   //UpdateContextCreator:      ()               => "", //MySQLExtentions.CreateAndOpenConnection (DatabaseAccessString),
                   //StartBulkUpdate:           (UpdateContext)  => {}, //EVSEDatabase.   StartBulkUpdate         (UpdateContext),
                   //StopBulkUpdate:            (UpdateContext)  => {}, //EVSEDatabase.   StopBulkUpdate          (UpdateContext),
                   //UpdateContextDisposer:     (UpdateContext)  => {}, //MySQLExtentions.CommitAndCloseConnection(UpdateContext),

                   #region EVSEOperatorDataHandler

                   EVSEDataHandler:           (UpdateContext, Timestamp, OperatorEvseData) => {

                                                  #region Data

                                                  ChargingStationOperator     _EVSEOperator         = null;
                                                  ChargingPool     _ChargingPool         = null;
                                                  ChargingStation  _ChargingStation      = null;
                                                  EVSE             _EVSE                 = null;

                                                  EVSEInfo         EVSEInfo              = null;
                                                  CPInfoList       _CPInfoList           = null;
                                                  ChargingPool_Id  PoolId                = null;
                                                  EVSEIdLookup     EVSEIdLookup          = null;

                                                  #endregion

                                                  OperatorEvseData.ForEach(_OperatorEvseData => {

                                                  try
                                                  {

                                                      #region Find a matching EVSE Operator and maybe update its properties... or create a new one!

                                                      if (!RoamingNetwork.TryGetChargingStationOperatorById(_OperatorEvseData.OperatorId, out _EVSEOperator))
                                                          _EVSEOperator = RoamingNetwork.CreateNewChargingStationOperator(_OperatorEvseData.OperatorId, I18NString.Create(Languages.unknown, _OperatorEvseData.OperatorName));

                                                      else
                                                      {

                                                          // Update via events!
                                                          _EVSEOperator.Name = I18NString.Create(Languages.unknown, _OperatorEvseData.OperatorName);

                                                      }

                                                      #endregion

                                                      #region Generate a list of all charging pools/stations/EVSEs

                                                      _CPInfoList = new CPInfoList(_OperatorEvseData.OperatorId);

                                                      foreach (var EvseDataRecord in _OperatorEvseData.EVSEDataRecords)
                                                      {

                                                          PoolId  = ChargingPool_Id.Generate(EvseDataRecord.Id.OperatorId,
                                                                                             EvseDataRecord.Address,
                                                                                             EvseDataRecord.GeoCoordinate);

                                                          _CPInfoList.AddOrUpdateCPInfo(PoolId,
                                                                                        EvseDataRecord.Address,
                                                                                        EvseDataRecord.GeoCoordinate,
                                                                                        EvseDataRecord.ChargingStationId, // Is a string in OICP v2.0!
                                                                                        EvseDataRecord.Id);

                                                      }

                                                      #endregion

                                                      EVSEIdLookup = _CPInfoList.AnalyseAndGenerateLookUp();

                                                      #region Data

                                                      Languages                         LocationLanguage               = Languages.unknown;
                                                      Languages                         LocalChargingStationLanguage   = Languages.unknown;

                                                      #endregion

                                                      foreach (var EvseDataRecord in _OperatorEvseData.EVSEDataRecords)
                                                      {

                                                          try
                                                          {

                                                              EVSEInfo = EVSEIdLookup[EvseDataRecord.Id];

                                                              #region Set derived WWCP properties

                                                              #region Set LocationLanguage

                                                              switch (EVSEInfo.PoolAddress.Country.Alpha2Code.ToLower())
                                                              {

                                                                  case "de": LocationLanguage = Languages.de; break;
                                                                  case "fr": LocationLanguage = Languages.fr; break;
                                                                  case "dk": LocationLanguage = Languages.dk; break;
                                                                  case "no": LocationLanguage = Languages.no; break;
                                                                  case "fi": LocationLanguage = Languages.fi; break;
                                                                  case "se": LocationLanguage = Languages.se; break;

                                                                  case "sk": LocationLanguage = Languages.sk; break;
                                                                  //case "be": LocationLanguage = Languages.; break;
                                                                  case "us": LocationLanguage = Languages.en; break;
                                                                  case "nl": LocationLanguage = Languages.nl; break;
                                                                  //case "fo": LocationLanguage = Languages.; break;
                                                                  case "at": LocationLanguage = Languages.de; break;
                                                                  case "ru": LocationLanguage = Languages.ru; break;
                                                                  //case "ch": LocationLanguage = Languages.; break;

                                                                  default:   LocationLanguage = Languages.unknown; break;

                                                              }

                                                              if (EVSEInfo.PoolAddress.Country == Country.Germany)
                                                                  LocalChargingStationLanguage = Languages.de;

                                                              else if (EVSEInfo.PoolAddress.Country == Country.Denmark)
                                                                  LocalChargingStationLanguage = Languages.dk;

                                                              else if (EVSEInfo.PoolAddress.Country == Country.France)
                                                                  LocalChargingStationLanguage = Languages.fr;

                                                              else
                                                                  LocalChargingStationLanguage = Languages.unknown;

                                                              #endregion

                                                              #region Guess the language of the 'ChargingStationName' by '_Address.Country'

                                                              //_ChargingStationName = new I18NString();

                                                              //if (LocalChargingStationName.IsNotNullOrEmpty())
                                                              //    _ChargingStationName.Add(LocalChargingStationLanguage,
                                                              //                             LocalChargingStationName);

                                                              //if (EnChargingStationName.IsNotNullOrEmpty())
                                                              //    _ChargingStationName.Add(Languages.en,
                                                              //                             EnChargingStationName);

                                                              #endregion

                                                              #endregion

                                                              #region Update matching charging pool... or create a new one!

                                                                                      if (_EVSEOperator.TryGetChargingPoolbyId(EVSEInfo.PoolId, out _ChargingPool))
                                                                                      {

                                                                                          // External update via events!
                                                                                          _ChargingPool.Description           = EvseDataRecord.AdditionalInfo;
                                                                                          _ChargingPool.LocationLanguage      = LocationLanguage;
                                                                                          _ChargingPool.EntranceLocation      = EvseDataRecord.GeoChargingPointEntrance;
                                                                                          _ChargingPool.OpeningTimes          = EvseDataRecord.OpeningTime;
                                                                                          _ChargingPool.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(EvseDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                          _ChargingPool.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (EvseDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                          _ChargingPool.Accessibility         = EvseDataRecord.Accessibility;
                                                                                          _ChargingPool.HotlinePhoneNumber    = EvseDataRecord.HotlinePhoneNumber;

                                                                                      }

                                                                                      else
                                                                                          _ChargingPool = _EVSEOperator.CreateNewChargingPool(

                                                                                                                            EVSEInfo.PoolId,

                                                                                                                            Configurator: pool => {

                                                                                                                                pool.Description                 = EvseDataRecord.AdditionalInfo;
                                                                                                                                pool.Address                     = EvseDataRecord.Address;
                                                                                                                                pool.GeoLocation                 = EvseDataRecord.GeoCoordinate;
                                                                                                                                pool.LocationLanguage            = LocationLanguage;
                                                                                                                                pool.EntranceLocation            = EvseDataRecord.GeoChargingPointEntrance;
                                                                                                                                pool.OpeningTimes                = EvseDataRecord.OpeningTime;
                                                                                                                                pool.AuthenticationModes         = new ReactiveSet<WWCP.AuthenticationModes>(EvseDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                                                                pool.PaymentOptions              = new ReactiveSet<WWCP.PaymentOptions>     (EvseDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                                                                pool.Accessibility               = EvseDataRecord.Accessibility;
                                                                                                                                pool.HotlinePhoneNumber          = EvseDataRecord.HotlinePhoneNumber;
                                                                                                                                //pool.StatusAggregationDelegate   = ChargingStationStatusAggregationDelegate;

                                                                                                                            }

                                                                                          );

                                                                                      #endregion

                                                              #region Update matching charging station... or create a new one!

                                                              if (_ChargingPool.TryGetChargingStationbyId(EVSEInfo.StationId, out _ChargingStation))
                                                              {

                                                                  // Update via events!
                                                                  _ChargingStation.Name                       = EvseDataRecord.ChargingStationName;
                                                                  _ChargingStation.HubjectStationId           = EvseDataRecord.ChargingStationId;
                                                                  _ChargingStation.Description                = EvseDataRecord.AdditionalInfo;
                                                                  _ChargingStation.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(EvseDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                  _ChargingStation.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (EvseDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                  _ChargingStation.Accessibility              = EvseDataRecord.Accessibility;
                                                                  _ChargingStation.HotlinePhoneNumber         = EvseDataRecord.HotlinePhoneNumber;
                                                                  _ChargingStation.IsHubjectCompatible        = EvseDataRecord.IsHubjectCompatible;
                                                                  _ChargingStation.DynamicInfoAvailable       = EvseDataRecord.DynamicInfoAvailable;
                                                                  _ChargingStation.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                                              }

                                                              else
                                                                  _ChargingStation = _ChargingPool.CreateNewStation(

                                                                                                       EVSEInfo.StationId,

                                                                                                       Configurator: station => {

                                                                                                           station.Name                       = EvseDataRecord.ChargingStationName;
                                                                                                           station.HubjectStationId           = EvseDataRecord.ChargingStationId;
                                                                                                           station.Description                = EvseDataRecord.AdditionalInfo;
                                                                                                           station.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(EvseDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OICPMapper.AsWWCPAuthenticationMode(mode)));
                                                                                                           station.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (EvseDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OICPMapper.AsWWCPPaymentOption(option)));
                                                                                                           station.Accessibility              = EvseDataRecord.Accessibility;
                                                                                                           station.HotlinePhoneNumber         = EvseDataRecord.HotlinePhoneNumber;
                                                                                                           station.IsHubjectCompatible        = EvseDataRecord.IsHubjectCompatible;
                                                                                                           station.DynamicInfoAvailable       = EvseDataRecord.DynamicInfoAvailable;
                                                                                                           station.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

                                                                                                           // photo_uri => "place_photo"

                                                                                                       }

                                                                         );

                                                              #endregion

                                                              #region Update matching EVSE... or create a new one!

                                                              if (_ChargingStation.TryGetEVSEbyId(EvseDataRecord.Id.ToWWCP(), out _EVSE))
                                                              {

                                                                  // Update via events!
                                                                  _EVSE.Description     = EvseDataRecord.AdditionalInfo;
                                                                  _EVSE.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(EvseDataRecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
                                                                  OICPMapper.ApplyChargingFacilities(_EVSE, EvseDataRecord.ChargingFacilities);
                                                                  _EVSE.MaxCapacity     = EvseDataRecord.MaxCapacity;
                                                                  _EVSE.SocketOutlets   = new ReactiveSet<SocketOutlet>(EvseDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

                                                              }

                                                              else
                                                                  _ChargingStation.CreateNewEVSE(EvseDataRecord.Id.ToWWCP(),

                                                                                                 Configurator: evse => {

                                                                                                     evse.Description     = EvseDataRecord.AdditionalInfo;
                                                                                                     evse.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(EvseDataRecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
                                                                                                     OICPMapper.ApplyChargingFacilities(_EVSE, EvseDataRecord.ChargingFacilities);
                                                                                                     evse.MaxCapacity     = EvseDataRecord.MaxCapacity;
                                                                                                     evse.SocketOutlets   = new ReactiveSet<SocketOutlet>(EvseDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

                                                                                                 }
                                                                                                );

                                                              #endregion

                                                          }
                                                          catch (Exception e)
                                                          {
                                                              DebugX.Log(e.Message);
                                                          }

                                                      }

                                                  }
                                                  catch (Exception e)
                                                  {
                                                      DebugX.Log("'UpdateEVSEData' led to an exception: " + e.Message);
                                                  }

                                              });

               }

               #endregion

                   #region Get EVSEIds for status update

                   //GetEVSEIdsForStatusUpdate: (UpdateContext, Timestamp) =>
                   //                               EVSEDatabase.
                   //                                   Where (entry => entry.EVSE.ChargingStation.DynamicInfoAvailable).
                   //                                   Select(entry => entry.EVSE.Id),

                   #endregion

                   #region EVSEStatusHandler

                   //EVSEStatusHandler:         (UpdateContext, Timestamp, EVSEId, Status) =>
                   //                               EVSEDatabase.
                   //                                   UpdateEVSEStatus(Timestamp, EVSEId, Status.AsEVSEStatusType())

                   #endregion

              )

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException("The given roaming network must not be null or empty!");

            #endregion

            this._RoamingNetwork =  RoamingNetwork;

        }

        #endregion

    }


    /// <summary>
    /// A service to import EVSE data (from Hubject) via OICP v2.0.
    /// </summary>
    public class OICPImporter<TContext>
    {

        #region Data

        public static readonly TimeSpan      DefaultUpdateEVSEDataEvery      = TimeSpan.FromHours  (  2);
        public static readonly TimeSpan      DefaultUpdateEVSEDataTimeout    = TimeSpan.FromMinutes( 30);   // First import might be big and slow!
        public static readonly TimeSpan      DefaultUpdateEVSEStatusEvery    = TimeSpan.FromSeconds( 20);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusTimeout  = TimeSpan.FromMinutes( 30);   // First import might be big and slow!

        private readonly EMP.EMPClient                                              _EMPClient;
        private readonly Object                                                     UpdateEVSEsLock  = new Object();
        private readonly Timer                                                      UpdateEVSEDataTimer;
        private readonly Timer                                                      UpdateEVSEStatusTimer;

        private readonly Func  <TContext>                                           _UpdateContextCreator;
        private readonly Action<TContext>                                           _UpdateContextDisposer;
        private readonly Action<TContext>                                           _StartBulkUpdate;

        private readonly Action<TContext, DateTime, IEnumerable<XElement>>          _EVSEDataXMLHandler;
        private readonly Action<TContext, DateTime, IEnumerable<OperatorEVSEData>>  _EVSEDataHandler;

        private readonly Func  <TContext, DateTime, IEnumerable<EVSE_Id>>           _GetEVSEIdsForStatusUpdate;
        private readonly Action<TContext, DateTime, XElement>                       _EVSEStatusXMLHandler;
        private readonly Action<TContext, DateTime, EVSE_Id, EVSEStatusTypes>       _EVSEStatusHandler;

        private readonly Action<TContext>                                           _StopBulkUpdate;

        private readonly Func<UInt64, StreamReader>                                 _LoadStaticDataFromStream;
        private readonly Func<UInt64, StreamReader>                                 _LoadDynamicDataFromStream;
        private volatile Boolean                                                    Paused = false;

        #endregion

        #region Properties

        public String Identification { get; }

        private readonly String                _Hostname;
        private readonly IPPort                _TCPPort;
        private readonly String                _HTTPVirtualHost;
        private readonly eMobilityProvider_Id  _ProviderId;

        private readonly TimeSpan              _UpdateEVSEDataEvery;
        private readonly TimeSpan              _UpdateEVSEDataTimeout;
        private readonly TimeSpan              _UpdateEVSEStatusEvery;
        private readonly TimeSpan              _UpdateEVSEStatusTimeout;

        #region LoadStaticDataCounter

        private UInt64 _LoadStaticDataCounter = 1;

        public UInt64 LoadStaticDataCounter
        {
            get
            {
                return _LoadStaticDataCounter;
            }
        }

        #endregion

        #region LoadDynamicDataCounter

        private UInt64 _LoadDynamicDataCounter = 1;

        public UInt64 LoadDynamicDataCounter
        {
            get
            {
                return _LoadDynamicDataCounter;
            }
        }

        #endregion

        public DNSClient DNSClient { get; }

        /// <summary>
        /// The timeout for upstream queries.
        /// </summary>
        public TimeSpan? RequestTimeout
            => _EMPClient.RequestTimeout;

        #endregion

        #region Events

        #region OnException

        /// <summary>
        /// An event fired whenever an exception occured.
        /// </summary>
        public event OnExceptionDelegate OnException;

        #endregion

        #region OnHTTPError

        /// <summary>
        /// A delegate called whenever a HTTP error occured.
        /// </summary>
        public delegate void OnHTTPErrorDelegate(DateTime Timestamp, Object Sender, HTTPResponse HttpResponse);

        /// <summary>
        /// An event fired whenever a HTTP error occured.
        /// </summary>
        public event OnHTTPErrorDelegate OnHTTPError;

        #endregion

        #region OnSOAPError

        /// <summary>
        /// A delegate called whenever a SOAP error occured.
        /// </summary>
        public delegate void OnSOAPErrorDelegate(DateTime Timestamp, Object Sender, XElement SOAPXML);

        /// <summary>
        /// An event fired whenever a SOAP error occured.
        /// </summary>
        public event OnSOAPErrorDelegate OnSOAPError;

        #endregion

        #endregion

        #region Constructor(s)

        #region OICPImporter(...)

        /// <summary>
        /// Create a new service for importing EVSE data via OICP/Hubject.
        /// </summary>
        public OICPImporter(String                                                      Identification,
                            String                                                      Hostname,
                            IPPort                                                      TCPPort,
                            String                                                      HTTPVirtualHost,
                            eMobilityProvider_Id                                                     ProviderId,
                            DNSClient                                                   DNSClient                   = null,
                            TimeSpan?                                                   QueryTimeout                = null,

                            TimeSpan?                                                   UpdateEVSEDataEvery         = null,
                            TimeSpan?                                                   UpdateEVSEDataTimeout       = null,
                            TimeSpan?                                                   UpdateEVSEStatusEvery       = null,
                            TimeSpan?                                                   UpdateEVSEStatusTimeout     = null,
                            Func<TContext>                                              UpdateContextCreator        = null,
                            Action<TContext>                                            UpdateContextDisposer       = null,
                            Action<TContext>                                            StartBulkUpdate             = null,
                            Action<TContext>                                            StopBulkUpdate              = null,

                            Func<UInt64, StreamReader>                                  LoadStaticDataFromStream    = null,
                            Func<UInt64, StreamReader>                                  LoadDynamicDataFromStream   = null,

                            Action<TContext, DateTime, IEnumerable<XElement>>           EVSEDataXMLHandler          = null,
                            Func  <TContext, DateTime, IEnumerable<EVSE_Id>>            GetEVSEIdsForStatusUpdate   = null,
                            Action<TContext, DateTime, XElement>                        EVSEStatusXMLHandler        = null)

        {

            #region Initial checks

            if (Identification.IsNullOrEmpty())
                throw new ArgumentNullException("The given service identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException("The given upstream service hostname must not be null or empty!");

            if (TCPPort == null)
                throw new ArgumentNullException("The given upstream service TCP port must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("The given EV Service Provider identification (EVSP Id) must not be null!");

            #endregion

            #region Init parameters

            this.Identification               = Identification;
            this._Hostname                     = Hostname;
            this._TCPPort                      = TCPPort;
            this._HTTPVirtualHost              = HTTPVirtualHost;
            this._ProviderId                   = ProviderId;

            if (!UpdateEVSEDataEvery.HasValue)
                this._UpdateEVSEDataEvery      = DefaultUpdateEVSEDataEvery;

            if (!UpdateEVSEDataTimeout.HasValue)
                this._UpdateEVSEDataTimeout    = DefaultUpdateEVSEDataTimeout;

            if (!UpdateEVSEStatusEvery.HasValue)
                this._UpdateEVSEStatusEvery    = DefaultUpdateEVSEStatusEvery;

            if (!UpdateEVSEStatusTimeout.HasValue)
                this._UpdateEVSEStatusTimeout  = DefaultUpdateEVSEStatusTimeout;

            this.DNSClient                    = DNSClient != null
                                                     ? DNSClient
                                                     : new DNSClient();

            this._LoadStaticDataFromStream     = LoadStaticDataFromStream;
            this._LoadDynamicDataFromStream    = LoadDynamicDataFromStream;

            this._UpdateContextCreator         = UpdateContextCreator;
            this._UpdateContextDisposer        = UpdateContextDisposer;
            this._StartBulkUpdate              = StartBulkUpdate;
            this._EVSEDataXMLHandler           = EVSEDataXMLHandler;
            this._EVSEDataHandler              = null;
            this._GetEVSEIdsForStatusUpdate    = GetEVSEIdsForStatusUpdate;
            this._EVSEStatusXMLHandler         = EVSEStatusXMLHandler;
            this._EVSEStatusHandler            = null;
            this._StopBulkUpdate               = StopBulkUpdate;

            #endregion

            #region Init OICP EMP UpstreamService

            _EMPClient  = new EMP.EMPClient(ClientId:         Identification,
                                            Hostname:         _Hostname,
                                            RemotePort:       _TCPPort,
                                            HTTPVirtualHost:  _HTTPVirtualHost,
                                            RequestTimeout:   QueryTimeout,
                                            DNSClient:        DNSClient);

            _EMPClient.OnException += SendException;
            _EMPClient.OnHTTPError += SendHTTPError;
            _EMPClient.OnSOAPError += SendSOAPError;

            #endregion

            #region Init Timers

            UpdateEVSEDataTimer    = new Timer(UpdateEVSEData,   null, TimeSpan.FromSeconds(1),  this._UpdateEVSEDataEvery);
            UpdateEVSEStatusTimer  = new Timer(UpdateEVSEStatus, null, TimeSpan.FromSeconds(10), this._UpdateEVSEStatusEvery);

            #endregion

        }

        #endregion

        #region OICPImporter(...)

        /// <summary>
        /// Create a new service for importing EVSE data via OICP/Hubject.
        /// </summary>
        public OICPImporter(String                                                      Identification,
                            String                                                      Hostname,
                            IPPort                                                      TCPPort,
                            String                                                      HTTPVirtualHost,
                            eMobilityProvider_Id                                                     ProviderId,
                            DNSClient                                                   DNSClient                   = null,
                            TimeSpan?                                                   QueryTimeout                = null,

                            TimeSpan?                                                   UpdateEVSEDataEvery         = null,
                            TimeSpan?                                                   UpdateEVSEDataTimeout       = null,
                            TimeSpan?                                                   UpdateEVSEStatusEvery       = null,
                            TimeSpan?                                                   UpdateEVSEStatusTimeout     = null,
                            Func<TContext>                                              UpdateContextCreator        = null,
                            Action<TContext>                                            UpdateContextDisposer       = null,
                            Action<TContext>                                            StartBulkUpdate             = null,
                            Action<TContext>                                            StopBulkUpdate              = null,

                            Action<TContext, DateTime, IEnumerable<OperatorEVSEData>>   EVSEDataHandler             = null,
                            Func  <TContext, DateTime, IEnumerable<EVSE_Id>>            GetEVSEIdsForStatusUpdate   = null,
                            Action<TContext, DateTime, EVSE_Id, EVSEStatusTypes>     EVSEStatusHandler           = null)

        {

            #region Initial checks

            if (Identification.IsNullOrEmpty())
                throw new ArgumentNullException("The given service identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException("The given upstream service hostname must not be null or empty!");

            if (TCPPort == null)
                throw new ArgumentNullException("The given upstream service TCP port must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("The given EV Service Provider identification (EVSP Id) must not be null!");

            #endregion

            #region Init parameters

            this.Identification               = Identification;
            this._Hostname                     = Hostname;
            this._TCPPort                      = TCPPort;
            this._HTTPVirtualHost              = HTTPVirtualHost;
            this._ProviderId                   = ProviderId;

            if (!UpdateEVSEDataEvery.HasValue)
                this._UpdateEVSEDataEvery      = DefaultUpdateEVSEDataEvery;

            if (!UpdateEVSEDataTimeout.HasValue)
                this._UpdateEVSEDataTimeout    = DefaultUpdateEVSEDataTimeout;

            if (!UpdateEVSEStatusEvery.HasValue)
                this._UpdateEVSEStatusEvery    = DefaultUpdateEVSEStatusEvery;

            if (!UpdateEVSEStatusTimeout.HasValue)
                this._UpdateEVSEStatusTimeout  = DefaultUpdateEVSEStatusTimeout;

            this.DNSClient                    = DNSClient != null
                                                     ? DNSClient
                                                     : new DNSClient();

            this._LoadStaticDataFromStream     = null;
            this._LoadDynamicDataFromStream    = null;

            this._UpdateContextCreator         = UpdateContextCreator;
            this._UpdateContextDisposer        = UpdateContextDisposer;
            this._StartBulkUpdate              = StartBulkUpdate;
            this._EVSEDataXMLHandler           = null;
            this._EVSEDataHandler              = EVSEDataHandler;
            this._GetEVSEIdsForStatusUpdate    = GetEVSEIdsForStatusUpdate;
            this._EVSEStatusXMLHandler         = null;
            this._EVSEStatusHandler            = EVSEStatusHandler;
            this._StopBulkUpdate               = StopBulkUpdate;

            #endregion

            #region Init OICP EMP UpstreamService

            _EMPClient  = new EMP.EMPClient(ClientId:         Identification,
                                            Hostname:         _Hostname,
                                            RemotePort:       _TCPPort,
                                            HTTPVirtualHost:  _HTTPVirtualHost,
                                            RequestTimeout:   QueryTimeout,
                                            DNSClient:        DNSClient);

            _EMPClient.OnException += SendException;
            _EMPClient.OnHTTPError += SendHTTPError;
            _EMPClient.OnSOAPError += SendSOAPError;

            #endregion

            #region Init Timers

            UpdateEVSEDataTimer    = new Timer(UpdateEVSEData,   null, TimeSpan.FromSeconds(1),  this._UpdateEVSEDataEvery);
            UpdateEVSEStatusTimer  = new Timer(UpdateEVSEStatus, null, TimeSpan.FromSeconds(10), this._UpdateEVSEStatusEvery);

            #endregion

        }

        #endregion

        #endregion


        #region Pause()

        /// <summary>
        /// Pause the OICP importer (after the current run).
        /// </summary>
        public void Pause()
        {
            lock (UpdateEVSEsLock)
            {
                Paused = true;
            }
        }

        #endregion

        #region Continue()

        /// <summary>
        /// Continue the OICP importer (with the next scheduled run).
        /// </summary>
        public void Continue()
        {
            lock (UpdateEVSEsLock)
            {
                Paused = false;
            }
        }

        #endregion


        #region (private, Timer) UpdateEVSEData(State)

        /// <summary>
        /// A timer controlled method to update all EVSE data.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEData(Object State)
        {

            if (Paused)
                return;

            // Wait till a concurrent UpdateEVSEStatus(...) has finished!
            if (Monitor.TryEnter(UpdateEVSEsLock, _UpdateEVSEDataTimeout))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.LogT("'UpdateEVSEData' started");

                        var StopWatch = new Stopwatch();
                        StopWatch.Start();

                    #endif

                    #endregion

                    #region Create update context and start bulk update

                    TContext UpdateContext = default(TContext);

                    try
                    {

                        var UpdateContextCreatorLocal = _UpdateContextCreator;
                        if (UpdateContextCreatorLocal != null)
                            UpdateContext = UpdateContextCreatorLocal();

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not create OICP importer update context: " + e.Message);
                    }

                    try
                    {

                        var StartBulkUpdateLocal = _StartBulkUpdate;
                        if (StartBulkUpdateLocal != null)
                            StartBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not start OICP importer bulk update: " + e.Message);
                    }

                    #endregion

                    #region Load static EVSE data from stream...

                    if (_LoadStaticDataFromStream != null)
                    {

                        try
                        {

                            var XML               = XDocument.Parse(_LoadStaticDataFromStream(_LoadStaticDataCounter++).ReadToEnd()).Root;

                            var SOAPXML           = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope_v1_1 + "Body").
                                                        Descendants().
                                                        FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var OperatorEvseDataXMLs  = (SOAPXML != null ? SOAPXML : XML).
                                                            Element (OICPNS.EVSEData + "EvseData").
                                                            Elements(OICPNS.EVSEData + "OperatorEvseData");

                            if (OperatorEvseDataXMLs != null)
                            {

                                if (_EVSEDataXMLHandler != null)
                                    _EVSEDataXMLHandler(UpdateContext,
                                                        DateTime.Now,
                                                        OperatorEvseDataXMLs);

                                var _OperatorEVSEData = OperatorEvseDataXMLs.
                                                            Select    (OperatorEvseDataXML => OperatorEVSEData.Parse(OperatorEvseDataXML)).
                                                            Where     (_OperatorEvseData   => _OperatorEvseData != null).
                                                            ToArray();

                                if (_OperatorEVSEData.Length  > 0 &&
                                    _EVSEDataXMLHandler      != null)
                                    _EVSEDataHandler(UpdateContext,
                                                     DateTime.Now,
                                                     _OperatorEVSEData);

                            }

                            else
                                DebugX.Log("Could not fetch any 'OperatorEvseData' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not fetch any 'OperatorEvseData' from XML stream: " + e.Message);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v2.0

                    else
                        _EMPClient.
                            PullEVSEData(_ProviderId).
                            ContinueWith(PullEVSEDataTask => {

                                if (PullEVSEDataTask.Result.Content.StatusCode.Code == 0)
                                {

                                    //if (_EVSEDataXMLHandler != null)
                                    //    _EVSEDataXMLHandler(UpdateContext,
                                    //                        DateTime.Now,
                                    //                        PullEVSEDataTask.Result.Content.OperatorEVSEData);

                                    _EVSEDataHandler(UpdateContext, DateTime.Now, PullEVSEDataTask.Result.Content.OperatorEVSEData);

                                }

                            }).
                            Wait();

                    #endregion

                    #region Stop bulk update and dispose update context

                    try
                    {

                        var StopBulkUpdateLocal = _StopBulkUpdate;
                        if (StopBulkUpdateLocal != null)
                            StopBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not stop OICP importer bulk update: " + e.Message);
                    }

                    try
                    {

                        var UpdateContextDisposerLocal = _UpdateContextDisposer;
                        if (UpdateContextDisposerLocal != null)
                            UpdateContextDisposerLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not dispose OICP importer context: " + e.Message);
                    }

                    #endregion

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("'UpdateEVSEData' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'UpdateEVSEData' led to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.LogT("'UpdateEVSEData' skipped!");

        }

        #endregion

        #region (private, Timer) UpdateEVSEStatus(State)

        /// <summary>
        /// A timer controlled method to update all EVSE status.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEStatus(Object State)
        {

            if (Paused)
                return;

            // If a concurrent UpdateEVSEData/UpdateEVSEStatus(...) is still running, skip this round!
            if (Monitor.TryEnter(UpdateEVSEsLock))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.LogT("'UpdateEVSEStatus' started");

                        var StopWatch = new Stopwatch();
                        StopWatch.Start();

                    #endif

                    #endregion

                    #region Create update context and start bulk update

                    TContext UpdateContext = default(TContext);

                    try
                    {

                        var UpdateContextCreatorLocal = _UpdateContextCreator;
                        if (UpdateContextCreatorLocal != null)
                            UpdateContext = UpdateContextCreatorLocal();

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not create OICP importer update context: " + e.Message);
                    }

                    try
                    {

                        var StartBulkUpdateLocal = _StartBulkUpdate;
                        if (StartBulkUpdateLocal != null)
                            StartBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not start OICP importer bulk update: " + e.Message);
                    }

                    #endregion

                    #region Load static EVSE data from stream...

                    if (_LoadStaticDataFromStream != null)
                    {

                        try
                        {

                            var XML         = XDocument.Parse(_LoadDynamicDataFromStream(_LoadDynamicDataCounter++).ReadToEnd()).Root;

                            var SOAPXML     = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope_v1_1 + "Body").
                                                  Descendants().
                                                  FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var EvseStatus  = (SOAPXML != null ? SOAPXML : XML).
                                                        Element (OICPNS.EVSEStatus + "EvseStatusRecords").
                                                        Elements(OICPNS.EVSEStatus + "EvseStatusRecord").
                                                        Select(v => new KeyValuePair<EVSE_Id, EVSEStatusTypes>(EVSE_Id.Parse(v.Element(OICPNS.EVSEStatus + "EvseId").Value),
                                                                                                                (EVSEStatusTypes) Enum.Parse(typeof(EVSEStatusTypes), v.Element(OICPNS.EVSEStatus + "EvseStatus").Value))).
                                                        ToArray();

                            if (EvseStatus.Length > 0)
                                EvseStatus.ForEach(EVSEStatusKVP => _EVSEStatusHandler(UpdateContext, DateTime.Now, EVSEStatusKVP.Key, EVSEStatusKVP.Value));

                            else
                                DebugX.Log("Could not fetch any 'EvseStatusRecords' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.Log("Could not fetch any 'EvseStatusRecords' from XML stream: " + e.Message);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v2.0

                    else
                    {

                        Boolean Finished = false;

                        var cancellationTokenS = new CancellationTokenSource();

                        #region 1) Fetch EVSEIds to update

                        IEnumerable<EVSE_Id> EVSEIds = null;

                        try
                        {

                            EVSEIds = _GetEVSEIdsForStatusUpdate(UpdateContext, DateTime.Now);

                        }
                        catch (Exception e)
                        {
                            DebugX.Log("Could not fetch the list of EVSE Ids for dynamic EVSE status update!");
                        }

                        if (EVSEIds == null)
                            DebugX.Log("Could not fetch the list of EVSE Ids for dynamic EVSE status update!");

                        #endregion

                        #region 2) Load the data (from Hubject) via OICP v2.0

                        // Get the data via the GetEVSEIds delegate!
                        else
                            Finished = Task.WaitAll(EVSEIds.

                                                        // Hubject has a limit of 100 EVSEIds per request!
                                                        ToPartitions(100).

                                                        Select(EVSEPartition => _EMPClient.
                                                                                    PullEVSEStatusById(_ProviderId, EVSEPartition).
                                                                                    ContinueWith(NewEVSEStatusTask => {

                                                                                        if (NewEVSEStatusTask.Result != null)
                                                                                            if (NewEVSEStatusTask.Result.Content != null)
                                                                                                NewEVSEStatusTask.Result.Content.EVSEStatusRecords.ForEach(NewEVSEStatusRecord =>
                                                                                                    _EVSEStatusHandler(UpdateContext,
                                                                                                                       DateTime.Now,
                                                                                                                       NewEVSEStatusRecord.Id,
                                                                                                                       NewEVSEStatusRecord.Status));

                                                                                    }, cancellationTokenS.Token)
                                                                                    )
                                                                                    .ToArray(),

                                                    millisecondsTimeout: (Int32) _UpdateEVSEStatusTimeout.TotalMilliseconds

                                                    //CancellationToken cancellationToken

                                                   );


                        // Wait 15 seconds and then cancel all subtasks!
                        if (!Finished)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(15));
                            cancellationTokenS.Cancel();
                            DebugX.Log("Canceled all 'UpdateEVSEStatus' subtasks!");
                        }

                        #endregion

                    }

                    #endregion

                    #region Stop bulk update and dispose update context

                    try
                    {

                        var StopBulkUpdateLocal = _StopBulkUpdate;
                        if (StopBulkUpdateLocal != null)
                            StopBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not stop OICP importer bulk update: " + e.Message);
                    }

                    try
                    {

                        var UpdateContextDisposerLocal = _UpdateContextDisposer;
                        if (UpdateContextDisposerLocal != null)
                            UpdateContextDisposerLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not dispose OICP importer context: " + e.Message);
                    }

                    #endregion

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("'UpdateEVSEStatus' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'UpdateEVSEStatus' led to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.LogT("'UpdateEVSEStatus' skipped!");

        }

        #endregion


        #region (protected) SendException(Timestamp, Sender, Exception)

        /// <summary>
        /// Notify that an exception occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the exception.</param>
        /// <param name="Sender">The sender of this exception.</param>
        /// <param name="Exception">The exception itself.</param>
        protected void SendException(DateTime   Timestamp,
                                     Object     Sender,
                                     Exception  Exception)
        {

            var OnExceptionLocal = OnException;
            if (OnExceptionLocal != null)
                OnExceptionLocal(Timestamp, Sender, Exception);

        }

        #endregion

        #region (protected) SendHTTPError(Timestamp, Sender, HttpResponse)

        /// <summary>
        /// Notify that an HTTP error occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the error received.</param>
        /// <param name="Sender">The sender of this error message.</param>
        /// <param name="HttpResponse">The HTTP response related to this error message.</param>
        protected void SendHTTPError(DateTime      Timestamp,
                                     Object        Sender,
                                     HTTPResponse  HttpResponse)
        {

            var OnHTTPErrorLocal = OnHTTPError;
            if (OnHTTPErrorLocal != null)
                OnHTTPErrorLocal(Timestamp, Sender, HttpResponse);

        }

        #endregion

        #region (protected) SendSOAPError(Timestamp, Sender, SOAPXML)

        /// <summary>
        /// Notify that an HTTP error occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the error received.</param>
        /// <param name="Sender">The sender of this error message.</param>
        /// <param name="SOAPXML">The SOAP fault/error.</param>
        protected void SendSOAPError(DateTime  Timestamp,
                                     Object    Sender,
                                     XElement  SOAPXML)
        {

            var OnSOAPErrorLocal = OnSOAPError;
            if (OnSOAPErrorLocal != null)
                OnSOAPErrorLocal(Timestamp, Sender, SOAPXML);

        }

        #endregion


    }

}
