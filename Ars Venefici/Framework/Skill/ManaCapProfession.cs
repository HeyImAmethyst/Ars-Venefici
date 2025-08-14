using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Skill
{
    public class ManaCapProfession : GenericProfession
    {
        /*********
        ** Public methods
        *********/
        public ManaCapProfession(ArsVeneficiSkill skill, string theId)
            : base(skill, theId) { }

        public override void DoImmediateProfessionPerk()
        {
            Game1.player.SetMaxMana(Game1.player.GetMaxMana() + 500);
        }
    }
}
