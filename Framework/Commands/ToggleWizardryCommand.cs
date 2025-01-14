using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Util;
using SpaceCore;
using SpaceShared;
using SpaceShared.ConsoleCommands;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;

namespace ArsVenefici.Framework.Commands
{
    public class ToggleWizardryCommand : Command
    {

        public ToggleWizardryCommand(ModEntry modEntry) : base(modEntry) 
        {

        }

        public void ToggleWizardry(string command, string[] args)
        {
            bool value;

            string s = $"{ModEntry.ArsVenificiContentPatcherId}_MagicAltar";

            if (args.Length > 0 && args[0] != null && bool.TryParse(args[0], out value))
            {
                if (value)
                {
                    if(Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) < 1)
                    {
                        Game1.player.AddCustomSkillExperience(FarmerMagicHelper.Skill, FarmerMagicHelper.Skill.ExperienceCurve[0]);
                        modEntry.farmerMagicHelper.FixManaPoolIfNeeded(Game1.player, overrideWizardryLevel: 1);
                    }
                    else if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) >= 1)
                    {
                        modEntry.farmerMagicHelper.FixManaPoolIfNeeded(Game1.player, overrideWizardryLevel: Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill));
                    }

                    Game1.player.eventsSeen.Add(modEntry.farmerMagicHelper.LearnedWizardryEventId.ToString());
                    
                    CraftingRecipe craftingRecipe = new CraftingRecipe(s);
                   
                    if (!Game1.player.craftingRecipes.Keys.Contains(s))
                        Game1.player.craftingRecipes.Add(s, 0);

                    modEntry.Monitor.Log("You have learned wizardry!", LogLevel.Info);
                }
                else if (value == false)
                {
                    Game1.player.SetMaxMana(0);
                    Game1.player.eventsSeen.Remove(modEntry.farmerMagicHelper.LearnedWizardryEventId.ToString());

                    //if (Game1.player.craftingRecipes.ContainsKey(s))
                    //    Game1.player.craftingRecipes.Remove(s);

                    modEntry.Monitor.Log("You have forgotten wizardry!", LogLevel.Info);
                }
            }
        }
    }
}
