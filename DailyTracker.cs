using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici
{
    public class DailyTracker
    {
        private int dailyGrowCastCount = 0;
        private int maxDailyGrowCastCount = 2;

        public void Update(ModEntry modEntry, UpdateTickedEventArgs e, GameLocation gameLocation)
        {

            if (gameLocation != null && Game1.activeClickableMenu == null && Game1.game1.IsActive)
            {
                bool regenMana = false;

                if (gameLocation.NameOrUniqueName.Equals("SkullCave"))
                {
                    regenMana = e.IsMultipleOf(540);
                }
                else 
                {
                    regenMana = e.IsMultipleOf(420);
                }

                if (regenMana)
                {
                    //float factor = (Game1.player.GetCustomSkillLevel(ModEntry.Skill) + 1) / 2; // start at +1 mana at level 1

                    //if (Game1.player.HasCustomProfession(Skill.ManaRegen2Profession))
                    //    factor *= 3;
                    //else if (Game1.player.HasCustomProfession(Skill.ManaRegen1Profession))
                    //    factor *= 2;

                    //int manaRegenValue = (int)(2 * factor);

                    int levelAmount = Game1.player.GetCustomSkillLevel(ModEntry.Skill);

                    if (Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaRegen1Profession))
                        levelAmount *= 2;
                    else if(Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaRegen2Profession))
                        levelAmount *= 4;

                    int manaRegenValue = modEntry.ModSaveData.ManaRegenRate * levelAmount;

                    Game1.player.AddMana(manaRegenValue);
                }
            }
        }

        public void SetDailyGrowCastCount(int dailyGrowCastCount)
        {
            this.dailyGrowCastCount = dailyGrowCastCount;
        }

        public int GetDailyGrowCastCount()
        {
            return dailyGrowCastCount;
        }

        public void SetMaxDailyGrowCastCount(int max)
        {
            maxDailyGrowCastCount = max;
        }

        public int GetMaxDailyGrowCastCount()
        {
            return maxDailyGrowCastCount;
        }
    }
}
