/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.WWCP;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A OICP v2.0 Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge a electric vehicle.
    /// </summary>
    public class EVSEDataRecord
    {

        #region Properties

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #region DeltaType

        private String _DeltaType;

        public String DeltaType
        {

            get
            {
                return _DeltaType;
            }

            set
            {
                if (value != null)
                    _DeltaType = value;
            }

        }

        #endregion

        #region LastUpdate

        private DateTime? _LastUpdate;

        public DateTime? LastUpdate
        {

            get
            {
                return _LastUpdate;
            }

            set
            {
                if (value != null)
                    _LastUpdate = value;
            }

        }

        #endregion


        #region ChargingStationId

        private ChargingStation_Id _ChargingStationId;

        public ChargingStation_Id ChargingStationId
        {

            get
            {
                return _ChargingStationId;
            }

            set
            {
                if (value != null)
                    _ChargingStationId = value;
            }

        }

        #endregion

        #region ChargingStationName

        private I18NString _ChargingStationName;

        public I18NString ChargingStationName
        {

            get
            {
                return _ChargingStationName;
            }

            set
            {
                if (value != null)
                    _ChargingStationName = value;
            }

        }

        #endregion

        #region Address

        private Address _Address;

        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {
                if (value != null)
                    _Address = value;
            }

        }

        #endregion

        #region GeoCoordinate

        private GeoCoordinate _GeoCoordinate;

        public GeoCoordinate GeoCoordinate
        {

            get
            {
                return _GeoCoordinate;
            }

            set
            {
                if (value != null)
                    _GeoCoordinate = value;
            }

        }

        #endregion

        #region Plugs

        private IEnumerable<PlugTypes> _Plugs;

        public IEnumerable<PlugTypes> Plugs
        {

            get
            {
                return _Plugs;
            }

            set
            {
                if (value != null)
                    _Plugs = value;
            }

        }

        #endregion

        #region ChargingFacilities

        private IEnumerable<ChargingFacilities> _ChargingFacilities;

        public IEnumerable<ChargingFacilities> ChargingFacilities
        {

            get
            {
                return _ChargingFacilities;
            }

            set
            {
                if (value != null)
                    _ChargingFacilities = value;
            }

        }

        #endregion

        #region ChargingModes

        private IEnumerable<ChargingModes> _ChargingModes;

        public IEnumerable<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {
                if (value != null)
                    _ChargingModes = value;
            }

        }

        #endregion

        #region AuthenticationModes

        private IEnumerable<AuthenticationModes> _AuthenticationModes;

        public IEnumerable<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {
                if (value != null)
                    _AuthenticationModes = value;
            }

        }

        #endregion

        #region MaxCapacity

        private Double? _MaxCapacity_kWh;

        public Double? MaxCapacity
        {

            get
            {
                return _MaxCapacity_kWh;
            }

            set
            {
                if (value != null)
                    _MaxCapacity_kWh = value;
            }

        }

        #endregion

        #region PaymentOptions

        private IEnumerable<PaymentOptions> _PaymentOptions;

        public IEnumerable<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {
                if (value != null)
                    _PaymentOptions = value;
            }

        }

        #endregion

        #region Accessibility

        private AccessibilityTypes _Accessibility;

        public AccessibilityTypes Accessibility
        {

            get
            {
                return _Accessibility;
            }

            set
            {
                if (value != null)
                    _Accessibility = value;
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {
                if (value != null)
                    _HotlinePhoneNumber = value;
            }

        }

        #endregion

        #region AdditionalInfo

        private I18NString _AdditionalInfo;

        public I18NString AdditionalInfo
        {

            get
            {
                return _AdditionalInfo;
            }

            set
            {
                if (value != null)
                    _AdditionalInfo = value;
            }

        }

        #endregion

        #region GeoChargingPointEntrance

        private GeoCoordinate _GeoChargingPointEntrance;

        public GeoCoordinate GeoChargingPointEntrance
        {

            get
            {
                return _GeoChargingPointEntrance;
            }

            set
            {
                if (value != null)
                    _GeoChargingPointEntrance = value;
            }

        }

        #endregion

        #region IsOpen24Hours

        public Boolean IsOpen24Hours
        {
            get
            {

                if (_OpeningTime == null)
                    return true;

                return _OpeningTime.IsOpen24Hours;

            }
        }

        #endregion

        #region OpeningTime

        private OpeningTime _OpeningTime;

        public OpeningTime OpeningTime
        {

            get
            {
                return _OpeningTime;
            }

            set
            {
                if (value != null)
                    _OpeningTime = value;
            }

        }

        #endregion

        #region HubOperatorId

        private HubOperator_Id _HubOperatorId;

        public HubOperator_Id HubOperatorId
        {

            get
            {
                return _HubOperatorId;
            }

            set
            {
                if (value != null)
                    _HubOperatorId = value;
            }

        }

        #endregion

        #region ClearingHouseId

        private RoamingProvider_Id _ClearingHouseId;

        public RoamingProvider_Id ClearingHouseId
        {

            get
            {
                return _ClearingHouseId;
            }

            set
            {
                if (value != null)
                    _ClearingHouseId = value;
            }

        }

        #endregion

        #region IsHubjectCompatible

        private Boolean _IsHubjectCompatible;

        public Boolean IsHubjectCompatible
        {

            get
            {
                return _IsHubjectCompatible;
            }

            set
            {
                _IsHubjectCompatible = value;
            }

        }

        #endregion

        #region DynamicInfoAvailable

        private Boolean _DynamicInfoAvailable;

        public Boolean DynamicInfoAvailable
        {

            get
            {
                return _DynamicInfoAvailable;
            }

            set
            {
                _DynamicInfoAvailable = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEDataRecord(EVSEId)

        public EVSEDataRecord(EVSE_Id  EVSEId)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            #endregion

            this._EVSEId               = EVSEId;
            this._ChargingStationName  = new I18NString();
            this._AdditionalInfo       = new I18NString();

        }

        #endregion

        #region EVSEDataRecord(EVSEId, ...)

        public EVSEDataRecord(EVSE_Id                           EVSEId,
                              ChargingStation_Id                ChargingStationId           = null,
                              I18NString                        ChargingStationName         = null,
                              Address                           Address                     = null,
                              GeoCoordinate                     GeoCoordinate               = null,
                              IEnumerable<PlugTypes>            Plugs                       = null,
                              IEnumerable<ChargingFacilities>   ChargingFacilities          = null,
                              IEnumerable<ChargingModes>        ChargingModes               = null,
                              IEnumerable<AuthenticationModes>  AuthenticationModes         = null,
                              Int32?                            MaxCapacity                 = null,
                              IEnumerable<PaymentOptions>       PaymentOptions              = null,
                              AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                              String                            HotlinePhoneNumber          = null,
                              I18NString                        AdditionalInfo              = null,
                              GeoCoordinate                     GeoChargingPointEntrance    = null,
                              Boolean?                          IsOpen24Hours               = null,
                              OpeningTime                       OpeningTime                 = null,
                              HubOperator_Id                    HubOperatorId               = null,
                              RoamingProvider_Id                ClearingHouseId             = null,
                              Boolean                           IsHubjectCompatible         = true,
                              Boolean                           DynamicInfoAvailable        = true)

            : this(EVSEId)

        {

            if (Address == null)
                throw new ArgumentNullException("The given parameter for 'Address' must not be null!");

            if (GeoCoordinate == null)
                throw new ArgumentNullException("The given parameter for 'GeoCoordinates' must not be null!");

            if (Plugs == null)
                throw new ArgumentNullException("The given parameter for 'Plugs' must not be null!");

            if (Plugs.Count() < 1)
                throw new ArgumentNullException("The given parameter for 'Plugs' must not be empty!");

            if (AuthenticationModes == null)
                throw new ArgumentNullException("The given parameter for 'AuthenticationModes' must not be null!");

            if (AuthenticationModes.Count() < 1)
                throw new ArgumentNullException("The given parameter for 'AuthenticationModes' must not be empty!");

            if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                throw new ArgumentNullException("The given parameter for 'HotlinePhoneNumber' must not be null or empty!");


            this._ChargingStationId         = ChargingStationId;
            this._ChargingStationName       = ChargingStationName != null ? ChargingStationName : new I18NString();
            this._Address                   = Address;
            this._GeoCoordinate             = GeoCoordinate;
            this._Plugs                     = Plugs;
            this._ChargingModes             = ChargingModes;
            this._ChargingFacilities        = ChargingFacilities;
            this._AuthenticationModes       = AuthenticationModes;
            this._MaxCapacity_kWh           = MaxCapacity;
            this._PaymentOptions            = PaymentOptions;
            this._Accessibility             = Accessibility;
            this._HotlinePhoneNumber        = HotlinePhoneNumber;
            this._AdditionalInfo            = AdditionalInfo      != null ? AdditionalInfo      : new I18NString();
            this._GeoChargingPointEntrance  = GeoChargingPointEntrance;
            this._HubOperatorId             = HubOperatorId;
            this._ClearingHouseId           = ClearingHouseId;
            this._IsHubjectCompatible       = IsHubjectCompatible;
            this._DynamicInfoAvailable      = DynamicInfoAvailable;


            if (IsOpen24Hours != null && IsOpen24Hours.HasValue && IsOpen24Hours.Value)
                this._OpeningTime  = OpeningTime.Open24Hours;

            if (OpeningTime != null)
                this._OpeningTime  = OpeningTime;

            if (OpeningTime == null && (IsOpen24Hours == null || !IsOpen24Hours.HasValue))
                this._OpeningTime = OpeningTime.Open24Hours;

        }

        #endregion

        #endregion

    }

}
