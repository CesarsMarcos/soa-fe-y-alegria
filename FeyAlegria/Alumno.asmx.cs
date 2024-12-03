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
    /// Descripción breve de Alumno
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Alumno : System.Web.Services.WebService
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
                    @"SELECT a.id_alumno
                       ,UPPER(CONCAT(b.nombre, CONCAT (' ', b.apellido))) nombres
                      ,a.matricula
                      ,a.fecha_ingreso
                      ,a.estado
                  FROM [dbo].[tb_alumnos] a
                  INNER JOIN tb_personas b
                  ON a.id_persona = b.id_persona
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
           String matricula,
           String fechaIngreso)
        {
            try
            {

                DateTime? feIngreso = null;
                if (!string.IsNullOrEmpty(fechaIngreso))
                {
                    if (DateTime.TryParse(fechaIngreso, out DateTime parsedDate))
                    {
                        feIngreso = parsedDate;
                    }
                    else
                    {
                        return "Error: Formato de fecha no válido. Use el formato yyyy-MM-dd.";
                    }
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO [dbo].[tb_alumnos]
                           ([id_persona]
                           ,[matricula]
                           ,[fecha_ingreso]
                           ,[fecha_registro]
                           ,[estado])
                     VALUES
                           (@idPersona
                           ,@matricula
                            ,@fechaIngreso)", con))
                {
                    cmd.Parameters.AddWithValue("@idPersona", idPersona);
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.Parameters.AddWithValue("@fechaIngreso", feIngreso);
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
