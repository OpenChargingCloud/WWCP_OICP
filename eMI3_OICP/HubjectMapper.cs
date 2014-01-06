using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.emi3group.IO.OICP
{

    public static class HubjectMapper
    {

        public static String MapToPlugType(SocketOutlet SocketOutlet)
        {

            switch (SocketOutlet.Plug)
            {

                // Type F Schuko                      -> CEE 7/4
                case PlugType.SCHUKO            : return "Type F Schuko";

                // Type 2 Outlet                      -> IEC 62196-1 type 2
                // Type 2 Connector (Cable Attached)  -> Cable attached to IEC 62196-1 type 2 connector.
                case PlugType.IEC62196_Type_2   : return (SocketOutlet.CableAttached == CableType.attached)
                                                    ? "Type 2 Connector (Cable Attached)"
                                                    : "Type 2 Outlet";

                // CHAdeMO, DC CHAdeMO Connector
                case PlugType.CHAdeMO           : return "CHAdeMO";

                    // Small Paddle Inductive
                    // Large Paddle Inductive
                    // AVCON Connector
                    // Tesla Connector
                    // NEMA 5-20
                    // Type E French Standard                  CEE 7/5
                    // Type G British Standard                 BS 1363
                    // Type J Swiss Standard                   SEV 1011
                    // Type 1 Connector (Cable Attached)       Cable attached to IEC 62196-1 type 1, SAE J1772 connector.
                    // Type 3 Outlet                           IEC 62196-1 type 3
                    // IEC 60309 Single Phase                  IEC 60309
                    // IEC 60309 Three Phase                   IEC 60309
                    // CCS Combo 2 Plug (Cable Attached)       IEC 62196-3 CDV DC Combined Charging Connector DIN SPEC 70121 refers to ISO / IEC 15118-1 DIS, -2 DIS and 15118-3
                    // CCS Combo 1 Plug (Cable Attached)       IEC 62196-3 CDV DC Combined Charging Connector with IEC 62196-1 type 2 SAE J1772 connector

                default: return "Unspecified";

            }

        }

    }

}
