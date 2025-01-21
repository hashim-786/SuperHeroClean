using MediatR;
using SuperHeroAPI.Application.DTOs;

namespace SuperHeroAPI.Application.Features.SuperHeros.Queries.GetSuperHeroById
{
    public class GetSuperHeroByIdQuery : IRequest<SuperHeroDTO>
    {
        public GetSuperHeroByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
