using System.Data;
using Microsoft.Data.SqlClient;
using MvcCoreLinqToSQL.Models;
using MvcCoreLinqToSQL2.Models;

namespace MvcCoreLinqToSQL.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;
        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from EMP";
            SqlDataAdapter adEmp = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            adEmp.Fill(this.tablaEmpleados);
        }

        public List<Empleado> GetEmpleados()
        {
            //LAS CONSULTAS LINQ SE ALMACENAN EN GENERICOS (var) 
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            //AHORA MISMO TENEMOS DENTRO DE CONSULTA LA INFORMACION DE  
            //LA TABLA EMPLEADOS 
            //EN ESTE EJEMPLO TENEMOS OBJETO DataRow QUE SON FILAS 
            //DENTRO DE LA TABLA 
            //DEBEMOS RECORRER DICHAS FILAS Y EXTRAER LA INFORMACION  
            //EN OBJETOS DE TIPO Empleado 
            List<Empleado> empleados = new List<Empleado>();
            //RECORREMOS CADA FILA DE LA CONSULTA 
            foreach (var row in consulta)
            {
                Empleado emp = new Empleado();
                //PARA EXTRAER DATOS DE UN DataRow 
                //DataRow.Field<tipo>("COLUMNA") 
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }
            return empleados;
        }
        public Empleado FindEmpleado(int idEmpleado)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idEmpleado
                           select datos;

            var row = consulta.FirstOrDefault();

            if (row == null)
            {
                return null;
            }

            Empleado emp = new Empleado
            {
                IdEmpleado = row.Field<int>("EMP_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Oficio = row.Field<string>("OFICIO"),
                Salario = row.Field<int>("SALARIO"),
                IdDepartamento = row.Field<int>("DEPT_NO")
            };
            return emp;
        }

        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
&& datos.Field<int>("SALARIO") >= salario
                           select datos;
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado emp = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(emp);
            }
            return empleados;
        }

        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            //QUISIERA ORDENAR LOS EMPLEADOS POR SU SALARIO
            consulta = consulta.OrderBy(z => z.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado empleado = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(empleado);
            }
            ResumenEmpleados resumen = new ResumenEmpleados();
            resumen.Personas = personas;
            resumen.MaximoSalario = maximo;
            resumen.MediaSalarial = media;
            resumen.Empleados = empleados;
            return resumen;
        }

        public List<string> GetOficios()
        { 
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                           select datos.Field<string>("OFICIO")).Distinct();
            return consulta.ToList();

        }
    }
}