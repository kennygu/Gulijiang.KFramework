using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Gulijiang.KFramework.Common;
namespace Gulijiang.KFramework.Log
{
    public class SysLog
    {

        public static string LogBasePath
        {
            get
            {
                return Utility.BasePath + "\\log\\";
            }
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="strformat">错误信息</param>
        /// <param name="path">日志存放位置</param>
        public static void WriteLog(string logInfo, int logType, int logID, string businessID)
        {
            try
            {
                string serverIP = ConfigurationManager.AppSettings["LogServerIP"];
                string APPID = ConfigurationManager.AppSettings["APPID"];
                if (!String.IsNullOrEmpty(serverIP) && !String.IsNullOrEmpty(APPID))
                {
                    LogRequest request = new LogRequest();
                    request.APPID = APPID;
                    request.LogType = logType;
                    request.BusinessID = businessID;
                    request.LogDesc = logInfo;
                    DateTime curDate = DateTime.Now;
                    request.CreateTime = curDate;
                    request.CreateMillisecond = curDate.Millisecond;
                    request.LogID = logID;
                    LogClient.Log(request);
                }
                else
                {
                    string strBasePath = LogBasePath;
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string strFilePath = strBasePath + fileName;
                    if (!File.Exists(strFilePath))
                    {
                        if (!Directory.Exists(strBasePath))
                        {
                            Directory.CreateDirectory(strBasePath);
                        }
                        FileStream fss = File.Create(strFilePath); ;
                        fss.Flush();
                        fss.Close();
                    }
                    FileStream fs = new FileStream(strFilePath, FileMode.Append);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.Write(DateTime.Now.ToString() + "：\r\n" + logInfo + "\r\n" + "=======================================================\r\n");
                    streamWriter.Flush();
                    streamWriter.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                WriteOtherLog("Log Error：" + ex.Message, "templog");
            }
        }
        public static void WriteErrorLog(int logID, string logInfo)
        {
            WriteLog(logInfo, 0, logID, "");
        }
        public static void WriteErrorLog(string logInfo)
        {
            WriteErrorLog(100000, logInfo);
        }
        public static void WriteBusinessLog(int logID, string businessID, string logInfo)
        {
            WriteLog(logInfo, 1, logID, businessID);
        }
        public static void WriteBusinessLog(string businessID, string logInfo)
        {
            WriteBusinessLog(100000, businessID, logInfo);
        }
        public static void WriteOtherLog(string logInfo, string fileName)
        {
            try
            {
                string strBasePath = LogBasePath;
                string strFilePath = strBasePath + fileName + ".txt";
                if (!File.Exists(strFilePath))
                {
                    if (!Directory.Exists(strBasePath))
                    {
                        Directory.CreateDirectory(strBasePath);
                    }
                    FileStream fss = File.Create(strFilePath); ;
                    fss.Flush();
                    fss.Close();
                }
                FileStream fs = new FileStream(strFilePath, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fs);
                streamWriter.Write(logInfo + "\r\n");
                streamWriter.Flush();
                streamWriter.Close();
                fs.Close();
            }
            catch
            {
            }
        }

    }
}
