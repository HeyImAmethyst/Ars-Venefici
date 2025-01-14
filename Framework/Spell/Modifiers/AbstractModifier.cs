using ArsVenefici.Framework.Interfaces.Spells;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Modifiers
{
    public abstract class AbstractModifier : ISpellModifier
    {

        public abstract string GetId();

        public abstract ISpellPartStatModifier GetStatModifier(ISpellPartStat stat);

        public abstract List<ISpellPartStat> GetStatsModified();

        public override bool Equals(object obj)
        {
            return obj is ISpellModifier mod && GetId().Equals(mod.GetId());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract float ManaCost();
    }
}
