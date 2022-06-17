using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.Pages.Equips;
using TechServe.SqlExt;

namespace TechServe.Pages.Equips_sell
{
    public class CreateModel : PageModel
    {
        public Models.EquipSellInfo equip_sell_info = new Models.EquipSellInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public List<ClientInfo> clientInfos = new List<ClientInfo>();
        public List<EquipInfo> equipInfos = new List<EquipInfo>();
        public List<SelectListItem> optionName { get; set; }
        public List<SelectListItem> optionModel { get; set; }
        public void OnGet()
        {
            try
            {
                string connectionString = Connection.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT id, name FROM client";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.Id = "" + reader.GetInt32(0);
                                clientInfo.Name = reader.GetString(1);
                                clientInfos.Add(clientInfo);
                            }
                        }

                    }
                    sql = "SELECT id, producer, model FROM equip";
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
                                equipInfos.Add(equipInfo);
                            }
                        }

                    }

                }
                optionName = clientInfos.Select(x =>
                                                new SelectListItem
                                                {
                                                    Value = x.Id,
                                                    Text = x.Name
                                                }).ToList();
                optionModel = equipInfos.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.id,
                                    Text = x.producer+" "+x.model
                                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost()
        {
            equip_sell_info.NameId = Request.Form["userName"];
            equip_sell_info.EquipId = Request.Form["model"];

            if (equip_sell_info.NameId.Length == 0 || equip_sell_info.EquipId.Length == 0)
            {
                errorMessage = "Необходимо заполнить все поля";
                return;
            }

            //save the new equip into the database
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=techDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO equip_sell " +
                                 "(equip_id, client_id) VALUES " +
                                 "(@equip_id, @client_id);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@equip_id", equip_sell_info.EquipId);
                        command.Parameters.AddWithValue("@client_id", equip_sell_info.NameId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            equip_sell_info.EquipId = "";
            equip_sell_info.NameId = "";
            successMessage = "Новая продажа успешно добавлена";

            Response.Redirect("/Equips_sell/Index");
        }
    }
}
