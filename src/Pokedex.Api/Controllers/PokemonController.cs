using System.Threading;
using System.Threading.Tasks;
using Pokedex.Application.Pokemon;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Models;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : Controller
    {
        private readonly IMediator mediator;

        public PokemonController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute]string name, CancellationToken cancellationToken)
        {
            var findPokemonContext = new FindPokemonByNameQuery(name);
            var findResult = await this.mediator.Send(findPokemonContext, cancellationToken);

            if (!findResult.WasSuccessful)
            {
                return NotFound();
            }
            
            return Ok(new Pokemon(findResult.Value.Name, findResult.Value.Description, findResult.Value.Habitat, findResult.Value.IsLegendary));
        }
    }
}