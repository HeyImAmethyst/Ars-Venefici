using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Components
{
    public abstract class AbstractComponent : ISpellComponent
    {
        private HashSet<ISpellPartStat> stats;

        protected AbstractComponent(params ISpellPartStat[] stats)
        {
            this.stats = stats.ToHashSet();
        }

        public abstract string GetId();

        public virtual HashSet<ISpellPartStat> GetStatsUsed()
        {
            return stats;
        }

        public abstract SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed);

        public abstract SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed);
        public abstract float ManaCost();
    }
}
