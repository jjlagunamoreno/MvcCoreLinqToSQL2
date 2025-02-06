using System.Data;
using Microsoft.Data.SqlClient;
using PracticaFinal.Models;

namespace PracticaFinal.Repositories
{
    public class RepositoryDoctores
    {
        private string connectionString;

        public RepositoryDoctores(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Doctor> GetDoctores()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT * FROM DOCTOR";
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Doctor> doctores = new List<Doctor>();
                while (reader.Read())
                {
                    doctores.Add(new Doctor
                    {
                        HospitalCod = Convert.ToInt32(reader["HOSPITAL_COD"]),
                        DoctorNo = Convert.ToInt32(reader["DOCTOR_NO"]),
                        Apellido = reader["APELLIDO"].ToString(),
                        Especialidad = reader["ESPECIALIDAD"].ToString(),
                        Salario = Convert.ToInt32(reader["SALARIO"])
                    });
                }
                return doctores;
            }
        }

        public List<string> GetEspecialidades()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT DISTINCT ESPECIALIDAD FROM DOCTOR";
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<string> especialidades = new List<string>();
                while (reader.Read())
                {
                    especialidades.Add(reader["ESPECIALIDAD"].ToString());
                }
                return especialidades;
            }
        }

        public List<Doctor> GetDoctoresByEspecialidad(string especialidad)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT * FROM DOCTOR WHERE ESPECIALIDAD = @especialidad";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@especialidad", especialidad);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Doctor> doctores = new List<Doctor>();
                while (reader.Read())
                {
                    doctores.Add(new Doctor
                    {
                        HospitalCod = Convert.ToInt32(reader["HOSPITAL_COD"]),
                        DoctorNo = Convert.ToInt32(reader["DOCTOR_NO"]),
                        Apellido = reader["APELLIDO"].ToString(),
                        Especialidad = reader["ESPECIALIDAD"].ToString(),
                        Salario = Convert.ToInt32(reader["SALARIO"])
                    });
                }
                return doctores;
            }
        }
    }
}
