namespace Aplicacion.Seguridad
{
    //clase que representa los datos que quiero enviar al cliente
    public class UsuarioData
    {
        public string NombreCompleto { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
}