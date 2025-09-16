using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using Filmify.Infrastructure.Persistence.Context;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class CategoryRepository(FilmifyDbContext db) : Repository<Category>(db), ICategoryRepository
{
    //public CategoryRepository(FilmifyDbContext db) : base(db)
    //{
    //}
}
