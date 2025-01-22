using MediatR;
using SuperHeroAPI.Application.DTOs;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.AddSuperHero
{
    public class AddSuperHeroCommand : IRequest<SuperHeroDTO>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Place { get; set; }
    }
}
