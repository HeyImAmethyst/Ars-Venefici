using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;
using xTile.Tiles;
using static ArsVenefici.Framework.GUI.DragNDrop.ShapeGroupArea;
using static System.Reflection.Metadata.BlobBuilder;

namespace ArsVenefici.Framework.Spells.Components
{
    public class CreateWater : AbstractComponent
    {
        public override string GetId()
        {
            return "create_water";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            TilePos pos = target.GetTilePos();
            Vector2 tile = pos.GetVector();
            Vector2 toolPixel = (tile * Game1.tileSize) + new Vector2(Game1.tileSize / 2f); // center of tile

            WateringCan wateringCan = new WateringCan();
            wateringCan.UpgradeLevel = 4;
            wateringCan.IsEfficient = true; // don't drain stamina
            wateringCan.IsBottomless = true;
            modEntry.Helper.Reflection.GetField<Farmer>(wateringCan, "lastUser").SetValue(caster.entity as Farmer);

            if (gameLocation.objects.TryGetValue(tile, out StardewValley.Object obj))
            {
                ((Farmer)caster.entity).lastClick = toolPixel;
                wateringCan.DoFunction(gameLocation, (int)toolPixel.X, (int)toolPixel.Y, 0, ((Farmer)caster.entity));

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }
            else if (gameLocation.terrainFeatures.TryGetValue(tile, out TerrainFeature feature))
            {
                //feature.performToolAction(wateringCan, 0, tile);

                ((Farmer)caster.entity).lastClick = toolPixel;
                wateringCan.DoFunction(gameLocation, (int)toolPixel.X, (int)toolPixel.Y, 0, ((Farmer)caster.entity));

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }
            else if(gameLocation.performToolAction(wateringCan, (int)tile.X, (int)tile.Y))
            {
                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }
            else
            {
                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }
        }

        public override int ManaCost()
        {
            return 5;
        }
    }
}
