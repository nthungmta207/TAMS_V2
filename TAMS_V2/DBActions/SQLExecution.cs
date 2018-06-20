using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;

namespace Database
{
    namespace SQLServer
    {
        public class Execution
        {
            private string FErrMessage = "";
            public string ErrMessage
            {
                get { return FErrMessage; }
                set { FErrMessage = value; }
            }

            public SqlTransaction Transaction = null;
            //-----------------------------------------------------------------------------------
            #region Khởi tạo Execution

            public Execution()
            {
                FErrMessage = "";
                Transaction = null;
            }

            #endregion

            #region Hủy

            public void Dispose()
            {
                if (FErrMessage == "")
                    FErrMessage = "";
                Transaction.Dispose();
            }

            #endregion

            #region Tạo và gọi Transaction

            public void BeginTransaction()
            {
                Transaction = Connection.SqlConnection.BeginTransaction();
            }

            //-----------------------------------------------------------------------------------
            public void BeginTransaction(string TransactionName)
            {
                Transaction = Connection.SqlConnection.BeginTransaction(TransactionName);
            }
            //-----------------------------------------------------------------------------------
            public void CommitTransaction()
            {
                if (Transaction != null)                   
                    Transaction.Commit();
            }
            //-----------------------------------------------------------------------------------
            public void Rollback()
            {
                if (Transaction != null)
                    Transaction.Rollback();
            }
            //-----------------------------------------------------------------------------------
            public void Rollback(string SavePointOrTransactionName)
            {
                if (Transaction != null)
                    Transaction.Rollback(SavePointOrTransactionName);
            }
            //-----------------------------------------------------------------------------------
            public void SavePoint(string SavePointName)
            {
                if (Transaction != null)
                    Transaction.Save(SavePointName);
            }

            #endregion

            #region Define SqlParameter (Tạo parameter và add vào SqlCommand)

            //Values là mảng string
            private void DefineSqlParameter(SqlCommand sqlCommand,
                         string[] Parameters, string[] Values)
            {
                SqlParameter sqlParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = Parameters[i];
                    sqlParameter.SqlValue = Values[i];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }

            //Values là mảng object
            private void DefineSqlParameter(SqlCommand sqlCommand,
                         string[] Parameters, object[] Values)
            {
                SqlParameter sqlParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = Parameters[i];
                    sqlParameter.SqlValue = Values[i];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }

            #endregion

            #region Các thực thi ExecuteNonQuery

            //Thực thi truy vấn không có tham số, có transaction
            /// <summary>
            /// Thực thi truy vấn có tham số, có sử dụng transaction, kết quả trả về là số dòng có hiệu lực
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery, CommandType commandType)
            {
                int efftectRecord = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = Connection.SqlConnection;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                try
                {
                    efftectRecord = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn : " + ex.Message;
                    throw;
                }
                return efftectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       string[] Values)
            {
                int efftectRecord = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                DefineSqlParameter(sqlCommand, Parameters, Values);
                try
                {
                    efftectRecord = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return efftectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values)
            {
                int effectRecord = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;

                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                DefineSqlParameter(sqlCommand, Parameters, Values);
                try
                {
                    effectRecord = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }
                return effectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo index trong mảng tham số
            /// <summary>
            /// Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo index trong mảng tham số
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="ReturnParameter"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values, int IndexOutParameter)
            {
                int result = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                SqlParameter sqlParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (i == IndexOutParameter)
                    {
                        sqlParameter = new SqlParameter(Parameters[i], SqlDbType.Int);
                        sqlParameter.Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = Parameters[i];
                        sqlParameter.SqlValue = Values[i];
                    }
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                try
                {
                    sqlCommand.ExecuteNonQuery();
                    result = (int)sqlCommand.Parameters[Parameters[IndexOutParameter]].Value;
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo tên tham số
            /// <summary>
            /// Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo tên tham số
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="ReturnParameter"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values, string OutParameterName)
            {
                int result = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;

                SqlParameter sqlParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (Parameters[i].Equals(OutParameterName))
                    {
                        sqlParameter = new SqlParameter(Parameters[i], SqlDbType.Int);
                        sqlParameter.Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = Parameters[i];
                        sqlParameter.SqlValue = Values[i];
                    }
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                try
                {
                    sqlCommand.ExecuteNonQuery();
                    if (sqlCommand.Parameters[OutParameterName].Value.ToString() != "")
                        result = (int)sqlCommand.Parameters[OutParameterName].Value;
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }

            #endregion

            #region Các thực thi ExecuteScalar (Thực thi truy vấn với kết quả trả về là hàng và cột đầu tiên)

            //Thực thi truy vấn không có tham số, có transaction
            /// <summary>
            /// Thực thi truy vấn có tham số, có sử dụng transaction, kết quả trả về là số dòng có hiệu lực
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery, CommandType commandType)
            {
                object result = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = Connection.SqlConnection;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                try
                {
                    result = sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn : " + ex.Message;
                    throw;
                }
                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery,
                       CommandType commandType, string[] Parameters,
                       string[] Values)
            {
                object result = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                DefineSqlParameter(sqlCommand, Parameters, Values);
                try
                {
                    result = sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values)
            {
                object result = 0;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.Connection = Connection.SqlConnection;
                sqlCommand.CommandType = commandType;

                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                DefineSqlParameter(sqlCommand, Parameters, Values);
                try
                {
                    result = sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }
                return result;
            }
            //-----------------------------------------------------------------------------------
            #endregion

            #region FillSqlDataReader (Lấy kết quả truy vấn và gán vào biến kiểu SqlDataReader)
            //-----------------------------------------------------------------------------------  
            public SqlDataReader FillSqlDataReader(string strQuery,
                CommandType commandType)
            {
                SqlDataReader sqlDataReader = null;
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection = Connection.SqlConnection;
                    if (Transaction != null)
                        sqlCommand.Transaction = Transaction;
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return sqlDataReader;
            }
            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public SqlDataReader FillSqlDataReader(string strQuery,
                CommandType commandType, string[] Parameters,
                string[] Values)
            {
                SqlDataReader sqlDataReader = null;
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, Connection.SqlConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    //sqlCommand.Connection = Connection.SqlConnection;
                    //if (Transaction != null)
                    //    sqlCommand.Transaction = Transaction;
                    DefineSqlParameter(sqlCommand, Parameters, Values);
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return sqlDataReader;
            }
            //-----------------------------------------------------------------------------------
            #endregion

            #region FillDataTable (Lấy kết quả truy vấn và gán vào biến kiểu SqlDataReader)
            //-----------------------------------------------------------------------------------  
            public DataTable FillDataTable(string strQuery,
                CommandType commandType)
            {
                DataTable DataTable = new DataTable();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection = Connection.SqlConnection;
                    if (Transaction != null)
                        sqlCommand.Transaction = Transaction;
                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);                    
                    SqlDataAdapter.Fill(DataTable);
                    SqlDataAdapter.Dispose();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataTable;
            }
            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public DataTable FillDataTable(string strQuery,
                CommandType commandType, string[] Parameters,
                string[] Values)
            {
                DataTable DataTable = new DataTable();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, Connection.SqlConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    //sqlCommand.Connection = Connection.SqlConnection;
                    //if (Transaction != null)
                    //    sqlCommand.Transaction = Transaction;
                    DefineSqlParameter(sqlCommand, Parameters, Values);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlDataAdapter.Fill(DataTable);
                    SqlDataAdapter.Dispose();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataTable;
            }
            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public DataTable FillDataTable(string strQuery,
                CommandType commandType, string[] Parameters,
                object[] Values)
            {
                DataTable DataTable = new DataTable();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, Connection.SqlConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.CommandTimeout = 300;
                    //sqlCommand.Connection = Connection.SqlConnection;
                    //if (Transaction != null)
                    //    sqlCommand.Transaction = Transaction;
                    DefineSqlParameter(sqlCommand, Parameters, Values);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlDataAdapter.Fill(DataTable);
                    SqlDataAdapter.Dispose();                    
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataTable;
            }

            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public DataTable FillDataTable(string strQuery,
                CommandType commandType, string[] Parameters,
                object[] Values, string OutParameterName, 
                out DateTime OutParameterValue)
            {
                OutParameterValue = DateTime.Now.Date;
                DataTable DataTable = new DataTable();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, Connection.SqlConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    //sqlCommand.Connection = Connection.SqlConnection;
                    //if (Transaction != null)
                    //    sqlCommand.Transaction = Transaction;
                    SqlParameter sqlParameter;
                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        if (Parameters[i].Equals(OutParameterName))
                        {
                            sqlParameter = new SqlParameter(Parameters[i], SqlDbType.SmallDateTime);
                            sqlParameter.Direction = ParameterDirection.Output;
                        }
                        else
                        {
                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = Parameters[i];
                            sqlParameter.SqlValue = Values[i];
                        }
                        sqlCommand.Parameters.Add(sqlParameter);
                    }

                    //DefineSqlParameter(sqlCommand, Parameters, Values);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlDataAdapter.Fill(DataTable);
                    SqlDataAdapter.Dispose();

                    sqlCommand.ExecuteNonQuery();
                    OutParameterValue = DateTime.Parse(sqlCommand.Parameters[OutParameterName].Value.ToString());
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataTable;
            }
            #endregion

            #region FillDataSet (Lấy kết quả truy vấn và gán vào biến kiểu SqlDataSet)
            //-----------------------------------------------------------------------------------  
            public DataSet FillDataSet(string strQuery,
                CommandType commandType)
            {
                DataSet DataSet = new DataSet();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection = Connection.SqlConnection;
                    if (Transaction != null)
                        sqlCommand.Transaction = Transaction;
                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlDataAdapter.Fill(DataSet);
                    SqlDataAdapter.Dispose();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataSet;
            }
            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public DataSet FillDataSet(string strQuery,
                CommandType commandType, string[] Parameters,
                string[] Values)
            {
                DataSet DataSet = new DataSet();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, Connection.SqlConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = commandType;                    
                    DefineSqlParameter(sqlCommand, Parameters, Values);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    SqlDataAdapter.Fill(DataSet);
                    SqlDataAdapter.Dispose();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return DataSet;
            }
            //-----------------------------------------------------------------------------------
            #endregion

            /*#region FillObjects (Lấy kết quả truy vấn và gán vào biến kiểu Object)

        public object[] FillObjects(string strQuery,
            CommandType commandType)
        {
            object[] result = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = Connection.SqlConnection;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                using(SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        result = new object[sqlDataReader.FieldCount];
                        sqlDataReader.GetValues(result);
                    }
                }
            }
            catch (Exception ex)
            {
                FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
            }
            return result;
        }

        //Truy vấn có tham số và giá trị 
        public object[] FillObjects(string strQuery,
            CommandType commandType, string[] Parameters,
            string[] Values)
        {
            object[] result = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = commandType;
                sqlCommand.Connection = Connection.SqlConnection;
                if (Transaction != null)
                    sqlCommand.Transaction = Transaction;
                DefineSqlParameter(sqlCommand, Parameters, Values);

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.Read())
                    {
                        result = new object[sqlDataReader.FieldCount];
                        sqlDataReader.GetValues(result);
                    }
                }
            }
            catch (Exception ex)
            {
                FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
            }
            return result;
        }
       
        #endregion */

            #region FillArrayList (Lấy kết quả truy vấn và gán vào biến kiểu ArrayList)

            public ArrayList FillArrayLists(string strSQL,
                CommandType commandType, string[] Parameters,
             string[] Values)
            {
                ArrayList arrayList = new ArrayList();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(strSQL, Connection.SqlConnection,
                                                          Transaction);
                    sqlCommand.CommandType = commandType;
                    DefineSqlParameter(sqlCommand, Parameters, Values);
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            object[] objs = new object[sqlDataReader.FieldCount];
                            sqlDataReader.GetValues(objs);
                            arrayList.Add(objs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }

                return arrayList;
            }
            //-----------------------------------------------------------------------------------
            public ArrayList FillArrayLists(string strQuery,
                CommandType commandType)
            {
                ArrayList arrayList = new ArrayList();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        strQuery, Connection.SqlConnection, Transaction);
                    sqlCommand.CommandType = commandType;
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            object[] objs = new object[sqlDataReader.FieldCount];
                            sqlDataReader.GetValues(objs);
                            arrayList.Add(objs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }

                return arrayList;
            }
            //-----------------------------------------------------------------------------------
            #endregion

        }
    }

    namespace OLEDB
    {
        public class Execution
        {
            private string FErrMessage = "";
            public string ErrMessage
            {
                get { return FErrMessage; }
                set { FErrMessage = value; }
            }

            public OleDbTransaction Transaction = null;
            //-----------------------------------------------------------------------------------
            #region Khởi tạo Execution

            public Execution()
            {
                FErrMessage = "";
                Transaction = null;
            }

            #endregion

            #region Hủy

            public void Dispose()
            {
                if (FErrMessage == "")
                    FErrMessage = "";
                Transaction.Dispose();
            }

            #endregion

            #region Tạo và gọi Transaction

            public void BeginTransaction()
            {
                Transaction = Connection.OLEDBConnection.BeginTransaction();
            }            
            
            //-----------------------------------------------------------------------------------
            public void CommitTransaction()
            {
                if (Transaction != null)
                    Transaction.Commit();
            }
            //-----------------------------------------------------------------------------------
            public void Rollback()
            {
                if (Transaction != null)
                    Transaction.Rollback();
            }                     

            #endregion

            #region Define OLEDBParameter (Tạo parameter và add vào SqlCommand)

            //Values là mảng string
            private void DefineOleDbParameter(OleDbCommand oleDBCommand,
                         string[] Parameters, string[] Values)
            {
                OleDbParameter OleDBParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    OleDBParameter = new OleDbParameter();
                    OleDBParameter.ParameterName = Parameters[i];
                    OleDBParameter.Value = Values[i];
                    oleDBCommand.Parameters.Add(OleDBParameter);
                }
            }

            //Values là mảng object
            private void DefineOleDbParameter(OleDbCommand OleDbCommand,
                         string[] Parameters, object[] Values)
            {
                OleDbParameter OleDbParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    OleDbParameter = new OleDbParameter();
                    OleDbParameter.ParameterName = Parameters[i];
                    OleDbParameter.Value = Values[i];
                    OleDbCommand.Parameters.Add(OleDbParameter);
                }
            }

            #endregion

            #region Các thực thi ExecuteNonQuery

            //Thực thi truy vấn không có tham số, có transaction
            /// <summary>
            /// Thực thi truy vấn có tham số, có sử dụng transaction, kết quả trả về là số dòng có hiệu lực
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery, CommandType commandType)
            {
                int efftectRecord = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.CommandType = commandType;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                try
                {
                    efftectRecord = OleDbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn : " + ex.Message;
                    throw;
                }
                return efftectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       string[] Values)
            {
                int efftectRecord = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                DefineOleDbParameter(OleDbCommand, Parameters, Values);
                try
                {
                    efftectRecord = OleDbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return efftectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values)
            {
                int effectRecord = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;

                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                DefineOleDbParameter(OleDbCommand, Parameters, Values);
                try
                {
                    effectRecord = OleDbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }
                return effectRecord;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo index trong mảng tham số
            /// <summary>
            /// Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo index trong mảng tham số
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="ReturnParameter"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values, int IndexOutParameter)
            {
                int result = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                //DefineSqlParameter(sqlCommand, Parameters, Values);
                OleDbParameter OleDbParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (i == IndexOutParameter)
                    {
                        OleDbParameter = new OleDbParameter(Parameters[i], OleDbType.Integer);
                        OleDbParameter.Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        OleDbParameter = new OleDbParameter();
                        OleDbParameter.ParameterName = Parameters[i];
                        OleDbParameter.Value = Values[i];
                    }
                    OleDbCommand.Parameters.Add(OleDbParameter);
                }
                try
                {
                    OleDbCommand.ExecuteNonQuery();
                    result = (int)OleDbCommand.Parameters[Parameters[IndexOutParameter]].Value;
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo tên tham số
            /// <summary>
            /// Thực thi truy vấn và có yêu cấu trả lại giá trị (thuộc tính output) theo tên tham số
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="ReturnParameter"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public int ExecuteNonQuery(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values, string OutParameterName)
            {
                int result = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;

                OleDbParameter OleDbParameter;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (Parameters[i].Equals(OutParameterName))
                    {
                        OleDbParameter = new OleDbParameter(Parameters[i], OleDbType.Integer);
                        OleDbParameter.Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        OleDbParameter = new OleDbParameter();
                        OleDbParameter.ParameterName = Parameters[i];
                        OleDbParameter.Value = Values[i];
                    }
                    OleDbCommand.Parameters.Add(OleDbParameter);
                }
                try
                {
                    OleDbCommand.ExecuteNonQuery();
                    result = (int)OleDbCommand.Parameters[OutParameterName].Value;
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }

            #endregion

            #region Các thực thi ExecuteScalar (Thực thi truy vấn với kết quả trả về là hàng và cột đầu tiên)

            //Thực thi truy vấn không có tham số, có transaction
            /// <summary>
            /// Thực thi truy vấn có tham số, có sử dụng transaction, kết quả trả về là số dòng có hiệu lực
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery, CommandType commandType)
            {
                object result = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.CommandType = commandType;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                try
                {
                    result = OleDbCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn : " + ex.Message;
                    throw;
                }
                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu string), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery,
                       CommandType commandType, string[] Parameters,
                       string[] Values)
            {
                object result = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;
                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                DefineOleDbParameter(OleDbCommand, Parameters, Values);
                try
                {
                    result = OleDbCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }

                return result;
            }
            //-----------------------------------------------------------------------------------
            //Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// <summary>
            /// Thực thi truy vấn với nhiều tham số và nhiều giá trị(mang kiểu object), có sử dụng transaction
            /// </summary>
            /// <param name="strQuery"></param>
            /// <param name="commandType"></param>
            /// <param name="Parameters"></param>
            /// <param name="Values"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            public object ExecuteScalar(string strQuery,
                       CommandType commandType, string[] Parameters,
                       object[] Values)
            {
                object result = 0;
                OleDbCommand OleDbCommand = new OleDbCommand();
                OleDbCommand.CommandText = strQuery;
                OleDbCommand.Connection = Connection.OLEDBConnection;
                OleDbCommand.CommandType = commandType;

                if (Transaction != null)
                    OleDbCommand.Transaction = Transaction;
                DefineOleDbParameter(OleDbCommand, Parameters, Values);
                try
                {
                    result = OleDbCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi thực hiện truy vấn: " + ex.Message;
                    throw;
                }
                return result;
            }
            //-----------------------------------------------------------------------------------
            #endregion

            #region FillSqlDataReader (Lấy kết quả truy vấn và gán vào biến kiểu SqlDataReader)
            //-----------------------------------------------------------------------------------  
            public OleDbDataReader FillSqlDataReader(string strQuery,
                CommandType commandType)
            {
                OleDbDataReader OleDbDataReader = null;
                try
                {
                    OleDbCommand OleDbCommand = new OleDbCommand();
                    OleDbCommand.CommandText = strQuery;
                    OleDbCommand.CommandType = commandType;
                    OleDbCommand.Connection = Connection.OLEDBConnection;
                    if (Transaction != null)
                        OleDbCommand.Transaction = Transaction;
                    OleDbDataReader = OleDbCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return OleDbDataReader;
            }
            //-----------------------------------------------------------------------------------
            //Truy vấn có mảng tham số và mảng giá trị tương ứng
            public OleDbDataReader FillOLEDBDataReader(string strQuery,
                CommandType commandType, string[] Parameters,
                string[] Values)
            {
                OleDbDataReader OleDbDataReader = null;
                try
                {
                    OleDbCommand OleDbCommand = new OleDbCommand(strQuery, Connection.OLEDBConnection, Transaction);
                    //sqlCommand.CommandText = strQuery;
                    OleDbCommand.CommandType = commandType;
                    //sqlCommand.Connection = Connection.SqlConnection;
                    //if (Transaction != null)
                    //    sqlCommand.Transaction = Transaction;
                    DefineOleDbParameter(OleDbCommand, Parameters, Values);
                    OleDbDataReader = OleDbCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }
                return OleDbDataReader;
            }
            //-----------------------------------------------------------------------------------
            #endregion            

            #region FillArrayList (Lấy kết quả truy vấn và gán vào biến kiểu ArrayList)

            public ArrayList FillArrayLists(string strSQL,
                CommandType commandType, string[] Parameters,
             string[] Values)
            {
                ArrayList arrayList = new ArrayList();
                try
                {
                    OleDbCommand OleDbCommand = new OleDbCommand(strSQL, Connection.OLEDBConnection, Transaction);
                    OleDbCommand.CommandType = commandType;
                    DefineOleDbParameter(OleDbCommand, Parameters, Values);
                    using (OleDbDataReader OleDbDataReader = OleDbCommand.ExecuteReader())
                    {
                        while (OleDbDataReader.Read())
                        {
                            object[] objs = new object[OleDbDataReader.FieldCount];
                            OleDbDataReader.GetValues(objs);
                            arrayList.Add(objs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }

                return arrayList;
            }
            //-----------------------------------------------------------------------------------
            public ArrayList FillArrayLists(string strQuery,
                CommandType commandType)
            {
                ArrayList arrayList = new ArrayList();
                try
                {
                    OleDbCommand OleDbCommand = new OleDbCommand(
                        strQuery, Connection.OLEDBConnection, Transaction);
                    OleDbCommand.CommandType = commandType;
                    using (OleDbDataReader OleDbDataReader = OleDbCommand.ExecuteReader())
                    {
                        while (OleDbDataReader.Read())
                        {
                            object[] objs = new object[OleDbDataReader.FieldCount];
                            OleDbDataReader.GetValues(objs);
                            arrayList.Add(objs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FErrMessage = "Lỗi khi truy vấn dữ liệu : " + ex.Message;
                }

                return arrayList;
            }
            //-----------------------------------------------------------------------------------
            #endregion

        }
    }
}
