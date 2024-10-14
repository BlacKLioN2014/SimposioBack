using Newtonsoft.Json;
using System.Net;

namespace SimposioBack.Models
{
    public class Api_Response<T>
    {

        public Api_Response()
        {
            ErrorMessages = new List<string>();
        }

        [JsonProperty("Exito")]
        public bool Exito { get; set; }

        [JsonProperty("StatusCode")]
        public HttpStatusCode StatusCode { get; set; }


        [JsonProperty("Respuesta")]
        public T Respuesta { get; set; }

        [JsonProperty("ErrorMessages")]
        public List<string> ErrorMessages { get; set; }
    }
}
