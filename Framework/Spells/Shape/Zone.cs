﻿using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Effects;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Zone : AbstractShape
    {

        public Zone() : base(new SpellPartStats(SpellPartStatType.DURATION), new SpellPartStats(SpellPartStatType.RANGE))
        {

        }

        public override string GetId()
        {
            return "zone";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            Vector2 position = Vector2.One; 

            if (hit != null)
            {
                //position = Utils.ConvertToTilePos(Utility.clampToTile(hit.GetLocation()));
                position = hit.GetLocation();
            }
            else
            {
                position = Utils.AbsolutePosToTilePos(Utility.clampToTile(caster.GetPosition()));
            }

            float radius = helper.GetModifiedStat(1, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index);
            int duration = (int)(200 + helper.GetModifiedStat(100, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, hit, index));

            ZoneEffect zoneEffect = new ZoneEffect(modEntry, spell, position, radius, duration);
            zoneEffect.SetIndex(index);
            zoneEffect.SetOwner(caster);

            Vector2 tilePos = new Vector2(zoneEffect.GetPosition().X - radius, zoneEffect.GetPosition().Y - radius);
            Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);

            int boundingBoxRadius = 3;

            switch ((int)radius)
            {
                case 1:
                    boundingBoxRadius = 3;
                    break;
                case 2:
                    boundingBoxRadius = 5;
                    break;
                case 3:
                    boundingBoxRadius = 7;
                    break;
                default: 
                    boundingBoxRadius = 3; 
                    break;

            }

            boundingBoxRadius *= Game1.tileSize;

            zoneEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), boundingBoxRadius, boundingBoxRadius));

            modEntry.ActiveEffects.Add(zoneEffect);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override float ManaCost()
        {
            return 2.5f;
        }

        public override bool IsEndShape()
        {
            return true;
        }

        public override bool NeedsPrecedingShape()
        {
            return true;
        }
    }
}
