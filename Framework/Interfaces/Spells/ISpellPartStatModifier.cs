using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace ArsVenefici.Framework.Interfaces.Spells
{
    /// <summary>
    /// Interface to modify a stat of a spell part.
    /// </summary>
    public class ISpellPartStatModifier
    {

        public delegate float SpellPartStatModifier(float baseValue, float modified, ISpell spell, IEntity caster, HitResult target, int componentIndex);
        SpellPartStatModifier method;

        public ISpellPartStatModifier(SpellPartStatModifier method)
        {
            this.method = method;
        }


        /// <summary>
        ///  Modifies the stat value.
        /// </summary>
        /// <param name="baseValue">The base value being modified.</param>
        /// <param name="modified">The value with all previous modifications.</param>
        /// <param name="spell">The spell this is calculated for.</param>
        /// <param name="caster">The character casting the spell.</param>
        /// <param name="target"></param>
        /// <param name="componentIndex">The target of the spell.</param>
        /// <returns></returns>
        public float Modify(float baseValue, float modified, ISpell spell, IEntity caster, HitResult target, int componentIndex)
        {
            return method.Invoke(baseValue, modified, spell, caster, target, componentIndex);
        }
    }
}
