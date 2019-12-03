/*___________________________________________________________*
 *                                                           *
 *   MSAW Common Unity                                       *
 *                                                           *
 *   Copyright (c) 2019 M.S.A.W.							 *
 *   http://www.merlin-saw.me                                *
 *                                                           *
 *   THIS SOURCE CODE IS PROPERTY OF M.S.A.W. You may not	 *
 *   copy, redistribute, modify, reuse or misuse this file   *
 *   in any way, except with permission of M.S.A.W.          *
 *                                                           *
 *___________________________________________________________*/

#if(! (UNITY_WEBPLAYER && !UNITY_EDITOR))
#define LOG_UNITY   // Debug.Log* active?
#define UNITY       // running inside the unity runtime environment?
#endif

using System;
#if UNITY
using UnityEngine;
using System.IO; // you can disable this - recommended when using outside of Unity!
#endif

/// <summary>
/// A simple logger class
/// </summary>
/// <remarks>
///     Usage Examples: 
///     <code>
///         private readonly Logger log = new Logger(typeof(myClass));
///     </code>
///     <code>
///         log.Info(Logger.User.Merlin, "Message 1");
///         log.Info(Logger.User.Merlin | Logger.User.Msaw, "Message 2");
///         log.Warn(Logger.User.Merlin, "Message 3");
///     </code>
///     
///     Shortcut for a simple info message:
///     <code>
///         log.InfoME("Message 4");
///     </code>
/// </remarks>
public class _Logger : MonoBehaviour {

    public static bool enableLogFile = false;
#if !UNITY_WEBPLAYER
    private string LogFilePath { get { return (Application.persistentDataPath + "/"); } }
#endif

    private static bool logFilesInitialized = false;
    private const int logHistorySize = 10;

    //Bitmasks to hold the users for which to output messages at runtime. Overridden in the constructor in case the debug output is globally disabled.
    private static User outputUserMaskDebug = User.None;    //User.Merlin | User.Webservice | User.Msaw;
    private static User outputUserMaskInfo = User.All;      //User.Merlin | User.Webservice | User.Mase;
    private static User outputUserMaskWarning = User.All;
    private static User outputUserMaskError = User.All;
    private static User outputUserMaskFatal = User.All;
    private Type type;

    public enum MessageType {
        Info = 0,
        Warning,
        Error,
        Fatal,
        Debug
    }

    public delegate void LogMessageDelegate(User outputBitmask, MessageType messageType, string loggerName, object message);
    public static LogMessageDelegate OnNewLogMessage;

    [Flags]
    public enum User : uint {
        None = 0,

        Merlin = 1 << 0,
        Msaw = 1 << 1,
        PoorDebugger = 1 << 2, //Enable this output if you want to see sent and received ws messages.

        //Add more here following the (1 << 3, 1 << 4, 1 << 5, pattern and so on).

        All = 0xFFFFFFFF,
    }

    public void InfoME(object message) { Info(User.Msaw, message); }
    public void InfoMS(object message) { Info(User.Msaw, message); }
    public void InfoPD(object message) { Info(User.PoorDebugger, message); }
  

    public void DebugME(object message) { Debug(User.Msaw, message); }
    public void DebugMS(object message) { Debug(User.Msaw, message); }
    public void DebugPD(object message) { Debug(User.PoorDebugger, message); }
   
    //Add more convenience methods here if necessary.

    /// <summary>
    /// A logger to be used for logging statements in the code.
    ///    It is recommended to follow a pattern for instantiating this:
    ///    <code>
    ///        private static readonly Logger log = new Logger(typeof(YourClassName));
    ///        ...
    ///        log.*(yourLoggingStuff); // Debug/Info/Warn/Error/Fatal[Format]
    ///    </code>
    ///</summary>
    ///<param name="type">the type that is using this logger </param> 

    public _Logger(Type type) {
        this.type = type;

        IsDebugEnabled = true;
        IsInfoEnabled = true;
        IsWarnEnabled = true;
        IsErrorEnabled = true;
        IsFatalEnabled = true;
    }

    /// <summary>
    /// Helper method that lets a game that makes use of this logger disable all logging.
    /// Usage: Invoke this method once at game startup time for release builds where you want to disable all debug output.
    /// </summary>
    public static void DisableDebugOutput() {
        outputUserMaskDebug = User.None;
        outputUserMaskInfo = User.None;
        outputUserMaskWarning = User.None;
        outputUserMaskError = User.None;
        outputUserMaskFatal = User.None;
    }

    /// <summary>
    /// Method that sets up the log file history by copying old log files into files
    /// with different name (e.g. newest log starts with filename 'log0', older one with 'log1' etc...
    /// </summary>
    private void InitializeLogFiles() {
#if !UNITY_WEBPLAYER
        //Check if old log files exist. In that case: Make a backup.
        for (int i = logHistorySize - 1; i > 0; --i) {
            string logFileName = LogFilePath + "log" + (i - 1).ToString() + ".txt";
            string logFileNameBackup = LogFilePath + "log" + i.ToString() + ".txt";
            if (File.Exists(logFileName)) {
                File.Copy(logFileName, logFileNameBackup, true);
                if (i == 1) {
                    File.Delete(logFileName); //Delete the file that is used as current log output file to have an empty file to start with.
                }
            }
        }
#endif
    }

    public static void ConfigureForServer() {

    }

    private static void Configure(string configFile) {

    }

    #region Test if a level is enabled for logging
    public bool IsDebugEnabled {
        get;
        set;
    }

    public bool IsInfoEnabled {
        get;
        set;
    }

    public bool IsWarnEnabled {
        get;
        set;
    }


    public bool IsErrorEnabled {
        get;
        set;
    }


    public bool IsFatalEnabled {
        get;
        set;
    }

    #endregion Test if a level is enabled for logging

    private object FormatMessage(User outputBitmask, object message) {
        return "[" + type.ToString() + "](" + outputBitmask.ToString() + "): " + message;
    }


    /* Log a message object */
    public void Debug(User outputBitmask, object message) {
        if (OnNewLogMessage != null) {
            OnNewLogMessage(outputBitmask, MessageType.Debug, type.ToString() + "\n", message);
        }

        if ((outputBitmask & outputUserMaskDebug) == 0) {
            return;
        }

        message = FormatMessage(outputBitmask, message);

#if LOG_UNITY
        if (IsDebugEnabled) {
            UnityEngine.Debug.Log(message);
        }
#endif
#if MSAW_ONSCREEN_CONSOLE
    OnScreenConsole.Instance.AddLine(message, MessageType.Info); // onscreen console only uses info and higher
#endif

        if (enableLogFile == true) {
            WriteLogToFile(message);
        }

    }

    public void Info(User outputBitmask, object message) {
        if (OnNewLogMessage != null) {
            OnNewLogMessage(outputBitmask, MessageType.Info, type.ToString() + "\n", message);
        }

        if ((outputBitmask & outputUserMaskInfo) == 0) {
            return;
        }

        message = FormatMessage(outputBitmask, message);

#if LOG_UNITY
        if (IsInfoEnabled) {
            UnityEngine.Debug.Log(message);
        }
#endif
#if MSAW_ONSCREEN_CONSOLE
    OnScreenConsole.Instance.AddLine(message, MessageType.Info);
#endif

        if (enableLogFile == true) {
            WriteLogToFile(message);
        }

    }

    public void Warn(User outputBitmask, object message) {
        if (OnNewLogMessage != null) {
            OnNewLogMessage(outputBitmask, MessageType.Warning, type.ToString() + "\n", message);
        }

        if ((outputBitmask & outputUserMaskWarning) == 0) {
            return;
        }

        message = FormatMessage(outputBitmask, message);

#if LOG_UNITY
        if (IsWarnEnabled) {
            UnityEngine.Debug.LogWarning(message);
        }
#endif
#if MSAW_ONSCREEN_CONSOLE
    OnScreenConsole.Instance.AddLine(message, MessageType.Warning);
#endif

        if (enableLogFile == true) {
            WriteLogToFile(message);
        }

    }


    public void Error(User outputBitmask, object message) {
        if (OnNewLogMessage != null) {
            OnNewLogMessage(outputBitmask, MessageType.Error, type.ToString() + "\n", message);
        }

        if ((outputBitmask & outputUserMaskError) == 0) {
            return;
        }

        message = FormatMessage(outputBitmask, message);

#if LOG_UNITY
        if (IsErrorEnabled) {
            UnityEngine.Debug.LogError(message);
        }
#endif
#if MSAW_ONSCREEN_CONSOLE
    OnScreenConsole.Instance.AddLine(message, MessageType.Error);
#endif

        if (enableLogFile == true) {
            WriteLogToFile(message);
        }

    }


    public void Fatal(User outputBitmask, object message) {
        if (OnNewLogMessage != null) {
            OnNewLogMessage(outputBitmask, MessageType.Fatal, type.ToString() + "\n", message);
        }

        if ((outputBitmask & outputUserMaskFatal) == 0) {
            return;
        }

        message = FormatMessage(outputBitmask, message);

#if LOG_UNITY
        if (IsFatalEnabled) {
            UnityEngine.Debug.LogError(message);
        }
#endif
#if MSAW_ONSCREEN_CONSOLE
    OnScreenConsole.Instance.AddLine(message, MessageType.Fatal);
#endif

        if (enableLogFile == true) {
            WriteLogToFile(message);
        }
    }

    private void WriteLogToFile(object message) {
#if (!UNITY_WEBPLAYER)

        //Lazy initialization of the log files. This is to prevent the editor error that happens when the
        //static constructor accesses Application.persistentDataPath.
        if (logFilesInitialized == false) {
            InitializeLogFiles();
            logFilesInitialized = true;
        }

        //NOTE: When changing the log filename, make sure to adapt the backup code in the InitializeLogFiles() method.
        using (StreamWriter fileStream = new StreamWriter(LogFilePath + "log0.txt", true)) { //Slow operation, but we'll assume in release mode, only important warning and error output is logged which shouldn't generate too many file write calls.
            fileStream.WriteLine(message);
        }
#endif
    }
}
