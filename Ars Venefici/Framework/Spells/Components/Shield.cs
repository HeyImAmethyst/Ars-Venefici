using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Spells.Buffs;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Shield : AbstractComponent
    {
        public Shield() : base()
        {

        }

        public override string GetId()
        {
            return "shield";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            int duration = (int)helper.GetModifiedStat(modEntry.buffs.shieldBuff.millisecondsDuration, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, target, index);

            if (target.GetCharacter() != null && target.GetCharacter() is Farmer farmer)
            {

                Buff newBuffInstance = new Buff(
                    id: modEntry.buffs.shieldBuff.id,
                    displayName: modEntry.buffs.shieldBuff.displayName,
                    iconTexture: Game1.buffsIcons,
                    iconSheetIndex: modEntry.buffs.shieldBuff.iconSheetIndex, //34
                    duration: modEntry.buffs.shieldBuff.millisecondsDuration
                );

                farmer.applyBuff(newBuffInstance);

            }

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 45;
        }
    }
}
