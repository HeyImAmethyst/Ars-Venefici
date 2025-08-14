using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ArsVenefici
{
    public class DailyTracker
    {
        private int dailyGrowCastCount = 0;
        private int maxDailyGrowCastCount = 2;

        public void Update(ModEntry modEntry, UpdateTickedEventArgs e, GameLocation gameLocation)
        {

            if (gameLocation != null && Game1.activeClickableMenu == null && Game1.game1.IsActive)
            {
                if (Game1.shouldTimePass())
                {
                    var ext = Game1.player.GetExtData();

                    bool regenMana = false;

                    if (gameLocation.NameOrUniqueName.Equals("SkullCave"))
                    {
                        regenMana = e.IsMultipleOf(540);
                    }
                    else if (Game1.player.hasBuff("HeyImAmethyst.ArsVenifici_ManaRegeneration") == true)
                    {
                        regenMana = e.IsMultipleOf(8);
                    }
                    else
                    {
                        regenMana = e.IsMultipleOf(420);
                    }

                    if (Game1.player.hasBuff("HeyImAmethyst.ArsVenifici_HealthRegeneration") == true)
                    {
                        if (Game1.player.buffs.AppliedBuffs.TryGetValue("HeyImAmethyst.ArsVenifici_HealthRegeneration", out Buff buff))
                        {
                            //if (ext.StaminaRegen > 0)
                            //{
                            //    ext.staminaBuffer += ext.StaminaRegen * (float)Game1.currentGameTime.ElapsedGameTime.TotalSeconds;
                            //    if (ext.staminaBuffer >= 1)
                            //    {
                            //        int whole = (int)Math.Truncate(ext.staminaBuffer);
                            //        ext.staminaBuffer -= whole;
                            //        Game1.player.Stamina += whole;
                            //    }
                            //}


                            ext.HealthRegen = 2f;

                            if (buff.customFields != null || buff.customFields.Count > 0)
                            {
                                if (buff.customFields.TryGetValue($"{ModEntry.ArsVenificiModId}/EffectPower", out string effectPowerValue))
                                {
                                    if (int.TryParse(effectPowerValue, out int value))
                                    {
                                        ext.HealthRegen *= value;
                                    }
                                }
                            }

                            if (ext.HealthRegen != 0)
                            {
                                ext.healthBuffer += ext.HealthRegen * (float)Game1.currentGameTime.ElapsedGameTime.TotalSeconds;

                                if (Math.Abs(ext.healthBuffer) >= 1)
                                {
                                    int whole = (int)Math.Truncate(ext.healthBuffer);
                                    ext.healthBuffer -= whole;
                                    Game1.player.health = Math.Min(Game1.player.health + whole, Game1.player.maxHealth);
                                }
                            }
                        }
                    }

                    if (regenMana)
                    {
                        //float factor = (Game1.player.GetCustomSkillLevel(ModEntry.Skill) + 1) / 2; // start at +1 mana at level 1

                        //if (Game1.player.HasCustomProfession(Skill.ManaRegen2Profession))
                        //    factor *= 3;
                        //else if (Game1.player.HasCustomProfession(Skill.ManaRegen1Profession))
                        //    factor *= 2;

                        //int manaRegenValue = (int)(2 * factor);

                        int levelAmount = Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill);

                        if (Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaRegen1Profession))
                            levelAmount *= 2;
                        else if (Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaRegen2Profession))
                            levelAmount *= 4;

                        int manaRegenValue = 0;

                        if (Game1.player.hasBuff("HeyImAmethyst.ArsVenifici_ManaRegeneration") == true)
                        {
                            if (Game1.player.buffs.AppliedBuffs.TryGetValue("HeyImAmethyst.ArsVenifici_ManaRegeneration", out Buff buff))
                            {
                                ext.ManaRegen = 0.5f;

                                if (buff.customFields != null || buff.customFields.Count > 0)
                                {
                                    if (buff.customFields.TryGetValue($"{ModEntry.ArsVenificiModId}/EffectPower", out string effectPowerValue))
                                    {
                                        if (int.TryParse(effectPowerValue, out int value))
                                        {
                                            ext.ManaRegen *= value;
                                        }
                                    }
                                }

                                if (ext.ManaRegen != 0)
                                {
                                    //ext.manaBuffer += ext.ManaRegen * (float)Game1.currentGameTime.ElapsedGameTime.TotalSeconds;

                                    //if (Math.Abs(ext.manaBuffer) >= 1)
                                    //{
                                    //    int whole = (int)Math.Truncate(ext.manaBuffer);
                                    //    ext.manaBuffer -= whole;

                                    //    manaRegenValue = Math.Min(Game1.player.GetCurrentMana() + whole, Game1.player.GetMaxMana());
                                    //}

                                    manaRegenValue = (int)(ext.ManaRegen * levelAmount);
                                }
                            }
                        }
                        else
                        {
                            manaRegenValue = modEntry.ModSaveData.ManaRegenRate * levelAmount;
                        }

                        Game1.player.AddMana(manaRegenValue);
                    }
                }
            }
        }

        public void SetDailyGrowCastCount(int dailyGrowCastCount)
        {
            this.dailyGrowCastCount = dailyGrowCastCount;
        }

        public int GetDailyGrowCastCount()
        {
            return dailyGrowCastCount;
        }

        public void SetMaxDailyGrowCastCount(int max)
        {
            maxDailyGrowCastCount = max;
        }

        public int GetMaxDailyGrowCastCount()
        {
            return maxDailyGrowCastCount;
        }
    }
}
