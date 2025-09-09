using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using Filmify.Infrastructure.Persistence.Context;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class BoxRepository(FilmifyDbContext context)
    : Repository<Box>(context), IBoxRepository
{
}
