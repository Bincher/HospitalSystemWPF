using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using WpfApp1.Models;

namespace DB_Linkage.Service
{
    public class MySQLManager
    {
        private readonly string connectionString = "Server=localhost;Database=hospital;Port=3306;User=root;Password=root";

        public MySqlConnection Connection { get; private set; }

        public MySQLManager()
        {
            Connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                {
                    Connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void CloseConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }


        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            string query = "SELECT id, name, department, birth, gender, profile_image FROM doctor";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Department = reader.GetString("department"),
                            Birth = reader.GetDateTime("birth"),
                            Gender = reader.GetByte("gender"),
                            ProfileImage = reader.IsDBNull(reader.GetOrdinal("profile_image")) ? null : reader.GetString("profile_image")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return doctors;
        }

        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();
            string query = "SELECT id, name, gender, birth, profile_image FROM patient";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? 0 : (reader.GetString("gender") == "남성" ? 1 : 0),
                            Birth = reader.IsDBNull(reader.GetOrdinal("birth")) ? null : (DateTime?)reader.GetDateTime("birth"),
                            ProfileImage = reader.IsDBNull(reader.GetOrdinal("profile_image")) ? null : reader.GetString("profile_image")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return patients;
        }

    }

}