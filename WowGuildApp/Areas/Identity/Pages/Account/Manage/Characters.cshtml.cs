using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WowGuildApp.Models;
using WowGuildApp.Data;
using Newtonsoft.Json.Linq;

namespace WowGuildApp.Areas.Identity.Pages.Account.Manage
{
    public class CharactersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private ApplicationDbContext db;

        public CharactersModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpClientFactory clientFactory,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            this.db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public CharacterList Characters { get; set; }

        public List<Character> InUseCharacters { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var _access_token = await _userManager.GetAuthenticationTokenAsync(user,"BattleNet","access_token");
            string requestUri = "https://eu.api.blizzard.com/wow/user/characters?access_token=" + _access_token;

            var request = new HttpRequestMessage(
                HttpMethod.Get,requestUri);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            Characters = JsonConvert.DeserializeObject<CharacterList>(await response.Content.ReadAsStringAsync());

            InUseCharacters = db.Characters.Where(c => c.UserId == user.Id).ToList();

            return Page();
        }

        public class CharacterList
        {
            public IEnumerable<Character> Characters { get; set; }
        }

        public class InputModel
        {
            public string Name { get; set; }
            public string Realm { get; set; }
            public bool Main { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Character character = await db.Characters.FirstOrDefaultAsync(c => c.Name == Input.Name && c.Realm == Input.Realm);

            //if character doesn exist, add to database
            if (character == null)
            {
                var _access_token = await _userManager.GetAuthenticationTokenAsync(user, "BattleNet", "access_token");
                //API request for character data
                string requestUri = "https://eu.api.blizzard.com/wow/character/" + Input.Realm + "/" + Input.Name + "?fields=items&locale=en_gb&access_token=" + _access_token;
                //API request for guild data with hardcoded guild name
                string guildRequestUri = "https://eu.api.blizzard.com/wow/guild/" + Input.Realm + "/Allurium?fields=members&locale=en_gb&access_token=" + _access_token;

                //API request for character data
                var request = new HttpRequestMessage(
                    HttpMethod.Get, requestUri);
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                //API request to get guild rank
                var request2 = new HttpRequestMessage(
                    HttpMethod.Get, guildRequestUri);
                var client2 = _clientFactory.CreateClient();
                var response2 = await client.SendAsync(request2);

                //If first request fails, update error message
                if (!response.IsSuccessStatusCode)
                {
                    StatusMessage = "Error: Could not update profile";
                    return RedirectToPage();
                }

                var json = await response.Content.ReadAsStringAsync();
                var json2 = await response2.Content.ReadAsStringAsync();
                var root = JObject.Parse(json);
                var root2 = JObject.Parse(json2);
                var averageItemLevel = root["items"]["averageItemLevelEquipped"];
                //Get guild rank for the selected character
                JToken guildRankToken = root2.SelectToken("$.members[?(@.character.name == '"+Input.Name+"')].rank");

                Character Character = JsonConvert.DeserializeObject<Character>(json);
                Character.AverageItemLevelEquipped = Convert.ToInt32(averageItemLevel);
                Character.User = user;
                Character.UserId = user.Id;
                Character.Main = Input.Main;

                await db.Characters.AddAsync(Character);
                user.Characters.Add(Character);
                //Check if guildRankToken is not null
                if (guildRankToken != null)
                {
                    //Convert JSON to int
                    int guildRank = guildRankToken.ToObject<int>();

                    //Check if the rank from the API call is higher than the users current rank. Update the users guildrank if true
                    //Lower number means higher rank (Guildmaster has number 0)
                    if (guildRank < user.GuildRank)
                    {
                        user.GuildRank = guildRank;
                    }
                }
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }

            //if character already exist, make it the users main if not already the main
            else if(!character.Main)
            {
                foreach(Character c in db.Characters.Where(i => i.UserId == user.Id).ToList())
                {
                    c.Main = !Input.Main;
                    db.Characters.Update(c);
                }

                character.Main = Input.Main;

                db.Characters.Update(character);
                await db.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
       
    }
}