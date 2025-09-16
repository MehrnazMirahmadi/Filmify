namespace Filmify.Domain.Contracts.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFilmRepository Films { get; }
    IBoxRepository Boxes { get; }
    ITagRepository Tags { get; }
    ICategoryRepository Categories { get; }
    Task<int> CommitAsync();
}
