using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Database
{
    namespace SQLServer
    {
        public class SQLServer
        {
            public Connection Connection;            
            public Execution Execution;

            #region Khởi tạo
            //-----------------------------------------------------------------------------------
            public SQLServer()
            {
                Connection = new Connection();
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------
            public SQLServer(string StringConnection)
            {
                Connection = new Connection(StringConnection);
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------
            public SQLServer(Connection connection)
            {
                Connection = connection;
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------
            public SQLServer(bool FromTextFileOrAppConfig, string FilePathOrNodeName)
            {
                Connection = new Connection(FromTextFileOrAppConfig, FilePathOrNodeName);
                Execution = new Execution();
            }
            #endregion

            #region Hủy

            public void Dispose()
            {
                Connection.Dispose();
                Execution.Dispose();
            }

            #endregion

        }
    }
    namespace OLEDB
    {
        public class OLEDB
        {
            public Connection Connection;
            public Execution Execution;

        #region Khởi tạo
            //-----------------------------------------------------------------------------------
            public OLEDB()
            {
                Connection = new Connection();
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------
            public OLEDB(string StringConnection)
            {
                Connection = new Connection(StringConnection);
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------
            public OLEDB(Connection connection)
            {
                Connection = connection;
                Execution = new Execution();
            }
            //-----------------------------------------------------------------------------------           
        #endregion

        #region Hủy

            public void Dispose()
            {
                Connection.Dispose();
                Execution.Dispose();
            }

        #endregion

        }
    }
}
