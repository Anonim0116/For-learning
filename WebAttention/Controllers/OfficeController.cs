using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace WebAttention.Controllers
{
    [ApiController]
    [Route("List of offices")]
    public class OfficeController : ControllerBase
    {
        private static List<Office> offices = new List<Office>();
        public static string connectionString = "Data Source=DESKTOP-MBAMDPS\\SQLEXPRESS;Initial Catalog = \'BasedData\'; Integrated Security = True; TrustServerCertificate = yes";

        [HttpGet]
        public IEnumerable<Office> Get()
        {
            string sql = "SELECT * FROM Office";
            offices.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    var titleColumnName = reader.GetOrdinal(nameof(Office.Id));
                    var companyColumnName = reader.GetOrdinal(nameof(Office.Name));
                    var priceColumnName = reader.GetOrdinal(nameof(Office.OfficialTelephone));

                    while (reader.Read())
                    {
                        Office record = new Office();

                        record.Id = reader.GetInt32(titleColumnName);
                        record.Name = reader.GetString(companyColumnName);
                        record.OfficialTelephone = reader.GetString(priceColumnName);
                        offices.Add(record);
                    }
                }
                connection.Close();
            }
            return offices;

        }

        [HttpGet("Add office")]
        public IActionResult Add(string Names, string Telephones)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string a = "INSERT INTO Office VALUES(@name, @telephone)";
                SqlParameter par1 = new SqlParameter("@name", Names);
                SqlParameter par2 = new SqlParameter("@telephone", Telephones);
                SqlCommand com = new SqlCommand(a, connection);
                com.Parameters.Add(par1);
                com.Parameters.Add(par2);
                com.ExecuteScalar();
                connection.Close();
            }
            return Ok();
        }

        [HttpGet("Change office")]
        public IActionResult Change(int Ids, string Names, string Telephones)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string a = "Update Office set Name = @name, OfficialTelephone = @telephone where id = @id";
                SqlParameter par1 = new SqlParameter("@name", Names);
                SqlParameter par2 = new SqlParameter("@telephone", Telephones);
                SqlParameter par3 = new SqlParameter("@id", Ids);
                SqlCommand com = new SqlCommand(a, connection);
                com.Parameters.Add(par1);
                com.Parameters.Add(par2);
                com.Parameters.Add(par3);
                com.ExecuteScalar();
                connection.Close();
            }
            return Ok();
        }

        [HttpGet("Delete office")]
        public IActionResult Delete(int Ids)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query1 = "DELETE FROM Office WHERE id = @a";
                SqlParameter par1 = new SqlParameter("@a", Ids);
                SqlCommand com = new SqlCommand(query1, connection);
                com.Parameters.Add(par1);
                com.ExecuteScalar();
                connection.Close();
            }
            return Ok();
        }

    }
}