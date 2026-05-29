using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Magic;
using ArsVenefici.Framework.Spells.affinity;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

namespace ArsVenefici.Framework.Spells.Components
{
    public class Damage : AbstractComponent
    {
        private ComponentDamageType damageType;
        //private readonly Func<Character, DamageSource> damageSourceFunction;
        private readonly Func<Character, double> damage;
        private readonly Predicate<Character> failIf;

        private string id;

        private float manaCost;

        private HashSet<Affinity> affinities;

        public Damage(string id, HashSet<Affinity> affinities, float manaCost, ComponentDamageType damageType, Func<Character, double> damage, Predicate<Character> failIf) : base(new SpellPartStats(SpellPartStatType.DAMAGE), new SpellPartStats(SpellPartStatType.HEALING))
        {
            this.id = id;

            this.manaCost = manaCost;
            this.damage = damage;
            this.failIf = failIf;
            this.damageType = damageType;
            this.affinities = affinities;
        }

        public Damage(string id, HashSet<Affinity> affinities, float manaCost, ComponentDamageType damageType, Func<double> damage, Predicate<Character> failIf): this(id, affinities, manaCost, damageType, e => damage(), failIf)
        {

        }

        public Damage(string id, HashSet<Affinity> affinities, float manaCost, ComponentDamageType damageType, Func<Character, double> damage): this(id, affinities, manaCost, damageType, damage, e => false)
        {

        }

        public Damage(string id, HashSet<Affinity> affinities, float manaCost, ComponentDamageType damageType, Func<double> damage): this(id, affinities, manaCost, damageType, e => damage(), e => false)
        {

        }

        public override string GetId()
        {
            return id;
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

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            Character living = target.GetCharacter();
            Vector2 characterLocation = target.GetLocation();

            //Get positions
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

            //Do damage based on the target and magic type of the damage component
            if (living is Monster monster)
            {
                if (damage < 0)
                {
                    damage = helper.GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.HEALING), modifiers, spell, caster, target, index);
                }

                damage = helper.GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index);

                Affinity monsterAffinity = monster.GetAffinity();

                foreach (var damageAffinity in GetAffinities())
                {
                    if (monsterAffinity.directOpposite == damageAffinity.id)
                    {
                        damage *= 3;
                        break;
                    }
                }

                if (caster.entity is Farmer f)
                {
                    if(damageType == ComponentDamageType.Physical)
                    {
                        return gameLocation.damageMonster(monster.GetBoundingBox(), (int)damage, (int)(damage * (1f + f.buffs.AttackMultiplier)), true, f) ? new SpellCastResult(SpellCastResultType.SUCCESS) : new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                    }
                    else
                    {
                        monster.Health -= (int)(damage * (1f + f.buffs.AttackMultiplier));

                        gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(monster.GetBoundingBox().Center.X, monster.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, monster));

                        modEntry.characterEvents.InvokeOnCharacterDamage(monster);

                        SpawnParticles(spell, modEntry, gameLocation, monster.Position, damageType);

                        if (monster.Health <= 0)
                        {
                            modEntry.Helper.Reflection.GetMethod(gameLocation, "onMonsterKilled").Invoke(f, monster, monster.GetBoundingBox(), false);
                            //gameLocation.onMonsterKilled(who, monster, monsterBox, isBomb);
                        }

                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }
                }
            }

            if (living is Farmer farmer && farmer != caster.entity)
            {
                if (modEntry.ModSaveData.EnablePVP)
                {
                    var api = ModEntry.INSTANCE.arsVeneficiAPILoader.GetAPI();
                    var affinityHelper = api.GetAffinityHelper();

                    if (damage < 0)
                    {
                        damage = helper.GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.HEALING), modifiers, spell, caster, target, index);
                    }

                    damage = helper.GetModifiedStat(damage, new SpellPartStats(SpellPartStatType.DAMAGE), modifiers, spell, caster, target, index);

                    AffinityHelper.AffinityHolder playerAffinityHolder = AffinityHelper.Instance().GetPlayerAffinityHolder(modEntry, farmer);
                    Affinity playerAffinity = Affinities.NONE.Get();

                    foreach (var affinityHolder in Affinities.AFFINITIES.GetObjectList())
                    {
                        if(playerAffinityHolder.Depths().Where(x => x.Value == 1F).First().Key == affinityHolder.Get().id)
                        {
                            playerAffinity = affinityHolder.Get();
                        }
                    }
                        
                    foreach (var damageAffinity in GetAffinities())
                    {
                        if (playerAffinity.directOpposite == damageAffinity.id)
                        {
                            damage *= 3;
                            break;
                        }
                    }

                    if (damageType == ComponentDamageType.Physical)
                    {
                        farmer.takeDamage((int)damage, false, null);
                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }
                    else
                    {
                        farmer.health -= (int)damage;
                        gameLocation.debris.Add(new Debris((int)damage, new Microsoft.Xna.Framework.Vector2(living.GetBoundingBox().Center.X, living.GetBoundingBox().Center.Y), Microsoft.Xna.Framework.Color.Red, 1f, farmer));
                        farmer.playNearbySoundAll("ow");

                        modEntry.characterEvents.InvokeOnCharacterDamage(farmer);

                        SpawnParticles(spell, modEntry, gameLocation, farmer.Position, damageType);

                        OnFarmerDeath(farmer);

                        return new SpellCastResult(SpellCastResultType.SUCCESS);
                    }
                }
                else
                {
                    return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                }
                
            }

            //return living.hurt(damageSourceFunction.apply(caster), damage) ? new SpellCastResult(SpellCastResultType.SUCCESS) : new SpellCastResult(SpellCastResultType.EFFECT_FAILED);

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public void OnFarmerDeath(Farmer farmer)
        {
            if (farmer.health <= 0 && farmer.GetEffectsOfRingMultiplier("863") > 0 && !farmer.hasUsedDailyRevive.Value)
            {
                farmer.startGlowing(new Color(255, 255, 0), border: false, 0.25f);
                DelayedAction.functionAfterDelay(farmer.stopGlowing, 500);
                Game1.playSound("yoba");
                for (int i = 0; i < 13; i++)
                {
                    float xPos = Game1.random.Next(-32, 33);
                    farmer.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors2", new Microsoft.Xna.Framework.Rectangle(114, 46, 2, 2), 200f, 5, 1, new Vector2(xPos + 32f, -96f), flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f)
                    {
                        attachedCharacter = farmer,
                        positionFollowsAttachedCharacter = true,
                        motion = new Vector2(xPos / 32f, -3f),
                        delayBeforeAnimationStart = i * 50,
                        alphaFade = 0.001f,
                        acceleration = new Vector2(0f, 0.1f)
                    });
                }

                farmer.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors2", new Microsoft.Xna.Framework.Rectangle(157, 280, 28, 19), 2000f, 1, 1, new Vector2(-20f, -16f), flicker: false, flipped: false, 1E-06f, 0f, Color.White, 4f, 0f, 0f, 0f)
                {
                    attachedCharacter = farmer,
                    positionFollowsAttachedCharacter = true,
                    alpha = 0.1f,
                    alphaFade = -0.01f,
                    alphaFadeFade = -0.00025f
                });

                farmer.health = (int)Math.Min(farmer.maxHealth, (float)farmer.maxHealth * 0.5f + (float)farmer.GetEffectsOfRingMultiplier("863"));
                farmer.hasUsedDailyRevive.Value = true;
            }
        }

        public void SpawnParticles(ISpell spell, ModEntry modEntry, GameLocation gameLocation, Vector2 monsterPosition, ComponentDamageType type)
        {
            //Game1.Multiplayer.broadcastSprites(gameLocation, SpawnParticle(ModTextures.ELEMENT_PARTICLE_TEXTURE, monsterPosition, MagicHelper.Instance().GetColorForMagicType(spell)));
            Game1.Multiplayer.broadcastSprites(gameLocation, SpawnParticle(ModTextures.ELEMENT_PARTICLE_TEXTURE, monsterPosition, spell.PrimaryAffinity().color));
        }

        public TemporaryAnimatedSprite SpawnParticle(Texture2D texture, Vector2 monsterPosition, Color color)
        {
            //TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 640, 64, 64), 40f, 8, 0, monster.Position, flicker: false, flipped: false);
            //TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 640, 64, 64), 40f, 8, 0, monster.Position, flicker: false, flipped: false);

            Rectangle imageSourceRect = new Rectangle(0, 0, 16, 16);
            float speed = -3f;

            TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite()
            {
                initialParentTileIndex = 0,
                interval = 40f,
                totalNumberOfLoops = 50,
                position = monsterPosition,
                animationLength = 4,
                flicker = false,
                flipped = false,
                texture = texture,
                sourceRect = imageSourceRect,
                sourceRectStartingPos = new Vector2(imageSourceRect.X, imageSourceRect.Y),
                initialPosition = monsterPosition,
                alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
                alpha = 1f,
                scale = 4f,
                shakeIntensity = 3f,
                //motion = new Vector2(0.0f, speed),
                //acceleration = new Vector2(0.0f, 0.0f),
                scaleChange = 0.01f,
                //rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0),
                color = color
            };

            return sprite;
        }

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

        public override float ManaCost()
        {
            return manaCost;
        }
    }
}
