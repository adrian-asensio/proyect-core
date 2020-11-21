using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using FluentValidation;
using Aplicacion.ErrorHandler;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta: IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            public List<Guid> ListaInstructores {get;set;}

            public decimal? Precio {get;set;}
            public decimal? Promocion {get;set;}
        }
        
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor( x => x.Titulo).NotEmpty();
                RuleFor( x => x.Descripcion).NotEmpty();
                RuleFor( x => x.FechaPublicacion).NotEmpty();

            }
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
                var curso = await _context.Cursos.FindAsync(request.CursoId);
                if(curso == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new { mensaje = "No se encontró el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Actualizar precio curso
                var precioEntidad = _context.Precios.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad != null)
                {
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await _context.Precios.AddAsync(precioEntidad);
                }

                if(request.ListaInstructores != null)
                {
                    if(request.ListaInstructores.Count > 0)
                    {
                        //Eliminar los instructores actuales del curso.
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructor in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructor);
                        }
                        //Agregar instructores que vienen del cliente
                        foreach(var id in request.ListaInstructores)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId =  id
                            };
                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }

                var resultado = await _context.SaveChangesAsync();
                
                if(resultado > 0) {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso.");
            }
        }
    }
}