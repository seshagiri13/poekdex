using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Pokedex.Controllers;
using Pokedex.Models;
using System.Threading.Tasks;

namespace PokedexTest
{
    public class Tests
    {
        private readonly ILogger<PokemonController> _logger;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetPokemon()
        {
            PokemonController pokemonController = new PokemonController(_logger);
            var result = await pokemonController.GetPokemon("mewtwo");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Name, "mewtwo");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Description, "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Habitat, "rare");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).isLegendary.ToString(), "True");
        }

        [Test]
        public async Task TestGetTranslatedPokemon()
        {
            PokemonController pokemonController = new PokemonController(_logger);
            var result = await pokemonController.GetTranslatedPokemon("mewtwo");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Name, "mewtwo");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Description, "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Habitat, "rare");
            Assert.AreEqual(((Pokemon)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).isLegendary.ToString(), "True");
        }
    }
}