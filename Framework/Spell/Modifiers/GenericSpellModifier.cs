using ArsVenefici.Framework.Interfaces.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Modifiers
{

    public class GenericSpellModifier : AbstractModifier
    {
        string id = "";
        float manaCost;

        protected Dictionary<ISpellPartStat, ISpellPartStatModifier> modifiers = new Dictionary<ISpellPartStat, ISpellPartStatModifier>();

        public GenericSpellModifier(float manaCost)
        {
            this.manaCost = manaCost;
        }

        public override string GetId()
        {
            return id;
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public override ISpellPartStatModifier GetStatModifier(ISpellPartStat stat)
        {
            //return modifiers[stat];
            return modifiers.Where(kvp => kvp.Key.equals(stat)).ToList().Find(c=> c.Key.equals(stat)).Value;
        }

        public override List<ISpellPartStat> GetStatsModified()
        {
            return modifiers.Keys.ToList();
        }

        public GenericSpellModifier AddStatModifier(ISpellPartStat stat, ISpellPartStatModifier modifier)
        {
            modifiers.Add(stat, modifier);
            return this;
        }

        public override float ManaCost()
        {
            return manaCost;
        }
    }
}
