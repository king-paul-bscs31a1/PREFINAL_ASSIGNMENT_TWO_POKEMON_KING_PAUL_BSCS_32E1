using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokemonApp.Models;
using System.Collections.Generic;
using PREFINAL_ASSIGNMENT_TWO_POKEMON_KING_PAUL_BSCS_32E1.Models;

namespace PokemonApp.Controllers
{
    public class PokemonController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokemonController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 20;
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon?offset={(page - 1) * pageSize}&limit={pageSize}");
            var json = JObject.Parse(response);
            var results = json["results"].ToObject<List<Pokemon>>();

            ViewBag.TotalCount = json["count"].ToObject<int>();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            return View(results);
        }

        public async Task<IActionResult> Details(string name)
        {
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
            var json = JObject.Parse(response);

            var pokemon = new Pokemon
            {
                Name = json["name"].ToString(),
                Moves = json["moves"].Select(m => m["move"]["name"].ToString()).ToList(),
                Abilities = json["abilities"].Select(a => a["ability"]["name"].ToString()).ToList()
            };

            return View(pokemon);
        }
    }
}
