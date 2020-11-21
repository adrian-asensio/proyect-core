using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ErrorHandler;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email {get;set;}
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {

            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _sigInManager;
            private readonly IJwtGenerador _jwtGenerador;
            public Handler(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                _userManager = userManager;
                _sigInManager = signInManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if(usuario == null)
                {
                    throw new ExceptionHandler(System.Net.HttpStatusCode.Unauthorized);
                }

                var resultado = await _sigInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if(resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Image = null
                    };
                }

                throw new ExceptionHandler(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}