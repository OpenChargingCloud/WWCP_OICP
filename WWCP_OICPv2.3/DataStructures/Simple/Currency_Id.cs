/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for currency identifications.
    /// </summary>
    public static class CurrencyIdExtensions
    {

        /// <summary>
        /// Indicates whether this currency identification is null or empty.
        /// </summary>
        /// <param name="CurrencyId">A currency identification.</param>
        public static Boolean IsNullOrEmpty(this Currency_Id? CurrencyId)
            => !CurrencyId.HasValue || CurrencyId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this currency identification is null or empty.
        /// </summary>
        /// <param name="CurrencyId">A currency identification.</param>
        public static Boolean IsNotNullOrEmpty(this Currency_Id? CurrencyId)
            => CurrencyId.HasValue && CurrencyId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a currency.
    /// ISO 4217 currencies, see: https://www.iso.org/iso-4217-currency-codes.html
    /// 01.04.2022
    /// </summary>
    public readonly struct Currency_Id : IId<Currency_Id>
    {

        #region Data

        //ToDo: Implement proper currency id format!
        // ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?P[A-Za-z0-9\*]{1,30})

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the currency identification.
        /// </summary>
        public UInt64 Length
            => (UInt64)(InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new currency identification.
        /// based on the given string.
        /// </summary>
        private Currency_Id(String   AlphabeticCode,
                            UInt16?  NumericCode,
                            UInt16?  MinorUnit,
                            String?  Currency,
                            String?  Entity)
        {

            InternalId = AlphabeticCode;

        }

        /// <summary>
        /// Create a new currency identification.
        /// based on the given string.
        /// </summary>
        private Currency_Id(String                AlphabeticCode,
                            UInt16?               NumericCode,
                            UInt16?               MinorUnit,
                            String?               Currency,
                            IEnumerable<String>?  Entities)
        {

            InternalId = AlphabeticCode;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text representation of a currency identification.</param>
        public static Currency_Id Parse(String Text)
        {

            if (TryParse(Text, out var currencyId))
                return currencyId;

            throw new ArgumentException("Invalid text representation of a currency identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text representation of a currency identification.</param>
        public static Currency_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Currency_Id currencyId))
                return currencyId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out CurrencyId)

        /// <summary>
        /// Try to parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text representation of a currency identification.</param>
        /// <param name="CurrencyId">The parsed currency identification.</param>
        public static Boolean TryParse(String Text, out Currency_Id CurrencyId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    CurrencyId = new Currency_Id(AlphabeticCode: Text.Trim(),
                                                 NumericCode:    null,
                                                 MinorUnit:      null,
                                                 Currency:       null,
                                                 Entity:         null);
                    return true;
                }
                catch
                { }
            }

            CurrencyId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this currency identification.
        /// </summary>
        public Currency_Id Clone

            => new(AlphabeticCode:  new String(InternalId?.ToCharArray()),
                   NumericCode:     null,
                   MinorUnit:       null,
                   Currency:        null,
                   Entity:          null);

        #endregion


        /// <summary>
        /// Afghani
        /// </summary>
        public static readonly Currency_Id AFN = new ("AFN", 971, 2, "Afghani", "AFGHANISTAN");

        /// <summary>
        /// Euro
        /// </summary>
        public static readonly Currency_Id EUR = new ("EUR", 978, 2, "Euro", new String[] { "ÅLAND ISLANDS", "ANDORRA", "AUSTRIA", "BELGIUM", "CYPRUS", "ESTONIA", "EUROPEAN UNION",
                                                                                            "FINLAND", "FRANCE", "FRENCH GUIANA", "FRENCH SOUTHERN TERRITORIES (THE)","GERMANY",
                                                                                            "GREECE", "GUADELOUPE", "HOLY SEE (THE)", "IRELAND", "ITALY", "LATVIA", "LITHUANIA",
                                                                                            "LUXEMBOURG", "MALTA", "MARTINIQUE", "SAINT MARTIN (FRENCH PART)", "SAINT PIERRE AND MIQUELON",
                                                                                            "MAYOTTE", "SLOVAKIA", "SAN MARINO", "SLOVENIA", "MONACO", "MONTENEGRO", "NETHERLANDS (THE)",
                                                                                            "PORTUGAL", "RÉUNION", "SAINT BARTHÉLEMY", "SPAIN" });

        /// <summary>
        /// Lek
        /// </summary>
        public static readonly Currency_Id ALL = new ("ALL", 008, 2, "Lek", "ALBANIA");

        /// <summary>
        /// Algerian Dinar
        /// </summary>
        public static readonly Currency_Id DZD = new ("DZD", 012, 2, "Algerian Dinar", "ALGERIA");

        /// <summary>
        /// US Dollar
        /// </summary>
        public static readonly Currency_Id USD = new ("USD", 840, 2, "US Dollar", new String[] { "AMERICAN SAMOA", "BONAIRE, SINT EUSTATIUS AND SABA", "BRITISH INDIAN OCEAN TERRITORY (THE)",
                                                                                                 "ECUADOR", "EL SALVADOR", "GUAM", "HAITI", "MARSHALL ISLANDS (THE)", "TIMOR-LESTE",
                                                                                                 "MICRONESIA (FEDERATED STATES OF)", "NORTHERN MARIANA ISLANDS (THE)", "PALAU", "PANAMA",
                                                                                                 "PUERTO RICO", "UNITED STATES MINOR OUTLYING ISLANDS (THE)", "UNITED STATES OF AMERICA (THE)",
                                                                                                 "TURKS AND CAICOS ISLANDS (THE)", "VIRGIN ISLANDS (BRITISH)", "VIRGIN ISLANDS (U.S.)" });

        /// <summary>
        /// Kwanza
        /// </summary>
        public static readonly Currency_Id AOA = new ("AOA", 973, 2, "Kwanza", "ANGOLA");

        /// <summary>
        /// East Caribbean Dollar
        /// </summary>
        public static readonly Currency_Id XCD = new ("XCD", 951, 2, "East Caribbean Dollar", new String[] { "ANGUILLA", "ANTIGUA AND BARBUDA", "DOMINICA", "GRENADA", "MONTSERRAT",
                                                                                                             "SAINT KITTS AND NEVIS", "SAINT LUCIA", "SAINT VINCENT AND THE GRENADINES" });

        /// <summary>
        /// Argentine Peso
        /// </summary>
        public static readonly Currency_Id ARS = new ("ARS", 032, 2, "Argentine Peso", "ARGENTINA");

        /// <summary>
        /// Armenian Dram
        /// </summary>
        public static readonly Currency_Id AMD = new ("AMD", 051, 2, "Armenian Dram", "ARMENIA");

        /// <summary>
        /// Aruban Florin
        /// </summary>
        public static readonly Currency_Id AWG = new ("AWG", 533, 2, "Aruban Florin", "ARUBA");

        /// <summary>
        /// Australian Dollar
        /// </summary>
        public static readonly Currency_Id AUD = new ("AUD", 036, 2, "Australian Dollar", new String[] { "AUSTRALIA", "CHRISTMAS ISLAND", "COCOS (KEELING) ISLANDS (THE)", "NORFOLK ISLAND",
                                                                                                         "HEARD ISLAND AND McDONALD ISLANDS", "KIRIBATI", "NAURU", "TUVALU" });

        /// <summary>
        /// Azerbaijan Manat
        /// </summary>
        public static readonly Currency_Id AZN = new ("AZN", 944, 2, "Azerbaijan Manat", "AZERBAIJAN");

        /// <summary>
        /// Bahamian Dollar
        /// </summary>
        public static readonly Currency_Id BSD = new ("BSD", 044, 2, "Bahamian Dollar", "BAHAMAS (THE)");

        /// <summary>
        /// Bahraini Dinar
        /// </summary>
        public static readonly Currency_Id BHD = new ("BHD", 048, 3, "Bahraini Dinar", "BAHRAIN");

        /// <summary>
        /// Taka
        /// </summary>
        public static readonly Currency_Id BDT = new ("BDT", 050, 2, "Taka", "BANGLADESH");

        /// <summary>
        /// Barbados Dollar
        /// </summary>
        public static readonly Currency_Id BBD = new ("BBD", 052, 2, "Barbados Dollar", "BARBADOS");

        /// <summary>
        /// Belarusian Ruble
        /// </summary>
        public static readonly Currency_Id BYN = new ("BYN", 933, 2, "Belarusian Ruble", "BELARUS");

        /// <summary>
        /// Belize Dollar
        /// </summary>
        public static readonly Currency_Id BZD = new ("BZD", 084, 2, "Belize Dollar", "BELIZE");

        /// <summary>
        /// CFA Franc BCEAO
        /// </summary>
        public static readonly Currency_Id XOF = new ("XOF", 952, 0, "CFA Franc BCEAO", new String[] { "BENIN", "BURKINA FASO", "CÔTE D'IVOIRE", "GUINEA-BISSAU", "MALI", "NIGER (THE)",
                                                                                                       "SENEGAL", "TOGO" });

        /// <summary>
        /// Bermudian Dollar
        /// </summary>
        public static readonly Currency_Id BMD = new ("BMD", 060, 2, "Bermudian Dollar", "BERMUDA");

        /// <summary>
        /// Indian Rupee
        /// </summary>
        public static readonly Currency_Id INR = new ("INR", 356, 2, "Indian Rupee", new String[] { "BHUTAN", "INDIA" });

        /// <summary>
        /// Ngultrum
        /// </summary>
        public static readonly Currency_Id BTN = new ("BTN", 064, 2, "Ngultrum", "BHUTAN");

        /// <summary>
        /// Boliviano
        /// </summary>
        public static readonly Currency_Id BOB = new ("BOB", 068, 2, "Boliviano", "BOLIVIA (PLURINATIONAL STATE OF)");

        /// <summary>
        /// Mvdol
        /// </summary>
        public static readonly Currency_Id BOV = new ("BOV", 984, 2, "Mvdol", "BOLIVIA (PLURINATIONAL STATE OF)");

        /// <summary>
        /// Convertible Mark
        /// </summary>
        public static readonly Currency_Id BAM = new ("BAM", 977, 2, "Convertible Mark", "BOSNIA AND HERZEGOVINA");

        /// <summary>
        /// Pula
        /// </summary>
        public static readonly Currency_Id BWP = new ("BWP", 072, 2, "Pula", "BOTSWANA");

        /// <summary>
        /// Norwegian Krone
        /// </summary>
        public static readonly Currency_Id NOK = new ("NOK", 578, 2, "Norwegian Krone", new String[] { "BOUVET ISLAND", "SVALBARD AND JAN MAYEN", "NORWAY" });

        /// <summary>
        /// Brazilian Real
        /// </summary>
        public static readonly Currency_Id BRL = new ("BRL", 986, 2, "Brazilian Real", "BRAZIL");

        /// <summary>
        /// Brunei Dollar
        /// </summary>
        public static readonly Currency_Id BND = new ("BND", 096, 2, "Brunei Dollar", "BRUNEI DARUSSALAM");

        /// <summary>
        /// Bulgarian Lev
        /// </summary>
        public static readonly Currency_Id BGN = new ("BGN", 975, 2, "Bulgarian Lev", "BULGARIA");

        /// <summary>
        /// Burundi Franc
        /// </summary>
        public static readonly Currency_Id BIF = new ("BIF", 108, 0, "Burundi Franc", "BURUNDI");

        /// <summary>
        /// Cabo Verde Escudo
        /// </summary>
        public static readonly Currency_Id CVE = new ("CVE", 132, 2, "Cabo Verde Escudo", "CABO VERDE");

        /// <summary>
        /// Riel
        /// </summary>
        public static readonly Currency_Id KHR = new ("KHR", 116, 2, "Riel", "CAMBODIA");

        /// <summary>
        /// CFA Franc BEAC
        /// </summary>
        public static readonly Currency_Id XAF = new ("XAF", 950, 0, "CFA Franc BEAC", new String[] { "CAMEROON", "CENTRAL AFRICAN REPUBLIC (THE)", "CHAD", "CONGO (THE)", "EQUATORIAL GUINEA", "GABON" });

        /// <summary>
        /// Canadian Dollar
        /// </summary>
        public static readonly Currency_Id CAD = new ("CAD", 124, 2, "Canadian Dollar", "CANADA");

        /// <summary>
        /// Cayman Islands Dollar
        /// </summary>
        public static readonly Currency_Id KYD = new ("KYD", 136, 2, "Cayman Islands Dollar", "CAYMAN ISLANDS (THE)");

        /// <summary>
        /// Chilean Peso
        /// </summary>
        public static readonly Currency_Id CLP = new ("CLP", 152, 0, "Chilean Peso", "CHILE");

        /// <summary>
        /// Unidad de Fomento
        /// </summary>
        public static readonly Currency_Id CLF = new ("CLF", 990, 4, "Unidad de Fomento", "CHILE");

        /// <summary>
        /// Yuan Renminbi
        /// </summary>
        public static readonly Currency_Id CNY = new ("CNY", 156, 2, "Yuan Renminbi", "CHINA");

        /// <summary>
        /// Colombian Peso
        /// </summary>
        public static readonly Currency_Id COP = new ("COP", 170, 2, "Colombian Peso", "COLOMBIA");

        /// <summary>
        /// Unidad de Valor Real
        /// </summary>
        public static readonly Currency_Id COU = new ("COU", 970, 2, "Unidad de Valor Real", "COLOMBIA");

        /// <summary>
        /// Comorian Franc
        /// </summary>
        public static readonly Currency_Id KMF = new ("KMF", 174, 0, "Comorian Franc", "COMOROS (THE)");

        /// <summary>
        /// Congolese Franc
        /// </summary>
        public static readonly Currency_Id CDF = new ("CDF", 976, 2, "Congolese Franc", "CONGO (THE DEMOCRATIC REPUBLIC OF THE)");

        /// <summary>
        /// New Zealand Dollar
        /// </summary>
        public static readonly Currency_Id NZD = new ("NZD", 554, 2, "New Zealand Dollar", new String[] { "COOK ISLANDS (THE)", "NIUE", "NEW ZEALAND", "PITCAIRN", "TOKELAU" });
        public static readonly Currency_Id CRC = new ("CRC", 188, 2, "Costa Rican Colon", "COSTA RICA");
        public static readonly Currency_Id HRK = new ("HRK", 191, 2, "Kuna", "CROATIA");
        public static readonly Currency_Id CUP = new ("CUP", 192, 2, "Cuban Peso", "CUBA");
        public static readonly Currency_Id CUC = new ("CUC", 931, 2, "Peso Convertible", "CUBA");
        public static readonly Currency_Id ANG = new ("ANG", 532, 2, "Netherlands Antillean Guilder", new String[] { "SINT MAARTEN (DUTCH PART)", "CURAÇAO" });
        public static readonly Currency_Id CZK = new ("CZK", 203, 2, "Czech Koruna", "CZECHIA");
        public static readonly Currency_Id DKK = new ("DKK", 208, 2, "Danish Krone", new String[] { "DENMARK", "FAROE ISLANDS (THE)", "GREENLAND" });
        public static readonly Currency_Id DJF = new ("DJF", 262, 0, "Djibouti Franc", "DJIBOUTI");
        public static readonly Currency_Id DOP = new ("DOP", 214, 2, "Dominican Peso", "DOMINICAN REPUBLIC (THE)");
        public static readonly Currency_Id EGP = new ("EGP", 818, 2, "Egyptian Pound", "EGYPT");
        public static readonly Currency_Id SVC = new ("SVC", 222, 2, "El Salvador Colon", "EL SALVADOR");
        public static readonly Currency_Id ERN = new ("ERN", 232, 2, "Nakfa", "ERITREA");
        public static readonly Currency_Id SZL = new ("SZL", 748, 2, "Lilangeni", "ESWATINI");
        public static readonly Currency_Id ETB = new ("ETB", 230, 2, "Ethiopian Birr", "ETHIOPIA");
        public static readonly Currency_Id FKP = new ("FKP", 238, 2, "Falkland Islands Pound", "FALKLAND ISLANDS (THE) [MALVINAS]");
        public static readonly Currency_Id FJD = new ("FJD", 242, 2, "Fiji Dollar", "FIJI");
        public static readonly Currency_Id XPF = new ("XPF", 953, 0, "CFP Franc", new String[] {"FRENCH POLYNESIA", "NEW CALEDONIA", "WALLIS AND FUTUNA" });
        public static readonly Currency_Id GMD = new ("GMD", 270, 2, "Dalasi", "GAMBIA (THE)");
        public static readonly Currency_Id GEL = new ("GEL", 981, 2, "Lari", "GEORGIA");
        public static readonly Currency_Id GHS = new ("GHS", 936, 2, "Ghana Cedi", "GHANA");
        public static readonly Currency_Id GIP = new ("GIP", 292, 2, "Gibraltar Pound", "GIBRALTAR");
        public static readonly Currency_Id GTQ = new ("GTQ", 320, 2, "Quetzal", "GUATEMALA");
        public static readonly Currency_Id GBP = new ("GBP", 826, 2, "Pound Sterling", new String[] { "UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND (THE)", "GUERNSEY", "ISLE OF MAN", "JERSEY" });
        public static readonly Currency_Id GNF = new ("GNF", 324, 0, "Guinean Franc", "GUINEA");
        public static readonly Currency_Id GYD = new ("GYD", 328, 2, "Guyana Dollar", "GUYANA");
        public static readonly Currency_Id HTG = new ("HTG", 332, 2, "Gourde", "HAITI");
        public static readonly Currency_Id HNL = new ("HNL", 340, 2, "Lempira", "HONDURAS");
        public static readonly Currency_Id HKD = new ("HKD", 344, 2, "Hong Kong Dollar", "HONG KONG");
        public static readonly Currency_Id HUF = new ("HUF", 348, 2, "Forint", "HUNGARY");
        public static readonly Currency_Id ISK = new ("ISK", 352, 0, "Iceland Krona", "ICELAND");
        public static readonly Currency_Id IDR = new ("IDR", 360, 2, "Rupiah", "INDONESIA");
        public static readonly Currency_Id XDR = new ("XDR", 960, null, "SDR (Special Drawing Right)", "INTERNATIONAL MONETARY FUND (IMF) ");
        public static readonly Currency_Id IRR = new ("IRR", 364, 2, "Iranian Rial", "IRAN (ISLAMIC REPUBLIC OF)");
        public static readonly Currency_Id IQD = new ("IQD", 368, 3, "Iraqi Dinar", "IRAQ");
        public static readonly Currency_Id ILS = new ("ILS", 376, 2, "New Israeli Sheqel", "ISRAEL");
        public static readonly Currency_Id JMD = new ("JMD", 388, 2, "Jamaican Dollar", "JAMAICA");
        public static readonly Currency_Id JPY = new ("JPY", 392, 0, "Yen", "JAPAN");
        public static readonly Currency_Id JOD = new ("JOD", 400, 3, "Jordanian Dinar", "JORDAN");
        public static readonly Currency_Id KZT = new ("KZT", 398, 2, "Tenge", "KAZAKHSTAN");
        public static readonly Currency_Id KES = new ("KES", 404, 2, "Kenyan Shilling", "KENYA");
        public static readonly Currency_Id KPW = new ("KPW", 408, 2, "North Korean Won", "KOREA (THE DEMOCRATIC PEOPLE’S REPUBLIC OF)");
        public static readonly Currency_Id KRW = new ("KRW", 410, 0, "Won", "KOREA (THE REPUBLIC OF)");
        public static readonly Currency_Id KWD = new ("KWD", 414, 3, "Kuwaiti Dinar", "KUWAIT");
        public static readonly Currency_Id KGS = new ("KGS", 417, 2, "Som", "KYRGYZSTAN");
        public static readonly Currency_Id LAK = new ("LAK", 418, 2, "Lao Kip", "LAO PEOPLE’S DEMOCRATIC REPUBLIC (THE)");
        public static readonly Currency_Id LBP = new ("LBP", 422, 2, "Lebanese Pound", "LEBANON");
        public static readonly Currency_Id LSL = new ("LSL", 426, 2, "Loti", "LESOTHO");
        public static readonly Currency_Id ZAR = new ("ZAR", 710, 2, "Rand", new String[] { "LESOTHO", "NAMIBIA", "SOUTH AFRICA" });
        public static readonly Currency_Id LRD = new ("LRD", 430, 2, "Liberian Dollar", "LIBERIA");
        public static readonly Currency_Id LYD = new ("LYD", 434, 3, "Libyan Dinar", "LIBYA");
        public static readonly Currency_Id CHF = new ("CHF", 756, 2, "Swiss Franc", new String[] { "SWITZERLAND", "LIECHTENSTEIN" });
        public static readonly Currency_Id MOP = new ("MOP", 446, 2, "Pataca", "MACAO");
        public static readonly Currency_Id MKD = new ("MKD", 807, 2, "Denar", "NORTH MACEDONIA");
        public static readonly Currency_Id MGA = new ("MGA", 969, 2, "Malagasy Ariary", "MADAGASCAR");
        public static readonly Currency_Id MWK = new ("MWK", 454, 2, "Malawi Kwacha", "MALAWI");
        public static readonly Currency_Id MYR = new ("MYR", 458, 2, "Malaysian Ringgit", "MALAYSIA");
        public static readonly Currency_Id MVR = new ("MVR", 462, 2, "Rufiyaa", "MALDIVES");
        public static readonly Currency_Id MRU = new ("MRU", 929, 2, "Ouguiya", "MAURITANIA");
        public static readonly Currency_Id MUR = new ("MUR", 480, 2, "Mauritius Rupee", "MAURITIUS");
        public static readonly Currency_Id XUA = new ("XUA", 965, null, "ADB Unit of Account", "MEMBER COUNTRIES OF THE AFRICAN DEVELOPMENT BANK GROUP");
        public static readonly Currency_Id MXN = new ("MXN", 484, 2, "Mexican Peso", "MEXICO");
        public static readonly Currency_Id MXV = new ("MXV", 979, 2, "Mexican Unidad de Inversion (UDI)", "MEXICO");
        public static readonly Currency_Id MDL = new ("MDL", 498, 2, "Moldovan Leu", "MOLDOVA (THE REPUBLIC OF)");
        public static readonly Currency_Id MNT = new ("MNT", 496, 2, "Tugrik", "MONGOLIA");
        public static readonly Currency_Id MAD = new ("MAD", 504, 2, "Moroccan Dirham", new String[] { "MOROCCO", "WESTERN SAHARA" });
        public static readonly Currency_Id MZN = new ("MZN", 943, 2, "Mozambique Metical", "MOZAMBIQUE");
        public static readonly Currency_Id MMK = new ("MMK", 104, 2, "Kyat", "MYANMAR");
        public static readonly Currency_Id NAD = new ("NAD", 516, 2, "Namibia Dollar", "NAMIBIA");
        public static readonly Currency_Id NPR = new ("NPR", 524, 2, "Nepalese Rupee", "NEPAL");
        public static readonly Currency_Id NIO = new ("NIO", 558, 2, "Cordoba Oro", "NICARAGUA");
        public static readonly Currency_Id NGN = new ("NGN", 566, 2, "Naira", "NIGERIA");
        public static readonly Currency_Id OMR = new ("OMR", 512, 3, "Rial Omani", "OMAN");
        public static readonly Currency_Id PKR = new ("PKR", 586, 2, "Pakistan Rupee", "PAKISTAN");
        public static readonly Currency_Id PAB = new ("PAB", 590, 2, "Balboa", "PANAMA");
        public static readonly Currency_Id PGK = new ("PGK", 598, 2, "Kina", "PAPUA NEW GUINEA");
        public static readonly Currency_Id PYG = new ("PYG", 600, 0, "Guarani", "PARAGUAY");
        public static readonly Currency_Id PEN = new ("PEN", 604, 2, "Sol", "PERU");
        public static readonly Currency_Id PHP = new ("PHP", 608, 2, "Philippine Peso", "PHILIPPINES (THE)");
        public static readonly Currency_Id PLN = new ("PLN", 985, 2, "Zloty", "POLAND");
        public static readonly Currency_Id QAR = new ("QAR", 634, 2, "Qatari Rial", "QATAR");
        public static readonly Currency_Id RON = new ("RON", 946, 2, "Romanian Leu", "ROMANIA");
        public static readonly Currency_Id RUB = new ("RUB", 643, 2, "Russian Ruble", "RUSSIAN FEDERATION (THE)");
        public static readonly Currency_Id RWF = new ("RWF", 646, 0, "Rwanda Franc", "RWANDA");
        public static readonly Currency_Id SHP = new ("SHP", 654, 2, "Saint Helena Pound", "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA");
        public static readonly Currency_Id WST = new ("WST", 882, 2, "Tala", "SAMOA");
        public static readonly Currency_Id STN = new ("STN", 930, 2, "Dobra", "SAO TOME AND PRINCIPE");
        public static readonly Currency_Id SAR = new ("SAR", 682, 2, "Saudi Riyal", "SAUDI ARABIA");
        public static readonly Currency_Id RSD = new ("RSD", 941, 2, "Serbian Dinar", "SERBIA");
        public static readonly Currency_Id SCR = new ("SCR", 690, 2, "Seychelles Rupee", "SEYCHELLES");
        public static readonly Currency_Id SLL = new ("SLL", 694, 2, "Leone", "SIERRA LEONE");
        public static readonly Currency_Id SLE = new ("SLE", 925, 2, "Leone", "SIERRA LEONE");
        public static readonly Currency_Id SGD = new ("SGD", 702, 2, "Singapore Dollar", "SINGAPORE");
        public static readonly Currency_Id XSU = new ("XSU", 994, null, "Sucre", "SISTEMA UNITARIO DE COMPENSACION REGIONAL DE PAGOS 'SUCRE'");
        public static readonly Currency_Id SBD = new ("SBD", 090, 2, "Solomon Islands Dollar", "SOLOMON ISLANDS");
        public static readonly Currency_Id SOS = new ("SOS", 706, 2, "Somali Shilling", "SOMALIA");
        public static readonly Currency_Id SSP = new ("SSP", 728, 2, "South Sudanese Pound", "SOUTH SUDAN");
        public static readonly Currency_Id LKR = new ("LKR", 144, 2, "Sri Lanka Rupee", "SRI LANKA");
        public static readonly Currency_Id SDG = new ("SDG", 938, 2, "Sudanese Pound", "SUDAN (THE)");
        public static readonly Currency_Id SRD = new ("SRD", 968, 2, "Surinam Dollar", "SURINAME");
        public static readonly Currency_Id SEK = new ("SEK", 752, 2, "Swedish Krona", "SWEDEN");
        public static readonly Currency_Id CHE = new ("CHE", 947, 2, "WIR Euro", "SWITZERLAND");
        public static readonly Currency_Id CHW = new ("CHW", 948, 2, "WIR Franc", "SWITZERLAND");
        public static readonly Currency_Id SYP = new ("SYP", 760, 2, "Syrian Pound", "SYRIAN ARAB REPUBLIC");
        public static readonly Currency_Id TWD = new ("TWD", 901, 2, "New Taiwan Dollar", "TAIWAN (PROVINCE OF CHINA)");
        public static readonly Currency_Id TJS = new ("TJS", 972, 2, "Somoni", "TAJIKISTAN");
        public static readonly Currency_Id TZS = new ("TZS", 834, 2, "Tanzanian Shilling", "TANZANIA, UNITED REPUBLIC OF");
        public static readonly Currency_Id THB = new ("THB", 764, 2, "Baht", "THAILAND");
        public static readonly Currency_Id TOP = new ("TOP", 776, 2, "Pa’anga", "TONGA");
        public static readonly Currency_Id TTD = new ("TTD", 780, 2, "Trinidad and Tobago Dollar", "TRINIDAD AND TOBAGO");
        public static readonly Currency_Id TND = new ("TND", 788, 3, "Tunisian Dinar", "TUNISIA");
        public static readonly Currency_Id TRY = new ("TRY", 949, 2, "Turkish Lira", "TURKEY");
        public static readonly Currency_Id TMT = new ("TMT", 934, 2, "Turkmenistan New Manat", "TURKMENISTAN");
        public static readonly Currency_Id UGX = new ("UGX", 800, 0, "Uganda Shilling", "UGANDA");
        public static readonly Currency_Id UAH = new ("UAH", 980, 2, "Hryvnia", "UKRAINE");
        public static readonly Currency_Id AED = new ("AED", 784, 2, "UAE Dirham", "UNITED ARAB EMIRATES (THE)");
        public static readonly Currency_Id USN = new ("USN", 997, 2, "US Dollar (Next day)", "UNITED STATES OF AMERICA (THE)");
        public static readonly Currency_Id UYU = new ("UYU", 858, 2, "Peso Uruguayo", "URUGUAY");
        public static readonly Currency_Id UYI = new ("UYI", 940, 0, "Uruguay Peso en Unidades Indexadas (UI)", "URUGUAY");
        public static readonly Currency_Id UYW = new ("UYW", 927, 4, "Unidad Previsional", "URUGUAY");
        public static readonly Currency_Id UZS = new ("UZS", 860, 2, "Uzbekistan Sum", "UZBEKISTAN");
        public static readonly Currency_Id VUV = new ("VUV", 548, 0, "Vatu", "VANUATU");
        public static readonly Currency_Id VES = new ("VES", 928, 2, "Bolívar Soberano", "VENEZUELA (BOLIVARIAN REPUBLIC OF)");
        public static readonly Currency_Id VED = new ("VED", 926, 2, "Bolívar Soberano", "VENEZUELA (BOLIVARIAN REPUBLIC OF)");
        public static readonly Currency_Id VND = new ("VND", 704, 0, "Dong", "VIET NAM");
        public static readonly Currency_Id YER = new ("YER", 886, 2, "Yemeni Rial", "YEMEN");
        public static readonly Currency_Id ZMW = new ("ZMW", 967, 2, "Zambian Kwacha", "ZAMBIA");
        public static readonly Currency_Id ZWL = new ("ZWL", 932, 2, "Zimbabwe Dollar", "ZIMBABWE");

        public static readonly Currency_Id XBA = new ("XBA", 955, null, "Bond Markets Unit European Composite Unit (EURCO)", "ZZ01_Bond Markets Unit European_EURCO");
        public static readonly Currency_Id XBB = new ("XBB", 956, null, "Bond Markets Unit European Monetary Unit (E.M.U.-6)", "ZZ02_Bond Markets Unit European_EMU-6");
        public static readonly Currency_Id XBC = new ("XBC", 957, null, "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", "ZZ03_Bond Markets Unit European_EUA-9");
        public static readonly Currency_Id XBD = new ("XBD", 958, null, "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", "ZZ04_Bond Markets Unit European_EUA-17");

        /// <summary>
        /// Codes specifically reserved for testing purposes
        /// </summary>
        public static readonly Currency_Id XTS = new ("XTS", 963, null, "Codes specifically reserved for testing purposes", "ZZ06_Testing_Code");

        /// <summary>
        /// The codes assigned for transactions where no currency is involved
        /// </summary>
        public static readonly Currency_Id XXX = new ("XXX", 999, null, "The codes assigned for transactions where no currency is involved", "ZZ07_No_Currency");

        /// <summary>
        /// Gold
        /// </summary>
        public static readonly Currency_Id XAU = new ("XAU", 959, null, "Gold", "ZZ08_Gold");

        /// <summary>
        /// Palladium
        /// </summary>
        public static readonly Currency_Id XPD = new ("XPD", 964, null, "Palladium", "ZZ09_Palladium");

        /// <summary>
        /// Platinum
        /// </summary>
        public static readonly Currency_Id XPT = new ("XPT", 962, null, "Platinum", "ZZ10_Platinum");

        /// <summary>
        /// Silver
        /// </summary>
        public static readonly Currency_Id XAG = new ("XAG", 961, null, "Silver", "ZZ11_Silver");


        #region Operator overloading

        #region Operator == (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.Equals(CurrencyId2);

        #endregion

        #region Operator != (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => !CurrencyId1.Equals(CurrencyId2);

        #endregion

        #region Operator <  (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Currency_Id CurrencyId1,
                                          Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) < 0;

        #endregion

        #region Operator <= (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) <= 0;

        #endregion

        #region Operator >  (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Currency_Id CurrencyId1,
                                          Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) > 0;

        #endregion

        #region Operator >= (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) >= 0;

        #endregion

        #endregion

        #region IComparable<CurrencyId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two currency identifications.
        /// </summary>
        /// <param name="Object">A currency identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Currency_Id currencyId
                   ? CompareTo(currencyId)
                   : throw new ArgumentException("The given object is not a currency identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CurrencyId)

        /// <summary>
        /// Compares two currency identifications.
        /// </summary>
        /// <param name="CurrencyId">A currency identification to compare with.</param>
        public Int32 CompareTo(Currency_Id CurrencyId)

            => String.Compare(InternalId,
                              CurrencyId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CurrencyId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two currency identifications for equality.
        /// </summary>
        /// <param name="Object">A currency identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Currency_Id currencyId &&
                   Equals(currencyId);

        #endregion

        #region Equals(CurrencyId)

        /// <summary>
        /// Compares two currency identifications for equality.
        /// </summary>
        /// <param name="CurrencyId">A currency identification to compare with.</param>
        public Boolean Equals(Currency_Id CurrencyId)

            => String.Equals(InternalId,
                             CurrencyId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
