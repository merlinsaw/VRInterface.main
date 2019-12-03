//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System.Collections.Generic;

#endregion

public static class CountryHelper
{

  private readonly static _Logger log = new _Logger(typeof(CountryHelper));

  private static Dictionary<string, string> iso2CodeToCountryNameMapping = new Dictionary<string, string>();
  private static Dictionary<string, string> countryNameToIso2CodeMapping = new Dictionary<string, string>();

  static CountryHelper()
  {
	AddCountry("AFGHANISTAN", "AF");
	AddCountry("ÅLAND ISLANDS", "AX");
	AddCountry("ALBANIA", "AL");
	AddCountry("ALGERIA", "DZ");
	AddCountry("AMERICAN SAMOA", "AS");
	AddCountry("ANDORRA", "AD");
	AddCountry("ANGOLA", "AO");
	AddCountry("ANGUILLA", "AI");
	AddCountry("ANTARCTICA", "AQ");
	AddCountry("ANTIGUA AND BARBUDA", "AG");
	AddCountry("ARGENTINA", "AR");
	AddCountry("ARMENIA", "AM");
	AddCountry("ARUBA", "AW");
	AddCountry("AUSTRALIA", "AU");
	AddCountry("AUSTRIA", "AT");
	AddCountry("AZERBAIJAN", "AZ");
	AddCountry("BAHAMAS", "BS");
	AddCountry("BAHRAIN", "BH");
	AddCountry("BANGLADESH", "BD");
	AddCountry("BARBADOS", "BB");
	AddCountry("BELARUS", "BY");
	AddCountry("BELGIUM", "BE");
	AddCountry("BELIZE", "BZ");
	AddCountry("BENIN", "BJ");
	AddCountry("BERMUDA", "BM");
	AddCountry("BHUTAN", "BT");
	AddCountry("BOLIVIA, PLURINATIONAL STATE OF", "BO");
	AddCountry("BONAIRE, SINT EUSTATIUS AND SABA", "BQ");
	AddCountry("BOSNIA AND HERZEGOVINA", "BA");
	AddCountry("BOTSWANA", "BW");
	AddCountry("BOUVET ISLAND", "BV");
	AddCountry("BRAZIL", "BR");
	AddCountry("BRITISH INDIAN OCEAN TERRITORY", "IO");
	AddCountry("BRUNEI DARUSSALAM", "BN");
	AddCountry("BULGARIA", "BG");
	AddCountry("BURKINA FASO", "BF");
	AddCountry("BURUNDI", "BI");
	AddCountry("CAMBODIA", "KH");
	AddCountry("CAMEROON", "CM");
	AddCountry("CANADA", "CA");
	AddCountry("CAPE VERDE", "CV");
	AddCountry("CAYMAN ISLANDS", "KY");
	AddCountry("CENTRAL AFRICAN REPUBLIC", "CF");
	AddCountry("CHAD", "TD");
	AddCountry("CHILE", "CL");
	AddCountry("CHINA", "CN");
	AddCountry("CHRISTMAS ISLAND", "CX");
	AddCountry("COCOS (KEELING) ISLANDS", "CC");
	AddCountry("COLOMBIA", "CO");
	AddCountry("COMOROS", "KM");
	AddCountry("CONGO", "CG");
	AddCountry("CONGO, THE DEMOCRATIC REPUBLIC OF THE", "CD");
	AddCountry("COOK ISLANDS", "CK");
	AddCountry("COSTA RICA", "CR");
	AddCountry("CÔTE D'IVOIRE", "CI");
	AddCountry("CROATIA", "HR");
	AddCountry("CUBA", "CU");
	AddCountry("CURAÇAO", "CW");
	AddCountry("CYPRUS", "CY");
	AddCountry("CZECH REPUBLIC", "CZ");
	AddCountry("DENMARK", "DK");
	AddCountry("DJIBOUTI", "DJ");
	AddCountry("DOMINICA", "DM");
	AddCountry("DOMINICAN REPUBLIC", "DO");
	AddCountry("ECUADOR", "EC");
	AddCountry("EGYPT", "EG");
	AddCountry("EL SALVADOR", "SV");
	AddCountry("EQUATORIAL GUINEA", "GQ");
	AddCountry("ERITREA", "ER");
	AddCountry("ESTONIA", "EE");
	AddCountry("ETHIOPIA", "ET");
	AddCountry("FALKLAND ISLANDS (MALVINAS)", "FK");
	AddCountry("FAROE ISLANDS", "FO");
	AddCountry("FIJI", "FJ");
	AddCountry("FINLAND", "FI");
	AddCountry("FRANCE", "FR");
	AddCountry("FRENCH GUIANA", "GF");
	AddCountry("FRENCH POLYNESIA", "PF");
	AddCountry("FRENCH SOUTHERN TERRITORIES", "TF");
	AddCountry("GABON", "GA");
	AddCountry("GAMBIA", "GM");
	AddCountry("GEORGIA", "GE");
	AddCountry("GERMANY", "DE");
	AddCountry("GHANA", "GH");
	AddCountry("GIBRALTAR", "GI");
	AddCountry("GREECE", "GR");
	AddCountry("GREENLAND", "GL");
	AddCountry("GRENADA", "GD");
	AddCountry("GUADELOUPE", "GP");
	AddCountry("GUAM", "GU");
	AddCountry("GUATEMALA", "GT");
	AddCountry("GUERNSEY", "GG");
	AddCountry("GUINEA", "GN");
	AddCountry("GUINEA-BISSAU", "GW");
	AddCountry("GUYANA", "GY");
	AddCountry("HAITI", "HT");
	AddCountry("HEARD ISLAND AND MCDONALD ISLANDS", "HM");
	AddCountry("HOLY SEE (VATICAN CITY STATE)", "VA");
	AddCountry("HONDURAS", "HN");
	AddCountry("HONG KONG", "HK");
	AddCountry("HUNGARY", "HU");
	AddCountry("ICELAND", "IS");
	AddCountry("INDIA", "IN");
	AddCountry("INDONESIA", "ID");
	AddCountry("IRAN, ISLAMIC REPUBLIC OF", "IR");
	AddCountry("IRAQ", "IQ");
	AddCountry("IRELAND", "IE");
	AddCountry("ISLE OF MAN", "IM");
	AddCountry("ISRAEL", "IL");
	AddCountry("ITALY", "IT");
	AddCountry("JAMAICA", "JM");
	AddCountry("JAPAN", "JP");
	AddCountry("JERSEY", "JE");
	AddCountry("JORDAN", "JO");
	AddCountry("KAZAKHSTAN", "KZ");
	AddCountry("KENYA", "KE");
	AddCountry("KIRIBATI", "KI");
	AddCountry("KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", "KP");
	AddCountry("KOREA, REPUBLIC OF", "KR");
	AddCountry("KUWAIT", "KW");
	AddCountry("KYRGYZSTAN", "KG");
	AddCountry("LAO PEOPLE'S DEMOCRATIC REPUBLIC", "LA");
	AddCountry("LATVIA", "LV");
	AddCountry("LEBANON", "LB");
	AddCountry("LESOTHO", "LS");
	AddCountry("LIBERIA", "LR");
	AddCountry("LIBYA", "LY");
	AddCountry("LIECHTENSTEIN", "LI");
	AddCountry("LITHUANIA", "LT");
	AddCountry("LUXEMBOURG", "LU");
	AddCountry("MACAO", "MO");
	AddCountry("MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", "MK");
	AddCountry("MADAGASCAR", "MG");
	AddCountry("MALAWI", "MW");
	AddCountry("MALAYSIA", "MY");
	AddCountry("MALDIVES", "MV");
	AddCountry("MALI", "ML");
	AddCountry("MALTA", "MT");
	AddCountry("MARSHALL ISLANDS", "MH");
	AddCountry("MARTINIQUE", "MQ");
	AddCountry("MAURITANIA", "MR");
	AddCountry("MAURITIUS", "MU");
	AddCountry("MAYOTTE", "YT");
	AddCountry("MEXICO", "MX");
	AddCountry("MICRONESIA, FEDERATED STATES OF", "FM");
	AddCountry("MOLDOVA, REPUBLIC OF", "MD");
	AddCountry("MONACO", "MC");
	AddCountry("MONGOLIA", "MN");
	AddCountry("MONTENEGRO", "ME");
	AddCountry("MONTSERRAT", "MS");
	AddCountry("MOROCCO", "MA");
	AddCountry("MOZAMBIQUE", "MZ");
	AddCountry("MYANMAR", "MM");
	AddCountry("NAMIBIA", "NA");
	AddCountry("NAURU", "NR");
	AddCountry("NEPAL", "NP");
	AddCountry("NETHERLANDS", "NL");
	AddCountry("NEW CALEDONIA", "NC");
	AddCountry("NEW ZEALAND", "NZ");
	AddCountry("NICARAGUA", "NI");
	AddCountry("NIGER", "NE");
	AddCountry("NIGERIA", "NG");
	AddCountry("NIUE", "NU");
	AddCountry("NORFOLK ISLAND", "NF");
	AddCountry("NORTHERN MARIANA ISLANDS", "MP");
	AddCountry("NORWAY", "NO");
	AddCountry("OMAN", "OM");
	AddCountry("PAKISTAN", "PK");
	AddCountry("PALAU", "PW");
	AddCountry("PALESTINIAN TERRITORY, OCCUPIED", "PS");
	AddCountry("PANAMA", "PA");
	AddCountry("PAPUA NEW GUINEA", "PG");
	AddCountry("PARAGUAY", "PY");
	AddCountry("PERU", "PE");
	AddCountry("PHILIPPINES", "PH");
	AddCountry("PITCAIRN", "PN");
	AddCountry("POLAND", "PL");
	AddCountry("PORTUGAL", "PT");
	AddCountry("PUERTO RICO", "PR");
	AddCountry("QATAR", "QA");
	AddCountry("RÉUNION", "RE");
	AddCountry("ROMANIA", "RO");
	AddCountry("RUSSIAN FEDERATION", "RU");
	AddCountry("RWANDA", "RW");
	AddCountry("SAINT BARTHÉLEMY", "BL");
	AddCountry("SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA", "SH");
	AddCountry("SAINT KITTS AND NEVIS", "KN");
	AddCountry("SAINT LUCIA", "LC");
	AddCountry("SAINT MARTIN (FRENCH PART)", "MF");
	AddCountry("SAINT PIERRE AND MIQUELON", "PM");
	AddCountry("SAINT VINCENT AND THE GRENADINES", "VC");
	AddCountry("SAMOA", "WS");
	AddCountry("SAN MARINO", "SM");
	AddCountry("SAO TOME AND PRINCIPE", "ST");
	AddCountry("SAUDI ARABIA", "SA");
	AddCountry("SENEGAL", "SN");
	AddCountry("SERBIA", "RS");
	AddCountry("SEYCHELLES", "SC");
	AddCountry("SIERRA LEONE", "SL");
	AddCountry("SINGAPORE", "SG");
	AddCountry("SINT MAARTEN (DUTCH PART)", "SX");
	AddCountry("SLOVAKIA", "SK");
	AddCountry("SLOVENIA", "SI");
	AddCountry("SOLOMON ISLANDS", "SB");
	AddCountry("SOMALIA", "SO");
	AddCountry("SOUTH AFRICA", "ZA");
	AddCountry("SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS", "GS");
	AddCountry("SOUTH SUDAN", "SS");
	AddCountry("SPAIN", "ES");
	AddCountry("SRI LANKA", "LK");
	AddCountry("SUDAN", "SD");
	AddCountry("SURINAME", "SR");
	AddCountry("SVALBARD AND JAN MAYEN", "SJ");
	AddCountry("SWAZILAND", "SZ");
	AddCountry("SWEDEN", "SE");
	AddCountry("SWITZERLAND", "CH");
	AddCountry("SYRIAN ARAB REPUBLIC", "SY");
	AddCountry("TAIWAN, PROVINCE OF CHINA", "TW");
	AddCountry("TAJIKISTAN", "TJ");
	AddCountry("TANZANIA, UNITED REPUBLIC OF", "TZ");
	AddCountry("THAILAND", "TH");
	AddCountry("TIMOR-LESTE", "TL");
	AddCountry("TOGO", "TG");
	AddCountry("TOKELAU", "TK");
	AddCountry("TONGA", "TO");
	AddCountry("TRINIDAD AND TOBAGO", "TT");
	AddCountry("TUNISIA", "TN");
	AddCountry("TURKEY", "TR");
	AddCountry("TURKMENISTAN", "TM");
	AddCountry("TURKS AND CAICOS ISLANDS", "TC");
	AddCountry("TUVALU", "TV");
	AddCountry("UGANDA", "UG");
	AddCountry("UKRAINE", "UA");
	AddCountry("UNITED ARAB EMIRATES", "AE");
	AddCountry("UNITED KINGDOM", "GB");
	AddCountry("UNITED STATES", "US");
	AddCountry("UNITED STATES MINOR OUTLYING ISLANDS", "UM");
	AddCountry("URUGUAY", "UY");
	AddCountry("UZBEKISTAN", "UZ");
	AddCountry("VANUATU", "VU");
	AddCountry("VENEZUELA, BOLIVARIAN REPUBLIC OF", "VE");
	AddCountry("VIET NAM", "VN");
	AddCountry("VIRGIN ISLANDS, BRITISH", "VG");
	AddCountry("VIRGIN ISLANDS, U.S.", "VI");
	AddCountry("WALLIS AND FUTUNA", "WF");
	AddCountry("WESTERN SAHARA", "EH");
	AddCountry("YEMEN", "YE");
	AddCountry("ZAMBIA", "ZM");
	AddCountry("ZIMBABWE", "ZW");
  }

  private static void AddCountry(string countryName, string iso2Code)
  {
	// convert iso code to lower case
	iso2Code = iso2Code.ToLower();

	// convert country name
	string[] parts = countryName.Split(' ');
	countryName = "";
	foreach (string part in parts)
	{
	  if (part.Length > 1)
	  {
		char first = char.ToUpper(part[0]);
		string rest = part.Substring(1).ToLower();
		countryName += first.ToString() + rest.ToString() + " ";
	  }
	}
	countryName = countryName.Trim();

	iso2CodeToCountryNameMapping[iso2Code] = countryName;
	countryNameToIso2CodeMapping[countryName] = iso2Code;
  }

  public static string GetCountryNameByIso2Code(string iso2Code)
  {
	string countryName = "";
	if (!iso2CodeToCountryNameMapping.TryGetValue(iso2Code, out countryName))
	{
	  log.Warn(_Logger.User.Msaw, "There is no country known by iso code: " + iso2Code);
	}
	return countryName;
  }

  public static string GetIso2CodeByCountryName(string countryName)
  {
	string iso2Code = "";
	if (!countryNameToIso2CodeMapping.TryGetValue(countryName, out iso2Code))
	{
	  log.Warn(_Logger.User.Msaw, "There is no iso code known for country: " + countryName);
	}
	return iso2Code;
  }
}
