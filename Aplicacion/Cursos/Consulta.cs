using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>>
        {

        }

        public class Handler : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Handler(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Cursos
                .Include(x => x.Comentarios)
                .Include(x => x.PrecioPromocion)
                .Include(x=>x.InstructoresLink)
                .ThenInclude(x => x.Instructor).ToListAsync();

                //primer parametro -> origen, segundo -> destino, cursos -> data a mapear
                var cursosDto = _mapper.Map<List<Curso>, List<CursoDTO>>(cursos);

                return cursosDto;
            }
        }
    }
}