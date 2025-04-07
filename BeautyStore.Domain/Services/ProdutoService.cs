using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Repository;
using BeautyStore.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Produto> CriarProduto(Produto produtoDomain)
        {
            return await _produtoRepository.CriarProduto(produtoDomain);
        }

        public async Task<Produto> AtualizarProduto(Produto produtoDomain)
        {
            return await _produtoRepository.AtualizarProduto(produtoDomain);
        }
        public async Task<Produto> BuscarProduto(Guid id)
        {
            return await _produtoRepository.BuscarProduto(id);
        }
        public async Task<List<Produto>> ListarTodosProdutos(Guid? usuarioId = null)
        {
            return await _produtoRepository.ListarTodosProdutos(usuarioId);
        }

        public async Task<List<Produto>> ListarTodosProdutosPorCategoria(Guid categoriaId)
        {
            return await _produtoRepository.ListarTodosProdutosPorCategoria(categoriaId);
        }

        public async Task<Produto> BuscarProdutoPorDescricao(string descricao)
        {
            return await _produtoRepository.BuscarProdutoPorDescricao(descricao);
        }
        public async Task ExcluirProduto(Guid id)
        {
            await _produtoRepository.ExcluirProduto(id);
        }

        public bool ExisteProduto(Guid id)
        {
            return _produtoRepository.ExisteProduto(id);
        }
        public void Dispose()
        {
            _produtoRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}