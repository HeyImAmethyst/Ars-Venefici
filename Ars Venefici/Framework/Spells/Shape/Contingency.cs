using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Magic;
using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Contingency : AbstractShape
    {
        private string id;
        private ContingencyType type;

        public Contingency(string id, ContingencyType contingencyType)
        {
            this.id = id;
            this.type = contingencyType;
        }

        public override string GetId()
        {
            return id;
        }

        public ContingencyType GetContingencyType()
        {
            return type;
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var spellHelper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();
            var contingencyHelper = ContingencyHelper.Instance();

            Character target = null;

            if (hit is CharacterHitResult result && result.GetCharacter() is Farmer entity) 
            {
                target = entity;
            }

            if (target == null)
            {
                target =  caster.entity as Character;
            }

            contingencyHelper.SetContingency(target, type, spell);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override bool NeedsToComeFirst()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 10;
        }
    }
}
