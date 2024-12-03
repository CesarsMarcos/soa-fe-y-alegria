using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace FeyAlegria
{
    /// <summary>
    /// Descripción breve de Personas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Personas : System.Web.Services.WebService
    {

        private readonly string connectionString =
                "Server=DESKTOP-DISQQ64;Initial Catalog=db_fe_y_alegria;Integrated Security=True;";

        [WebMethod]
        public DataSet listarPersonas()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            SqlDataAdapter da = new SqlDataAdapter("SELECT id_persona, nombre, apellido, fecha_nacimiento, tipo_documento, numero_documento, email, telefono, direccion, fecha_registro, estado FROM tb_personas", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        [WebMethod]
        public string registrarPersona(
        string nombre,
        string apellido,
        string fecha_nacimiento, // Recibido como string para mayor compatibilidad
        string tipo_documento,
        string numero_documento,
        string email,
        string telefono,
        string direccion,
        bool estado)
        {
            try
            {
                // Validar y convertir la fecha de nacimiento (si se proporciona)
                DateTime? fechaNacimiento = null;
                if (!string.IsNullOrEmpty(fecha_nacimiento))
                {
                    if (DateTime.TryParse(fecha_nacimiento, out DateTime parsedDate))
                    {
                        fechaNacimiento = parsedDate;
                    }
                    else
                    {
                        return "Error: Formato de fecha no válido. Use el formato yyyy-MM-dd.";
                    }
                }

                // Crear la conexión a la base de datos
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Server=DESKTOP-DISQQ64;Initial Catalog=db_fe_y_alegria;Integrated Security=True;";

                // Crear el comando SQL para insertar los datos
                string query = @"INSERT INTO [dbo].[tb_personas] 
                        (nombre, apellido, fecha_nacimiento, tipo_documento, numero_documento, email, telefono, direccion, fecha_registro, estado) 
                        VALUES 
                        (@nombre, @apellido, @fecha_nacimiento, @tipo_documento, @numero_documento, @email, @telefono, @direccion, @fecha_registro, @estado)";
                SqlCommand cmd = new SqlCommand(query, con);

                // Agregar los parámetros
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@apellido", apellido);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", (object)fechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tipo_documento", (object)tipo_documento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@numero_documento", (object)numero_documento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefono", (object)telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@direccion", (object)direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_registro", DateTime.Now); // Fecha de registro automática
                cmd.Parameters.AddWithValue("@estado", estado);

                // Abrir la conexión y ejecutar el comando
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                // Verificar si la operación fue exitosa
                if (rowsAffected > 0)
                {
                    return "Persona registrada correctamente.";
                }
                else
                {
                    return "No se pudo registrar la persona.";
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return "Ocurrió un error: " + ex.Message;
            }
        }
    }
}
