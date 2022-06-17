using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.SqlExt;

namespace TechServe.Pages.Equips
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<EquipInfo> listEquip = new List<EquipInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = Connection.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM equip";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EquipInfo equipInfo = new EquipInfo();
                                equipInfo.id = "" + reader.GetInt32(0);
                                equipInfo.producer = reader.GetString(1);
                                equipInfo.model = reader.GetString(2);
                                equipInfo.guarantee_period_month = ""+ reader.GetInt32(3);
                                listEquip.Add(equipInfo);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex.ToString());
            }
        }
    }
}
