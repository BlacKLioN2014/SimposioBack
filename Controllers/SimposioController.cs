using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using SimposioBack.Repository.IRepository;
using Swashbuckle.AspNetCore.Annotations;

namespace SimposioBack.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SimposioController : ControllerBase
    {


        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly IMapper _Mapper;


        public SimposioController(IUsuarioRepository UsuarioRepository, IMapper Mapper)
        {
            _UsuarioRepository = UsuarioRepository;
            _Mapper = Mapper;
        }


        [HttpPost("Login")]
        [SwaggerOperation(
        Summary = "Método login",
        Description = "El método Login verifica las credenciales del usuario. Si son correctas, permite el acceso; de lo contrario, devuelve un error.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            Api_Response<UsuarioLoginRespuestaDto> Api_Response = new Api_Response<UsuarioLoginRespuestaDto>();

            var respuestaLogin = _UsuarioRepository.Login(usuarioLoginDto);

            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {

                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("El nombre de usuario o password, son incorrectos");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.OK;
            Api_Response.Respuesta = respuestaLogin;

            return Ok(Api_Response);

        }


        [HttpPost("Registro")]
        [SwaggerOperation(
        Summary = "Método Registro",
        Description = "El método Registro Permite el registro de un usuario en base de datos.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MyRegistro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            Api_Response<UsuarioLoginRespuestaDto> Api_Response = new Api_Response<UsuarioLoginRespuestaDto>();

            bool Email_Exist =  _UsuarioRepository.EmailExist(usuarioRegistroDto.Correo);

            if (Email_Exist) 
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("El correo ya esta registrado");

                return BadRequest(Api_Response);
            }

            bool respuestaLogin = await _UsuarioRepository.Registro(usuarioRegistroDto);

            if (!respuestaLogin)
            {

                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("Ocurrio un error");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = null;

            return Ok(Api_Response);

        }



    }
}
