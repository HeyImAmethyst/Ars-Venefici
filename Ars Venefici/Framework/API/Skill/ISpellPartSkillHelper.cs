using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;

namespace ArsVenefici.Framework.API.Skill
{
    public interface ISpellPartSkillHelper
    {
        bool Knows(ModEntry modEntry, Farmer player, string skillID);

        bool Knows(ModEntry modEntry, Farmer player, SpellPartSkill skill);

        bool CanLearn(ModEntry modEntry, Farmer player, SpellPartSkill skill);

        void Learn(ModEntry modEntry, Farmer player, SpellPartSkill skill);

        void Learn(ModEntry modEntry, Farmer player, string spellPartId);

        void Forget(ModEntry modEntry, Farmer player, SpellPartSkill skill);

        void Forget(ModEntry modEntry, Farmer player, string spellPartId);

        void LearnAll(ModEntry modEntry, Farmer player);

        void ForgetAll(ModEntry modEntry, Farmer player);

        void UpdateIfNeeded(ModEntry modEntry, Farmer player);

        IDictionary<string, SpellPartSkill> GetKnownSpellPartSkills(ModEntry modEntry, Farmer player);
    }
}
