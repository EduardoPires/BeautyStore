using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Repository;
using BeautyStore.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<Vendedor> CriarVendedor(Vendedor vendedorDomain)
        {
            return await _vendedorRepository.CriarVendedor(vendedorDomain);
        }

        public async Task<Vendedor> BuscarVendedor(Guid id)
        {
            return await _vendedorRepository.BuscarVendedor(id);
        }

        public async Task<Vendedor> BuscarVendedorPorNome(string nome)
        {
            return await _vendedorRepository.BuscarVendedorPorNome(nome);
        }


        public void Dispose()
        {
            _vendedorRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
