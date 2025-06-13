using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Magic;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework
{
    public class ArsVeneficiAPIImpl : ArsVeneficiAPI
    {
        //private static ArsVeneficiAPIImpl INSTANCE = new ArsVeneficiAPIImpl();

        ///// <returns>The API instance.</returns>
        //public static ArsVeneficiAPI Get()
        //{
        //    return INSTANCE;
        //}

        public ISpellPartSkillHelper GetSpellPartSkillHelper()
        {
            return SpellPartSkillHelper.Instance();
        }

        public IMagicHelper GetMagicHelper()
        {
            return MagicHelper.Instance();
        }

        public ISpellHelper GetSpellHelper()
        {
            return SpellHelper.Instance();
        }

        public IContingencyHelper GetContingencyHelper()
        {
            return ContingencyHelper.Instance();
        }

        public void OpenMagicAltarGui(Farmer farmer)
        {
            Vector2 toolLocationTile = Utils.AbsolutePosToTilePos(Utility.clampToTile(Game1.player.GetToolLocation(true)));
            Vector2 toolPixel = toolLocationTile * Game1.tileSize + new Vector2(Game1.tileSize / 2f); // center of tile

            if (Game1.player.currentLocation.objects.TryGetValue(toolLocationTile, out StardewValley.Object obj))
            {
                Game1.player.lastClick = toolPixel;

                //string s = ModEntry.JsonAssetsApi.GetBigCraftableId("Magic Altar");
                string s = $"(BC){ModEntry.ArsVenificiContentPatcherId}_Magic_Altar";

                //if (obj.QualifiedItemId.Equals("(O){{spacechase0.JsonAssets/BigCraftable: Magic Altar}}"))

                //craftingAltarTokenString.UpdateContext();
                //string value = craftingAltarTokenString.Value;

                if (obj.QualifiedItemId.Equals(s))
                {
                    obj.readyForHarvest.Value = true;

                    if (obj.checkForAction(Game1.player, true))
                    {
                        Game1.activeClickableMenu = new MagicAltarMenu(ModEntry.INSTANCE);
                    }
                }
            }
        }

        public void OpenSpellBookGui(ModEntry modEntry)
        {
            Game1.activeClickableMenu = new SpellBookMenu(modEntry);
        }

        public ISpell MakeSpell(List<ShapeGroup> shapeGroups, SpellStack spellStack)
        {
            return new Spell(ModEntry.INSTANCE, shapeGroups, spellStack);
        }

        public ISpell MakeSpell(SpellStack spellStack, params ShapeGroup[] shapeGroups)
        {
            return Spell.of(ModEntry.INSTANCE, spellStack, shapeGroups);
        }
    }
}
