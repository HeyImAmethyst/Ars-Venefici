using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

            if (bool.TryParse(args[0], out value))
            {
                if (value)
                {
                    Game1.player.eventsSeen.Add(modEntry.LearnedWizardryEventId.ToString());
                    
                    CraftingRecipe craftingRecipe = new CraftingRecipe(s);
                   
                    if (!Game1.player.craftingRecipes.Keys.Contains(s))
                        Game1.player.craftingRecipes.Add(s, 0);

                    modEntry.Monitor.Log("You have learned wizardry!", LogLevel.Info);
                }
                else if (value == false)
                {
                    Game1.player.eventsSeen.Remove(modEntry.LearnedWizardryEventId.ToString());

                    //if (Game1.player.craftingRecipes.ContainsKey(s))
                    //    Game1.player.craftingRecipes.Remove(s);

                    modEntry.Monitor.Log("You have forgotten wizardry!", LogLevel.Info);
                }
            }
        }
    }
}
