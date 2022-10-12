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
    [Route("List of workers")]
    public class WorkerController : ControllerBase
    {
        private static List<Worker> workers = new List<Worker>();
        public static string connectionString = "Data Source=DESKTOP-MBAMDPS\\SQLEXPRESS;Initial Catalog = \'BasedData\'; Integrated Security = True; TrustServerCertificate = yes";

        [HttpGet]
        public IEnumerable<Worker> Get() 
        {
                string sql = "SELECT * FROM Worker";
                workers.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        var titleColumnName = reader.GetOrdinal(nameof(Worker.Id));
                        var companyColumnName = reader.GetOrdinal(nameof(Worker.Name));
                        var priceColumnName = reader.GetOrdinal(nameof(Worker.Telephone));
                        var squareColumnName = reader.GetOrdinal(nameof(Worker.Office));

                        while (reader.Read())
                        {
                            Worker record = new Worker();

                            record.Id = reader.GetInt32(titleColumnName);
                            record.Name = reader.GetString(companyColumnName);
                            record.Telephone = reader.GetString(priceColumnName);
                            record.Office = reader.GetInt32(squareColumnName);
                            workers.Add(record);
                        }
                    }
                    connection.Close();
                }
                return workers;
            
        }

        [HttpGet("Add worker")]
        public IActionResult Add(string Names, string Telephones, int Offices)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string a = "INSERT INTO Worker VALUES(@name, @telephone, @office)";
                SqlParameter par1 = new SqlParameter("@name", Names);
                SqlParameter par2 = new SqlParameter("@telephone", Telephones);
                SqlParameter par3 = new SqlParameter("@office", Offices);
                SqlCommand com = new SqlCommand(a, connection);
                com.Parameters.Add(par1);
                com.Parameters.Add(par2);
                com.Parameters.Add(par3);
                com.ExecuteScalar();
                connection.Close();
            }
            return Ok();
        }

        [HttpGet("Change worker")]
        public IActionResult Change(int Ids, string Names, string Telephones, int Offices)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string a = "Update Worker set Name = @name, Telephone = @telephone, Office = @office where id = @id";
                SqlParameter par1 = new SqlParameter("@name", Names);
                SqlParameter par2 = new SqlParameter("@telephone", Telephones);
                SqlParameter par3 = new SqlParameter("@office", Offices);
                SqlParameter par4 = new SqlParameter("@id", Ids);
                SqlCommand com = new SqlCommand(a, connection);
                com.Parameters.Add(par1);
                com.Parameters.Add(par2);
                com.Parameters.Add(par3);
                com.Parameters.Add(par4);
                com.ExecuteScalar();
                connection.Close();
            }
            return Ok();
        }

        [HttpGet("Delete worker")]
        public IActionResult Delete(int Ids)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query1 = "DELETE FROM Worker WHERE id = @a";
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