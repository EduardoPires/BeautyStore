using BeautyStore.Infra.Data.Contexts;
using BeautyStore.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeautyStore.Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected BeautyStoreDbContext Db;
        protected DbSet<TEntity> DbSet;

        public Repository(BeautyStoreDbContext context)
        {
            Db = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Db.Set<TEntity>();
        }

        public virtual TEntity Criar(TEntity obj)
        {
            var objReturn = DbSet.Add(obj);
            return objReturn.Entity;
        }

        public TEntity BuscarPorId(Guid id)
        {
            return DbSet.Find(id);
        }
        public virtual IEnumerable<TEntity> ListarTodos()
        {
            return DbSet.ToList();
        }

        public virtual TEntity Atualizar(TEntity obj)
        {
            var entry = Db.Entry(obj);
            DbSet.Attach(obj);
            entry.State = EntityState.Modified;
            return obj;
        }

        public void Deletar(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
                Db = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
