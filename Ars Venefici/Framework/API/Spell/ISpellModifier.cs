using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArsVenefici.Framework.API.Spell.ISpellPart;

namespace ArsVenefici.Framework.API.Spell
{
    /// <summary>
    /// Base interface for a spell modifier.
    /// </summary>
    public interface ISpellModifier : ISpellPart
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stat">The stat to modify.</param>
        /// <returns>A modifier for the given stat.</returns>
        ISpellPartStatModifier GetStatModifier(ISpellPartStat stat);

        /// <returns>A set containing all stats this modifier modifies.</returns>
        List<ISpellPartStat> GetStatsModified();

        SpellPartType ISpellPart.GetType()
        {
            return SpellPartType.MODIFIER;
        }
    }
}
