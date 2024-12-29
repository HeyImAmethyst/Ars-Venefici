using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Components
{
    public class Effect : AbstractComponent
    {
        private Buff buff;
        private string id;
        private int manaCost;

        public Effect(string id, int manaCost, Buff buff) : base(new SpellPartStats(SpellPartStatType.DURATION), new SpellPartStats(SpellPartStatType.POWER))
        {
            this.id = id;
            this.manaCost = manaCost;
            this.buff = buff;
        }

        public override string GetId()
        {
            return id;
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            var helper = SpellHelper.Instance();

            //int amplifier = (int)helper.GetModifiedStat(30, new SpellPartStats(SpellPartStatType.POWER), modifiers, spell, caster, target, index);
            int duration = (int)helper.GetModifiedStat(buff.millisecondsDuration, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, target, index);

            //buff.millisecondsDuration = duration;

            if(target.GetCharacter() != null && target.GetCharacter() is Farmer farmer)
            {

                Buff newBuffInstance = new Buff(
                      id: buff.id,
                      displayName: buff.displayName,
                      iconTexture: Game1.buffsIcons,
                      iconSheetIndex: buff.iconSheetIndex, //34
                      duration: duration
                  );

                farmer.applyBuff(newBuffInstance);

            }

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override int ManaCost()
        {
            return manaCost;
        }
    }
}
