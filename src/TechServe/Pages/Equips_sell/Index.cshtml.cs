using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.SqlExt;

namespace TechServe.Pages.Equips_sell
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public string NameSort { get; set; } = "name_asc";
        public string DateSort { get; set; } = "date_asc";
        public string SearchString { get; set; }

        public List<EquipSellInfo> SellsInfo = new List<EquipSellInfo>();
        public void OnGet(string nameSortOrder, string dateSortOrder)
        {
            var sort = "";
            if (!string.IsNullOrEmpty(nameSortOrder))
            {
                NameSort = nameSortOrder == "name_desc" ? "name_asc" : "name_desc";
                sort += NameSort;
            }

            if (!string.IsNullOrEmpty(dateSortOrder))
            {
                DateSort = dateSortOrder == "date_desc" ? "date_asc" : "date_desc";
                sort += DateSort;
            }
            SearchString = Request.Query["name"];
            string connectionString = Connection.ConnectionString;
            try
            {
                using(var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT equip_sell.id as id, name, producer, model,sell_date, guarantee_period_month " +
                                  "FROM equip_sell INNER JOIN equip ON equip_sell.equip_id = equip.id " +
                                   "INNER JOIN client ON equip_sell.client_id = client.id ";
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        sql += @"WHERE name="+"\'"+SearchString+"\'";
                    }
                    sql += sort switch
                    {
                        "name_desc" => @"ORDER BY name DESC",
                        "name_asc" => @"ORDER BY name",
                        "date_desc" => @"ORDER BY sell_date DESC",
                        "date_asc" => @"ORDER BY sell_date",
                        _ => ""
                    };
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EquipSellInfo equipSellInfo = new EquipSellInfo();
                                equipSellInfo.id = ""+reader.GetInt32(0);
                                equipSellInfo.name = reader.GetString(1);
                                equipSellInfo.producer = reader.GetString(2);
                                equipSellInfo.model = reader.GetString(3);
                                equipSellInfo.sell_date = ""+reader.GetDateTime(4);
                                int guarant = reader.GetInt32(5);
                                int diff = DateTime.Now.Month - reader.GetDateTime(4).Month + 12 * (DateTime.Now.Year -
                                    reader.GetDateTime(4).Year);
                                equipSellInfo.guarantee = (diff < guarant) ? true : false;
                                SellsInfo.Add(equipSellInfo);                            
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex.Message);
            }
        }
    }
}
