using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Repository;
using BeautyStore.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Infra.Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(BeautyStoreDbContext context) : base(context) { }

        public async Task<Categoria> CriarCategoria(Categoria categoriaDomain)
        {
            try
            {
                var categoriaCriada = Db.Categorias.Add(categoriaDomain);
                await Db.SaveChangesAsync();
                return await BuscarCategoria(categoriaCriada.Entity.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Categoria> AtualizarCategoria(Categoria categoriaDomain)
        {
            try
            {
                var entry = Db.Entry(categoriaDomain);
                DbSet.Attach(categoriaDomain);
                entry.State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return await BuscarCategoria(categoriaDomain.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Categoria> BuscarCategoria(Guid id)
        {
            try
            {
                var categoria = await Db.Categorias.FindAsync(id);
                return categoria;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Categoria> BuscarCategoriaPorDescricao(string descricao)
        {
            try
            {
                var categoria = await Db.Categorias
                    .FirstOrDefaultAsync(c => c.Descricao == descricao);
                return categoria;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<Categoria>> ListarTodasCategorias()
        {
            try
            {
                var lista = await Db.Categorias.ToListAsync();    
                return lista;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task ExcluirCategoria(Guid id)
        {
            try
            {
                Db.Categorias.Remove(await Db.Categorias.FindAsync(id));
                await Db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ExisteCategoria(Guid id)
        {
            try
            {
                return Db.Categorias.Any(x=> x.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
