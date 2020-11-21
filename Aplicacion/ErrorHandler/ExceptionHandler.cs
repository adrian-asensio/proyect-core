using System;
using System.Net;

namespace Aplicacion.ErrorHandler
{
    public class ExceptionHandler : Exception
    {
        public HttpStatusCode Codigo { get; }
        public object Errores { get; }
        public ExceptionHandler(HttpStatusCode codigo, object errores = null)
        {
            Codigo = codigo;
            Errores = errores;
        }
    }
}