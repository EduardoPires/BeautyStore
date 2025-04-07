using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Criar(TEntity obj);
        TEntity Atualizar(TEntity obj);
        IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);
        TEntity BuscarPorId(Guid id);
        IEnumerable<TEntity> ListarTodos();
        void Deletar(Guid id);
    }
}
