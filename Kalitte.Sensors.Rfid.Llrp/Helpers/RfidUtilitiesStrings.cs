namespace Kalitte.Sensors.Rfid.Llrp.Helpers
{
    using System;
    using System.Globalization;
    using System.Resources;

    public class RfidUtilitiesStrings
    {
        public static string FixedTimerQueueIsStopping(string name)
        {
            return Keys.GetString("FixedTimerQueueIsStopping", name);
        }

        public static string HexDecodeArgumentIsOfOddSize(int inputLength)
        {
            return Keys.GetString("HexDecodeArgumentIsOfOddSize", inputLength);
        }

        public static string HostingKeyFormatter(string appName, string appType)
        {
            return Keys.GetString("HostingKeyFormatter", appName, appType);
        }

        public static string RegistryValueIncorrect(string key)
        {
            return Keys.GetString("RegistryValueIncorrect", key);
        }

        public static string SchemaExists(int existingSchemaVersion, int expectedSchemaVersion)
        {
            return Keys.GetString("SchemaExists", existingSchemaVersion, expectedSchemaVersion);
        }

        public static string UnsupportedType(string type)
        {
            return Keys.GetString("UnsupportedType", type);
        }

        public static string BackupFileNameIsInvalid
        {
            get
            {
                return Keys.GetString("BackupFileNameIsInvalid");
            }
        }

        public static string CommandTimeoutNegative
        {
            get
            {
                return Keys.GetString("CommandTimeoutNegative");
            }
        }

        //public static CultureInfo Culture
        //{
        //    get
        //    {
        //        return Keys.Culture;
        //    }
        //    set
        //    {
        //        Keys.Culture = value;
        //    }
        //}

        public static string DuplicateEliminationIntervalNegative
        {
            get
            {
                return Keys.GetString("DuplicateEliminationIntervalNegative");
            }
        }

        public static string ExceptionMessageSeparator
        {
            get
            {
                return Keys.GetString("ExceptionMessageSeparator");
            }
        }

        public static string HexPrefix
        {
            get
            {
                return Keys.GetString("HexPrefix");
            }
        }

        public static string InvalidBackupIndex
        {
            get
            {
                return Keys.GetString("InvalidBackupIndex");
            }
        }

        public static string InvalidFileCheckFrequency
        {
            get
            {
                return Keys.GetString("InvalidFileCheckFrequency");
            }
        }

        public static string InvalidMaximumSize
        {
            get
            {
                return Keys.GetString("InvalidMaximumSize");
            }
        }

        public static string InvalidNumberOfFiles
        {
            get
            {
                return Keys.GetString("InvalidNumberOfFiles");
            }
        }

        public static string LogSizeInvalid
        {
            get
            {
                return Keys.GetString("LogSizeInvalid");
            }
        }

        public static string MobileSkuTimeBombExpired
        {
            get
            {
                return Keys.GetString("MobileSkuTimeBombExpired");
            }
        }

        public static string NeedWindowsIdentity
        {
            get
            {
                return Keys.GetString("NeedWindowsIdentity");
            }
        }

        public static string NestedTransactionsNotSupported
        {
            get
            {
                return Keys.GetString("NestedTransactionsNotSupported");
            }
        }

        public static string NoLogFileLessThan2
        {
            get
            {
                return Keys.GetString("NoLogFileLessThan2");
            }
        }

        public static string SkuFatalError
        {
            get
            {
                return Keys.GetString("SkuFatalError");
            }
        }

        public static string UserNameFormatIsInvalid
        {
            get
            {
                return Keys.GetString("UserNameFormatIsInvalid");
            }
        }

        public class Keys
        {
            private static CultureInfo _culture = null;
            public const string BackupFileNameIsInvalid = "BackupFileNameIsInvalid";
            public const string CommandTimeoutNegative = "CommandTimeoutNegative";
            public const string DuplicateEliminationIntervalNegative = "DuplicateEliminationIntervalNegative";
            public const string ExceptionMessageSeparator = "ExceptionMessageSeparator";
            public const string FixedTimerQueueIsStopping = "FixedTimerQueueIsStopping";
            public const string HexDecodeArgumentIsOfOddSize = "HexDecodeArgumentIsOfOddSize";
            public const string HexPrefix = "HexPrefix";
            public const string HostingKeyFormatter = "HostingKeyFormatter";
            public const string InvalidBackupIndex = "InvalidBackupIndex";
            public const string InvalidFileCheckFrequency = "InvalidFileCheckFrequency";
            public const string InvalidMaximumSize = "InvalidMaximumSize";
            public const string InvalidNumberOfFiles = "InvalidNumberOfFiles";
            public const string LogSizeInvalid = "LogSizeInvalid";
            public const string MobileSkuTimeBombExpired = "MobileSkuTimeBombExpired";
            public const string NeedWindowsIdentity = "NeedWindowsIdentity";
            public const string NestedTransactionsNotSupported = "NestedTransactionsNotSupported";
            public const string NoLogFileLessThan2 = "NoLogFileLessThan2";
            public const string RegistryValueIncorrect = "RegistryValueIncorrect";
            private static ResourceManager resourceManager = new ResourceManager(typeof(RfidUtilitiesStrings).FullName, typeof(RfidUtilitiesStrings).Module.Assembly);
            public const string SchemaExists = "SchemaExists";
            public const string SkuFatalError = "SkuFatalError";
            public const string UnsupportedType = "UnsupportedType";
            public const string UserNameFormatIsInvalid = "UserNameFormatIsInvalid";

            public static string GetString(string key)
            {
                //return resourceManager.GetString(key, _culture);
                return key;
            }

            public static string GetString(string key, object arg0)
            {
                return string.Format(key, new object[] { arg0 });
            }

            public static string GetString(string key, object arg0, object arg1)
            {
                return string.Format(key, new object[] { arg0, arg1 });
            }

            //public static CultureInfo Culture
            //{
            //    get
            //    {
            //        return _culture;
            //    }
            //    set
            //    {
            //        _culture = value;
            //    }
            //}
        }
    }
}
