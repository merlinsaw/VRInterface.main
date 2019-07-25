//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System.Collections.Generic;
using UnityEngine;

#endregion

public static class LanguageHelper
{

  private static Dictionary<SystemLanguage, string> SystemLanguageToIso2Mapping { get; set; }
  private static Dictionary<string, SystemLanguage> Iso2ToSystemLanguageMapping { get; set; }

  public static string FallbackIsoCode { get; set; }
  public static SystemLanguage FallbackSystemLanguage { get; set; }

  static LanguageHelper()
  {
	FallbackIsoCode = "en";
	FallbackSystemLanguage = SystemLanguage.English;
	SystemLanguageToIso2Mapping = new Dictionary<SystemLanguage, string>();
	Iso2ToSystemLanguageMapping = new Dictionary<string, SystemLanguage>();

	// create mapping
	AddMapping(SystemLanguage.Afrikaans, "af");
	AddMapping(SystemLanguage.Arabic, "ar");
	AddMapping(SystemLanguage.Basque, "eu");
	AddMapping(SystemLanguage.Belarusian, "by");
	AddMapping(SystemLanguage.Bulgarian, "bg");
	AddMapping(SystemLanguage.Catalan, "ca");
	AddMapping(SystemLanguage.Chinese, "zh");
	AddMapping(SystemLanguage.Czech, "cs");
	AddMapping(SystemLanguage.Danish, "da");
	AddMapping(SystemLanguage.Dutch, "nl");
	AddMapping(SystemLanguage.English, "en");
	AddMapping(SystemLanguage.Estonian, "et");
	AddMapping(SystemLanguage.Faroese, "fo");
	AddMapping(SystemLanguage.Finnish, "fi");
	AddMapping(SystemLanguage.French, "fr");
	AddMapping(SystemLanguage.German, "de");
	AddMapping(SystemLanguage.Greek, "el");
	AddMapping(SystemLanguage.Hebrew, "he");
	AddMapping(SystemLanguage.Hungarian, "hu"); 
	AddMapping(SystemLanguage.Hungarian, "hu");
	AddMapping(SystemLanguage.Icelandic, "is");
	AddMapping(SystemLanguage.Indonesian, "id");
	AddMapping(SystemLanguage.Italian, "it");
	AddMapping(SystemLanguage.Japanese, "ja");
	AddMapping(SystemLanguage.Korean, "ko");
	AddMapping(SystemLanguage.Latvian, "lv");
	AddMapping(SystemLanguage.Lithuanian, "li");
	AddMapping(SystemLanguage.Norwegian, "no");
	AddMapping(SystemLanguage.Polish, "pl");
	AddMapping(SystemLanguage.Portuguese, "pt");
	AddMapping(SystemLanguage.Romanian, "ro");
	AddMapping(SystemLanguage.Russian, "ru");
	AddMapping(SystemLanguage.SerboCroatian, "sh"); 
	AddMapping(SystemLanguage.Slovak, "sk");
	AddMapping(SystemLanguage.Slovenian, "sl");
	AddMapping(SystemLanguage.Spanish, "es");
	AddMapping(SystemLanguage.Swedish, "sv");
	AddMapping(SystemLanguage.Thai, "th");
	AddMapping(SystemLanguage.Turkish, "tr");
	AddMapping(SystemLanguage.Ukrainian, "uk");
	AddMapping(SystemLanguage.Vietnamese, "vi");
  }

  private static void AddMapping(SystemLanguage language, string isoCode)
  {
	SystemLanguageToIso2Mapping[language] = isoCode;
	Iso2ToSystemLanguageMapping[isoCode] = language;
  }

  /// <summary>
  /// Returns the iso code for a language
  /// </summary>
  public static string GetIsoCode(SystemLanguage language)
  {
	string isoCode = null;
	if (!SystemLanguageToIso2Mapping.TryGetValue(language, out isoCode))
	{
	  isoCode = FallbackIsoCode;
	}
	return isoCode;
  }

  /// <summary>
  /// Returns the system language for an iso code
  /// </summary>
  public static SystemLanguage GetSystemLanguage(string isoCode)
  {
	SystemLanguage language = FallbackSystemLanguage;
	if (!Iso2ToSystemLanguageMapping.TryGetValue(isoCode, out language))
	{
	  language = FallbackSystemLanguage;
	}
	return language;
  }
}