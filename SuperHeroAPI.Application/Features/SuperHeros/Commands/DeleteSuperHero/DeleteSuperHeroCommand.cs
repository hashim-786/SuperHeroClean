using MediatR;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.DeleteSuperHero
{
    public class DeleteSuperHeroCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
