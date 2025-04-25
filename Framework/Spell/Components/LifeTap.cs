using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Components
{
    public class LifeTap : AbstractComponent
    {
        public LifeTap() : base(new SpellPartStats(SpellPartStatType.DAMAGE), new SpellPartStats(SpellPartStatType.HEALING))
        {

        }

        public override string GetId()
        {
            return "life_tap";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            if (target.GetCharacter() is Monster living)
            {
                var helper = SpellHelper.Instance();
                float damage = helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index) * 2;

                Farmer farmer = ((Farmer)caster.entity);

                int monsterHealth = living.Health;

                if (gameLocation.damageMonster(living.GetBoundingBox(), (int)damage, (int)(damage * (1f + farmer.buffs.AttackMultiplier)), true, farmer))
                {
                    float value = (Game1.random.Next((int)damage, (int)(damage * (1f + farmer.buffs.AttackMultiplier)) + 1) * monsterHealth * 0.07f) / 1.0f;

                    //int percentage = 25;
                    //float result = (percentage / 100) * monsterHealth;
                    //int finalValue = (int)Math.Min(value, result);

                    farmer.AddMana((int)value);
                }

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            if (target.GetCharacter() is Farmer targetFarmer && target.GetCharacter() != caster.entity)
            {
                Farmer farmer = ((Farmer)caster.entity);
                var helper = SpellHelper.Instance();
                float damage = helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index) * 2;

                targetFarmer.health -= (int)damage;
                float value = (Game1.random.Next((int)damage, (int)(damage * (1f + farmer.buffs.AttackMultiplier)) + 1) * targetFarmer.health * 0.07f) / 1.0f;
                
                //int percentage = 25;
                //float result = (percentage / 100) * targetFarmer.health;
                //int finalValue = (int)Math.Min(value, result);

                farmer.AddMana((int)value);

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 5;
        }
    }
}
