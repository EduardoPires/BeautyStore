using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Interfaces.Service
{
    public interface ICategoriaService : IDisposable
    {
        Task<Categoria> CriarCategoria(Categoria categoriaDomain);
        Task<Categoria> AtualizarCategoria(Categoria categoriaDomain);
        Task<Categoria> BuscarCategoria(Guid id);
        Task<Categoria> BuscarCategoriaPorDescricao(string descricao);
        Task<List<Categoria>> ListarTodasCategorias();
        Task ExcluirCategoria(Guid id);
        bool ExisteCategoria(Guid id);
    }
}
