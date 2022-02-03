using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pokedex.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [Route("/Pokemon/{name}")]
        public async Task<IActionResult> GetPokemon(string name)
        {
            try
            {
                var client = new RestClient($"https://pokeapi.co/api/v2/pokemon-species/{name}");
                var request = new RestRequest();
                var apiResponse = await client.ExecuteAsync(request);
                if (apiResponse.IsSuccessful)
                {
                    var content = JsonConvert.DeserializeObject<JToken>(apiResponse.Content);
                    var response = new Pokemon
                    {
                        Name = content["name"].Value<string>(),
                        Description = content["flavor_text_entries"][0]["flavor_text"].Value<string>(),
                        Habitat = ((JProperty)content["habitat"].First()).Value.Value<string>(),
                        isLegendary = content["is_legendary"].Value<bool>(),
                    };

                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, new { errmsg = "Issue with external api" });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { errmsg = "Issue with external api" });
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Route("/Pokemon/translated/{name}")]
        public async Task<IActionResult> GetTranslatedPokemon(string name)
        {
            try
            {
                var client = new RestClient($"https://pokeapi.co/api/v2/pokemon-species/{name}");
                var request = new RestRequest();
                var apiResponse = await client.ExecuteAsync(request);
                if (apiResponse.IsSuccessful)
                {
                    var content = JsonConvert.DeserializeObject<JToken>(apiResponse.Content);
                    var habitat = ((JProperty)content["habitat"].First()).Value.Value<string>();
                    var isLegendary = content["is_legendary"].Value<bool>();
                    var description = content["flavor_text_entries"][0]["flavor_text"].Value<string>();
                    if (habitat == "cave" || isLegendary)
                    {
                        using (var httpClient = new HttpClient())
                        {
                            using (var yodaTranslationRequest = new HttpRequestMessage(new HttpMethod("POST"), "https://api.funtranslations.com/translate/yoda.json"))
                            {

                                yodaTranslationRequest.Content = new StringContent($"text={description}");
                                yodaTranslationRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                                var yodaTranslatedResponse = await httpClient.SendAsync(yodaTranslationRequest);
                                var yodaContent = JsonConvert.DeserializeObject<JToken>(await yodaTranslatedResponse.Content.ReadAsStringAsync());
                                description = yodaContent["contents"]["translated"].Value<string>();
                            }
                        }
                    }
                    else
                    {
                        using (var httpClient = new HttpClient())
                        {
                            using (var shakespeareTranslationRequest = new HttpRequestMessage(new HttpMethod("POST"), "https://api.funtranslations.com/translate/shakespeare.json"))
                            {

                                shakespeareTranslationRequest.Content = new StringContent($"text={description}");
                                shakespeareTranslationRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                                var shakespeareTranslatedResponse = await httpClient.SendAsync(shakespeareTranslationRequest);
                                var shakespeareContent = JsonConvert.DeserializeObject<JToken>(await shakespeareTranslatedResponse.Content.ReadAsStringAsync());
                                description = shakespeareContent["contents"]["translated"].Value<string>();
                            }
                        }
                    }

                    var response = new Pokemon
                    {
                        Name = content["name"].Value<string>(),
                        Description = description,
                        Habitat = ((JProperty)content["habitat"].First()).Value.Value<string>(),
                        isLegendary = content["is_legendary"].Value<bool>(),
                    };

                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, new { errmsg = "Issue with external api" });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { errmsg = "Issue with external api" });
            }
            }
        }
    }
