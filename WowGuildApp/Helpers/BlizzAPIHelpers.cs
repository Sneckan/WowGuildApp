using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Helpers
{
    public static class BlizzAPIHelpers
    {
        public static string GetRace(int val)
        {
            var race = "";

            switch (val)
            {
                case 1:
                    race = "Human";
                    break;
                case 2:
                    race = "Orc";
                    break;
                case 3:
                    race = "Dwarf";
                    break;
                case 4:
                    race = "Night Elf";
                    break;
                case 5:
                    race = "Undead";
                    break;
                case 6:
                    race = "Tauren";
                    break;
                case 7:
                    race = "Gnome";
                    break;
                case 8:
                    race = "Troll";
                    break;
                case 9:
                    race = "Goblin";
                    break;
                case 10:
                    race = "Blood Elf";
                    break;
                case 11:
                    race = "Draenei";
                    break;
                case 22:
                    race = "Worgen";
                    break;
                case 24:
                    race = "Pandaren";
                    break;
                case 25:
                    race = "Pandaren";
                    break;
                case 26:
                    race = "Pandaren";
                    break;
                case 27:
                    race = "Nightborne";
                    break;
                case 28:
                    race = "Highmountain Tauren";
                    break;
                case 29:
                    race = "Void Elf";
                    break;
                case 30:
                    race = "Lightforged Draenei";
                    break;
                case 34:
                    race = "Dark Iron Dwarf";
                    break;
                case 36:
                    race = "Mag'har Orc";
                    break;
            }
            return race;
        }

        public static string GetClassName(int val)
        {
            var name = "";

            switch (val)
            {
                case 1:
                    name = "Warrior";
                    break;
                case 2:
                    name = "Paladin";
                    break;
                case 3:
                    name = "Hunter";
                    break;
                case 4:
                    name = "Rogue";
                    break;
                case 5:
                    name = "Priest";
                    break;
                case 6:
                    name = "Death Knight";
                    break;
                case 7:
                    name = "Shaman";
                    break;
                case 8:
                    name = "Mage";
                    break;
                case 9:
                    name = "Warlock";
                    break;
                case 10:
                    name = "Monk";
                    break;
                case 11:
                    name = "Druid";
                    break;
                case 22:
                    name = "Demon Hunter";
                    break;
            }
            return name;
        }

        public static string GetClassCss(int val)
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

        public static string GetGuildrank(int val)
        {
            var rank = "";

            switch (val)
            {
                case 0:
                    rank = "Guild Master";
                    break;
                case 1:
                    rank = "Officer";
                    break;
                case 2:
                    rank = "Raider";
                    break;
                case 3:
                    rank = "Member";
                    break;
                case 9:
                    rank = "Scrub";
                    break;
            }
            return rank;
        }
    }
}
