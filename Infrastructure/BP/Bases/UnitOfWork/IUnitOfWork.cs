using Infrastructure.BP.Bases.Entity;
using Infrastructure.BP.Bases.GenericRepository;

namespace Infrastructure.BP.Bases.UnitOfWork;

/// <summary>
/// The interface of the unit of work pattern.
/// </summary>
public interface IUnitOfWork : IDisposable
{

    /// <summary>
    /// Method that commits the results to db.
    /// </summary>
    /// <returns>the number of lines affected.</returns>
    public int Save();

    /// <summary>
    /// Get the repository of an entity or create one if it does not exist.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/> concerned.</typeparam>
    /// <returns>The repository of the <see cref="TEntity"/></returns>
    public IGenericRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity;
}