using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kalitte.Sensors.Security
{
public class LogRotator
{
    // Fields
    private int callsCount;
    private int currentBackupIndex;
    private const int DEFAULT_FILE_CHECK_FREQUENCY = 0x19;
    private const int DEFAULT_LOG_FILES_COUNT = 10;
    private const int DEFAULT_SIZE_COUNT = 10;
    private int fileCheckFrequency;
    private FileInfo logFile;
    private long maximumSizeInBytes;
    private const int MB_TO_BYTES = 0x100000;
    private int numberOfFiles;

    // Methods
    public LogRotator(string logFileName)
    {
        this.numberOfFiles = 10;
        this.maximumSizeInBytes = 10000; //0xa00000L;
        this.fileCheckFrequency = 0x19;
        if (string.IsNullOrEmpty(logFileName))
        {
            throw new ArgumentNullException("logFileName");
        }
        this.logFile = new FileInfo(logFileName);
    }

    public LogRotator(string logFileName, int numberOfFiles, int maximumSize, int fileCheckFrequency, int currentBackupIndex) : this(logFileName)
    {
        this.NumberOfFiles = numberOfFiles;
        this.MaximumSize = maximumSize;
        this.FileCheckFrequency = fileCheckFrequency;
        if (currentBackupIndex < 0)
        {
            throw new ArgumentException("InvalidBackupIndex", "currentBackupIndex");
        }
        this.currentBackupIndex = currentBackupIndex;
    }

    private string GetBackupFileName(int index)
    {
        StringBuilder builder2;
        StringBuilder builder = new StringBuilder();
        if ((this.logFile.Extension != null) && (this.logFile.Extension.Length != 0))
        {
            int length = this.logFile.FullName.IndexOf(this.logFile.Extension, StringComparison.CurrentCultureIgnoreCase);
            if (((this.logFile.FullName.Length - 1) < length) || (0 > length))
            {
                throw new InvalidOperationException("BackupFileNameIsInvalid");
            }
            builder.Append(this.logFile.FullName.Substring(0, length)).Append("_").Append(index);
            builder2 = builder.Append(this.logFile.Extension);
        }
        else
        {
            builder.Append(this.logFile.FullName).Append("_").Append(index);
            builder2 = builder;
        }
        return builder2.ToString();
    }

    internal static int GetCurBackupIndex(string logfilePath)
    {
        string directoryName = Path.GetDirectoryName(logfilePath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(logfilePath);
        string extension = Path.GetExtension(logfilePath);
        string[] files = Directory.GetFiles(directoryName, string.Format(CultureInfo.InvariantCulture, "{0}_*{1}", new object[] { fileNameWithoutExtension, extension }));
        return GetLastRotatedFileIndex(fileNameWithoutExtension, extension, files);
    }

    private static int GetLastRotatedFileIndex(string fileName, string extension, string[] files)
    {
        Regex regex = new Regex(string.Format(CultureInfo.InvariantCulture, @"{0}_(.*)\{1}", new object[] { fileName, extension }));
        int num = 0;
        foreach (string str2 in files)
        {
            string s = regex.Match(str2).Groups[1].ToString();
            try
            {
                int num2 = int.Parse(s, CultureInfo.InvariantCulture);
                if (num2 > num)
                {
                    num = num2;
                }
            }
            catch (System.Exception)
            {
            }
        }
        return num;
    }

    public bool IncrementCountAndIsFileNeedsToBeChanged()
    {
        if (++this.callsCount > this.fileCheckFrequency)
        {
            this.logFile.Refresh();
            return (this.logFile.Length > this.maximumSizeInBytes);
        }
        return false;
    }

    public void RotateLog()
    {
        int currentBackupIndex = this.currentBackupIndex;
        this.callsCount = 0;
        int index = ++this.currentBackupIndex;
        try
        {
            string backupFileName = this.GetBackupFileName(index);
            FileInfo info = new FileInfo(backupFileName);
            if (info.Exists)
            {
                info.Delete();
            }
            new FileInfo(this.logFile.FullName).MoveTo(backupFileName);
        }
        catch (System.Exception)
        {
            this.currentBackupIndex = currentBackupIndex;
            throw;
        }
        int num3 = (index - this.NumberOfFiles) + 1;
        if (num3 > 0)
        {
            FileInfo info3 = new FileInfo(this.GetBackupFileName(num3));
            if (info3.Exists)
            {
                info3.Delete();
            }
        }
    }

    // Properties
    public int CurrentBackupIndex
    {
        get
        {
            return this.currentBackupIndex;
        }
    }

    public int FileCheckFrequency
    {
        get
        {
            return this.fileCheckFrequency;
        }
        set
        {
            if (this.fileCheckFrequency < 1)
            {
                throw new ArgumentException("InvalidFileCheckFrequency", "fileCheckFrequency");
            }
            this.fileCheckFrequency = value;
        }
    }

    public FileInfo LogFile
    {
        get
        {
            return this.logFile;
        }
        set
        {
            this.logFile = value;
        }
    }

    public int MaximumSize
    {
        get
        {
            return (int) this.maximumSizeInBytes;
        }
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("InvalidMaximumSize", "maximumSize");
            }
            this.maximumSizeInBytes = value;
        }
    }

    public int NumberOfFiles
    {
        get
        {
            return this.numberOfFiles;
        }
        set
        {
            if (this.numberOfFiles < 2)
            {
                throw new ArgumentException("InvalidNumberOfFiles", "numberOfFiles");
            }
            this.numberOfFiles = value;
        }
    }
}


 

}
