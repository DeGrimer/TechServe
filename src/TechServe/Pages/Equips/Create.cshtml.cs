using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TechServe.Models;
using TechServe.SqlExt;

namespace TechServe.Pages.Equips
{
    public class CreateModel : PageModel
    {
        public EquipInfo equipInfo = new EquipInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            equipInfo.producer = Request.Form["producer"];
            equipInfo.model = Request.Form["model"];
            equipInfo.guarantee_period_month = Request.Form["guarantee_period_month"];

            if(equipInfo.producer.Length == 0 || equipInfo.model.Length == 0 || 
                equipInfo.guarantee_period_month.Length == 0)
            {
                errorMessage = "Необходимо заполнить все поля";
                return;
            }

            //save the new equip into the database
            try
            {
                string connectionString = Connection.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO equip "+
                                 "(producer, model, guarantee_period_month) VALUES "+
                                 "(@producer, @model, @guarantee_period_month);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@producer", equipInfo.producer);
                        command.Parameters.AddWithValue("@model", equipInfo.model);
                        command.Parameters.AddWithValue("@guarantee_period_month", equipInfo.guarantee_period_month);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            equipInfo.producer = "";
            equipInfo.model = "";
            equipInfo.guarantee_period_month = "";
            successMessage = "Новое оборудование успешно добавлено";

            Response.Redirect("/Equips/Index");
        }
    }
}
