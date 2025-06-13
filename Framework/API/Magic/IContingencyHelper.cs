using ArsVenefici.Framework.API.Spell;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Magic
{
    public interface IContingencyHelper
    {
        /// <summary>
        /// Set the contingency type and spell for a given target.
        /// </summary>
        /// <param name="target">The target to set the contingency for.</param>
        /// <param name="type">The contingency type</param>
        /// <param name="spell">The spell to set.</param>
        void SetContingency(Character target, ContingencyType type, ISpell spell);


        /// <summary>
        ///  Get the contingency type for a given target.
        /// </summary>
        /// <param name="target">The target to get the contingency type for.</param>
        /// <returns>The contingency type.</returns>
        ContingencyType GetContingencyType(Character target);


        /// <summary>
        /// Trigger a contingency for a given target.
        /// </summary>
        /// <param name="target">The target to trigger the contingency for.</param>
        /// <param name="type">The contingency type.</param>
        void TriggerContingency(Character target, ContingencyType type);

        /// <summary>
        /// Clear the contingency for a given target.
        /// </summary>
        /// <param name="target">The target to clear the contingency for.</param>
        void ClearContingency(Character target);
    }
}
