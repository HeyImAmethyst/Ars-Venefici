﻿using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Events;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Heal : AbstractComponent
    {

        ModEntry modEntry;

        public Heal(ModEntry modEntry) : base(new SpellPartStats(SpellPartStatType.HEALING))
        {
            this.modEntry = modEntry;
        }

        public override string GetId()
        {
            return "heal";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            if (target.GetCharacter() is Farmer living) 
            {
                float healing = helper.GetModifiedStat(15, new SpellPartStats(SpellPartStatType.HEALING), modifiers, spell, caster, target, index);

                //if (living.isInvertedHealAndHarm())
                //{
                //    living.hurt(level.damageSources().indirectMagic(caster, caster), healing);
                //}
                //else
                //{
                //    living.heal(healing);
                //}

                //living.health += (int)healing;
                //living.health -= (int)healing;

                living.health = Math.Min(living.maxHealth, living.health + (int)healing);

                level.playSound("healSound");
                Game1.Multiplayer.broadcastSprites(level, new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(0, 256, 64, 64), 40f, 8, 0, living.Position, flicker: false, flipped: false));
                level.debris.Add(new Debris((int)healing, new Vector2(living.GetBoundingBox().Center.X, living.GetBoundingBox().Center.Y), Color.Green, 1f, living));

                //CharacterHeal?.Invoke(this, new CharacterHealEventArgs(living));
                modEntry.characterEvents.InvokeOnCharacterHeal(living);

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 25;
        }
    }
}
