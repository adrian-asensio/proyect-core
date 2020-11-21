using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public async static Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "Adrián Asensio", UserName = "adrianasensio", Email = "adriasen22@gmail.com" };
                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}