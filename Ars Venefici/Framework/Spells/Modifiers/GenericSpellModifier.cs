using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Modifiers
{

    public class GenericSpellModifier : AbstractModifier
    {
        string id = "";
        float manaCost;
        IModHelper modHelper;

        private HashSet<Affinity> affinities;

        protected Dictionary<ISpellPartStat, ISpellPartStatModifier> modifiers = new Dictionary<ISpellPartStat, ISpellPartStatModifier>();

        public GenericSpellModifier(string id, HashSet<Affinity> affinities, IModHelper modHelper,float manaCost)
        {
            this.id = id;
            this.manaCost = manaCost;

            this.modHelper = modHelper;

            this.affinities = affinities;
        }

        public override string GetId()
        {
            return id;
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public override HashSet<Affinity> GetAffinities()
        {
            return affinities;
        }

        public override Dictionary<Affinity, float> GetAffinityShifts()
        {
            Dictionary<Affinity, float> shifts = new Dictionary<Affinity, float>();

            foreach (var aff in affinities)
            {
                shifts.Add(aff, 0.001f);
            }

            return shifts;
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

        public override string DisplayName()
        {
            return modHelper.Translation.Get($"spellpart.{GetId()}.name");
        }

        public override string DisplayDiscription()
        {
            return modHelper.Translation.Get($"spellpart.{GetId()}.description");
        }
    }
}
