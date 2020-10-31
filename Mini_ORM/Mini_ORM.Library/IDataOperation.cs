using System;
using System.Collections.Generic;

namespace Mini_ORM.Library
{
    public interface IDataOperation<TEntity, TKey>: IDisposable
        where TEntity: class, IEntity<TKey>
    {
        void Insert(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        TEntity GetById(TKey id);
        IList<TEntity> GetAll();
    }
}
