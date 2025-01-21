using AutoMapper;
using MediatR;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Application.Features.SuperHeros.Queries.GetAllSuperHeroes
{
    public class GetAllSuperHeroesQueryHandler : IRequestHandler<GetAllSuperHeroesQuery, List<SuperHeroDTO>>
    {
        private readonly ISuperHeroRepository _superHeroRepository;
        private readonly IMapper _mapper;

        public GetAllSuperHeroesQueryHandler(ISuperHeroRepository repository, IMapper mapper)
        {
            _superHeroRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<SuperHeroDTO>> Handle(GetAllSuperHeroesQuery request, CancellationToken cancellationToken)
        {
            // Fetch heroes from the repository
            var heros = await _superHeroRepository.GetAllAsync();

            // Map the list of SuperHero entities to SuperHeroDTOs using AutoMapper
            var heroDTOs = _mapper.Map<List<SuperHeroDTO>>(heros);

            return heroDTOs;
        }
    }
}
