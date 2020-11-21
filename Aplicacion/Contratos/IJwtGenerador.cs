using Dominio;

//Contratos porque las interfaces se conocen como contratos
namespace Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
         string CrearToken(Usuario usuario);
    }
}