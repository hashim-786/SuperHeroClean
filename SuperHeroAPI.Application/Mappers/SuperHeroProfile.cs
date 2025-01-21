using AutoMapper;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Application.Mappings
{
    public class SuperHeroProfile : Profile
    {
        public SuperHeroProfile()
        {
            CreateMap<SuperHero, SuperHeroDTO>(); // Map from SuperHero entity to SuperHeroDTO
            CreateMap<SuperHeroDTO, SuperHero>(); // Map from SuperHeroDTO to SuperHero entity
            CreateMap<CreateSuperHeroDTO, SuperHero>(); // Map CreateSuperHeroDTO to SuperHero entity
            CreateMap<UpdateSuperHeroDTO, SuperHero>(); // Map UpdateSuperHeroDTO to SuperHero entity
        }
    }
}
