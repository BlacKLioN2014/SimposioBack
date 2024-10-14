using SimposioBack.Models;
using System.Security.Cryptography;
using System.Text;

namespace SimposioBack.Data
{
    public class Methods
    {


        #region Variables Globales

        //Variables de en Encryptado en metodo getToken
        public static string AES256_USER_Key = "hUf3eof71VCa7IIjFlNewZ73yBWjckdX";

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


    }
}
