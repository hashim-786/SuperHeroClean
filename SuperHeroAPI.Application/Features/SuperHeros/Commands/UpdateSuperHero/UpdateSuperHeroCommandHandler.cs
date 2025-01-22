using AutoMapper;
using MediatR;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.UpdateSuperHero
{
    public class UpdateSuperHeroCommandHandler : IRequestHandler<UpdateSuperHeroCommand, SuperHeroDTO>
    {
        private readonly ISuperHeroRepository _superHerorepository;
        private readonly IMapper _mapper;

        public UpdateSuperHeroCommandHandler(ISuperHeroRepository superHerorepository, IMapper mapper)
        {
            _superHerorepository = superHerorepository;
            _mapper = mapper;
        }

        public async Task<SuperHeroDTO> Handle(UpdateSuperHeroCommand request, CancellationToken cancel)
        {
            var superHero = await _superHerorepository.GetByIdAsync(request.Id);

            if (superHero == null) { return null; }

            superHero.FirstName = request.FirstName;
            superHero.LastName = request.LastName;
            superHero.Place = request.Place;

            await _superHerorepository.UpdateAsync(superHero);
            return _mapper.Map<SuperHeroDTO>(superHero);

        }
    }
}
