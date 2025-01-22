using AutoMapper;
using MediatR;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Application.Features.SuperHeros.Commands.AddSuperHero
{
    public class AddSuperHeroCommandHandler : IRequestHandler<AddSuperHeroCommand, SuperHeroDTO>
    {
        private readonly ISuperHeroRepository _superHerorepository;
        private readonly IMapper _mapper;

        public AddSuperHeroCommandHandler(ISuperHeroRepository superHerorepository, IMapper mapper)
        {
            _superHerorepository = superHerorepository;
            _mapper = mapper;
        }

        public async Task<SuperHeroDTO> Handle(AddSuperHeroCommand request, CancellationToken cancel)
        {
            var newHero = new SuperHero
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Place = request.Place
            };

            await _superHerorepository.AddAsync(newHero);
            return _mapper.Map<SuperHeroDTO>(newHero);
        }
    }
}
