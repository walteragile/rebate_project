namespace SW.Core.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Get(string id);
    TEntity Save(TEntity entity);
}
