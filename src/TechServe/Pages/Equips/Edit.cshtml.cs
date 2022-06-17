using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.SqlExt;

namespace TechServe.Pages.Equips
{
    public class EditModel : PageModel
    {
        public EquipInfo equipInfo = new EquipInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = Connection.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM equip WHERE id=@id";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                equipInfo.id = "" + reader.GetInt32(0);
                                equipInfo.producer = reader.GetString(1);
                                equipInfo.model = reader.GetString(2);
                                equipInfo.guarantee_period_month = "" + reader.GetInt32(3);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            equipInfo.producer = Request.Form["producer"];
            equipInfo.model = Request.Form["model"];
            equipInfo.guarantee_period_month = Request.Form["guarantee_period_month"];
            equipInfo.id = Request.Query["id"];

            if (equipInfo.producer.Length == 0 || equipInfo.model.Length == 0 ||
                equipInfo.guarantee_period_month.Length == 0)
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
                    string sql = "UPDATE equip " +
                                 "SET producer=@producer, model=@model, guarantee_period_month=@guarantee_period_month " +
                                 "WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@producer", equipInfo.producer);
                        command.Parameters.AddWithValue("@model", equipInfo.model);
                        command.Parameters.AddWithValue("@guarantee_period_month", equipInfo.guarantee_period_month);
                        command.Parameters.AddWithValue("@id", equipInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Данные оборудования успешно обновлено";

            Response.Redirect("/Equips/Index");
        }
    }
}
