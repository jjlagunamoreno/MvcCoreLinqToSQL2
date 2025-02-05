using Microsoft.Data.SqlClient;
using MvcCoreLinqToSQL2.Models;
using System.Data;

namespace MvcCoreLinqToSQL2.Repositories
{
    public class RepositoryEnfermos
    {
        private DataTable tablaEnfermos;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            this.tablaEnfermos = new DataTable();
            string sql = "select * from ENFERMO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            ad.Fill(this.tablaEnfermos);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach (var row in consulta)
            {
                Enfermo enfermo = new Enfermo
                {
                    Inscripcion = row.Field<string>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechaNacimiento = row.Field<DateTime>("FECHA_NAC")
                };
                enfermos.Add(enfermo);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo(string inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<string>("INSCRIPCION") == inscripcion
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                var row = consulta.First();
                Enfermo enfermo = new Enfermo
                {
                    Inscripcion = row.Field<string>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechaNacimiento = row.Field<DateTime>("FECHA_NAC")
                };
                return enfermo;
            }
        }

        public void DeleteEnfermo(string inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }

}
