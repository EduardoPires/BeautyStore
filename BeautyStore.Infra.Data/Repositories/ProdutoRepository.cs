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
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(BeautyStoreDbContext context) : base(context) { }

        public async Task<Produto> CriarProduto(Produto produtoDomain)
        {
            try
            {
                produtoDomain.Categoria = null;
                produtoDomain.Vendedor = null;
                var produtoCriado = Db.Produtos.Add(produtoDomain);                
                await Db.SaveChangesAsync();
                return await BuscarProduto(produtoCriado.Entity.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Produto> AtualizarProduto(Produto produtoDomain)
        {
            try
            {
                var entry = Db.Entry(produtoDomain);
                DbSet.Attach(produtoDomain);
                entry.State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return await BuscarProduto(produtoDomain.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Produto> BuscarProduto(Guid id)
        {
            try
            {
                var produto = await Db.Produtos
                    .Include(c=>c.Categoria)
                    .Include(v=>v.Vendedor)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return produto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<Produto>> ListarTodosProdutos(Guid? usuarioId = null)
        {
            try
            {
                var Lista = await Db.Produtos
                    .Include(c => c.Categoria)
                    .Include(v => v.Vendedor)
                    .Where(x => !usuarioId.HasValue || x.VendedorId == usuarioId.Value)
                    .ToListAsync();
                return Lista;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<Produto>> ListarTodosProdutosPorCategoria(Guid categoriaId)
        {
            try
            {
                var Lista = await Db.Produtos
                    .Include(c => c.Categoria)
                    .Include(v => v.Vendedor)
                    .Where(x => x.CategoriaId == categoriaId)
                    .ToListAsync();
                return Lista;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<Produto> BuscarProdutoPorDescricao(string descricao)
        {
            try
            {
                var produto = await Db.Produtos
                    .FirstOrDefaultAsync(c => c.Descricao == descricao);
                return produto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task ExcluirProduto(Guid id)
        {
            try
            {
                Db.Produtos.Remove(await Db.Produtos.FindAsync(id));
                await Db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ExisteProduto(Guid id)
        {
            try
            {
                return Db.Produtos.Any(x => x.CategoriaId == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
