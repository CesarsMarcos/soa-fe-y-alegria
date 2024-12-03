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
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        private readonly string connectionString =
           "Server=DESKTOP-DISQQ64;Initial Catalog=db_fe_y_alegria;Integrated Security=True;";

        [WebMethod]
        public DataSet listado()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT [id_especialidad]
                          ,[nombre]
                          ,[descripcion]
                          ,[fecha_registro]
                          ,[estado]
                      FROM [dbo].[tb_especialidades] WHERE estado=1", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar especialidades: " + ex.Message);
            }
        }

        [WebMethod]
        public String registrar(String nombre,
            String descripcion)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO [dbo].[tb_especialidades]
                           ([nombre]
                           ,[descripcion]
                           ,[fecha_registro]
                           ,[estado])
                     VALUES
                           (@nombre
                           ,@descripcion
                           ,@fecha_registro
                           ,@estado)", con))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", true);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "Especialidad registrada correctamente." : "No se pudo registrar la especialidad.";
                }
            }
            catch (SqlException sqlEx)
            {
                return "Error SQL: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                return "Error general: " + ex.Message;
            }
        }


    }
}
