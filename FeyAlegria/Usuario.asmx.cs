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
    /// Descripción breve de Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Usuario : System.Web.Services.WebService
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
                    @"SELECT a.id_usuario
                      ,UPPER(CONCAT(b.nombre, CONCAT (' ', b.apellido)))
                      ,c.nombre_rol
                      ,a.username
                      ,a.estado
                  FROM [dbo].[tb_usuarios] a
                  INNER JOIN tb_personas b
                  ON a.id_persona = b.id_persona
                  INNER JOIN tb_roles c
                  ON a.id_rol = c.id_rol WHERE estado=1", con))
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
            int idPersona,
            int idRol,
            String username,
            String password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO  [dbo].[tb_usuarios]
                           ([id_persona]
                           ,[id_rol]
                           ,[username]
                           ,[password]
                           ,[estado])
                     VALUES
                           (@idPersona
                           ,@idRol
                           ,@username
                           ,@password
                           ,@estado)", con))
                {
                    cmd.Parameters.AddWithValue("@idPersona", idPersona);
                    cmd.Parameters.AddWithValue("@idRol", idRol);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@estado", true);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "Usuario registrada correctamente." : "No se pudo registrar el usuario.";
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
