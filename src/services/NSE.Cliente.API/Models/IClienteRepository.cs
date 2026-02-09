using System.Collections.Generic;
using System.Threading.Tasks;
using NSE.Core.Data;

namespace NSE.Clientes.API.Models
{
    public interface IClienteRepository : IRepository<Cliente1>
    {
        void Adicionar(Cliente1 cliente);

        Task<IEnumerable<Cliente1>> ObterTodos();
        Task<Cliente1> ObterPorCpf(string cpf);
    }
}