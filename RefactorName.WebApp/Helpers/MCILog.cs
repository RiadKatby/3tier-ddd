using System;
using System.Diagnostics;
using System.IO;

namespace RefactorName.Web
{
    public class MCILog
    {
        private string _FileName;
        private string _FilePath;
        private string _FullPath;
        private string _FileBaseName;
        private StreamWriter fs;

        private Object thisLock = new Object();

        public MCILog(string fileBaseName = "", string filePath = "C:\\mciLog")
        {
            this._FilePath = filePath;
            this._FileBaseName = fileBaseName;
        }

        private void UpdateFileName()
        {
            if (!Directory.Exists(_FilePath))
                Directory.CreateDirectory(_FilePath);

            //create new file for new day
            this._FileName = string.Format("{0}{1}{2}.log", _FileBaseName.Trim(), string.IsNullOrWhiteSpace(_FileBaseName) ? "" : "_", DateTime.Now.ToString("yyyyMMdd"));
            this._FullPath = Path.Combine(_FilePath, _FileName);
        }

        public void WriteLine(string strToWrite)
        {
            lock (thisLock)
            {
                try
                {
                    UpdateFileName();
                    fs = File.AppendText(_FullPath);
                    fs.WriteLine(string.Format("{0}\t  {1}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), strToWrite));
                    fs.Flush();
                }
                catch (Exception ex)
                {
                    string message = string.Format("writing to the log file failed. So,it is here: (The message is: {0}).{1}Error:{2}.",
                        strToWrite,
                        Environment.NewLine,
                        ex.Message);
                    try
                    {
                        //TODO: Change the application name 
                        if (!EventLog.SourceExists("MOLInspectorsWS"))
                            EventLog.CreateEventSource("MOLInspectorsWS", "Application");

                        EventLog.WriteEntry("MOLInspectorsWS", message, EventLogEntryType.Error);
                    }
                    catch { }
                }
                finally
                {
                    fs.Close();
                }
            }
        }
    }
}