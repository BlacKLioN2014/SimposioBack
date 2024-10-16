using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using SimposioBack.Repository.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using SimposioBack.Data;

namespace SimposioBack.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SimposioController : ControllerBase
    {


        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly IClienteRepository _ClienteRepository;
        private readonly IInvitadosExtraRepository _invitadosExtraRepository;

        private readonly IMapper _Mapper;


        public SimposioController(IUsuarioRepository UsuarioRepository, IMapper Mapper, IClienteRepository clienteRepository, IInvitadosExtraRepository invitadosExtraRepository)
        {
            _UsuarioRepository = UsuarioRepository;
            _Mapper = Mapper;
            _ClienteRepository = clienteRepository;
            _invitadosExtraRepository = invitadosExtraRepository;
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


        [HttpPost("Registro_Usuarios")]
        [SwaggerOperation(
        Summary = "Método registro usuarios",
        Description = "El Método registro usuarios permite el registro de un usuario en base de datos.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistroUser([FromBody] UsuarioRegistroDto usuarioRegistroDto)
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


        [HttpPost("Registro_Clientes")]
        [SwaggerOperation(
        Summary = "Método registro clientes",
        Description = "El Método registro clientes permite el registro de un cliente en base de datos.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistroCliente([FromBody] ClienteRegistroDto ClienteRegistroDto)
        {
            Api_Response<UsuarioLoginRespuestaDto> Api_Response = new Api_Response<UsuarioLoginRespuestaDto>();

            bool Nombre_Exist = _ClienteRepository.NombreExist(ClienteRegistroDto.Nombre);

            if (Nombre_Exist)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("El nombre de cliente ya esta registrado");

                return BadRequest(Api_Response);
            }

            bool respuestaLogin = await _ClienteRepository.Registro(ClienteRegistroDto);

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


        [HttpGet("ObtenerClientes")]
        [SwaggerOperation(
        Summary = "Método obtener clientes",
        Description = "El método obtener clientes permite obtener un listado de los clientes en base")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Clients()
        {
            Api_Response<List <Cliente>> Api_Response = new Api_Response<List<Cliente>>();

            List<Cliente> Clientes = (List<Cliente>)_ClienteRepository.GetClientes();

            if (Clientes.Count < 1)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("No existen registros de clientes");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = Clientes;

            return Ok(Api_Response);

        }


        [HttpGet("obtenerCliente")]
        [SwaggerOperation(
        Summary = "Método obtener cliente",
        Description = "El método obtener cliente permite obtener los datos del cliente y una listado de los invitados del cliente.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Client(string Nombre_Cliente)
        {
            Api_Response<Client_Response> Api_Response = new Api_Response<Client_Response>();

            Client_Response Cliente = Methods.GetClient(Nombre_Cliente);

            if (Cliente.Invitados.Count < 1)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add($"No existen invitados con el nombre de cliente {Nombre_Cliente}");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = Cliente;

            return Ok(Api_Response);

        }


        [HttpPost("Actualizar_Invitado")]
        [SwaggerOperation(
        Summary = "Método actualizar invitado",
        Description = "El método actualizar invitado permite cambiar el estatus de asistencia de un invitado")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInvitado(int Id_Invitado)
        {
            Api_Response<string> Api_Response = new Api_Response<string>();

            bool actualizacion = Methods.Actualizar_Invitado(Id_Invitado);

            if (!actualizacion)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add($"No fue posible actualizar el registro, Id_Invitado {Id_Invitado}");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = $"Invitado con registro Id_Invitado {Id_Invitado}, actualizado correctamente";

            return Ok(Api_Response);

        }


        [HttpPost("Registro_InvitadosExtra")]
        [SwaggerOperation(
        Summary = "Método registro invitados Extra",
        Description = "El método registro invitados Extra permite el registro de un invitado extra en base de datos.")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistroInvitadoExtra([FromBody] InvitadoExtraRegistroDto invitadoExtraRegistroDto)
        {
            Api_Response<UsuarioLoginRespuestaDto> Api_Response = new Api_Response<UsuarioLoginRespuestaDto>();

            bool Nombre_Exist = _invitadosExtraRepository.NombreExist(invitadoExtraRegistroDto.Nombre);

            if (Nombre_Exist)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("El invitado ya esta registrado");

                return BadRequest(Api_Response);
            }

            bool respuestaLogin = await _invitadosExtraRepository.Registro(invitadoExtraRegistroDto);

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


        [HttpGet("obtenerInvitadosExtra")]
        [SwaggerOperation(
        Summary = "Método obtener invitados extra",
        Description = "El método obtener invitados extra permite obtener un listado de los invitados extra en base")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InvitadosExtra()
        {
            Api_Response<List<InvitadosExtra>> Api_Response = new Api_Response<List<InvitadosExtra>>();

            List<InvitadosExtra> invitadosExtras = (List<InvitadosExtra>)_invitadosExtraRepository.GetInvitadosExtra();

            if (invitadosExtras.Count < 1)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("No existen registros de invitados Extra");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = invitadosExtras;

            return Ok(Api_Response);

        }


        [HttpGet("contadoresDeInvitados")]
        [SwaggerOperation(
        Summary = "Método obtener contadores de Invitados",
        Description = "El método obtener contadores de Invitados permite obtener un json que contiene el total de invitados, el total de asistencia y el total de pendientes de asistencia. ")]
        [ResponseCache(Duration = 10)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> contadoresDeInvitados()
        {
            Api_Response<Contadores_> Api_Response = new Api_Response<Contadores_>();

            Contadores_ contadores = Methods.GetContadores_();

            if (contadores.total_Invitados == 666)
            {
                Api_Response.Exito = false;
                Api_Response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                Api_Response.Respuesta = null;
                Api_Response.ErrorMessages.Add("Ocurrio un error, revisar log");

                return BadRequest(Api_Response);
            }

            Api_Response.Exito = true;
            Api_Response.StatusCode = System.Net.HttpStatusCode.Created;
            Api_Response.Respuesta = contadores;

            return Ok(Api_Response);

        }



    }
}
