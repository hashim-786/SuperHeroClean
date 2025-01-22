﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Application.Features.SuperHeros.Commands.AddSuperHero;
using SuperHeroAPI.Application.Features.SuperHeros.Commands.DeleteSuperHero;
using SuperHeroAPI.Application.Features.SuperHeros.Commands.UpdateSuperHero;
using SuperHeroAPI.Application.Features.SuperHeros.Queries.GetAllSuperHeroes;
using SuperHeroAPI.Application.Features.SuperHeros.Queries.GetSuperHeroById;

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


        [HttpPost]
        public async Task<IActionResult> AddSuperHero(AddSuperHeroCommand command)
        {
            if (command == null) return BadRequest("Invalid data.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSuperHero(UpdateSuperHeroCommand command)
        {
            if (command == null) return BadRequest("Invalid data.");

            var result = await _mediator.Send(command);

            if (result == null) return NotFound("SuperHero not found.");


            return Ok(result);
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteSuperHero(DeleteSuperHeroCommand command)
        {
            if (command == null) return BadRequest("Invalid data.");

            var result = await _mediator.Send(command);

            if (result == null) return NotFound("SuperHero not found.");

            return Ok(result);

        }





    }
}