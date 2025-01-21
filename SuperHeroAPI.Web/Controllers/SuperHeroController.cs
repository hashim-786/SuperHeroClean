using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Application.Features.SuperHeros.Queries.GetAllSuperHeroes;
using SuperHeroAPI.Application.Features.SuperHeros.Queries.GetSuperHeroById;
using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperHeroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuperHeroController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDTO>>> GetAllHeroes()
        {
            var heroes = await _mediator.Send(new GetAllSuperHeroesQuery());
            return Ok(heroes);

        }

        [HttpGet("{id}")]

        public async Task<ActionResult<SuperHeroDTO>> GetHeroById(int id)
        {
            var hero = await _mediator.Send(new GetSuperHeroByIdQuery(id));

            if (hero == null) { return NotFound(); }

            return Ok(hero);
        }

        [HttpPut]

        public async Task<ActionResult<SuperHeroDTO>> AddAsync(SuperHero hero)
        {
            var hero = await _mediator.Send(new)
        }


    }
}