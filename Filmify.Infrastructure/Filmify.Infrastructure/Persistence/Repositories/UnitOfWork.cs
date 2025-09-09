using Filmify.Domain.Contracts.Interfaces;
using Filmify.Infrastructure.Persistence.Context;
using System;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class UnitOfWork(FilmifyDbContext dbContext) : IUnitOfWork
{
   

    private IFilmRepository? _films;
    private IBoxRepository? _boxes;
    private ITagRepository? _tags;

  
    // Lazy Initialization برای Repositoryها
    public IFilmRepository Films => _films ??= new FilmRepository(dbContext);
    public IBoxRepository Boxes => _boxes ??= new BoxRepository(dbContext);
    public ITagRepository Tags => _tags ??= new TagRepository(dbContext);

    // Commit تغییرات
    public async Task<int> CommitAsync() => await dbContext.SaveChangesAsync();

    // IDisposable
    public void Dispose() => dbContext.Dispose();
}
