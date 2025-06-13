using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Buffs;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Effect : AbstractComponent
    {
        private Buff buff;
        private string id;
        private float manaCost;

        public Effect(string id, float manaCost, Buff buff) : base(new SpellPartStats(SpellPartStatType.DURATION), new SpellPartStats(SpellPartStatType.POWER))
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
            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var helper = api.GetSpellHelper();

            int amplifier = (int)helper.GetModifiedStat(1, new SpellPartStats(SpellPartStatType.POWER), modifiers, spell, caster, target, index);
            int duration = (int)helper.GetModifiedStat(buff.millisecondsDuration, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, target, index);
            
            //buff.millisecondsDuration = duration;

            if (target.GetCharacter() != null && target.GetCharacter() is Farmer farmer)
            {

                Buff newBuffInstance = new Buff(
                      id: buff.id,
                      displayName: buff.displayName,
                      iconTexture: Game1.buffsIcons,
                      iconSheetIndex: buff.iconSheetIndex, //34
                      duration: duration,
                      effects: buff.effects
                );

                if (newBuffInstance.id.Equals("HeyImAmethyst.ArsVenifici_ManaRegeneration"))
                {
                    newBuffInstance.iconTexture = modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/buff/mana_regeneration.png");
                }

                if (newBuffInstance.id.Equals("HeyImAmethyst.ArsVenifici_HealthRegeneration"))
                {
                    newBuffInstance.iconTexture = modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/buff/health_regeneration.png");
                }

                if (newBuffInstance.HasAnyEffects())
                {
                    NetFloat[] AdditiveFields = new NetFloat[12]
                    {
                        newBuffInstance.effects.CombatLevel, newBuffInstance.effects.FarmingLevel, newBuffInstance.effects.FishingLevel, newBuffInstance.effects.MiningLevel,
                        newBuffInstance.effects.LuckLevel, newBuffInstance.effects.ForagingLevel, newBuffInstance.effects.MaxStamina, newBuffInstance.effects.MagneticRadius, 
                        newBuffInstance.effects.Speed, newBuffInstance.effects.Defense,
                        newBuffInstance.effects.Attack, newBuffInstance.effects.Immunity
                    };

                    NetFloat[] MultiplicativeFields = new NetFloat[6] 
                    {
                        newBuffInstance.effects.AttackMultiplier, newBuffInstance.effects.KnockbackMultiplier, 
                        newBuffInstance.effects.WeaponSpeedMultiplier, newBuffInstance.effects.CriticalChanceMultiplier, 
                        newBuffInstance.effects.CriticalPowerMultiplier, 
                        newBuffInstance.effects.WeaponPrecisionMultiplier 
                    };

                    for (int i = 0; i < AdditiveFields.Length; i++)
                    {
                        if (AdditiveFields[i].Value != 0f)
                        {
                            AdditiveFields[i].Value *= amplifier;
                        }
                    }
                    
                    for (int i = 0; i < MultiplicativeFields.Length; i++)
                    {
                        if (MultiplicativeFields[i].Value != 0f)
                        {
                            MultiplicativeFields[i].Value *= amplifier;
                        }
                    }

                    newBuffInstance.effects.Add(new BuffEffects()
                    {
                        CombatLevel = { AdditiveFields[0].Value },
                        FarmingLevel = { AdditiveFields[1].Value },
                        FishingLevel = { AdditiveFields[2].Value },
                        MiningLevel = { AdditiveFields[3].Value },
                        LuckLevel = { AdditiveFields[4].Value },
                        ForagingLevel = { AdditiveFields[5].Value },
                        MaxStamina = { AdditiveFields[6].Value },
                        MagneticRadius = { AdditiveFields[7].Value },
                        Speed = { AdditiveFields[8].Value },
                        Defense = { AdditiveFields[9].Value },
                        Attack = { AdditiveFields[10].Value },
                        Immunity = { AdditiveFields[11].Value },

                        AttackMultiplier = { MultiplicativeFields[0].Value },
                        KnockbackMultiplier = { MultiplicativeFields[1].Value },
                        WeaponSpeedMultiplier = { MultiplicativeFields[2].Value },
                        CriticalChanceMultiplier = { MultiplicativeFields[3].Value },
                        CriticalPowerMultiplier = { MultiplicativeFields[4].Value },
                        WeaponPrecisionMultiplier = { MultiplicativeFields[5].Value }
                    });
                }

                newBuffInstance.customFields.Add($"{ModEntry.ArsVenificiModId}/EffectPower", amplifier.ToString());

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
            return manaCost;
        }
    }
}
