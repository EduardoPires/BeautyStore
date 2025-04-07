using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Repository;
using BeautyStore.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace BeautyStore.Infra.Data.Repositories
{
    public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(BeautyStoreDbContext context) : base(context) { }

        public async Task<Vendedor> CriarVendedor(Vendedor vendedorDomain)
        {
            try
            {
                var vendedorCriado = Db.Vendedores.Add(vendedorDomain);
                await Db.SaveChangesAsync();
                return await BuscarVendedor(vendedorCriado.Entity.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Vendedor> BuscarVendedor(Guid id)
        {
            try
            {
                var vendedor = await Db.Vendedores
                    .FirstOrDefaultAsync(x=> x.Id == id);
                return vendedor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Vendedor> BuscarVendedorPorNome(string nome)
        {
            try
            {
                var vendedor = await Db.Vendedores
                    .FirstOrDefaultAsync(v=> v.Nome.ToUpper() ==(nome.ToUpper()));
                return vendedor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
