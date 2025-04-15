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
            string query = "SELECT * FROM doctor WHERE is_deleted = 0";

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
            string query = "SELECT id, name, gender, birth, profile_image FROM patient WHERE is_deleted = 0";

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

        public bool AddDoctor(Doctor doctor)
        {
            string query = "INSERT INTO doctor (name, department, birth, gender, profile_image) VALUES (@Name, @Department, @Birth, @Gender, @ProfileImage)";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Name", doctor.Name);
                    cmd.Parameters.AddWithValue("@Department", doctor.Department);
                    cmd.Parameters.AddWithValue("@Birth", doctor.Birth);
                    cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                    cmd.Parameters.AddWithValue("@ProfileImage", doctor.ProfileImage ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool HardDeleteDoctor(int doctorId)
        {
            string query = "DELETE FROM doctor WHERE id = @Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", doctorId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deleting doctor: {ex.Message}");

                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool SoftDeleteDoctor(int doctorId)
        {
            string query = "UPDATE doctor SET is_deleted = 1 WHERE id = @Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", doctorId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deleting doctor: {ex.Message}");

                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            string query = "UPDATE doctor SET name=@Name, department=@Department, birth=@Birth, gender=@Gender, profile_image=@ProfileImage WHERE id=@Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", doctor.Id);
                    cmd.Parameters.AddWithValue("@Name", doctor.Name);
                    cmd.Parameters.AddWithValue("@Department", doctor.Department);
                    cmd.Parameters.AddWithValue("@Birth", doctor.Birth);
                    cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                    cmd.Parameters.AddWithValue("@ProfileImage", doctor.ProfileImage ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during updating doctor: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AddPatient(Patient patient)
        {
            string query = "INSERT INTO patient (name, birth, gender, profile_image) VALUES (@Name, @Birth, @Gender, @ProfileImage)";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@Birth", patient.Birth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@ProfileImage", patient.ProfileImage ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool SoftDeletePatient(int patientId)
        {
            string query = "UPDATE patient SET is_deleted = 1 WHERE id = @Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", patientId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deleting patient: {ex.Message}");

                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool HardDeletePatient(int patientId)
        {
            string query = "DELETE FROM patient WHERE id = @Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", patientId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deleting patient: {ex.Message}");

                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdatePatient(Patient patient)
        {
            string query = "UPDATE patient SET name=@Name, birth=@Birth, gender=@Gender, profile_image=@ProfileImage WHERE id=@Id";

            try
            {
                OpenConnection();

                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", patient.Id);
                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@Birth", patient.Birth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@ProfileImage", patient.ProfileImage ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during updating patient: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AddTreatment(Treatment treatment)
        {
            string query = "INSERT INTO treatment (date, complete, doctor_id, patient_id) VALUES (@DateTimeValue, @CompleteStatus, @DoctorIdValue, @PatientIdValue)";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@DateTimeValue", treatment.Date);
                    cmd.Parameters.AddWithValue("@CompleteStatus", treatment.Complete ? 1 : 0);
                    cmd.Parameters.AddWithValue("@DoctorIdValue", treatment.DoctorId);
                    cmd.Parameters.AddWithValue("@PatientIdValue", treatment.PatientId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during saving treatment data: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateTreatment(Treatment treatment)
        {
            string query = @"
        UPDATE treatment 
        SET 
            date = @Date,
            doctor_id = @DoctorId,
            patient_id = @PatientId,
            complete = @Complete
        WHERE id = @Id";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", treatment.Id);
                    cmd.Parameters.AddWithValue("@Date", treatment.Date);
                    cmd.Parameters.AddWithValue("@DoctorId", treatment.DoctorId);
                    cmd.Parameters.AddWithValue("@PatientId", treatment.PatientId);
                    cmd.Parameters.AddWithValue("@Complete", treatment.Complete ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating treatment: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteTreatment(int treatmentId)
        {
            string query = "DELETE FROM treatment WHERE id = @Id";

            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Id", treatmentId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting treatment: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
    }

}