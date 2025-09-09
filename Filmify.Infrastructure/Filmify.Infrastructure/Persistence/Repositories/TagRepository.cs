using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using Filmify.Infrastructure.Persistence.Context;

namespace Filmify.Infrastructure.Persistence.Repositories;


public class TagRepository(FilmifyDbContext context)
    : Repository<Tag>(context), ITagRepository
{
}
