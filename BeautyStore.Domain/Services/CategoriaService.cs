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
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
      
        public async Task<Categoria> CriarCategoria(Categoria categoriaDomain) 
        {
            return await _categoriaRepository.CriarCategoria(categoriaDomain);
        }

        public async Task<Categoria> AtualizarCategoria(Categoria categoriaDomain)
        {
            return await _categoriaRepository.AtualizarCategoria(categoriaDomain);
        }
        public async Task<Categoria> BuscarCategoria(Guid id) 
        {
            return  await _categoriaRepository.BuscarCategoria(id);            
        }

        public async Task<Categoria> BuscarCategoriaPorDescricao(string descricao)
        {
            return await _categoriaRepository.BuscarCategoriaPorDescricao(descricao);
        }
        public async Task<List<Categoria>> ListarTodasCategorias() 
        {
            return await _categoriaRepository.ListarTodasCategorias();
        }
        public async Task ExcluirCategoria(Guid id)
        {
            await _categoriaRepository.ExcluirCategoria(id);
        }

        public bool ExisteCategoria(Guid id)
        {
            return _categoriaRepository.ExisteCategoria(id);
        }

        public void Dispose()
        {
            _categoriaRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}