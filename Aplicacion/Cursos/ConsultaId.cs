using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using AutoMapper;
using Dominio;
using MediatR;
using Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id{ get;set; }
        }

        public class Handler : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Handler(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos
                .Include(x => x.Comentarios)
                .Include(x => x.PrecioPromocion)
                .Include(x=>x.InstructoresLink)
                .ThenInclude(y=>y.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);
                
                if(curso == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new { mensaje = "No se encontró ningún curso."} );
                }
                var cursoDto = _mapper.Map<Curso, CursoDTO>(curso);
                return cursoDto;
            }
        }
    }
}