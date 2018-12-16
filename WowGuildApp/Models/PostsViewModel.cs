using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Data;

namespace WowGuildApp.Models
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public List<Post> Posts { get; set; }
        public List<Post> LatestPosts { get; set; }
        public string Category { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int MaxPage { get; set; }
        public string ReturnUrl { get; set; }
        public string CurrentUser { get; set; }
        //[StringLength(300, MinimumLength = 3, ErrorMessage = "css must be atleast 3 charcters long and cannot exceed 300 characters.")]
        public string Text { get; set; }

        public string GetRace(int val)
        {
            var css = "";

            switch (val)
            {
                case 1:
                    css = "Human";
                    break;
                case 2:
                    css = "Orc";
                    break;
                case 3:
                    css = "Dwarf";
                    break;
                case 4:
                    css = "Night Elf";
                    break;
                case 5:
                    css = "Undead";
                    break;
                case 6:
                    css = "Tauren";
                    break;
                case 7:
                    css = "Gnome";
                    break;
                case 8:
                    css = "Troll";
                    break;
                case 9:
                    css = "Goblin";
                    break;
                case 10:
                    css = "Blood Elf";
                    break;
                case 11:
                    css = "Draenei";
                    break;
                case 22:
                    css = "Worgen";
                    break;
                case 24:
                    css = "Pandaren";
                    break;
                case 25:
                    css = "Pandaren";
                    break;
                case 26:
                    css = "Pandaren";
                    break;
                case 27:
                    css = "Nightborne";
                    break;
                case 28:
                    css = "Highmountain Tauren";
                    break;
                case 29:
                    css = "Void Elf";
                    break;
                case 30:
                    css = "Lightforged Draenei";
                    break;
                case 34:
                    css = "Dark Iron Dwarf";
                    break;
                case 36:
                    css = "Mag'har Orc";
                    break;
            }
            return css;
        }

        public string GetClass(int val)
        {
            var css = "";

            switch (val)
            {
                case 1:
                    css = "Warrior";
                    break;
                case 2:
                    css = "Paladin";
                    break;
                case 3:
                    css = "Hunter";
                    break;
                case 4:
                    css = "Rogue";
                    break;
                case 5:
                    css = "Priest";
                    break;
                case 6:
                    css = "Death Knight";
                    break;
                case 7:
                    css = "Shaman";
                    break;
                case 8:
                    css = "Mage";
                    break;
                case 9:
                    css = "Warlock";
                    break;
                case 10:
                    css = "Monk";
                    break;
                case 11:
                    css = "Druid";
                    break;
                case 22:
                    css = "Demon Hunter";
                    break;
            }
            return css;
        }

        public string GetClassCss(int val)
        {
            var css = "";

            switch (val)
            {
                case 1:
                    css = "class-warrior";
                    break;
                case 2:
                    css = "class-paladin";
                    break;
                case 3:
                    css = "class-hunter";
                    break;
                case 4:
                    css = "class-rogue";
                    break;
                case 5:
                    css = "class-priest";
                    break;
                case 6:
                    css = "class-dk";
                    break;
                case 7:
                    css = "class-shaman";
                    break;
                case 8:
                    css = "class-mage";
                    break;
                case 9:
                    css = "class-warlock";
                    break;
                case 10:
                    css = "class-monk";
                    break;
                case 11:
                    css = "class-druid";
                    break;
                case 22:
                    css = "class-dh";
                    break;
            }
            return css;
        }
    }
}
