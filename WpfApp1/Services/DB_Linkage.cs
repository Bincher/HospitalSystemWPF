using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using WpfApp1.Model;
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
                            Gender = reader.GetByte("gender"),
                            Birth = reader.GetDateTime("birth"),
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

        public List<Treatment> GetTreatment()
        {
            List<Treatment> treatments = new List<Treatment>();
            string query = @"
                SELECT 
                    t.id AS TreatmentId,
                    t.date AS TreatmentDate,
                    d.department AS DoctorDepartment,
                    d.name AS DoctorName,
                    p.name AS PatientName,
                    t.complete AS TreatmentComplete
                FROM 
                    treatment t
                INNER JOIN doctor d ON t.doctor_id = d.id
                INNER JOIN patient p ON t.patient_id = p.id";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        treatments.Add(new Treatment
                        {
                            Id = reader.GetInt32("TreatmentId"),
                            Date = reader.GetString("TreatmentDate"),
                            DoctorDepartment = reader.GetString("DoctorDepartment"),
                            DoctorName = reader.GetString("DoctorName"),
                            PatientName = reader.GetString("PatientName"),
                            Complete = reader.GetBoolean("TreatmentComplete")
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

            return treatments;
        }

    }

}