using ArsVenefici.Framework.Interfaces.Spells;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Magic
{
    public interface IMagicHelper
    {
        /// <param name="farmer">The farmer to check.</param>
        /// <returns>Whether the current player learned wizardry.</returns>
        bool LearnedWizardy(Farmer farmer);

        /// <summary>
        /// Awards the given amount of magic xp to the given player. Also handles leveling.
        /// </summary>
        /// <param name="farmer">The farmer to award the magic xp to.</param>
        /// <param name="amount">The amount of magic xp to award.</param>
        void AwardXp(Farmer farmer, int amount);

        int GetLearnedWizardryEventId();
    }
}
