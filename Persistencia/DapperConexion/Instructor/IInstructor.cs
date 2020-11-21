using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    //Operaciones sobre entidad instructor
    public interface IInstructor
    {
         Task<List<InstructorModel>> ObtenerLista();
         Task<InstructorModel> ObtenerPorId();
         Task<int> Nuevo(InstructorModel parametros);
         Task<int> Actualiza(InstructorModel parametros);
         Task<int> Elimina(Guid id);
    }
}