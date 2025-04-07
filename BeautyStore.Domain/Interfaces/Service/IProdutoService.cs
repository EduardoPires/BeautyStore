using BeautyStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Interfaces.Service
{
    public interface IProdutoService : IDisposable
    {
        Task<Produto> CriarProduto(Produto produtoDomain);
        Task<Produto> AtualizarProduto(Produto produtoDomain);
        Task<Produto> BuscarProduto(Guid id);
        Task<List<Produto>> ListarTodosProdutos(Guid? usuarioId = null);
        Task<List<Produto>> ListarTodosProdutosPorCategoria(Guid categoriaId);
        Task<Produto> BuscarProdutoPorDescricao(string descricao);
        Task ExcluirProduto(Guid id);
        bool ExisteProduto(Guid id);
    }
}
