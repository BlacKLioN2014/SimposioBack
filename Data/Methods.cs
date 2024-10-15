using Microsoft.Data.SqlClient;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace SimposioBack.Data
{
    public class Methods
    {


        #region Variables Globales

        //Variables de en Encryptado en metodo getToken
        public static string AES256_USER_Key = "hUf3eof71VCa7IIjFlNewZ73yBWjckdX";

        private static string _path = Directory.GetCurrentDirectory() + @"Data\";

        #endregion


        #region Metodos Encriptado

        //Generar token
        public static string getToken(string correo, string password)
        {
            string token = correo + "#" + "Simposio" + "#" + password + "#" + "2024";
            token = AES256_Encriptar(AES256_USER_Key, token);
            return token;
        }

        // Método para encriptar texto
        public static string AES256_Encriptar(string key, string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        #endregion


        #region Metodos Desencriptado

        public static Gut_Token validateTokenInRequest(string token_On_Request)
        {
            Gut_Token tokenRequest = new Gut_Token();
            try
            {

                token_On_Request = CorregirToken(token_On_Request);
                string tokenDescodificado = AES256_Desencriptar(AES256_USER_Key, token_On_Request);
                tokenRequest.Correo = tokenDescodificado.Split('#')[0];
                tokenRequest.Proyecto = tokenDescodificado.Split('#')[1];
                tokenRequest.Contraseña = tokenDescodificado.Split('#')[2];
                tokenRequest.Año = tokenDescodificado.Split('#')[3];

                return tokenRequest;

            }
            catch (Exception)
            {
                return tokenRequest;
            }
        }

        public static string CorregirToken(string token)
        {
            return token.Replace("%2F", "/");
        }

        public static string AES256_Desencriptar(string key, string textoCifrado)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(textoCifrado);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        #endregion


        #region metodo UpdateInvitado

        //Metodo para guardar Log
        public static void Log(string path, string Message)
        {
            Task.Run(() =>
            {
                using (var sw = new StreamWriter(path, true))
                {
                    //Log Afectacion
                    DateTime date = DateTime.Now;
                    sw.WriteAsync("\n");
                    sw.WriteLineAsync(date + " " + Message);
                }
            });

        }

        public static bool Actualizar_Invitado(int Id_Invitado)
        {
            bool Actualizacion = false;
            string connectionString = "Data Source=172.16.101.41\\DBINVENTORY;Initial Catalog=Simposio;User ID=sa;Password=SQLserver24; Trusted_Connection=False;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();

                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        string Query = "UPDATE SIMPOSIO.dbo.Invitados SET Asistencia = 1 WHERE Id_Invitado = @IdInvitado;";

                        using (SqlCommand command = new SqlCommand(Query, conexion))
                        {
                            // Usamos un parámetro para evitar inyecciones SQL
                            command.Parameters.AddWithValue("@IdInvitado", Id_Invitado);

                            // Ejecutar la actualización
                            int filasAfectadas = command.ExecuteNonQuery();

                            // Verificar si se actualizó algún registro
                            Actualizacion = filasAfectadas > 0;

                            if (Actualizacion)
                            {
                                return true;
                            }

                        }
                    }
                    else
                    {
                        string error = "No fue posible establecer conexion con la base de datos ";

                        DateTime date = DateTime.Now;
                        string fechaFormateada = date.ToString("yyyyMMdd");

                        //Generar log
                        Log(_path + @"Log\" + fechaFormateada + ".txt", error);
                    }
                }
                catch (Exception ex)
                {
                    string error = "Error al obtener paymentMethods " + ex.Message.ToString();

                    DateTime date = DateTime.Now;
                    string fechaFormateada = date.ToString("yyyyMMdd");

                    //Generar log
                    Log(_path + @"Log\" + fechaFormateada + ".txt", error);
                }
            }
            return Actualizacion;
        }

        #endregion


        #region Metodo Client

        public static Client_Response GetClient(string Nombre_Cliente)
        {
            string connectionString = "Data Source=172.16.101.41\\DBINVENTORY;Initial Catalog=Simposio;User ID=sa;Password=SQLserver24; Trusted_Connection=False;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;";

            Client_Response client_Response = new Client_Response();

            List<InvitadoRegistroDto> invitados = new List<InvitadoRegistroDto>();

            //InvitadoRegistroDto invitado  = new InvitadoRegistroDto();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();

                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        string Query = $"SELECT Id_Cliente, Nombre_Cliente, Numero_Invitados, Estado, Id_Evento\r\nFROM SIMPOSIO.dbo.Cliente  where Nombre_Cliente = '{Nombre_Cliente}';";

                        using (SqlCommand command = new SqlCommand(Query, conexion))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {

                                    client_Response = new Client_Response()
                                    {
                                        Id_Cliente = reader.GetInt32(0),
                                        Nombre_Cliente = reader.GetString(1),
                                        Numero_Invitados = reader.GetInt32(2),
                                        Estado = reader.GetString(3),
                                        Id_Evento = reader.GetInt32(4),
                                    };

                                }
                            }
                        }

                        Query = $"SELECT Id_Invitado, Id_Cliente, Nombre_Invitado, Asistencia\r\nFROM SIMPOSIO.dbo.Invitados  where Id_Cliente = '{client_Response.Id_Cliente}' ;";

                        using (SqlCommand command = new SqlCommand(Query, conexion))
                        {
                            using (SqlDataReader reader2 = command.ExecuteReader())
                            {
                                while (reader2.Read())
                                {

                                    InvitadoRegistroDto invitado = new InvitadoRegistroDto()
                                    {
                                        Id_Invitado = reader2.GetInt32(0),
                                        Id_Cliente = reader2.GetInt32(1),
                                        Nombre_Invitado = reader2.GetString(2),
                                        Asistencia = reader2.GetBoolean(3),
                                    };

                                    invitados.Add(invitado);
                                    //invitado = new InvitadoRegistroDto();
                                }
                                client_Response.Invitados = invitados;
                            }
                        }
                    }
                    else
                    {
                        string error = "No fue posible establecer conexion con la base de datos ";

                        DateTime date = DateTime.Now;
                        string fechaFormateada = date.ToString("yyyyMMdd");

                        //Generar log
                        Log(_path + @"Log\" + fechaFormateada + ".txt", error);

                    }

                }
                catch (Exception ex)
                {
                    string error = $"Error al obtener cliente response de {Nombre_Cliente}" + ex.Message.ToString();

                    DateTime date = DateTime.Now;
                    string fechaFormateada = date.ToString("yyyyMMdd");

                    //Generar log
                    Log(_path + @"Log\" + fechaFormateada + ".txt", error);

                    client_Response = new Client_Response();
                }
            }

            return client_Response;

        }

        #endregion


    }
}
