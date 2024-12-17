using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using xTile.Dimensions;
using StardewValley.GameData.Machines;
using ArsVenefici.Framework.Spell.Shape;
using StardewValley.ItemTypeDefinitions;
using Microsoft.Xna.Framework.Graphics;

namespace ArsVenefici.Framework.Spell.Components
{
    public class Forge : AbstractComponent
    {
        ModEntry modEntry;

        public Forge(ModEntry modEntry) : base()
        {
            this.modEntry = modEntry;
        }

        public override string GetId()
        {
            return "forge";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            TilePos tilePos = target.GetTilePos();
            Vector2 tile = tilePos.GetVector();

            MachineData machineData = DataLoader.Machines(Game1.content).GetValueOrDefault("(BC)13");

            if (machineData != null)
            {

                foreach (var outputRule in machineData.OutputRules)
                {
                    //modEntry.Monitor.Log(outputRule.Id, StardewModdingAPI.LogLevel.Info);

                    foreach (var trigger in outputRule.Triggers)
                    {
                        //modEntry.Monitor.Log(trigger.Id, StardewModdingAPI.LogLevel.Info);

                        List<Debris> objectAndResourceDepris = gameLocation.debris.Where(debris => debris.debrisType.Value == Debris.DebrisType.RESOURCE || debris.debrisType.Value == Debris.DebrisType.OBJECT).ToList();

                        for (int i = 0; i < objectAndResourceDepris.Count; i++)
                        {
                            Debris debris = objectAndResourceDepris[i];

                            foreach (var chunk in debris.Chunks)
                            {
                                Vector2 chunkTilePos = Utils.AbsolutePosToTilePos(Utility.clampToTile(chunk.GetVisualPosition()));
                                //Vector2 chunkTilePos = Utils.AbsolutePosToTilePos(chunk.position.Value);
                                //Vector2 chunkTilePos = Utility.clampToTile(chunk.position.Value);
                                Vector2 absolutePos = Utils.TilePosToAbsolutePos(chunkTilePos);

                                for (int x = (int)(chunkTilePos.X - 3); x <= chunkTilePos.X + 3; ++x)
                                {
                                    for (int y = (int)(chunkTilePos.Y - 3); y <= chunkTilePos.Y + 3; ++y)
                                    {
                                        Vector2 pos = new Vector2(x, y);

                                        if (pos == tile)
                                        {
                                            if (trigger.RequiredItemId == debris.itemId.Value)
                                            {
                                                int forgePercentage = 35;
                                                int randomValueBetween0And99 = ModEntry.RandomGen.Next(100);

                                                //if (randomValueBetween0And99 < forgePercentage)
                                                {
                                                    //do
                                                    //{
                                                    //    debris.InitializeItem(debris.itemId.Value);
                                                    //}
                                                    //while (debris.item == null);

                                                    //while(debris.item == null)
                                                    //{
                                                    //    debris.InitializeItem(debris.itemId.Value);
                                                    //}

                                                    //InitializeDebrisItem(debris);

                                                    debris.InitializeItem(debris.itemId.Value);

                                                    if (debris.item != null)
                                                    {
                                                        modEntry.Monitor.Log(debris.item == null ? "null" : "not null", StardewModdingAPI.LogLevel.Info);
                                                        debris.item = ItemRegistry.Create(outputRule.OutputItem[0].ItemId, ModEntry.RandomGen.Next(1, debris.item.Stack));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        //public void InitializeDebrisItem(Debris debris)
        //{
        //    debris.InitializeItem(debris.itemId.Value);

        //    if (debris.item != null) return;

        //    if (debris.item == null)
        //    {
        //        InitializeDebrisItem(debris);
        //    }
        //}

        public override int ManaCost()
        {
            return 5;
        }
    }
}
