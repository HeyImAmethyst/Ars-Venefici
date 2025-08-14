using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Spells;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Magic
{
    public class MagicHelper : IMagicHelper
    {
        private static MagicHelper INSTANCE = new MagicHelper();

        /// <summary>The ID of the event in which the player learns magic from the Wizard.</summary>
        //public static int LearnedMagicEventId { get; } = 90002;
        int LearnedWizardryEventId { get; } = 9918172;

        public static MagicHelper Instance()
        {
            return INSTANCE;
        }

        public void AwardXp(Farmer farmer, int amount)
        {
            farmer.AddCustomSkillExperience(FarmerMagicHelper.Skill, amount);
        }

        public int GetLearnedWizardryEventId()
        {
            return LearnedWizardryEventId;
        }

        public bool LearnedWizardy(Farmer farmer)
        {
            return Game1.player?.eventsSeen?.Contains(LearnedWizardryEventId.ToString()) == true ? true : false;
        }
    }
}
