using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Buffs;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Tiles;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Grow : AbstractComponent
    {
        public override string GetId()
        {
            return "grow";
        }

        public override HashSet<Affinity> GetAffinities()
        {
            return new HashSet<Affinity> { Affinities.NATURE.Get() };
        }

        public override Dictionary<Affinity, float> GetAffinityShifts()
        {
            return new Dictionary<Affinity, float> { { Affinities.EARTH.Get(), 0.001f } };
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {

            TilePos tilePos = target.GetTilePos();
            Vector2 tile = tilePos.GetVector();

            if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) < 6)
            {
                modEntry.buffs.growSickness.millisecondsDuration = modEntry.ModSaveData.GrowSicknessDurationMillisecondsLessThanLevelSix;
                //Game1.player.applyBuff(growSickNess);
            }
            else if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) >= 6)
            {
                modEntry.buffs.growSickness.millisecondsDuration = modEntry.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix;
                //Game1.player.applyBuff(growSickNess);
            }
            else if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) >= 8)
            {
                modEntry.buffs.growSickness.millisecondsDuration = modEntry.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight;
                //Game1.player.applyBuff(growSickNess);
            }
            else if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) == 10)
            {
                modEntry.buffs.growSickness.millisecondsDuration = modEntry.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen;
                //Game1.player.applyBuff(growSickNess);
            }

            if (modEntry.ModSaveData.EnableGrowSickness && Game1.player.hasBuff("HeyImAmethyst.ArsVenifici_GrowSickness") == false)
            {

                Buff newBuffInstance = new Buff(
                    id: modEntry.buffs.growSickness.id,
                    displayName: modEntry.buffs.growSickness.displayName,
                    iconTexture: Game1.buffsIcons,
                    iconSheetIndex: modEntry.buffs.growSickness.iconSheetIndex, //34
                    //isDebuff: true,
                    duration: modEntry.buffs.growSickness.millisecondsDuration
                );

                //bool? growSicknessDebuff = new bool?(true);

                //Buff newBuffInstance  = new Buff(
                //    modEntry.buffs.growSickness.id,
                //    null,
                //    null,
                //    modEntry.buffs.growSickness.millisecondsDuration,
                //    Game1.buffsIcons,
                //    modEntry.buffs.growSickness.iconSheetIndex,
                //    null,
                //    growSicknessDebuff,
                //    modEntry.buffs.growSickness.displayName,
                //    null
                //);

                Game1.player.applyBuff(newBuffInstance);
            }

            if (modEntry.ModSaveData.EnableGrowCastLimit == false || modEntry.dailyTracker.GetDailyGrowCastCount() <= modEntry.dailyTracker.GetMaxDailyGrowCastCount())
            {
                if (gameLocation.terrainFeatures.TryGetValue(tile, out TerrainFeature terrainFeature))
                {
                    switch (terrainFeature)
                    {
                        case HoeDirt dirt:
                            this.GrowHoeDirt(dirt);
                            break;
                        case Bush bush:
                            if (bush.size.Value == Bush.greenTeaBush)
                            {
                                if (bush.getAge() < Bush.daysToMatureGreenTeaBush)
                                {
                                    bush.datePlanted.Value = (int)(Game1.stats.DaysPlayed - Bush.daysToMatureGreenTeaBush);
                                    bush.dayUpdate(); // update sprite, etc
                                }

                                if (bush.inBloom() && bush.tileSheetOffset.Value == 0)
                                    bush.dayUpdate(); // grow tea leaves
                            }
                            break;
                        case FruitTree fruitTree:
                            if (fruitTree.daysUntilMature.Value > 0)
                            {
                                fruitTree.daysUntilMature.Value = Math.Max(0, fruitTree.daysUntilMature.Value - 7);
                                fruitTree.growthStage.Value = fruitTree.daysUntilMature.Value > 0 ? (fruitTree.daysUntilMature.Value > 7 ? (fruitTree.daysUntilMature.Value > 14 ? (fruitTree.daysUntilMature.Value > 21 ? 0 : 1) : 2) : 3) : 4;
                            }
                            else if (!fruitTree.stump.Value && fruitTree.growthStage.Value == 4 && (fruitTree.IsInSeasonHere() || gameLocation.Name == "Greenhouse"))
                                fruitTree.TryAddFruit();
                            //else
                            //{
                            //    for (int i = 0; i < 3; i++)
                            //    {
                            //        fruitTree.TryAddFruit();
                            //    }
                            //}

                            //if (!fruitTree.stump.Value)
                            //{
                            //    if (fruitTree.growthStage.Value < FruitTree.treeStage)
                            //    {
                            //        fruitTree.growthStage.Value = Tree.treeStage;
                            //        fruitTree.daysUntilMature.Value = 0;
                            //    }

                            //    if (fruitTree.IsInSeasonHere())
                            //        fruitTree.TryAddFruit();
                            //}

                            break;

                        case Tree tree:
                            if (tree.growthStage.Value < 5)
                                tree.growthStage.Value++;
                            break;
                    }

                    return new SpellCastResult(SpellCastResultType.SUCCESS);
                }

                if (gameLocation.objects.TryGetValue(tile, out StardewValley.Object obj))
                {
                    if (obj is IndoorPot pot)
                        this.GrowHoeDirt(pot.hoeDirt.Value);

                    return new SpellCastResult(SpellCastResultType.SUCCESS);
                }
            }
            
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 50;
        }

        private void GrowHoeDirt(HoeDirt dirt)
        {
            if (dirt?.crop is not null)
            {
                Crop crop = dirt.crop;

                //int extraCropGrowth = 25;
                //int secondExtraCropGrowth = 10;

                //int randomValueBetween0And99 = ModEntry.RandomGen.Next(100);

                crop.newDay(HoeDirt.watered);

                //if (randomValueBetween0And99 < extraCropGrowth)
                if (Utils.PercentChance(0.25))
                {
                    crop.newDay(HoeDirt.watered);

                    //if (randomValueBetween0And99 < secondExtraCropGrowth)
                    if (Utils.PercentChance(0.10))
                    {
                        crop.newDay(HoeDirt.watered);
                    }
                }
            }
        }
    }
}
