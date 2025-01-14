using ArsVenefici.Framework.GUI;
using ArsVenefici.Framework.Interfaces.Magic;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spell.Components;
using ArsVenefici.Framework.Spell.Modifiers;
using ArsVenefici.Framework.Spell.Shape;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effect = ArsVenefici.Framework.Spell.Components.Effect;
using Rune = ArsVenefici.Framework.Spell.Shape.Rune;

namespace ArsVenefici.Framework.Spell
{
    public class SpellPartManager
    {
        public Dictionary<string, ISpellPart> spellParts = new Dictionary<string, ISpellPart>();
        private ModEntry modEntry;

        public SpellPartManager(ModEntry modEntry) 
        {
            this.modEntry = modEntry;
            PopluateDictionary();
        }

        public void PopluateDictionary()
        {
            AddShapes();
            AddComonents();
            AddModifiers();
        }

        private void AddShapes()
        {
            Self self = new Self();
            Projectile projectile = new Projectile();
            Touch touch = new Touch();
            EtherialTouch etherialTouch = new EtherialTouch();
            AoE aoE = new AoE();
            Zone zone = new Zone();
            Wave wave = new Wave();
            //Beam beam = new Beam();
            Wall wall = new Wall();
            Rune rune = new Rune();
            Channel channel = new Channel();
            Contingency contingency_health = new Contingency("contingency_health", ContingencyType.HEALTH);
            Contingency contingency_damage = new Contingency("contingency_damage", ContingencyType.DAMAGE);

            spellParts.Add(self.GetId(), self);
            spellParts.Add(projectile.GetId(), projectile);
            spellParts.Add(touch.GetId(), touch);
            spellParts.Add(etherialTouch.GetId(), etherialTouch);
            spellParts.Add(aoE.GetId(), aoE);
            spellParts.Add(zone.GetId(), zone);
            spellParts.Add(wave.GetId(), wave);
            //spellParts.Add(beam.GetId(), beam);
            spellParts.Add(wall.GetId(), wall);
            spellParts.Add(rune.GetId(), rune);
            spellParts.Add(channel.GetId(), channel);
            spellParts.Add(contingency_health.GetId(), contingency_health);
            spellParts.Add(contingency_damage.GetId(), contingency_damage);
        }

        private void AddComonents()
        {
            Dig dig = new Dig(modEntry);
            Plow plow = new Plow(modEntry);
            Grow grow = new Grow();
            Harvest harvest = new Harvest();
            CreateWater createWater = new CreateWater();
            Explosion explosion = new Explosion();
            Blink blink = new Blink();
            Light light = new Light(modEntry.Helper.Multiplayer.GetNewID);

            Heal heal = new Heal(modEntry);
            LifeDrain lifeDrain = new LifeDrain();
            LifeTap lifeTap = new LifeTap();

            // -> e instanceof Player p ? p.damageSources().playerAttack(p) : e.damageSources().mobAttack(e), Config.SERVER.DAMAGE)
            //Func<double> physicalDamageValue = () => 5.0;
            
            Func<double> physicalDamageValue = () => 8.0 * (Game1.player.CombatLevel + 1);
            Damage physicalDamage = new Damage("physical_damage", 25, ComponentDamageType.Physical, physicalDamageValue);

            Func<double> magicDamageValue = () => 8.0 * (Game1.player.CombatLevel + 1);
            Damage magicDamage = new Damage("magic_damage", 25, ComponentDamageType.Magic, magicDamageValue);

            Func<double> frostDamageValue = () => 8.0 * (Game1.player.CombatLevel + 1);
            Damage frostDamage = new Damage("frost_damage", 25, ComponentDamageType.Frost, frostDamageValue);

            Func<double> lightningDamageValue = () => 20.0 * (Game1.player.CombatLevel + 1);
            Damage lightningDamage = new Damage("lightning_damage", 55, ComponentDamageType.Lightning, lightningDamageValue);

            Func<double> fireDamageValue = () => 10.0 * (Game1.player.CombatLevel + 1);
            Damage fireDamage = new Damage("fire_damage", 25, ComponentDamageType.Fire, fireDamageValue);

            Effect haste = new Effect("haste", 30, modEntry.buffs.hasteBuff);
            Effect regeneration = new Effect("regeneration", 30, modEntry.buffs.regenerationBuff);
            Effect mana_regeneration = new Effect("mana_regeneration", 30, modEntry.buffs.manaRegenerationBuff);

            Dispel dispel = new Dispel();

            Forge forge = new Forge(modEntry);

            Shield shield = new Shield();
            Summon summon = new Summon();

            spellParts.Add(createWater.GetId(), createWater);
            spellParts.Add(heal.GetId(), heal);
            spellParts.Add(physicalDamage.GetId(), physicalDamage);
            spellParts.Add(magicDamage.GetId(), magicDamage);
            spellParts.Add(frostDamage.GetId(), frostDamage);
            spellParts.Add(lightningDamage.GetId(), lightningDamage);
            spellParts.Add(fireDamage.GetId(), fireDamage);
            spellParts.Add(dig.GetId(), dig);
            spellParts.Add(plow.GetId(), plow);
            spellParts.Add(grow.GetId(), grow);
            spellParts.Add(harvest.GetId(), harvest);
            spellParts.Add(lifeDrain.GetId(), lifeDrain);
            spellParts.Add(lifeTap.GetId(), lifeTap);
            spellParts.Add(explosion.GetId(), explosion);
            spellParts.Add(blink.GetId(), blink);
            spellParts.Add(light.GetId(), light);
            spellParts.Add(haste.GetId(), haste);
            spellParts.Add(regeneration.GetId(), regeneration);
            spellParts.Add(dispel.GetId(), dispel);
            spellParts.Add(forge.GetId(), forge);
            spellParts.Add(shield.GetId(), shield);
            spellParts.Add(summon.GetId(), summon);
            spellParts.Add(mana_regeneration.GetId(), mana_regeneration);
        }

        private void AddModifiers()
        {
            GenericSpellModifier damage = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.DAMAGE), DefaultSpellPartStatModifier.Add(5f));
            //GenericSpellModifier damage = new GenericSpellModifier().addStatModifier(new SpellPartStats(SpellPartStatType.DAMAGE), DefaultSpellPartStatModifier.add(50f));
            damage.SetId("damage");

            //GenericSpellModifier range = new GenericSpellModifier().addStatModifier(new SpellPartStats(SpellPartStatType.RANGE), DefaultSpellPartStatModifier.multiply(4f));
            GenericSpellModifier range = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.RANGE), DefaultSpellPartStatModifier.Add(1f));
            range.SetId("range");

            GenericSpellModifier bounce = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.BOUNCE), DefaultSpellPartStatModifier.Add(2f));
            bounce.SetId("bounce");

            GenericSpellModifier piercing = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.PIERCING), DefaultSpellPartStatModifier.COUNTING);
            piercing.SetId("piercing");

            GenericSpellModifier velocity = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.SPEED), DefaultSpellPartStatModifier.AddMultipliedBase(0.5f));
            velocity.SetId("velocity");

            GenericSpellModifier healing = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.HEALING), DefaultSpellPartStatModifier.Multiply(2f));
            healing.SetId("healing");

            GenericSpellModifier duration = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.DURATION), DefaultSpellPartStatModifier.Multiply(2f));
            duration.SetId("duration");

            GenericSpellModifier miningPower = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.MINING_TIER), DefaultSpellPartStatModifier.Add(1f));
            miningPower.SetId("mining_power");

            GenericSpellModifier effectPower = new GenericSpellModifier(1).AddStatModifier(new SpellPartStats(SpellPartStatType.POWER), DefaultSpellPartStatModifier.COUNTING);
            effectPower.SetId("effect_power");

            spellParts.Add(damage.GetId(), damage);
            spellParts.Add(range.GetId(), range);
            spellParts.Add(bounce.GetId(), bounce);
            spellParts.Add(piercing.GetId(), piercing);
            spellParts.Add(velocity.GetId(), velocity);
            spellParts.Add(healing.GetId(), healing);
            spellParts.Add(duration.GetId(), duration);
            spellParts.Add(miningPower.GetId(), miningPower);
            spellParts.Add(effectPower.GetId(), effectPower);
        }
    }
}
