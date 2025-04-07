using BeautyStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Interfaces.Service
{
    public interface IVendedorService : IDisposable
    {
        Task<Vendedor> CriarVendedor(Vendedor vendedorDomain);
        Task<Vendedor> BuscarVendedor(Guid id);
        Task<Vendedor> BuscarVendedorPorNome(string nome);
    }
}
