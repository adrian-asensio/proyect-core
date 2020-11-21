using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get;set; }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Eliminar instructores relacionados al curso
                var instructoresDB = _context.CursoInstructor.Where(x=> x.CursoId == request.Id);
                foreach(var instructor in instructoresDB)
                {
                    _context.CursoInstructor.Remove(instructor);
                }

                //Eliminar comentario relacionados al curso
                var comentariosDB = _context.Comentarios.Where(x => x.CursoId == request.Id);
                foreach(var comentario in comentariosDB)
                {
                    _context.Comentarios.Remove(comentario);
                }

                //Eliminar precio relacionado al curso
                var precioDB = _context.Precios.Where(x => x.CursoId == request.Id).FirstOrDefault();
                _context.Precios.Remove(precioDB);
                
                //Eliminar curso
                var curso = await _context.Cursos.FindAsync(request.Id);
                if(curso == null)
                {
                    //throw new System.Exception("No se puede eliminar el curso.");
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new { mensaje = "No se encontró el curso" });
                }

                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();

                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new System.Exception("No se pudieron guardar los cambios");
            }
        }
    }
}