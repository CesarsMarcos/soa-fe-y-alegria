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
    /// Descripción breve de consultarAlumnos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class consultarAlumnos : System.Web.Services.WebService
    {

        private string connectionString = "Data Source=JHANCO\\SQLExpress; Initial Catalog=db_fe_y_alegria; Persist Security Info=True; Integrated Security=SSPI";

        // Método para listar todas las asignaciones de cursos
        [WebMethod]
        public DataSet listarAsignacionesCursos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            SqlDataAdapter da = new SqlDataAdapter("SELECT ac.id_docente, c.nombre AS curso, p.nombre AS docente, ac.fecha_asignacion, ac.estado FROM tb_asignaciones_curso ac " +
                "LEFT JOIN tb_cursos c ON ac.id_curso = c.id_curso LEFT JOIN tb_docentes d ON ac.id_docente = d.id_docente LEFT JOIN tb_personas p ON d.id_persona = p.id_persona", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        // Método para asignar un curso a un docente
        [WebMethod]
        public string AsignarCurso(int idDocente, int idCurso)
        {
            string resultado = string.Empty;

            // Configurar la conexión a la base de datos
            SqlConnection con = new SqlConnection("Data Source=JHANCO\\SQLExpress; Initial Catalog=db_fe_y_alegria; Persist Security Info=True; Integrated Security=SSPI");

            try
            {
                // Abrir la conexión
                con.Open();

                // Preparar la consulta SQL para insertar la asignación
                string query = "INSERT INTO tb_asignaciones_curso (id_docente, id_curso, fecha_asignacion, fecha_registro, estado) " +
                               "VALUES (@idDocente, @idCurso, GETDATE(), GETDATE(), 1)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idDocente", idDocente);
                cmd.Parameters.AddWithValue("@idCurso", idCurso);

                // Ejecutar la consulta
                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    resultado = "Curso asignado exitosamente al docente.";
                }
                else
                {
                    resultado = "Error al asignar el curso.";
                }
            }
            catch (Exception ex)
            {
                resultado = "Error al asignar el curso: " + ex.Message;
            }
            finally
            {
                // Cerrar la conexión
                con.Close();
            }

            return resultado;
        }

    }
}
