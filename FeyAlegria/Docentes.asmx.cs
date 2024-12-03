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
    /// Descripción breve de Docentes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Docentes : System.Web.Services.WebService
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
                    @"SELECT a.id_docente
                      ,UPPER(CONCAT(c.nombre, CONCAT (' ', c.apellido)))
                      ,b.descripcion
                      ,a.fecha_registro
                      ,a.estado
                  FROM [dbo].[tb_docentes]  a
                  INNER JOIN tb_especialidades b
                  ON a.id_especialidad = b.id_especialidad 
				  INNER JOIN tb_personas c
				  ON a.id_persona = c.id_persona
				  WHERE a.estado=1", con))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar cursos: " + ex.Message);
            }
        }

        [WebMethod]
        public String registrar(
           int idPersona,
           int idEspecialidad)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO[dbo].[tb_docentes]
                       ([id_persona]
                       ,[id_especialidad])
                     VALUES
                           (@idPersona
                           ,@idEspecialidad)", con))
                {
                    cmd.Parameters.AddWithValue("@idPersona", idPersona);
                    cmd.Parameters.AddWithValue("@idEspecialidad", idEspecialidad);
                    cmd.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", true);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "Docente registrada correctamente." : "No se pudo registrar al docente.";
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
