/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using WWCP = org.GraphDefined.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Helper methods to map OICP data type values to
    /// WWCP data type values and vice versa.
    /// </summary>
    public static class OICPMapper
    {

        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id EVSEId)
        {

            if (EVSE_Id.TryParse(EVSEId.ToString(), out EVSE_Id OICPEVSEId))
                return OICPEVSEId;

            return null;

        }

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id EVSEId)
        {

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)
        {

            if (!EVSEId.HasValue)
                return null;

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }



        public static Session_Id ToOICP(this WWCP.ChargingSession_Id SessionId)
            => Session_Id.Parse(SessionId.ToString());

        public static WWCP.ChargingSession_Id ToWWCP(this Session_Id SessionId)
            => WWCP.ChargingSession_Id.Parse(SessionId.ToString());


        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id? SessionId)
            => SessionId.HasValue
                   ? Session_Id.Parse(SessionId.ToString())
                   : new Session_Id?();

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id? SessionId)
            => SessionId.HasValue
                   ? WWCP.ChargingSession_Id.Parse(SessionId.ToString())
                   : new WWCP.ChargingSession_Id?();


        public static PartnerProduct_Id? ToOICP(this WWCP.ChargingProduct_Id ProductId)
            => PartnerProduct_Id.Parse(ProductId.ToString());

        public static WWCP.ChargingProduct_Id ToWWCP(this PartnerProduct_Id ProductId)
            => WWCP.ChargingProduct_Id.Parse(ProductId.ToString());



        public static Operator_Id ToOICP(this WWCP.ChargingStationOperator_Id OperatorId,
                                         WWCP.OperatorIdFormats Format = WWCP.OperatorIdFormats.ISO_STAR)
            => Operator_Id.Parse(OperatorId.ToString(Format));

        public static WWCP.ChargingStationOperator_Id? ToWWCP(this Operator_Id OperatorId)
        {

            if (WWCP.ChargingStationOperator_Id.TryParse(OperatorId.ToString(), out WWCP.ChargingStationOperator_Id ChargingStationOperatorId))
                return ChargingStationOperatorId;

            return null;

        }


        public static Provider_Id ToOICP(this WWCP.eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static WWCP.eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => WWCP.eMobilityProvider_Id.Parse(ProviderId.ToString());


        public static Provider_Id? ToOICP(this WWCP.eMobilityProvider_Id? ProviderId)
            => ProviderId.HasValue
                   ? Provider_Id.Parse(ProviderId.ToString())
                   : new Provider_Id?();

        public static WWCP.eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)
            => ProviderId.HasValue
                   ? WWCP.eMobilityProvider_Id.Parse(ProviderId.ToString())
                   : new WWCP.eMobilityProvider_Id?();



        //public static EVCO_Id ToOICP(this RemoteAuthentication RemoteAuthentication)
        //    => EVCO_Id.Parse(RemoteAuthentication.RemoteIdentification.ToString());

        public static WWCP.RemoteAuthentication ToWWCP(this EVCO_Id EVCOId)
            => WWCP.RemoteAuthentication.FromRemoteIdentification(WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString()));


        public static EVCO_Id ToOICP(this WWCP.eMobilityAccount_Id eMAId)
            => EVCO_Id.Parse(eMAId.ToString());

        public static WWCP.eMobilityAccount_Id ToWWCP_eMAId(this EVCO_Id EVCOId)
            => WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString());


        public static EVCO_Id? ToOICP(this WWCP.eMobilityAccount_Id? eMAId)
            => eMAId.HasValue
                   ? EVCO_Id.Parse(eMAId.ToString())
                   : new EVCO_Id?();

        public static WWCP.eMobilityAccount_Id? ToWWCP(this EVCO_Id? EVCOId)
            => EVCOId.HasValue
                   ? WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString())
                   : new WWCP.eMobilityAccount_Id?();




        public static UID ToOICP(this WWCP.Auth_Token AuthToken)
            => UID.Parse(AuthToken.ToString());

        public static WWCP.Auth_Token ToWWCP(this UID UID)
            => WWCP.Auth_Token.Parse(UID.ToString());

        public static UID? ToOICP(this WWCP.Auth_Token? AuthToken)
            => AuthToken.HasValue
                   ? UID.Parse(AuthToken.ToString())
                   : new UID?();




        #region ToWWCP(this Identification)

        public static WWCP.RemoteAuthentication ToWWCP(this Identification Identification)
        {

            if (Identification.RFIDId.HasValue)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.Auth_Token.Parse (Identification.RFIDId.ToString()));

            if (Identification.RFIDIdentification != null)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.Auth_Token.Parse (Identification.RFIDIdentification.         UID.   ToString()));

            if (Identification.QRCodeIdentification.HasValue && Identification.QRCodeIdentification.Value.PIN.HasValue)
                return WWCP.RemoteAuthentication.FromQRCodeIdentification       (Identification.QRCodeIdentification.Value.EVCOId.ToWWCP_eMAId(),
                                                                                 Identification.QRCodeIdentification.Value.PIN.      Value.ToString());

            if (Identification.QRCodeIdentification.HasValue && Identification.QRCodeIdentification.Value.HashedPIN.HasValue)
                return WWCP.RemoteAuthentication.FromQRCodeIdentification       (Identification.QRCodeIdentification.Value.EVCOId.ToWWCP_eMAId(),
                                                                                 Identification.QRCodeIdentification.Value.HashedPIN.Value.ToString());

            if (Identification.PlugAndChargeIdentification.HasValue)
                return WWCP.RemoteAuthentication.FromPlugAndChargeIdentification(Identification.PlugAndChargeIdentification.Value.ToWWCP_eMAId());

            if (Identification.RemoteIdentification.HasValue)
                return WWCP.RemoteAuthentication.FromRemoteIdentification       (Identification.RemoteIdentification.       Value.ToWWCP_eMAId());

            return null;

        }

        #endregion



        #region ToWWCP(this ChargeDetailRecord, ChargeDetailRecord2WWCPChargeDetailRecord = null)

        /// <summary>
        /// Convert an OICP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP charge detail record.</param>
        /// <param name="ChargeDetailRecord2WWCPChargeDetailRecord">A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.</param>
        public static WWCP.ChargeDetailRecord ToWWCP(this ChargeDetailRecord                                ChargeDetailRecord)
                                                   //  CPO.ChargeDetailRecord2WWCPChargeDetailRecordDelegate  ChargeDetailRecord2WWCPChargeDetailRecord = null)
        {

            var CustomData = new Dictionary<String, Object> {
                                 { "OICP.CDR", ChargeDetailRecord }
                             };

            if (ChargeDetailRecord.CPOPartnerSessionId.HasValue)
                CustomData.Add("OICP.CPOPartnerSessionId",  ChargeDetailRecord.CPOPartnerSessionId.ToString());

            if (ChargeDetailRecord.EMPPartnerSessionId.HasValue)
                CustomData.Add("OICP.EMPPartnerSessionId",  ChargeDetailRecord.EMPPartnerSessionId.ToString());

            if (ChargeDetailRecord.HubOperatorId.HasValue)
                CustomData.Add("OICP.HubOperatorId",        ChargeDetailRecord.HubOperatorId.      ToString());

            if (ChargeDetailRecord.HubProviderId.HasValue)
                CustomData.Add("OICP.HubProviderId",        ChargeDetailRecord.HubProviderId.      ToString());


            var CDR = new WWCP.ChargeDetailRecord(
                          Id:                    WWCP.ChargeDetailRecord_Id.Parse(ChargeDetailRecord.SessionId.ToWWCP().ToString()),
                          SessionId:             ChargeDetailRecord.SessionId.ToWWCP(),
                          EVSEId:                ChargeDetailRecord.EVSEId.   ToWWCP(),
                          ProviderIdStart:       ChargeDetailRecord.HubProviderId.HasValue ? new WWCP.eMobilityProvider_Id?(WWCP.eMobilityProvider_Id.Parse(ChargeDetailRecord.HubProviderId.ToString())) : null,

                          ChargingProduct:       ChargeDetailRecord.PartnerProductId.HasValue
                                                     ? WWCP.ChargingProduct.FromId(ChargeDetailRecord.PartnerProductId.Value.ToString())
                                                     : null,

                          SessionTime:           new StartEndDateTime(ChargeDetailRecord.SessionStart,
                                                                      ChargeDetailRecord.SessionEnd),

                          AuthenticationStart:   ChargeDetailRecord.Identification.ToWWCP(),

                          EnergyMeteringValues:  new Timestamped<Decimal>[] {

                                                     new Timestamped<Decimal>(
                                                         ChargeDetailRecord.ChargingStart,
                                                         ChargeDetailRecord.MeterValueStart ?? 0
                                                     ),

                                                     //ToDo: Meter values in between... but we don't have timestamps for them!

                                                     new Timestamped<Decimal>(
                                                         ChargeDetailRecord.ChargingEnd,
                                                         ChargeDetailRecord.MeterValueEnd   ?? ChargeDetailRecord.ConsumedEnergy
                                                     )

                                                 },

                          //ConsumedEnergy:      Will be calculated!

                          //Signatures:            new String[] { ChargeDetailRecord.MeteringSignature },

                          CustomData:            CustomData

                      );

            //if (ChargeDetailRecord2WWCPChargeDetailRecord != null)
            //    CDR = ChargeDetailRecord2WWCPChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

    }

}
