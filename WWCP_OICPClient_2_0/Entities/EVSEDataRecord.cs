/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICP>
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

namespace org.GraphDefined.WWCP.OICPClient_2_0
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

        private String _ChargingStationId;

        public String ChargingStationId
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

        private String _ChargingStationName;

        public String ChargingStationName
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

        #region EnChargingStationName

        private String _EnChargingStationName;

        public String EnChargingStationName
        {

            get
            {
                return _EnChargingStationName;
            }

            set
            {
                if (value != null)
                    _EnChargingStationName = value;
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

        #region GeoCoordinates

        private GeoCoordinate _GeoCoordinates;

        public GeoCoordinate GeoCoordinates
        {

            get
            {
                return _GeoCoordinates;
            }

            set
            {
                if (value != null)
                    _GeoCoordinates = value;
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

        private Double? _MaxCapacity;

        public Double? MaxCapacity
        {

            get
            {
                return _MaxCapacity;
            }

            set
            {
                if (value != null)
                    _MaxCapacity = value;
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

        #region HotlinePhoneNum

        private String _HotlinePhoneNum;

        public String HotlinePhoneNum
        {

            get
            {
                return _HotlinePhoneNum;
            }

            set
            {
                if (value != null)
                    _HotlinePhoneNum = value;
            }

        }

        #endregion

        #region AdditionalInfo

        private String _AdditionalInfo;

        public String AdditionalInfo
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

        #region EnAdditionalInfo

        private I18NString _EnAdditionalInfo;

        public I18NString EnAdditionalInfo
        {

            get
            {
                return _EnAdditionalInfo;
            }

            set
            {
                if (value != null)
                    _EnAdditionalInfo = value;
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

        #region HubOperatorID

        private EVSEOperator_Id _HubOperatorID;

        public EVSEOperator_Id HubOperatorId
        {

            get
            {
                return _HubOperatorID;
            }

            set
            {
                if (value != null)
                    _HubOperatorID = value;
            }

        }

        #endregion

        #region ClearinghouseID

        private RoamingProvider_Id _ClearinghouseID;

        public RoamingProvider_Id ClearinghouseId
        {

            get
            {
                return _ClearinghouseID;
            }

            set
            {
                if (value != null)
                    _ClearinghouseID = value;
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

        public EVSEDataRecord(EVSE_Id  EVSEId)

        {

            #region Initial checks

            if (EVSEId != null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            #endregion

            this._EVSEId  = EVSEId;

        }

        #endregion

    }

}
