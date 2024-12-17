using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.Enchantments;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;
using static System.Net.Mime.MediaTypeNames;

namespace ArsVenefici.Framework.Spell.Components
{
    public class Damage : AbstractComponent
    {
        private ComponentDamageType damageType;
        //private readonly Func<Character, DamageSource> damageSourceFunction;
        private readonly Func<Character, double> damage;
        private readonly Predicate<Character> failIf;

        private string id;

        private int manaCost;

        public Damage(string id, int manaCost, ComponentDamageType damageType, Func<Character, double> damage, Predicate<Character> failIf) : base(new SpellPartStats(SpellPartStatType.DAMAGE), new SpellPartStats(SpellPartStatType.HEALING))
        {
            this.id = id;

            this.manaCost = manaCost;
            this.damage = damage;
            this.failIf = failIf;
            this.damageType = damageType;
        }

        public Damage(string id, int manaCost, ComponentDamageType damageType, Func<double> damage, Predicate<Character> failIf): this(id, manaCost, damageType, e => damage(), failIf)
        {

        }

        public Damage(string id, int manaCost, ComponentDamageType damageType, Func<Character, double> damage): this(id, manaCost, damageType, damage, e => false)
        {

        }

        public Damage(string id, int manaCost, ComponentDamageType damageType, Func<double> damage): this(id, manaCost, damageType, e => damage(), e => false)
        {

        }

        public override string GetId()
        {
            return id;
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            Character living = target.GetCharacter();
            Vector2 characterLocation = target.GetLocation();

            Vector2 tilePos = Utils.AbsolutePosToTilePos(Utility.clampToTile(characterLocation));
            Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);
            Vector2 screenPos = Utils.TilePosToScreenPos(tilePos);

            float speed = -0.5f;
            //float speed = -0.3f;

            if (failIf != null)
            {
                if (failIf.Invoke(living))
                    return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
            }

            float damage = (float)this.damage.Invoke(living);

            //if (living is Farmer && living != caster && !((ServerLevel)level).getServer().isPvpAllowed() && damage > 0)
            //    return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);

            if (living is Monster monster)
            {
                if (damage < 0)
                {
                    damage = SpellHelper.Instance().GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.HEALING), modifiers, spell, caster, target, index);
                }

                damage = SpellHelper.Instance().GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index);

                if(caster.entity is Farmer f)
                {
                    if(damageType == ComponentDamageType.Physical)
                    {
                        return gameLocation.damageMonster(monster.GetBoundingBox(), (int)damage, (int)(damage * (1f + f.buffs.AttackMultiplier)), true, f) ? new SpellCastResult(SpellCastResultType.SUCCESS) : new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                    }

                    if (damageType == ComponentDamageType.Magic)
                    {
                        //monster.Health -= (int)damage;
                        monster.Health -= (int)(damage * (1f + f.buffs.AttackMultiplier));

                        gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(monster.GetBoundingBox().Center.X, monster.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, monster));


                        //TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("assets/particle/magic.png", new Rectangle(0, 0, 16, 16), absolutePos, false, 1f / 500f, Color.White)
                        TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(372, 1956, 10, 10), absolutePos, false, 1f / 500f, new Color(0, 48, 255, 127))
                        {
                            alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
                            alpha = 0.1f,
                            //motion = new Vector2(0.0f, speed),
                            //acceleration = new Vector2(0.0f, 0.0f),
                            interval = 99999f,
                            layerDepth = (float)(living.GetBoundingBox().Bottom - 3 - Game1.random.Next(5)) / 10000f,
                            scale = 8f,
                            scaleChange = 0.01f,
                            //rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0)
                        };

                        gameLocation.temporarySprites.Add(sprite);

                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }

                    if (damageType == ComponentDamageType.Frost)
                    {

                        if ( monster is LavaLurk || 
                            monster is HotHead || 
                            monster.Name.Equals("Magma Sprite") ||
                            monster.Name.Equals("Magma Sparker") ||
                            monster.Name.Equals("False Magma Cap") ||
                            monster.Name.Equals("Magma Duggy") ||
                            monster.Name.Equals("Lava Crab") ||
                            monster.Name.Equals("Lava Bat"))
                        {
                            damage *= 3;
                            monster.Health -= (int)damage;
                        }

                        gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(monster.GetBoundingBox().Center.X, monster.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, monster));

                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }

                    if (damageType == ComponentDamageType.Fire)
                    {

                        if (monster.Name.Equals("Dust Sprite") ||
                            monster.Name.Equals("Frost Bat"))
                        {
                            damage *= 3;
                            monster.Health -= (int)damage;
                        }

                        gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(monster.GetBoundingBox().Center.X, monster.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, monster));


                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }

                }
            }

            if (living is Farmer farmer)
            {
                if (damage < 0)
                {
                    damage = SpellHelper.Instance().GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.HEALING), modifiers, spell, caster, target, index);
                }

                damage = SpellHelper.Instance().GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index);
                
                farmer.health -= (int)damage;

                //level.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(farmer.getStandingPositionstandingPixel.X + 8, standingPixel.Y), Color.Red, 1f, farmer));
                gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(living.GetBoundingBox().Center.X, living.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, farmer));
                farmer.playNearbySoundAll("ow");

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            //return living.hurt(damageSourceFunction.apply(caster), damage) ? new SpellCastResult(SpellCastResultType.SUCCESS) : new SpellCastResult(SpellCastResultType.EFFECT_FAILED);

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        //public void DamageFarmer(Farmer who, int minDamage, int maxDamage)
        //{
        //    bool flag3 = false;

        //    int num3;
        //    if (maxDamage >= 0)
        //    {
        //        num3 = Game1.random.Next(minDamage, maxDamage + 1);
        //        if (who != null && Game1.random.NextDouble() < (double)(critChance + (float)who.LuckLevel * (critChance / 40f)))
        //        {
        //            flag3 = true;
        //            who.currentLocation.playSound("crit");
        //            if (who.hasTrinketWithID("IridiumSpur"))
        //            {
        //                BuffEffects buffEffects = new BuffEffects();
        //                buffEffects.Speed.Value = 1f;
        //                who.applyBuff(new Buff("iridiumspur", null, Game1.content.LoadString("Strings\\1_6_Strings:IridiumSpur_Name"), who.getFirstTrinketWithID("IridiumSpur").GetEffect().general_stat_1 * 1000, Game1.objectSpriteSheet_2, 76, buffEffects, false));
        //            }
        //        }

        //        num3 = (flag3 ? ((int)((float)num3 * critMultiplier)) : num3);
        //        num3 = Math.Max(1, num3 + ((who != null) ? (who.Attack * 3) : 0));
        //        if (who != null && who.professions.Contains(24))
        //        {
        //            num3 = (int)Math.Ceiling((float)num3 * 1.1f);
        //        }

        //        if (who != null && who.professions.Contains(26))
        //        {
        //            num3 = (int)Math.Ceiling((float)num3 * 1.15f);
        //        }

        //        if (who != null && flag3 && who.professions.Contains(29))
        //        {
        //            num3 = (int)((float)num3 * 2f);
        //        }

        //        //if (who != null)
        //        //{
        //        //    foreach (BaseEnchantment enchantment in who.enchantments)
        //        //    {
        //        //        enchantment.OnCalculateDamage(monster, this, who, ref num3);
        //        //    }
        //        //}
        //    }
        //}

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public void SetComponentDamageType(ComponentDamageType componentDamageType)
        {
            damageType = componentDamageType;
        }

        public ComponentDamageType GetComponentDamageType()
        {
            return damageType;
        }

        public override int ManaCost()
        {
            return manaCost;
        }
    }
}
