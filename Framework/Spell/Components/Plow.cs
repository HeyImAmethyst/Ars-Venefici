using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;
using xTile.Tiles;
using static ArsVenefici.Framework.GUI.DragNDrop.SpellPartDraggable;

namespace ArsVenefici.Framework.Spell.Components
{
    public class Plow : AbstractComponent
    {
        ModEntry modEntry;

        public Plow(ModEntry modEntry) : base()
        {
            this.modEntry = modEntry;
        }

        public override string GetId()
        {
            return "plow";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            TilePos tilePos = target.GetTilePos();

            // create fake tools
            Hoe hoe = new();

            foreach (var t in new Tool[] { hoe })
            {
                t.UpgradeLevel = 3;
                t.IsEfficient = true; // don't drain stamina
                modEntry.Helper.Reflection.GetField<Farmer>(t, "lastUser").SetValue(caster.entity as Farmer);
            }

            Vector2 tilePosVector = tilePos.GetVector();
            Vector2 toolPixel = (tilePosVector * Game1.tileSize) + new Vector2(Game1.tileSize / 2f); // center of tile

            // skip if blocked

            if (gameLocation.terrainFeatures.ContainsKey(tilePosVector))
            {
                if (gameLocation.terrainFeatures.TryGetValue(tilePosVector, out var terrainFeature))
                {
                    if (terrainFeature.performToolAction(hoe, 0, tilePosVector))
                    {
                        gameLocation.terrainFeatures.Remove(tilePosVector);
                    }
                }
            }

            // handle artifact spot, else skip if blocked
            if (gameLocation.objects.TryGetValue(tilePosVector, out StardewValley.Object obj))
            {
                if (obj.ParentSheetIndex == 590)
                {
                    gameLocation.digUpArtifactSpot(tilePos.GetTilePosX(), tilePos.GetTilePosY(), caster.entity as Farmer);
                    gameLocation.objects.Remove(tilePosVector);
                    return new SpellCastResult(SpellCastResultType.SUCCESS);
                }
                else if(obj.QualifiedItemId == "(O)SeedSpot")
                {
                    obj.performToolAction(hoe);
                }
                else
                    return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
            }

            // till dirt
            //if (gameLocation.doesTileHaveProperty(blockPos.GetTilePosX(), blockPos.GetTilePosY(), "Diggable", "Back") != null && !gameLocation.IsTileOccupiedBy(tile))
            if (gameLocation.doesTileHaveProperty(tilePos.GetTilePosX(), tilePos.GetTilePosY(), "Diggable", "Back") != null)
            {
                //gameLocation.makeHoeDirt(tile);
                //gameLocation.playSound("hoeHit", tile);

                // select tool
                Tool tool = hoe;

                if (gameLocation.terrainFeatures.TryGetValue(tilePosVector, out TerrainFeature terrainFeature))
                {
                    switch (terrainFeature)
                    {
                        case HoeDirt dirt:
                            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                    }
                }

                ((Farmer)caster.entity).lastClick = toolPixel;
                tool.DoFunction(gameLocation, (int)toolPixel.X, (int)toolPixel.Y, 0, ((Farmer)caster.entity));

                //Game1.removeSquareDebrisFromTile(tileX, tileY);
                //Game1.removeDebris(tileX, tileY);

                gameLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(tilePos.GetTilePosX() * (float)Game1.tileSize, tilePos.GetTilePosY() * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f));
                gameLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(tilePos.GetTilePosX() * (float)Game1.tileSize, tilePos.GetTilePosY() * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(tilePosVector, tilePos.GetVector()) * 30f));
                gameLocation.checkForBuriedItem(tilePos.GetTilePosX(), tilePos.GetTilePosY(), false, false, caster.entity as Farmer);

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 5;
        }
    }
}
