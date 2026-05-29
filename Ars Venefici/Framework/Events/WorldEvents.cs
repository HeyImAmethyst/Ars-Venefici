using ArsVenefici.Framework.FarmerPlayer;
using StardewModdingAPI.Events;
using StardewValley.Monsters;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceCore;
using ArsVenefici.Framework.Util;

namespace ArsVenefici.Framework.Events
{
    public class WorldEvents
    {
        ModEntry modEntryInstance;

        public WorldEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;
        }

        public void OnNpcListChanged(object sender, NpcListChangedEventArgs e)
        {

            if (modEntryInstance.arsVeneficiAPILoader.GetAPI().GetMagicHelper().LearnedWizardy(Game1.player) && Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) >= 3)
            {
                foreach (var character in e.Added)
                {
                    if (character != null && character is Monster monster)
                    {
                        monster.AddAffinityBasedDropsForMonster();
                    }
                }
            }
        }
    }
}
