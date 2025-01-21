using MediatR;
using SuperHeroAPI.Application.DTOs;

namespace SuperHeroAPI.Application.Features.SuperHeros.Queries.GetAllSuperHeroes
{
    public class GetAllSuperHeroesQuery : IRequest<List<SuperHeroDTO>>
    {
    }
}
