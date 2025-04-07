using BeautyStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Interfaces.Repository
{
    public interface IVendedorRepository : IDisposable
    {
        Task<Vendedor> CriarVendedor(Vendedor vendedorDomain);
        Task<Vendedor> BuscarVendedor(Guid id);
        Task<Vendedor> BuscarVendedorPorNome(string nome);
    }
}
