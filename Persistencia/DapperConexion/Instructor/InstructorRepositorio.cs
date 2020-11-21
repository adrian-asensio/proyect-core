using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;

        public InstructorRepositorio(FactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public Task<int> Actualiza(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<int> Elimina(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Nuevo(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;;
            var storedProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storedProcedure, null, commandType : System.Data.CommandType.StoredProcedure);
            }
            catch(Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList.ToList();
        }

        public Task<InstructorModel> ObtenerPorId()
        {
            throw new NotImplementedException();
        }
    }
}