﻿using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArsVenefici.Framework.GUI.DragNDrop.SpellPartDraggable;
using xTile.Dimensions;
using StardewValley.Locations;
using ArsVenefici.Framework.API.Spell;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Light : AbstractComponent
    {
        private readonly Func<long> GetNewId;

        public Light(Func<long> getNewId) : base()
        {
            this.GetNewId = getNewId;
        }

        public override string GetId()
        {
            return "light";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {

            Farmer player = ((Farmer)caster.entity);

            TilePos tile = new TilePos(Utils.AbsolutePosToTilePos(Utility.clampToTile(target.GetCharacter().getStandingPosition())));

            Torch torch = new Torch();

            if(!gameLocation.objects.ContainsKey(tile.GetVector()))
            {
                gameLocation.objects.Add(tile.GetVector(), torch);
                torch.initializeLightSource(tile.GetVector());

                torch.Fragility = 1;

                if (player != null)
                {
                    Game1.playSound("woodyStep");
                }

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {

            Farmer player = ((Farmer)caster.entity);
            
            TilePos tile = target.GetTilePos();

            Torch torch = new Torch();

            if (!gameLocation.objects.ContainsKey(tile.GetVector()))
            {
                gameLocation.objects.Add(tile.GetVector(), torch);
                torch.initializeLightSource(tile.GetVector());

                torch.Fragility = 1;

                if (player != null)
                {
                    Game1.playSound("woodyStep");
                }

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        private int GetUnusedLightSourceId(GameLocation location)
        {
            while (true)
            {
                int id = (int)this.GetNewId();
                if (!location.hasLightSource(id.ToString()))
                    return id;
            }
        }

        public override float ManaCost()
        {
            return 7;
        }
    }
}
