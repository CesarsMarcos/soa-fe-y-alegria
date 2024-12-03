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
    /// Descripción breve de Rol
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Rol : System.Web.Services.WebService
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
                    @"SELECT [id_rol]
                      ,[nombre_rol]
                      ,[estado] FROM [tb_roles] WHERE estado=1", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar roles: " + ex.Message);
            }
        }

        [WebMethod]
        public String registrar(
            String nombre)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO [dbo].[tb_roles]
                           ([nombre_rol]
                           ,[estado])
                     VALUES
                           (@nombre,@estado)", con))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@estado", true);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "Rol registrada correctamente." : "No se pudo registrar el rol.";
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
