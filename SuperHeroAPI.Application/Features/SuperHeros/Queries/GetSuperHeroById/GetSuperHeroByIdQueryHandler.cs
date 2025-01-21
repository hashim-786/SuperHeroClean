using AutoMapper;
using MediatR;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Interfaces;

namespace SuperHeroAPI.Application.Features.SuperHeros.Queries.GetSuperHeroById
{
    internal class GetSuperHeroByIdQueryHandler : IRequestHandler<GetSuperHeroByIdQuery, SuperHeroDTO>
    {
        private readonly ISuperHeroRepository _superHeroRepository;
        private readonly IMapper _mapper;

        public GetSuperHeroByIdQueryHandler(ISuperHeroRepository superHeroRepository, IMapper mapper)
        {
            _superHeroRepository = superHeroRepository;
            _mapper = mapper;
        }

        public async Task<SuperHeroDTO> Handle(GetSuperHeroByIdQuery query, CancellationToken cancel)
        {
            var fetechHero = await _superHeroRepository.GetByIdAsync(query.Id);
            if (fetechHero == null) { return null; }

            var heroDTO = _mapper.Map<SuperHeroDTO>(fetechHero);
            return heroDTO;
        }
    }
}
