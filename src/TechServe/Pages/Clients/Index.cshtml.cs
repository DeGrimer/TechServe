using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.SqlExt;

namespace TechServe.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClient = new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = Connection.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT id, name, email, phone FROM client";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.Id = "" + reader.GetInt32(0);
                                clientInfo.Name = reader.GetString(1);
                                clientInfo.Email = reader.GetString(2);
                                clientInfo.Phone = "" + reader.GetString(3);
                                listClient.Add(clientInfo);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
}
