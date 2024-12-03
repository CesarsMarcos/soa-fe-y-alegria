using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace FeyAlegria
{
    /// <summary>
    /// Descripción breve de Cursos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]


    public class Cursos : System.Web.Services.WebService
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
                    @"SELECT [id_curso],[nombre],[creditos],[estado] FROM [tb_cursos] WHERE estado=1", con))
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
        public object obtenerPorId(int id)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    string query = @"SELECT id_curso, nombre, creditos, estado FROM tb_cursos WHERE id_curso = @id";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@id", id);

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return "No se encontró un usuario con el ID proporcionado.";
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        ds.WriteXml(sw, XmlWriteMode.WriteSchema);
                        return sw.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obteber el  curso : " + ex.Message);
            }
        }

        [WebMethod]
        public String registrar(
            String nombre,
            String descripcion,
            int creditos)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                     @"INSERT INTO [dbo].[tb_cursos]
                           ([nombre]
                           ,[descripcion]
                           ,[creditos]
                           ,[fecha_registro]
                           ,[estado])
                     VALUES
                           (@nombre
                           ,@descripcion
                           ,@creditos
                           ,@fecha_registro
                           ,@estado)", con))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@creditos", creditos);
                    cmd.Parameters.AddWithValue("@fecha_registro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", true);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "Curso registrada correctamente." : "No se pudo registrar el curso.";
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


        [WebMethod]
        public String actualizar(
        int idCurso,
        String nombre,
        String descripcion,
        int creditos
        )
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand checkCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM tb_cursos WHERE id_curso = @idCurso", con))
                    {
                        checkCmd.Parameters.AddWithValue("@idCurso", idCurso);
                        con.Open();

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            return "El curso no existe en la base de datos.";
                        }
                        con.Close();
                    }


                    using (SqlCommand cmd = new SqlCommand(
                        @"UPDATE tb_cursos
               SET nombre = @nombre,
                   descripcion = @descripcion,
                   creditos = @creditos,
               WHERE id_curso = @idCurso", con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", idCurso);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@creditos", creditos);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0 ? "Curso actualizado  correctamente." : "No se pudo actualizar  el curso.";
                    }
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
        
        
        
       
        [WebMethod]
        public object eliminar(int idCurso)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))

                {
                    using (SqlCommand checkCmd = new SqlCommand(
                  @"SELECT COUNT(*) FROM tb_cursos WHERE id_curso = @idCurso", con))
                    {
                        checkCmd.Parameters.AddWithValue("@idCurso", idCurso);
                        con.Open();

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            return "El curso no existe en la base de datos.";
                        }
                        con.Close();
                    }


                    using (SqlCommand cmd = new SqlCommand(
                     @"UPDATE tb_cursos SET estado=0 WHERE id_curso = @idCurso", con))
                    {

                        cmd.Parameters.AddWithValue("@idCurso", idCurso);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Curso eliminado correctamente." : "No se pudo eliminar el curso.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obteber el  curso : " + ex.Message);
            }
        }
    }
}