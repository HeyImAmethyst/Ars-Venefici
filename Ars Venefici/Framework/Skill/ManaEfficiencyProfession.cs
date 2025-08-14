using ArsVenefici.Framework.Util;
using Newtonsoft.Json.Linq;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Skill
{
    public class ManaEfficiencyProfession : GenericProfession
    {
        private int manaCostReductionAmount;


        public ManaEfficiencyProfession(ArsVeneficiSkill skill, string theId, int manaCostReductionAmount) : base(skill, theId)
        {
            this.manaCostReductionAmount = manaCostReductionAmount;
        }

        public override void DoImmediateProfessionPerk()
        {
            Game1.player.GetSpellBook().SetManaCostReductionAmount(manaCostReductionAmount);
        }

        public override T GetValue<T>()
        {
            return (T)Convert.ChangeType(manaCostReductionAmount, typeof(T));
        }
    }
}
