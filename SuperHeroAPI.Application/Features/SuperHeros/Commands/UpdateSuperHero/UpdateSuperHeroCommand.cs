using MediatR;
using SuperHeroAPI.Application.DTOs;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.UpdateSuperHero
{
    public class UpdateSuperHeroCommand : IRequest<SuperHeroDTO>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Place { get; set; }
    }
}
