//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if !UNITY_WEBPLAYER
using System.IO;
#endif

using System.Xml.Serialization;
using System;
using System.Text;
#if !USE_MINIJSON_LEGACY
using Newtonsoft.Json;
#endif

#endregion



/// <summary>
/// Localization class - Usage: Loca.Get(text)
/// </summary>
public sealed class Loca
{

	public enum LocaFileReadMethod
	{
		SystemIO,
		ResourcesLoad,
	}

	private static List<SystemLanguage> supportedLanguages = new List<SystemLanguage>();
	/// <summary>
	/// Supported languages - must be set prior to any Loca.Get() calls
	/// If empty, default language is always English
	/// </summary>
	public static List<SystemLanguage> SupportedLanguages
	{
		get
		{
			return supportedLanguages;
		}
		set
		{
			if (value != null)
			{
				supportedLanguages = value;
			}
			else
			{
				supportedLanguages = new List<SystemLanguage>();
			}
		}
	}


	private static Loca instance;
	private static Loca Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Loca();
			}
			return instance;
		}
	}

	private static readonly _Logger log = new _Logger(typeof(Loca));

	private static Dictionary<string, string> translations = new Dictionary<string, string>();
	private static Dictionary<string, string> missingTranslations = new Dictionary<string, string>();

	private static SystemLanguage currentLanguage = Application.systemLanguage;
	public static SystemLanguage CurrentLanguage
	{
		get { return currentLanguage; }
		set { currentLanguage = value; }
	}

	public static string GUILanguage
	{
		get
		{
			if (supportedLanguages.Contains(currentLanguage))
			{
				return (currentLanguage.ToString().ToLower());
			}
			else
			{
				// fallback: English
				log.Debug(_Logger.User.Msaw, "Using fallback language: English");
				return "english";
			}
		}
	}

	[System.Obsolete("For better discimination between system and gui language, this property was renamed. Use GUILanguage instead in the future.")]
	public static string GameLanguage { get { return (GUILanguage); } }

	//Points to the file /Resources/languages/translations_xy.txt
	private static string locaFileBasePath = "languages/translations_";
	public static string LocaFileBaseName
	{
		get { return locaFileBasePath + GUILanguage; }
	}

	private static string locaFileExtension = ".txt";
	public static string CurrentLanguageFileResources
	{
		get { return (Application.dataPath + "/Resources/" + LocaFileBaseName + locaFileExtension); }
	}

	public static string CurrentLanguageFileTextAsset
	{ //Use this path in combination with TextAsset and Resources.Load to be able to load from the resources folder and thus unify things on PC and IPhone.
		get { return (LocaFileBaseName); }
	}

	public class DictHolder
	{ //Helper class to simplify JSON serialization.
		public Dictionary<string, string> Translations { get; set; }
	}

	public static void SwitchLanguageAndReloadFromDisk(SystemLanguage language)
	{
		CurrentLanguage = language;
		Instance.ReadTranslations();
	}

	public static void SwitchLanguageAndReloadFromDisk(string language)
	{
		foreach (SystemLanguage supportedLanguage in SupportedLanguages)
		{
			if (supportedLanguage.ToString() == language)
			{
				SwitchLanguageAndReloadFromDisk(supportedLanguage);
				log.Warn(_Logger.User.Msaw, "SwitchLanguageAndReloadFromDisk(language: " + language + "). This call may potentially remove all localizations retrieved via loca update manager.");
				return;
			}
		}
		log.Warn(_Logger.User.Msaw, "Failed to switch to language '" + language + "' as it's not in the list of supported languages. Make sure there are no typos (case must be correct).");
	}

	public static bool IsInitialLoadFinished { get; private set; }

	private Loca()
	{
		ReadTranslations();
		IsInitialLoadFinished = true;
	}

	private void ReadTranslations()
	{

		log.Info(_Logger.User.Msaw, "Loca.ReadTranslations()");

#if UNITY_EDITOR
		ReadTranslations(LocaFileReadMethod.SystemIO, CurrentLanguageFileResources);
#else
	ReadTranslations(LocaFileReadMethod.ResourcesLoad, CurrentLanguageFileTextAsset);
#endif
	}

	/// <summary>
	/// Method to read translations from disk given the read method and the filepath.
	/// </summary>
	public static void ReadTranslations(LocaFileReadMethod readMethod, string filePath)
	{

		Dictionary<string, string> translationsOnDisk = new Dictionary<string, string>();
		if (readMethod == LocaFileReadMethod.SystemIO)
		{
			ReadTranslationsFromDiskSystemIO(translationsOnDisk, filePath);
		}
		else
		{
			ReadTranslationsFromDiskTextAsset(translationsOnDisk, filePath);
		}

		if (translationsOnDisk != null && translationsOnDisk.Count > 0)
		{
#if UNITY_EDITOR //This code-path helps removing constantly changing translation files during development.
			if (translations != null)
			{
				foreach (KeyValuePair<string, string> kvp in translationsOnDisk)
				{
					translations[kvp.Key] = kvp.Value;
				}
			}
			else
			{
				translations = translationsOnDisk;
			}
#else
	  translations = translationsOnDisk;
#endif
		}
		else
		{
			log.Warn(_Logger.User.Msaw, "Not assigning translations from disk to Loca because the translations file was empty or did not exist.");
		}
	}

	/// <summary>
	/// Main translation method. Takes a key and returns the translated text.
	/// </summary>
	public string GetText(string locaMessageId)
	{
		if (locaMessageId == null)
		{
			log.Warn(_Logger.User.Msaw, "No localization for null value possible, returning empty string");
			return "";
		}

		string translation = locaMessageId;
		if (!translations.TryGetValue(locaMessageId, out translation))
		{
			if (!missingTranslations.ContainsKey(locaMessageId))
			{
				missingTranslations.Add(locaMessageId, locaMessageId);
#if UNITY_EDITOR && LOCA_AUTO_GENERATE_MISSING_KEYS
		Dictionary<string, string> translationsToWrite = new Dictionary<string, string>(missingTranslations);
		foreach (KeyValuePair<string, string> kvp in translations) {
		  if (translationsToWrite.ContainsKey(kvp.Key) == false) {
			translationsToWrite.Add(kvp.Key, kvp.Value);
		  }
		}
		WriteTranslationsToFile(translationsToWrite);
#endif
			}
			return locaMessageId;
		}

		return translation;
	}

	/// <summary>
	/// Method to find out whether or not the given key is found in the translations.
	/// </summary>
	public bool ExistsKey(string key)
	{
		if (key != null)
		{
			return (translations.ContainsKey(key));
		}
		return (false);
	}

	/// <summary>
	/// Method to write all not-yet-translated keys as well as already translated ones to a file for translation.
	/// </summary>
	public static void WriteTranslationsToFile(Dictionary<string, string> translationsToWrite)
	{

#if UNITY_EDITOR
		if (translationsToWrite == null)
		{
			log.Error(_Logger.User.Msaw, "WriteTranslationsToFile() failed because given translationsToWrite was null. Not writing anything.");
			return;
		}

		DictHolder dh = new DictHolder();
		dh.Translations = translationsToWrite;

		using (System.IO.StreamWriter singleProfileFile = new System.IO.StreamWriter(CurrentLanguageFileResources, false, new UTF8Encoding(false)))
		{
#if removeJsonFX
	  System.Text.StringBuilder jsonLine = new System.Text.StringBuilder();
	  JsonFx.Json.JsonWriter writer = new JsonFx.Json.JsonWriter(jsonLine);
	  writer.Write(dh);
	  singleProfileFile.WriteLine(jsonLine.ToString());
#else
			StringBuilder sb = new StringBuilder(translationsToWrite.Count * 80); // assumption
			sb.Append("{\n    \"Translations\": {\n");
			bool firstLine = true;
			foreach (KeyValuePair<string, string> kvp in translationsToWrite.OrderBy(o => o.Key))
			{
				if (firstLine == false)
				{
					sb.Append(",\n");
				}
				sb.Append("        \"");
				sb.Append(kvp.Key);
				sb.Append("\": \"");
				sb.Append(kvp.Value.Replace("\n", "\\n").Replace("\"", "\\\""));
				sb.Append("\"");
				firstLine = false;
			}
			sb.Append("\n     }\n}");
			if (log.IsDebugEnabled)
			{
				log.Debug(_Logger.User.Msaw, "Writing loca to " + CurrentLanguageFileResources + ": " + sb.ToString());
			}
			singleProfileFile.WriteLine(sb.ToString());
#endif
		}
#endif
	}

	/// <summary>
	/// Reading a loca file using System.IO methods.
	/// </summary>
	private static Dictionary<string, string> ReadTranslationsFromDiskSystemIO(Dictionary<string, string> translationDict, string editorFilePath)
	{

		if (System.IO.File.Exists(editorFilePath) == false)
		{ //Translations file not found.
			log.Warn(_Logger.User.Msaw, "Translations file '" + editorFilePath + "' couldn't be found");
			return (translationDict);
		}

		using (System.IO.StreamReader file = new System.IO.StreamReader(editorFilePath, new UTF8Encoding(true)))
		{
			string jsonLine = "";
			if ((jsonLine = file.ReadToEnd()) != null)
			{

				ParseJSON(jsonLine, translationDict);
			}
			else
			{
				log.Error(_Logger.User.Msaw, "Couldn't read translations from '" + editorFilePath + "'.");
			}
		}
		return (translationDict);
	}

	/// <summary>
	/// Reading a loca file using Resources.Load().
	/// </summary>
	private static Dictionary<string, string> ReadTranslationsFromDiskTextAsset(Dictionary<string, string> translationDict, string textAssetFilePath)
	{

		//This code allows to read the translations on an IPhone directly from the Resources folder without having to manually move a translations file somewhere to the IPhone.
		TextAsset textAsset = Resources.Load(textAssetFilePath, typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			string jsonLine = textAsset.text;
			if (string.IsNullOrEmpty(jsonLine) == false)
			{

				ParseJSON(jsonLine, translationDict);
			}
			else
			{
				log.Error(_Logger.User.Msaw, "Couldn't read translations from '" + textAssetFilePath + "'.");
			}
		}
		else
		{
			log.Warn(_Logger.User.Msaw, "Translations file '" + textAssetFilePath + "' couldn't be read.");
		}
		return (translationDict);
	}

	/// <summary>
	/// Method that is necessary to avoid reflection and thus allow to use code stripping to reduce filesize.
	/// </summary>
	private static void ParseJSON(string jsonLine, Dictionary<string, string> translationDict)
	{

#if USE_MINIJSON_LEGACY
	int indexSecondBraceOpen = jsonLine.IndexOf("{", 1);
	int indexSecondBraceClose = jsonLine.IndexOf("}");
	string cleanedJsonLine = jsonLine.Substring(indexSecondBraceOpen + 1, indexSecondBraceClose - indexSecondBraceOpen - 1);
	//log.InfoJG("cleanedJsonLine = " + cleanedJsonLine);

	List<string> keys = new List<string>();
	List<string> values = new List<string>();

	Hashtable hashTable2 = MiniJSONLegacy.jsonDecode("{" + cleanedJsonLine + "}") as Hashtable;
	if (hashTable2 != null) {
	  foreach (object key in hashTable2.Keys) {
		keys.Add(key.ToString());
	  }
	  foreach (object value in hashTable2.Values) {
		values.Add(value.ToString());
	  }
	} else {
	  log.Error(Logger.User.Msaw, "Cannot parse JSON");
	}

	for (int i = 0; i < keys.Count; ++i) {
	  translationDict.Add(keys[i], values[i]);
	}
#else
		DictHolder locaData = JsonConvert.DeserializeObject<DictHolder>(jsonLine);
		foreach (KeyValuePair<string, string> kvp in locaData.Translations)
		{
			translationDict.Add(kvp.Key, kvp.Value);
		}
#endif
	}

#if UNITY_EDITOR
	public static void CreateMissingTranslationFilesForSupportedLanguages()
	{
		log.Info(_Logger.User.Msaw, "Creating missing translation files for supported languages...");

		// default language is always English
		SystemLanguage defaultLanguage = SystemLanguage.English;
		string locaPath = Application.dataPath + "/Resources/" + locaFileBasePath;
		string defaultLanguageFile = locaPath + defaultLanguage.ToString().ToLower() + locaFileExtension;

		// if the default translation file does not exist, create an empty one
		if (System.IO.File.Exists(defaultLanguageFile) == false)
		{
			log.Warn(_Logger.User.Msaw, "Default translation file does not exist - creating an empty one: " + defaultLanguageFile);
			System.IO.File.Create(defaultLanguageFile);
		}

		if (SupportedLanguages != null)
		{
			SupportedLanguages.ForEach(language =>
			{
				if (language != defaultLanguage)
				{
					// if the translation file does not exist, copy the default language file
					string languageFile = locaPath + language.ToString().ToLower() + locaFileExtension;
					if (System.IO.File.Exists(languageFile) == false)
					{
						log.Info(_Logger.User.Msaw, "Creating a translation file for " + language.ToString());
						System.IO.File.Copy(defaultLanguageFile, languageFile);
					}
				}
			});
		}
	}
#endif

	#region Convenience Methods

	/// <summary>
	/// Translates a string
	/// </summary>
	/// <param name="text">loca code</param>
	/// <returns>the translated string</returns>
	public static string Get(string text)
	{
		return Loca.Instance.GetText(text);
	}

	/// <summary>
	/// Method to peek whether or not a translation with given key exists.
	/// </summary>
	public static bool Exists(string key)
	{
		return Loca.Instance.ExistsKey(key);
	}

	/// <summary>
	/// Translates a string and replaces placeholders
	/// Attention: Replacement indices start at 0
	/// </summary>
	/// <param name="text">loca code</param>
	/// <param name="replacements">replacement values (order matters)</param>
	/// <returns>the translated string</returns>
	public static string Get(string text, params string[] replacements)
	{
		if (replacements == null || replacements.Length == 0)
		{
			return (Get(text));
		}
		StringBuilder sb = new StringBuilder(100); // assumption
		sb.Append(Get(text));
		for (int i = 0; i < replacements.Length; ++i)
		{
			string identifier = "%" + i.ToString();
			sb.Replace(identifier, replacements[i]);
		}
		return sb.ToString();
	}

	#endregion
}

