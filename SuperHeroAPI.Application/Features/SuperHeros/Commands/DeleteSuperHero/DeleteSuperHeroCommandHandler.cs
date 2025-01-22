using MediatR;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.DeleteSuperHero
{
    public class DeleteSuperHeroCommandHandler : IRequestHandler<DeleteSuperHeroCommand, bool>
    {
        private readonly ISuperHeroRepository _superHeroRepository;

        public DeleteSuperHeroCommandHandler(ISuperHeroRepository superHeroRepository)
        {
            _superHeroRepository = superHeroRepository;
        }

        public async Task<bool> Handle(DeleteSuperHeroCommand request, CancellationToken cancel)
        {
            var superHero = await _superHeroRepository.GetByIdAsync(request.Id);

            if (superHero == null) { return false; }

            await _superHeroRepository.DeleteAsync(request.Id);
            return true;
        }

    }
}
