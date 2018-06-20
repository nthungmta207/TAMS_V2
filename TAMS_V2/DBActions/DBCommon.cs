using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Manager = System.Configuration.ConfigurationManager;
using Settings = System.Configuration.ConnectionStringSettings;

namespace Database
{
    static class DBCommon
    {
        private static string FErrMessage = "";
        public static string ErrMessage
        {
            get {return FErrMessage;}
            set {FErrMessage = value;}
        }

        //Lấy chuỗi kết nối từ file
        static public string GetConnectionStringFromFile(string FilePath)
        {
            ConnectionStringType ConnStr = new ConnectionStringType();
            string strConnection = "";

            try
            {
                using (StreamReader streamReader = new StreamReader(FilePath))
                {
                    string Row = "";
                    while ((Row = streamReader.ReadLine()) != null)
                    {
                        string tmpSwitch = Row.Substring(0, Row.IndexOf("=")).ToLower();
                        switch (tmpSwitch)
                        {
                            case "servername":
                                ConnStr.Server = GetValue(Row);
                                break;

                            case "instancename":
                                ConnStr.Instance = GetValue(Row);
                                break;

                            case "attachment ":
                                ConnStr.IsAttachment = Convert.ToBoolean(GetValue(Row));
                                break;

                            case "databasename":
                                ConnStr.Database = GetValue(Row);
                                break;

                            case "username":
                                {
                                    ConnStr.User = GetValue(Row);
                                    ConnStr.IsWindowAuthencation = true;
                                    if (ConnStr.User != "")
                                        ConnStr.IsWindowAuthencation = false;
                                    break;
                                }

                            case "password":
                                ConnStr.Pass = GetValue(Row);
                                break;

                            case "portnumber":
                                ConnStr.Port = GetValue(Row);
                                break;

                            case "timeout":
                                ConnStr.TimeOut = GetValue(Row);
                                break;
                        }
                    }
                    strConnection = GetConnectionString(ConnStr);
                }                
            }
            catch (Exception ex )
            {
               FErrMessage = "Lỗi khi lấy chuỗi kết nối : " + ex.Message;
            }
            return strConnection;
        }
        

        //Hàm lấy giá trị của hàng đọc từ file .Ini, (lấy giá trị sau dấu "=")
        /// <summary>
        /// Lấy giá trị của hàng đọc từ file .Ini, (lấy giá trị sau dấu "=")
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private static string GetValue(string Row)
        {
            string stringValue = "";
            if (!Row.Equals(""))
                stringValue = Row.Substring(Row.LastIndexOf("=") + 1);
            return stringValue;
        }

        //Hàm tạo chuỗi kết nối từ biến mang kiểu ConnectionString
        /// <summary>
        /// Tạo chuỗi kết nối từ biến mang kiểu ConnectionString
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private static string GetConnectionString(ConnectionStringType ConnStr)
        {
            string strConnection = "";
            if (!ConnStr.Instance.Equals(""))
                ConnStr.Server += @"\" + ConnStr.Instance;

            if (ConnStr.IsWindowAuthencation)
            {
                if (ConnStr.IsAttachment)
                {
                    strConnection = "server = " + ConnStr.Server +
                                ";AttachDbFilename = " + ConnStr.Database +
                                ";Integrated Security = true ";
                }
                else
                {
                    strConnection = "server = " + ConnStr.Server +
                                ";database = " + ConnStr.Database +
                                ";Integrated Security = true ";
                }
            }
            else
            {
                if (ConnStr.IsAttachment)
                {
                    strConnection = "server = " + ConnStr.Server +
                                ";AttachDbFilename = " + ConnStr.Database +
                                ";uid = " + ConnStr.User +
                                ";pwd = " + ConnStr.Pass;
                }
                else
                {
                    strConnection = "server = " + ConnStr.Server +
                                ";database = " + ConnStr.Database +
                                ";uid = " + ConnStr.User +
                                ";pwd = " + ConnStr.Pass;
                }
            }

            if (!ConnStr.TimeOut.Equals(""))
            {
                strConnection += ";Connection Timeout = " + ConnStr.TimeOut;
            }

            if (!ConnStr.Port.Equals(""))
            {
                strConnection += ";Port = " + ConnStr.Port;
            }
            
            return strConnection;
        }

        //Lấy chuỗi kết nối từ file AppConfigureFile
        static public string GetConnectionStringFromAppConfigure(string NodeNameInAppConfigFile)
        {                        
            Settings settings;
            try
            {
                settings = Manager.ConnectionStrings[NodeNameInAppConfigFile];
                return settings.ConnectionString;
            }
            catch (Exception ex)
            {                
                FErrMessage = "Lỗi khi lấy chuỗi kết nối : " + ex.Message;
                return "";
            }            
        }
    }
}
