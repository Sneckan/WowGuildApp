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

            var _access_token = await _userManager.GetAuthenticationTokenAsync(user, "BattleNet", "access_token");
            string requestUri = "https://eu.api.blizzard.com/wow/character/"+Input.Realm+"/"+Input.Name+"?access_token="+ _access_token;    

            var request = new HttpRequestMessage(
                HttpMethod.Get, requestUri);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            Character Character = JsonConvert.DeserializeObject<Character>(await response.Content.ReadAsStringAsync());

            Character.User = user;
            Character.UserId = user.Id;
            Character.Main = Input.Main;

            Character character = await db.Characters.FirstOrDefaultAsync(c => c.Name == Character.Name && c.Realm == Character.Realm);

            if (character == null)
            {
                await db.Characters.AddAsync(Character);
                user.Characters.Add(Character);
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }
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