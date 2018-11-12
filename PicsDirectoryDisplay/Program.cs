using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplay
{
    class Program
    {
       public static void Main(string[] args)
        {
            //string password = "Garrett!@#$";
            //SecureString sec_pass = new SecureString();
            //Array.ForEach(password.ToArray(), sec_pass.AppendChar);
            //sec_pass.MakeReadOnly();
            //SqlConnection con = new SqlConnection(@"Server = Data Source = CZ19LTBL7XZM2\SQLEXPRESS01; Database = CalibrationTool", new SqlCredential("tlogin", sec_pass));
            ////SqlConnection con = new SqlConnection(@"Server =CZ19LTBL7XZM2\SQLEXPRESS01; Database =CalibrationTool; Trusted_Connection = true");
            //con.Open();
            //con.Close();
            //con.Dispose();

            string password = "Garrett!@#$";
            SecureString sec_pass = new SecureString();
            Array.ForEach(password.ToArray(), sec_pass.AppendChar);
            sec_pass.MakeReadOnly();
            //working//SqlConnection con = new SqlConnection(@"Data Source=CZ19LTBL7XZM2\SQLEXPRESS01;Persist Security Info=True;Initial Catalog=CalibrationTool", new SqlCredential("tlogin", sec_pass));
            //////SqlConnection con = new SqlConnection(@"Server =CZ19LTBL7XZM2\SQLEXPRESS01; Database =CalibrationTool; Trusted_Connection = true");
            ///
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS01;Persist Security Info=True;Initial Catalog=CalibrationTool;User ID=tlogin;Password=Garrett!@#$;");
            //SqlConnection con = new SqlConnection("Data Source=CZ19LTBL7XZM2/SQLEXPRESS01;Persist Security Info=True;Initial Catalog=CalibrationTool;User ID=tlogin;Password=Garrett!@#$;");
            con.Open();
            con.Close();
            con.Dispose();
            return;


            //return;
            using (SqlConnection conn = new SqlConnection())
            { 
                //conn.ConnectionString = "Server=Data Source=CZ19LTBL7XZM2\\SQLEXPRESS01;Database=CalibrationTool;Persist Security Info=True;User ID=tlogin;Password=Garrett!@#$;MultipleActiveResultSets=True";
                //conn.ConnectionString = "Server=Data Source=CZ19LTBL7XZM2\\SQLEXPRESS01;Database=CalibrationTool;Persist Security Info=True;User ID=tlogin;Password=Garrett!@#$;MultipleActiveResultSets=True";
                conn.ConnectionString = @"Server=Data Source=CZ19LTBL7XZM2\SQLEXPRESS01;Initial Catalog=CalibrationTool;Persist Security Info=True;User ID=tlogin;Password=Garrett!@#$;";
                // using the code here...
                //< add name = "CalTool" connectionString = "Data Source=CZ19LTBL7XZM2\SQLEXPRESS01;Initial Catalog=CalibrationTool; Integrated Security=SSPI;" providerName = "System.Data.SqlClient" />
                conn.Open();
                
                SqlCommand command = new SqlCommand("SELECT * FROM Users", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        Console.WriteLine(String.Format("{0} \t | {1} \t | {2} \t | {3}",
                        // call the objects from their index
                        reader[0], reader[1], reader[2], reader[3]));
                    }
                }
                // use the connection here

                conn.Close();
                //conn.Dipose();
            }

           

        }

        public static async Task DirSearchAsync()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 400; i++)
                {
                    Console.WriteLine(" Method 1");
                }
            });
            Console.WriteLine("End async");
        }
    }
}
