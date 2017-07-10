using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp.Utils
{
    public class SqlManager
    {
        private SqlConnection conn;
        private SqlCommand cmd;

        public SqlManager(String storedProcedure)
        {
            conn = new SqlConnection("Data Source=maxoft.hu,51063;Initial Catalog=eMaxWb;Integrated Security=false;User ID=BalintToth;Password=BalintToth2017;Connect Timeout=200;");
            cmd = new SqlCommand(storedProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public void addParam(String column, System.Data.SqlDbType dbType, Object param)
        {
            cmd.Parameters.Add(column, dbType).Value = param;
        }

        public List<List<String>> getData()
        {
            conn.Open();
            List<List<String>> result = new List<List<String>>();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    List<String> record = new List<String>();
                    string data;
                    for (var i = 0; i < rdr.FieldCount; i = i + 1)
                    {
                        data = rdr.GetValue(i).ToString();
                        record.Add(data);
                    }
                    result.Add(record);
                }
            }
            conn.Close();
            return result;
        }

        public string updateRow()
        {
            conn.Open();
            string result = (string) cmd.ExecuteScalar();
            conn.Close();
            return result;
        }
    }
}
