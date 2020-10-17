using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.Runtime.CompilerServices;

namespace ScholarCounter
{
    class MySqlDao
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private int port;

        //Constructor
        public MySqlDao()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            try
            {
                server = "localhost";
                port = 3306;
                database = "Data_Base_Name";
                uid = "Username";
                password = "Password";

                string connectionString;
                connectionString =
                    "Server = " + server +
                    "; Port = " + port +
                    "; Database =  " + database +
                    "; Uid = " + uid +
                    "; Pwd = " + password +
                    "; ";

                connection = new MySqlConnection(connectionString);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public DataTable ExecuteInsertUpdateQuery(string query)
        {
            DataTable ds = new DataTable();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                da.Fill(ds);
                return ds;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return ds;
        }


        public DataTable ExecuteSelectQuery(string SQLQuery)
        {
            DataTable ds = new DataTable();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(SQLQuery, connection);
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show("ExecuteSelectQuery: " + e.Message);
            }
            return ds;
        }

    }
}
