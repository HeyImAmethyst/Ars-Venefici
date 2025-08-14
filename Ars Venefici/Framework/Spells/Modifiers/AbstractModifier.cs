using ArsVenefici.Framework.API.Spell;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Modifiers
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

        public virtual string DisplayName()
        {
            return ModEntry.INSTANCE.Helper.Translation.Get($"spellpart.{GetId()}.name");
        }

        public virtual string DisplayDiscription()
        {
            return ModEntry.INSTANCE.Helper.Translation.Get($"spellpart.{GetId()}.description");
        }
    }
}
