using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

namespace Database
{
    namespace SQLServer
    {
        public class Connection
        {
            #region Khai báo thuộc tính
            //Thông báo lỗi
            private string FErrMessage = "";
            public string ErrMessage
            {
                get { return FErrMessage; }
                set { FErrMessage = value; }
            }

            //Chuỗi kết nối
            //private string FConnectionString = "";
            public string ConnectionString
            {
                get { return SqlConnection.ConnectionString; }
                set { SqlConnection.ConnectionString = value; }
            }

            //Kết nối SQL
            public static SqlConnection SqlConnection;

            public ConnectionState State
            {
                get { return SqlConnection.State; }               
            }

            #endregion

            #region Khởi tạo
            //Khởi tạo không tham số
            public Connection()
            {
                //SqlConnection = new SqlConnection();            
            }

            //Khởi tạo có tham số là chuỗi kết nỗi
            /// <summary>
            /// Khởi tạo Connection với tham số là chuỗi kết nối
            /// </summary>
            /// <param name="StringConnection"></param>
            public Connection(string StringConnection)
            {
                SqlConnection = new SqlConnection(StringConnection);
            }

            //khởi tạo với tham số từ file
            /// <summary>
            /// Nếu FromTexFileOrAppConfig = true sẽ trả về chuỗi kết nối từ text file theo đường dẫn, ngược lại
            /// sẽ lấy chuỗi kết nối từ file AppConfig theo tên Node
            /// </summary>
            /// <param name="FromTexFileOrAppConfig"></param>
            /// <param name="FilePathOrNodeName"></param>
            public Connection(bool FromTextFileOrAppConfig, string FilePathOrNodeName)
            {
                SqlConnection = new SqlConnection();
                GetConnectionStringFromFile(FromTextFileOrAppConfig, FilePathOrNodeName);
            }
            #endregion

            #region Hủy

            public void Dispose()
            {
                if (FErrMessage == "")
                    FErrMessage = "";
                if (ConnectionString == "")
                    ConnectionString = "";

                SqlConnection.Close();
                SqlConnection.Dispose();
            }

            #endregion

            #region Khai báo phương thức

            //Lấy chuỗi kết nối từ file Text hoặc App.config
            /// <summary>
            /// Lấy chuỗi kết nối từ file text hoặc App.config và gán vào ConnectionString
            /// </summary>
            /// <param name="FromTextFileOrAppConfig"></param>
            /// <param name="FilePathOrNodeName"></param>
            public void GetConnectionStringFromFile(bool FromTextFileOrAppConfig, string FilePathOrNodeName)
            {
                if (FromTextFileOrAppConfig)
                {
                    ConnectionString = DBCommon.GetConnectionStringFromFile(FilePathOrNodeName);
                    if (DBCommon.ErrMessage != "")
                        FErrMessage = DBCommon.ErrMessage;
                }
                else
                {
                    ConnectionString = DBCommon.GetConnectionStringFromAppConfigure(FilePathOrNodeName);
                    if (DBCommon.ErrMessage != "")
                        FErrMessage = DBCommon.ErrMessage;
                }
            }

            //Mở kết nối
            /// <summary>
            /// Mở kết nối không có tham số
            /// </summary>
            /// <returns></returns>
            public bool Open()
            {
                bool IsExist = false;
                if (SqlConnection.State != ConnectionState.Open)
                {
                    if (!ConnectionString.Equals(""))
                    {
                        try
                        {
                            SqlConnection.Open();
                            IsExist = true;
                        }
                        catch (Exception ex)
                        {
                            FErrMessage = ex.Message;
                        }
                    }
                    else
                    {
                        FErrMessage = "Chuỗi kết nối rỗng";
                    }
                }
                else
                {
                    FErrMessage = "Sqlconnection đang mở, không thể mở lại";
                }
                return IsExist;
            }

            //Mở kết nối có tham số là chuỗi kết nối
            /// <summary>
            /// Mở kết nối với có chứa tham số là chuỗi kết nối
            /// </summary>
            /// <param name="StringConnection"></param>
            /// <returns></returns>
            public bool Open(string StringConnection)
            {
                bool IsExist = false;
                if (SqlConnection.State != ConnectionState.Open)
                {
                    ConnectionString = StringConnection;
                    try
                    {
                        SqlConnection.Open();
                        IsExist = true;
                    }
                    catch (Exception ex)
                    {
                        FErrMessage = ex.Message;
                    }
                }
                else
                {
                    FErrMessage = "Kết nối đang mở, không thể mở tiếp";
                }
                return IsExist;
            }

            //Đóng kết nối
            public void Close()
            {
                if (SqlConnection.State != ConnectionState.Closed)
                    SqlConnection.Close();

            }

            #endregion
        }
    }

    namespace OLEDB
    {
        public class Connection
        {

        #region Khai báo thuộc tính
            //Thông báo lỗi
            private string FErrMessage = "";
            public string ErrMessage
            {
                get { return FErrMessage; }
                set { FErrMessage = value; }
            }

            //Chuỗi kết nối
            //private string FConnectionString = "";
            public string ConnectionString
            {
                get { return OLEDBConnection.ConnectionString; }
                set { OLEDBConnection.ConnectionString = value; }
            }

            //Kết nối SQL
            public static OleDbConnection OLEDBConnection;

            #endregion

        #region Khởi tạo
            //Khởi tạo không tham số
            public Connection()
            {                           
            }

            //Khởi tạo có tham số là chuỗi kết nỗi
            /// <summary>
            /// Khởi tạo Connection với tham số là chuỗi kết nối
            /// </summary>
            /// <param name="StringConnection"></param>
            public Connection(string StringConnection)
            {
                OLEDBConnection = new OleDbConnection(StringConnection);
            }
   
        #endregion

        #region Hủy

            public void Dispose()
            {
                if (FErrMessage == "")
                    FErrMessage = "";
                if (ConnectionString == "")
                    ConnectionString = "";

                OLEDBConnection.Close();
                OLEDBConnection.Dispose();
            }

            #endregion

        #region Khai báo phương thức

            //Mở kết nối
            /// <summary>
            /// Mở kết nối không có tham số
            /// </summary>
            /// <returns></returns>
            public bool Open()
            {
                bool IsExist = false;
                if (OLEDBConnection.State != ConnectionState.Open)
                {
                    if (!ConnectionString.Equals(""))
                    {
                        try
                        {
                            OLEDBConnection.Open();
                            IsExist = true;
                        }
                        catch (Exception ex)
                        {
                            FErrMessage = ex.Message;
                        }
                    }
                    else
                    {
                        FErrMessage = "Chuỗi kết nối rỗng";
                    }
                }
                else
                {
                    FErrMessage = "OLEDBconnection đang mở, không thể mở lại";
                }
                return IsExist;
            }

            //Mở kết nối có tham số là chuỗi kết nối
            /// <summary>
            /// Mở kết nối với có chứa tham số là chuỗi kết nối
            /// </summary>
            /// <param name="StringConnection"></param>
            /// <returns></returns>
            public bool Open(string StringConnection)
            {
                bool IsExist = false;
                if (OLEDBConnection.State != ConnectionState.Open)
                {
                    ConnectionString = StringConnection;
                    try
                    {
                        OLEDBConnection.Open();
                        IsExist = true;
                    }
                    catch (Exception ex)
                    {
                        FErrMessage = ex.Message;
                    }
                }
                else
                {
                    FErrMessage = "Kết nối đang mở, không thể mở tiếp";
                }
                return IsExist;
            }

            //Đóng kết nối
            public void Close()
            {
                if (OLEDBConnection.State != ConnectionState.Closed)
                    OLEDBConnection.Close();

            }

            #endregion
        }
    }
}
