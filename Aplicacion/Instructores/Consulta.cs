using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class Lista : IRequest<List<InstructorModel>>{}

        //Lo que devuelve y su tipo
        public class Handler : IRequestHandler<Lista, IEnumerable<InstructorModel>>
        {

            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }

            public async Task<IEnumerable<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                
                return await _instructorRepository.ObtenerLista();
            }
        }
    }
}