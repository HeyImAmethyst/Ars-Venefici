using ArsVenefici.Framework.Skill;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceCore;
using StardewValley;
using StardewValley.Menus;
using System;
using static SpaceCore.Skills;
using System.Collections.Generic;
using ArsVenefici.Framework.Util;
using StardewValley.Locations;
using System.Text;
using ArsVenefici.Framework.Spells.Components;
using static StardewValley.Minigames.CraneGame;
using System.Runtime.Intrinsics;
using Netcode;
using StardewValley.ItemTypeDefinitions;
using static System.Net.Mime.MediaTypeNames;
using StardewValley.BellsAndWhistles;
using StardewValley.Buffs;
using StardewValley.Enchantments;
using StardewValley.Objects;
using StardewValley.Tools;
using Object = StardewValley.Object;
using ArsVenefici.Framework.GUI.SkillTree;
using StardewValley.TerrainFeatures;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using ItemExtensions;
using ArsVenefici.Framework.API.Client;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.GUI.Menus
{
    public class MagicAltarSkillTreeTabRenderer : MagicAltarTabRenderer
    {
        int SKILL_MARGIN_X = 50;
        int SKILL_MARGIN_Y = 40;
        private float SKILL_SIZE = 32f;

        private float SCALE = 1f;

        private int lastMouseX = 0;
        private int lastMouseY = 0;
        private float offsetX = 0;
        private float offsetY = 0;

        private int virtualWidth;
        private int virtualHeight;

        private SpellPartSkill hoverItem = null;
        bool isHoveringSkill = false;

        HashSet<SpellPartSkill> skills;
        private TreeNodeModel<SpellPartSkill> skillTree;

        Matrix transformMatrix;

        StringBuilder learnedDescription = new StringBuilder();
        StringBuilder HoverTextStringBuilder = new StringBuilder();

        public MagicAltarSkillTreeTabRenderer(MagicAltarTab magicAltarTab, MagicAltarMenu parent) : base(magicAltarTab, parent)
        {
            //Init(magicAltarTab.GetWidth() / 2, magicAltarTab.GetHeight() / 2, parent.width, parent.height, parent.xPositionOnScreen, parent.yPositionOnScreen);
            Init((magicAltarTab.GetWidth() / 2), (magicAltarTab.GetHeight() / 2), parent.width, parent.height, parent.xPositionOnScreen - 150, parent.yPositionOnScreen - 65);
        }

        protected override void Init()
        {
            SCALE = 1f;

            SKILL_SIZE = 64 * SCALE;

            offsetX = (magicAltarTab.GetStartX() - width / 2f) + 350;

            if (magicAltarTab.GetName() == parent.modEntry.Helper.Translation.Get("ui.magic_altar.offense_tab.name"))
            {
                offsetX = (magicAltarTab.GetStartX() - width / 2f) + 300;
            }

            if (offsetX < 0)
                offsetX = 0;

            if (offsetX > textureWidth - width)
                offsetX = textureWidth - width;

            offsetY = (magicAltarTab.GetStartY() - width / 2f) + 45;

            if (offsetY < 0)
                offsetY = 0;

            if (offsetY > textureHeight - height)
                offsetY = textureHeight - height;

            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            skills = modEntry.spellPartSkillManager.GetSpellPartSkills().Values.Where(skill => skill.GetOcculusTab().Equals(magicAltarTab)).ToHashSet();
            skills.RemoveWhere(skill => skill.IsHidden() && !helper.Knows(modEntry, player, skill));

            skillTree = GetSampleTree(skills);
            TreeHelpers<SpellPartSkill>.CalculateNodePositions(skillTree);
        }

        public void ResetOffset()
        {
            offsetX = (magicAltarTab.GetStartX() - width / 2f);

            if(magicAltarTab.GetName() == parent.modEntry.Helper.Translation.Get("ui.magic_altar.offense_tab.name"))
            {
                offsetX = (magicAltarTab.GetStartX() - width / 2f) + 300;
            }

            if (offsetX < 0)
                offsetX = 0;

            if (offsetX > textureWidth - width)
                offsetX = textureWidth - width;

            offsetY = (magicAltarTab.GetStartY() - width / 2f) + 45;

            if (offsetY < 0)
                offsetY = 0;

            if (offsetY > textureHeight - height)
                offsetY = textureHeight - height;
        }

        // converts list of sample items to hierarchial list of TreeNodeModels
        private TreeNodeModel<SpellPartSkill> GetSampleTree(HashSet<SpellPartSkill> data)
        {
            var root = data.FirstOrDefault(p => p.Parents().Count == 0);
            var rootTreeNode = new TreeNodeModel<SpellPartSkill>(root, null);

            // add tree node children recursively
            rootTreeNode.Children = GetChildNodes(data, rootTreeNode);

            return rootTreeNode;
        }

        private static List<TreeNodeModel<SpellPartSkill>> GetChildNodes(HashSet<SpellPartSkill> data, TreeNodeModel<SpellPartSkill> parent)
        {
            var nodes = new List<TreeNodeModel<SpellPartSkill>>();

            //foreach (var item in data.Where(p => p.ParentId == parent.Item.Id))
            foreach (var item in data.Where(p => p.Parents().Contains(parent.Item)))
            {
                var treeNode = new TreeNodeModel<SpellPartSkill>(item, parent);
                treeNode.Children = GetChildNodes(data, treeNode);
                nodes.Add(treeNode);
            }

            return nodes;
        }

        protected override void RenderBg(SpriteBatch spriteBatch, int mouseX, int mouseY, float partialTicks)
        {
            float scaledOffsetX = offsetX * SCALE;
            float scaledOffsetY = offsetY * SCALE;

            float scaledWidth = width * (1 / SCALE);
            float scaledHeight = height * (1 / SCALE);

            float minU = Math.Clamp(scaledOffsetX, 0, textureWidth - scaledWidth) / textureWidth;
            float minV = Math.Clamp(scaledOffsetY, 0, textureHeight - scaledHeight) / textureHeight;
            float maxU = Math.Clamp(scaledOffsetX + scaledWidth, scaledWidth, textureWidth) / textureWidth;
            float maxV = Math.Clamp(scaledOffsetY + scaledHeight, scaledHeight, textureHeight) / textureHeight;

            if (parent.getDragging())
            {
                offsetX = Math.Clamp(offsetX - (mouseX - lastMouseX), 0, textureWidth - scaledWidth);
                offsetY = Math.Clamp(offsetY - (mouseY - lastMouseY), 0, textureHeight - scaledHeight);
            }

            lastMouseX = mouseX;
            lastMouseY = mouseY;

            Rectangle sourceRect = new Rectangle((int)offsetX, (int)offsetY, (int)scaledWidth, (int)scaledHeight);

            IClickableMenu.drawTextureBox(spriteBatch, bounds.X - 10, bounds.Y - 10, bounds.Width + 20, bounds.Height + 20, Color.White);

            spriteBatch.Draw(magicAltarTab.GetBackground(), bounds, sourceRect, Color.White);
        }

        protected override void RenderFg(SpriteBatch spriteBatch, int mouseX, int mouseY, float partialTicks)
        {
            ModEntry modEntry = parent.modEntry;
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();
            Farmer player = Game1.player;

            if (player == null)
                return;

            spriteBatch.End();

            //Create Scissor Region

            if(width / Game1.options.preferredResolutionX > height / Game1.options.preferredResolutionY)
            {
                float aspect = height / Game1.options.preferredResolutionY;

                virtualWidth = (int)(aspect * Game1.options.preferredResolutionX);
                virtualHeight = (int)height;
            }
            else
            {
                float aspect = width / Game1.options.preferredResolutionX;


                virtualWidth =width;
                virtualHeight = (int)(aspect * Game1.options.preferredResolutionY);
            }

            transformMatrix = Matrix.CreateTranslation(-offsetX, -offsetY, 0);
            //transformMatrix = Matrix.CreateTranslation(bounds.X - 10, bounds.Y - 10, 0);
            //transformMatrix = Matrix.CreateScale(SCALE);
            //transformMatrix = Matrix.CreateTranslation(posX - offsetX, posY - offsetY, 0);
            //transformMatrix = Matrix.CreateTranslation(-offsetX, -offsetY, 0) * Matrix.CreateScale(SCALE, SCALE, 0);
            //transformMatrix = Matrix.CreateScale(virtualWidth / Game1.options.preferredResolutionX) * Matrix.CreateTranslation(-offsetX, -offsetY, 0);
            //transformMatrix = Matrix.CreateTranslation(-offsetX * SCALE, (float)Math.Floor(-offsetY * SCALE), 0) * Matrix.CreateScale(SCALE, SCALE, 0);

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.AlphaBlend,null, null, rasterizerState: new RasterizerState { ScissorTestEnable = true }, null, transformMatrix);

            Rectangle clippingRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = bounds;

            DrawNode(modEntry, player, skillTree, helper, spriteBatch);

            spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;

            spriteBatch.End();

            //End Scissor Region

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            if (hoverItem != null)
            {
                DrawHoverToolTip(modEntry, player, helper, spriteBatch);
            }

            if (!isHoveringSkill)
            {
                hoverItem = null;
            }
        }

        // Tree rendering code adapted from https://rachel53461.wordpress.com/2014/04/20/algorithm-for-drawing-trees/
        #region Tree rendering

        private void DrawNode(ModEntry modEntry, Farmer player, TreeNodeModel<SpellPartSkill> node, ISpellPartSkillHelper helper, SpriteBatch spriteBatch)
        {

            bool knows = helper.Knows(modEntry, player, node.Item);
            bool hasPrereq = helper.CanLearn(modEntry, player, node.Item) || knows;

            // rectangle where node will be positioned
            var nodeRect = new Rectangle
                (
                    (int)(bounds.X + SKILL_MARGIN_X + (node.X * (SKILL_SIZE + SKILL_MARGIN_X))),
                    (int)(bounds.Y + SKILL_MARGIN_Y + (node.Y * (SKILL_SIZE + SKILL_MARGIN_Y))),
                    (int)SKILL_SIZE, (int)SKILL_SIZE
                );

            //var nodeRect = new Rectangle
            //(
            //    (int)(parent.xPositionOnScreen + SKILL_MARGIN_X + (node.X * (SKILL_SIZE + SKILL_MARGIN_X))),
            //    (int)(parent.yPositionOnScreen + SKILL_MARGIN_Y + (node.Y * (SKILL_SIZE + SKILL_MARGIN_Y))),
            //    (int)SKILL_SIZE, (int)SKILL_SIZE
            //);

            //Draw lines

            DrawNodeLines(modEntry, nodeRect, hasPrereq, knows, node, spriteBatch);

            //Draw content

            DrawNodeIcon(modEntry, nodeRect, hasPrereq, knows, node, spriteBatch);

            //Draw children

            foreach (var item in node.Children)
            {
                DrawNode(modEntry, player, item, helper, spriteBatch);
            }
        }

        private void DrawNodeIcon(ModEntry modEntry, Rectangle nodeRect, bool hasPrereq, bool knows, TreeNodeModel<SpellPartSkill> node, SpriteBatch spriteBatch)
        {
            //g.DrawString(node.ToString(), this.Font, Brushes.Black, nodeRect.X + 10, nodeRect.Y + 10);

            //Vector2 skillPos = new Vector2(bounds.X + nodeRect.X + 10, bounds.Y + nodeRect.Y + 10);
            Vector2 skillPos = new Vector2(nodeRect.X, nodeRect.Y);

            Color color = Color.Blue;

            if (!hasPrereq)
            {
                //color = new Color(125, 125, 125);
                //color = new Color(75, 75, 75);
                //color = new Color(Color.BlueViolet.PackedValue * Color.DarkGray.PackedValue);
                //color = new Color(73, 58, 87);
                //color = new Color(51, 59, 82);
                color = new Color(141, 149, 161);
                //color = Color.Black;
            }
            else if (!knows)
            {
                //color = Color.CornflowerBlue;
                color = new Color(59, 247, 231);
            }
            else
            {
                color = Color.White;
            }

            Texture2D skillTexture = modEntry.spellPartIconManager.GetSprite(node.Item.GetId());

            //spriteBatch.Draw(skillTexture, new Rectangle((int)skillPos.X, (int)skillPos.Y, (int)SKILL_SIZE, (int)SKILL_SIZE), color);
            spriteBatch.Draw(skillTexture, nodeRect, color);
        }

        private void DrawNodeLines(ModEntry modEntry, Rectangle nodeRect, bool hasPrereq, bool knows, TreeNodeModel<SpellPartSkill> node, SpriteBatch spriteBatch)
        {
            uint uColor;
            int offset;

            Color KNOWS_COLOR = Color.Blue;
            //Color UNKNOWN_SKILL_LINE_COLOR_MASK = new Color(KNOWS_COLOR.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue);
            Color UNKNOWN_SKILL_LINE_COLOR_MASK = new Color(KNOWS_COLOR.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.DarkGray.PackedValue * Color.Gray.PackedValue);

            if (hasPrereq)
            {
                uColor = knows ? KNOWS_COLOR.PackedValue : (KNOWS_COLOR.PackedValue & UNKNOWN_SKILL_LINE_COLOR_MASK.PackedValue | 0xFF000000);
                //uColor = knows ? KNOWS_COLOR.PackedValue : (KNOWS_COLOR.PackedValue);
                offset = 1;
            }
            else
            {
                uColor = (UNKNOWN_SKILL_LINE_COLOR_MASK.PackedValue | 0xFF000000);
                //color = Color.White.PackedValue;
                offset = 0;
            }

            // draw line to parent
            if (node.Parent != null)
            {
                var nodeTopMiddle = new Point(nodeRect.X + (nodeRect.Width / 2), nodeRect.Y);
                //spriteBatch.DrawLine(NODE_PEN, nodeTopMiddle, new Point(nodeTopMiddle.X, nodeTopMiddle.Y - (SKILL_MARGIN_Y / 2)));
                DrawSprite.DrawLine(spriteBatch, nodeTopMiddle.ToVector2(), new Point(nodeTopMiddle.X, nodeTopMiddle.Y - (SKILL_MARGIN_Y / 2)).ToVector2(), new Color(uColor), 4);
            }

            // draw line to children
            if (node.Children.Count > 0)
            {
                var nodeBottomMiddle = new Point(nodeRect.X + (nodeRect.Width / 2), nodeRect.Y + nodeRect.Height);
                //spriteBatch.DrawLine(NODE_PEN, nodeBottomMiddle, new Point(nodeBottomMiddle.X, nodeBottomMiddle.Y + (SKILL_MARGIN_Y / 2)));
                DrawSprite.DrawLine(spriteBatch, nodeBottomMiddle.ToVector2(), new Point(nodeBottomMiddle.X, nodeBottomMiddle.Y + (SKILL_MARGIN_Y / 2)).ToVector2(), new Color(uColor), 4);


                // draw line over children
                if (node.Children.Count > 1)
                {
                    var childrenLineStart = new Point(
                        Convert.ToInt32(bounds.X + SKILL_MARGIN_X + (node.GetRightMostChild().X * (SKILL_SIZE + SKILL_MARGIN_X)) + (SKILL_SIZE / 2)),
                        nodeBottomMiddle.Y + (SKILL_MARGIN_Y / 2));
                    var childrenLineEnd = new Point(
                        Convert.ToInt32(bounds.X + SKILL_MARGIN_X + (node.GetLeftMostChild().X * (SKILL_SIZE + SKILL_MARGIN_X)) + (SKILL_SIZE / 2)),
                        nodeBottomMiddle.Y + (SKILL_MARGIN_Y / 2));

                    //spriteBatch.DrawLine(NODE_PEN, childrenLineStart, childrenLineEnd);
                    DrawSprite.DrawLine(spriteBatch, childrenLineStart.ToVector2(), childrenLineEnd.ToVector2(), new Color(uColor), 4);
                }
            }
        }

        #endregion

        public override void MouseHover(float mouseX, float mouseY)
        {
            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            positionSkillHoverRect(mouseX, mouseY, skillTree);
        }

        private void positionSkillHoverRect(float mouseX, float mouseY, TreeNodeModel<SpellPartSkill> node)
        {
            var nodeRect = new Rectangle
                (
                    Convert.ToInt32(bounds.X + SKILL_MARGIN_X + (node.X * (SKILL_SIZE + SKILL_MARGIN_X))),
                    (int)(bounds.Y + SKILL_MARGIN_Y + (node.Y * (SKILL_SIZE + SKILL_MARGIN_Y))),
                    (int)SKILL_SIZE, (int)SKILL_SIZE
                );

            Vector2 vector = Vector2.Transform(nodeRect.Location.ToVector2(), transformMatrix);

            Rectangle rect = new Rectangle((int)vector.X, (int)vector.Y, (int)SKILL_SIZE + 16, (int)SKILL_SIZE + 16);
            //Rectangle rect = new Rectangle((int)vector.X, (int)vector.Y, (int)SKILL_SIZE, (int)SKILL_SIZE);

            if (rect.Contains(mouseX, mouseY))
            {
                hoverItem = node.Item;
                isHoveringSkill = true;
            }
            else
            {
                isHoveringSkill = false;
            }

            foreach (var item in node.Children)
            {
                positionSkillHoverRect(mouseX, mouseY, item);
            }
        }

        public override void MouseClicked(float mouseX, float mouseY)
        {
            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            if (player != null && hoverItem != null && !helper.Knows(modEntry, player, hoverItem))
            {
                //modEntry.Monitor.Log("Mouse Clicked", StardewModdingAPI.LogLevel.Info);

                if (helper.CanLearn(modEntry, player, hoverItem))
                {
                    foreach (var item in hoverItem.Cost())
                    {
                        player.Items.ReduceId(item.Key.QualifiedItemId, item.Value);
                    }
                    
                    helper.Learn(modEntry, player, hoverItem);
                }
            }
        }

        public void DrawHoverToolTip(ModEntry modEntry, Farmer player, ISpellPartSkillHelper helper, SpriteBatch spriteBatch)
        {

            string spellPartNameText = modEntry.Helper.Translation.Get($"spellpart.{hoverItem.GetId()}.name");
            string spellPartDescriptionText = modEntry.Helper.Translation.Get($"spellpart.{hoverItem.GetId()}.description");

            bool knows = helper.Knows(modEntry, player, hoverItem);

            bool hasPrereq = helper.CanLearn(modEntry, player, hoverItem) || knows;

            if (!hasPrereq)
            {
                spellPartNameText = spellPartNameText + " " + modEntry.Helper.Translation.Get("ui.magic_altar.tooltip_name_cant_learn.name");
            }
            else if (!knows)
            {
                spellPartNameText = spellPartNameText + " " + modEntry.Helper.Translation.Get("ui.magic_altar.tooltip_name_not_learned.name");
            }
            else
            {
                spellPartNameText = spellPartNameText + " " + modEntry.Helper.Translation.Get("ui.magic_altar.tooltip_name_learned.name");
            }

            int val1 = 272;
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                val1 = 384;
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                val1 = 336;

            int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(spellPartNameText == null ? "" : spellPartNameText).X);

            StringBuilder s = new StringBuilder();
            s.AppendLine(Game1.parseText(spellPartDescriptionText, Game1.smallFont, value));
            //s.AppendLine("");

            if (spellPartNameText != null && spellPartDescriptionText != null)
                drawSkillToolTip(spriteBatch, s.ToString(), spellPartNameText, null, false, -1, 0, null, -1, hoverItem);
        }

        public void drawSkillToolTip(SpriteBatch b, string hoverText, string hoverTitle, Item hoveredItem, bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, string extraItemToShowIndex = null, int extraItemToShowAmount = -1, SpellPartSkill craftingIngredients = null, int moneyAmountToShowAtBottom = -1)
        {
            Object @object = hoveredItem as Object;
            bool flag = @object != null && (int)@object.Edibility != -300;
            string[] array = null;
            if (flag && Game1.objectData.TryGetValue(hoveredItem.ItemId, out var value))
            {
                BuffEffects buffEffects = new BuffEffects();
                int num = int.MinValue;
                foreach (Buff item in Object.TryCreateBuffsFromData(value, hoveredItem.Name, hoveredItem.DisplayName, 1f, hoveredItem.ModifyItemBuffs))
                {
                    buffEffects.Add(item.effects);
                    if (item.millisecondsDuration == -2 || (item.millisecondsDuration > num && num != -2))
                    {
                        num = item.millisecondsDuration;
                    }
                }

                if (buffEffects.HasAnyValue())
                {
                    array = buffEffects.ToLegacyAttributeFormat();
                    if (num != -2)
                    {
                        array[12] = " " + Utility.getMinutesSecondsStringFromMilliseconds(num);
                    }
                }
            }

            drawSkillHoverText(b, hoverText, Game1.smallFont, heldItem ? 40 : 0, heldItem ? 40 : 0, moneyAmountToShowAtBottom, hoverTitle, flag ? ((hoveredItem as Object).Edibility) : (-1), array, hoveredItem, currencySymbol, extraItemToShowIndex, extraItemToShowAmount, -1, -1, 1f, craftingIngredients);
        }

        public void drawSkillHoverText(SpriteBatch b, string text, SpriteFont font, int xOffset = 0, int yOffset = 0, int moneyAmountToDisplayAtBottom = -1, string boldTitleText = null, int healAmountToDisplay = -1, string[] buffIconsToDisplay = null, Item hoveredItem = null, int currencySymbol = 0, string extraItemToShowIndex = null, int extraItemToShowAmount = -1, int overrideX = -1, int overrideY = -1, float alpha = 1f, SpellPartSkill craftingIngredients = null, IList<Item> additional_craft_materials = null, Texture2D boxTexture = null, Rectangle? boxSourceRect = null, Color? textColor = null, Color? textShadowColor = null, float boxScale = 1f, int boxWidthOverride = -1, int boxHeightOverride = -1)
        {
            learnedDescription.Clear();
            HoverTextStringBuilder.Clear();
            HoverTextStringBuilder.Append(text);
            drawSkillHoverText(b, HoverTextStringBuilder, font, xOffset, yOffset, moneyAmountToDisplayAtBottom, boldTitleText, healAmountToDisplay, buffIconsToDisplay, hoveredItem, currencySymbol, extraItemToShowIndex, extraItemToShowAmount, overrideX, overrideY, alpha, craftingIngredients, additional_craft_materials, boxTexture, boxSourceRect, textColor, textShadowColor, boxScale, boxWidthOverride, boxHeightOverride);
        }

        public void drawSkillHoverText(SpriteBatch b, StringBuilder text, SpriteFont font, int xOffset = 0, int yOffset = 0, int moneyAmountToDisplayAtBottom = -1, string boldTitleText = null, int healAmountToDisplay = -1, string[] buffIconsToDisplay = null, Item hoveredItem = null, int currencySymbol = 0, string extraItemToShowIndex = null, int extraItemToShowAmount = -1, int overrideX = -1, int overrideY = -1, float alpha = 1f, SpellPartSkill craftingIngredients = null, IList<Item> additional_craft_materials = null, Texture2D boxTexture = null, Rectangle? boxSourceRect = null, Color? textColor = null, Color? textShadowColor = null, float boxScale = 1f, int boxWidthOverride = -1, int boxHeightOverride = -1)
        {
            boxTexture = boxTexture ?? Game1.menuTexture;
            boxSourceRect = boxSourceRect ?? new Rectangle(0, 256, 60, 60);
            textColor = textColor ?? Game1.textColor;
            textShadowColor = textShadowColor ?? Game1.textShadowColor;
            if (text == null || text.Length == 0)
            {
                return;
            }

            if (moneyAmountToDisplayAtBottom <= -1 && currencySymbol == 0 && hoveredItem != null && Game1.player.stats.Get("Book_PriceCatalogue") != 0 && !(hoveredItem is Furniture) && hoveredItem.CanBeLostOnDeath() && !(hoveredItem is Clothing) && !(hoveredItem is Wallpaper) && (!(hoveredItem is Object) || !(hoveredItem as Object).bigCraftable.Value) && hoveredItem.sellToStorePrice(-1L) > 0)
            {
                moneyAmountToDisplayAtBottom = hoveredItem.sellToStorePrice(-1L) * hoveredItem.Stack;
            }

            string text2 = null;
            if (boldTitleText != null && boldTitleText.Length == 0)
            {
                boldTitleText = null;
            }

            int num = Math.Max((healAmountToDisplay != -1) ? ((int)font.MeasureString(healAmountToDisplay + "+ Energy" + 32).X) : 0, Math.Max((int)font.MeasureString(text).X, (boldTitleText != null) ? ((int)Game1.dialogueFont.MeasureString(boldTitleText).X) : 0)) + 32;
            int num2 = Math.Max(20 * 3, (int)font.MeasureString(text).Y + 32 + (int)((moneyAmountToDisplayAtBottom > -1) ? Math.Max(font.MeasureString(moneyAmountToDisplayAtBottom.ToString() ?? "").Y + 4f, 44f) : 0f) + (int)((boldTitleText != null) ? (Game1.dialogueFont.MeasureString(boldTitleText).Y + 16f) : 0f));
            if (extraItemToShowIndex != null)
            {
                ParsedItemData dataOrErrorItem = ItemRegistry.GetDataOrErrorItem("(O)" + extraItemToShowIndex);
                string displayName = dataOrErrorItem.DisplayName;
                Rectangle sourceRect = dataOrErrorItem.GetSourceRect();
                string text3 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", extraItemToShowAmount, (extraItemToShowAmount > 1) ? Lexicon.makePlural(displayName) : displayName);
                int num3 = sourceRect.Width * 2 * 4;
                num = Math.Max(num, num3 + (int)font.MeasureString(text3).X);
            }

            if (buffIconsToDisplay != null)
            {
                foreach (string text4 in buffIconsToDisplay)
                {
                    if (!text4.Equals("0") && text4 != "")
                    {
                        num2 += 39;
                    }
                }

                num2 += 4;
            }

            //if (craftingIngredients != null && Game1.options.showAdvancedCraftingInformation && craftingIngredients.getCraftCountText() != null)
            //{
            //    num2 += (int)font.MeasureString("T").Y + 2;
            //}

            string text5 = null;
            if (hoveredItem != null)
            {
                if (hoveredItem is FishingRod)
                {
                    if (hoveredItem.attachmentSlots() == 1)
                    {
                        num2 += 68;
                    }
                    else if (hoveredItem.attachmentSlots() > 1)
                    {
                        num2 += 136;
                    }
                }
                else
                {
                    num2 += 68 * hoveredItem.attachmentSlots();
                }

                text5 = hoveredItem.getCategoryName();
                if (text5.Length > 0)
                {
                    num = Math.Max(num, (int)font.MeasureString(text5).X + 32);
                    num2 += (int)font.MeasureString("T").Y;
                }

                int num4 = 9999;
                int num5 = 92;
                Point extraSpaceNeededForTooltipSpecialIcons = hoveredItem.getExtraSpaceNeededForTooltipSpecialIcons(font, num, num5, num2, text, boldTitleText, moneyAmountToDisplayAtBottom);
                num = ((extraSpaceNeededForTooltipSpecialIcons.X != 0) ? extraSpaceNeededForTooltipSpecialIcons.X : num);
                num2 = ((extraSpaceNeededForTooltipSpecialIcons.Y != 0) ? extraSpaceNeededForTooltipSpecialIcons.Y : num2);
                MeleeWeapon meleeWeapon = hoveredItem as MeleeWeapon;
                if (meleeWeapon != null)
                {
                    if (meleeWeapon.GetTotalForgeLevels() > 0)
                    {
                        num2 += (int)font.MeasureString("T").Y;
                    }

                    if (meleeWeapon.GetEnchantmentLevel<GalaxySoulEnchantment>() > 0)
                    {
                        num2 += (int)font.MeasureString("T").Y;
                    }
                }

                Object @object = hoveredItem as Object;
                if (@object != null && (int)@object.Edibility != -300)
                {
                    healAmountToDisplay = @object.staminaRecoveredOnConsumption();
                    num2 = ((healAmountToDisplay == -1) ? (num2 + 40) : (num2 + 40 * ((healAmountToDisplay <= 0 || @object.healthRecoveredOnConsumption() <= 0) ? 1 : 2)));
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh)
                    {
                        num2 += 16;
                    }

                    num = (int)Math.Max(num, Math.Max(font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Energy", num4)).X + (float)num5, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Health", num4)).X + (float)num5));
                }

                if (buffIconsToDisplay != null)
                {
                    for (int j = 0; j < buffIconsToDisplay.Length; j++)
                    {
                        if (!buffIconsToDisplay[j].Equals("0") && j <= 12)
                        {
                            num = (int)Math.Max(num, font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + j, num4)).X + (float)num5);
                        }
                    }
                }
            }

            Vector2 vector = Vector2.Zero;
            if (craftingIngredients != null)
            {
                //if (Game1.options.showAdvancedCraftingInformation)
                //{
                //    int craftableCount = craftingIngredients.getCraftableCount(additional_craft_materials);
                //    if (craftableCount > 1)
                //    {
                //        text2 = " (" + craftableCount + ")";
                //        vector = Game1.smallFont.MeasureString(text2);
                //    }
                //}

                num = (int)Math.Max(Game1.dialogueFont.MeasureString(boldTitleText).X + vector.X + 12f, 384f);
                //num2 += craftingIngredients.getDescriptionHeight(num + 4 - 8) - 32;

                int w = num + 4 - 8;

                if (craftingIngredients.Parents().Any())
                {
                    learnedDescription.AppendLine("Learned:");

                    foreach (SpellPartSkill parentSkill in craftingIngredients.Parents())
                    {
                        learnedDescription.AppendLine(parentSkill.GetId());
                    }

                    learnedDescription.AppendLine("");
                }


                learnedDescription.AppendLine("");

                num2 += (int)(Game1.smallFont.MeasureString(Game1.parseText(learnedDescription.ToString(), Game1.smallFont, w)).Y + (float)(craftingIngredients.Cost().Count * 36) + (float)(int)Game1.smallFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567")).Y + 21f) - 40;
                
                if (craftingIngredients != null && hoveredItem != null && hoveredItem.getDescription().Equals(text.ToString()))
                {
                    num2 -= (int)font.MeasureString(text.ToString()).Y;
                }

                if (craftingIngredients != null && Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh)
                {
                    num2 += 8;
                }
            }
            else if (text2 != null && boldTitleText != null)
            {
                vector = Game1.smallFont.MeasureString(text2);
                num = (int)Math.Max(num, Game1.dialogueFont.MeasureString(boldTitleText).X + vector.X + 12f);
            }

            int x = Game1.getOldMouseX() + 32 + xOffset;
            int num6 = Game1.getOldMouseY() + 32 + yOffset;
            if (overrideX != -1)
            {
                x = overrideX;
            }

            if (overrideY != -1)
            {
                num6 = overrideY;
            }

            if (x + num > Utility.getSafeArea().Right)
            {
                x = Utility.getSafeArea().Right - num;
                num6 += 16;
            }

            if (num6 + num2 > Utility.getSafeArea().Bottom)
            {
                x += 16;
                if (x + num > Utility.getSafeArea().Right)
                {
                    x = Utility.getSafeArea().Right - num;
                }

                num6 = Utility.getSafeArea().Bottom - num2;
            }

            num += 4;
            int num7 = ((boxWidthOverride != -1) ? boxWidthOverride : (num + ((craftingIngredients != null) ? 21 : 0)));
            int num8 = ((boxHeightOverride != -1) ? boxHeightOverride : num2);
            IClickableMenu.drawTextureBox(b, boxTexture, boxSourceRect.Value, x, num6, num7, num8, Color.White * alpha, boxScale);
            if (boldTitleText != null)
            {
                Vector2 vector2 = Game1.dialogueFont.MeasureString(boldTitleText);
                IClickableMenu.drawTextureBox(b, boxTexture, boxSourceRect.Value, x, num6, num + ((craftingIngredients != null) ? 21 : 0), (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + 32 + (int)((hoveredItem != null && text5.Length > 0) ? font.MeasureString("asd").Y : 0f) - 4, Color.White * alpha, 1f, drawShadow: false);
                b.Draw(Game1.menuTexture, new Rectangle(x + 12, num6 + (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + 32 + (int)((hoveredItem != null && text5.Length > 0) ? font.MeasureString("asd").Y : 0f) - 4, num - 4 * ((craftingIngredients != null) ? 1 : 6), 4), new Rectangle(44, 300, 4, 4), Color.White);
                b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 2f), textShadowColor.Value);
                b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2(x + 16, num6 + 16 + 4) + new Vector2(0f, 2f), textShadowColor.Value);
                b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2(x + 16, num6 + 16 + 4), textColor.Value);
                if (text2 != null)
                {
                    Utility.drawTextWithShadow(b, text2, Game1.smallFont, new Vector2((float)(x + 16) + vector2.X, (int)((float)(num6 + 16 + 4) + vector2.Y / 2f - vector.Y / 2f)), Game1.textColor);
                }

                num6 += (int)Game1.dialogueFont.MeasureString(boldTitleText).Y;
            }

            if (hoveredItem != null && text5.Length > 0)
            {
                num6 -= 4;
                Utility.drawTextWithShadow(b, text5, font, new Vector2(x + 16, num6 + 16 + 4), hoveredItem.getCategoryColor(), 1f, -1f, 2, 2);
                num6 += (int)font.MeasureString("T").Y + ((boldTitleText != null) ? 16 : 0) + 4;
                Tool tool = hoveredItem as Tool;
                if (tool != null && tool.GetTotalForgeLevels() > 0)
                {
                    string text6 = Game1.content.LoadString("Strings\\UI:Item_Tooltip_Forged");
                    Utility.drawTextWithShadow(b, text6, font, new Vector2(x + 16, num6 + 16 + 4), Color.DarkRed, 1f, -1f, 2, 2);
                    int totalForgeLevels = tool.GetTotalForgeLevels();
                    if (totalForgeLevels < tool.GetMaxForges() && !tool.hasEnchantmentOfType<DiamondEnchantment>())
                    {
                        Utility.drawTextWithShadow(b, " (" + totalForgeLevels + "/" + tool.GetMaxForges() + ")", font, new Vector2((float)(x + 16) + font.MeasureString(text6).X, num6 + 16 + 4), Color.DimGray, 1f, -1f, 2, 2);
                    }

                    num6 += (int)font.MeasureString("T").Y;
                }

                MeleeWeapon meleeWeapon2 = hoveredItem as MeleeWeapon;
                if (meleeWeapon2 != null && meleeWeapon2.GetEnchantmentLevel<GalaxySoulEnchantment>() > 0)
                {
                    GalaxySoulEnchantment enchantmentOfType = meleeWeapon2.GetEnchantmentOfType<GalaxySoulEnchantment>();
                    string text7 = Game1.content.LoadString("Strings\\UI:Item_Tooltip_GalaxyForged");
                    Utility.drawTextWithShadow(b, text7, font, new Vector2(x + 16, num6 + 16 + 4), Color.DarkRed, 1f, -1f, 2, 2);
                    int level = enchantmentOfType.GetLevel();
                    if (level < enchantmentOfType.GetMaximumLevel())
                    {
                        Utility.drawTextWithShadow(b, " (" + level + "/" + enchantmentOfType.GetMaximumLevel() + ")", font, new Vector2((float)(x + 16) + font.MeasureString(text7).X, num6 + 16 + 4), Color.DimGray, 1f, -1f, 2, 2);
                    }

                    num6 += (int)font.MeasureString("T").Y;
                }
            }
            else
            {
                num6 += ((boldTitleText != null) ? 16 : 0);
            }

            if (hoveredItem != null && craftingIngredients == null)
            {
                hoveredItem.drawTooltip(b, ref x, ref num6, font, alpha, text);
            }
            else if (text != null && text.Length != 0 && (text.Length != 1 || text[0] != ' ') && (craftingIngredients == null || hoveredItem == null || !hoveredItem.getDescription().Equals(text.ToString())))
            {
                if (text.ToString().Contains("[line]"))
                {
                    string[] array = text.ToString().Split("[line]");
                    b.DrawString(font, array[0], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 2f), textShadowColor.Value * alpha);
                    b.DrawString(font, array[0], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(0f, 2f), textShadowColor.Value * alpha);
                    b.DrawString(font, array[0], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 0f), textShadowColor.Value * alpha);
                    b.DrawString(font, array[0], new Vector2(x + 16, num6 + 16 + 4), textColor.Value * 0.9f * alpha);
                    num6 += (int)font.MeasureString(array[0]).Y - 16;
                    Utility.drawLineWithScreenCoordinates(x + 16 - 4, num6 + 16 + 4, x + 16 + num - 28, num6 + 16 + 4, b, textShadowColor.Value);
                    Utility.drawLineWithScreenCoordinates(x + 16 - 4, num6 + 16 + 5, x + 16 + num - 28, num6 + 16 + 5, b, textShadowColor.Value);
                    if (array.Length > 1)
                    {
                        num6 -= 16;
                        b.DrawString(font, array[1], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 2f), textShadowColor.Value * alpha);
                        b.DrawString(font, array[1], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(0f, 2f), textShadowColor.Value * alpha);
                        b.DrawString(font, array[1], new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 0f), textShadowColor.Value * alpha);
                        b.DrawString(font, array[1], new Vector2(x + 16, num6 + 16 + 4), textColor.Value * 0.9f * alpha);
                        num6 += (int)font.MeasureString(array[1]).Y;
                    }

                    num6 += 4;
                }
                else
                {
                    b.DrawString(font, text, new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 2f), textShadowColor.Value * alpha);
                    b.DrawString(font, text, new Vector2(x + 16, num6 + 16 + 4) + new Vector2(0f, 2f), textShadowColor.Value * alpha);
                    b.DrawString(font, text, new Vector2(x + 16, num6 + 16 + 4) + new Vector2(2f, 0f), textShadowColor.Value * alpha);
                    b.DrawString(font, text, new Vector2(x + 16, num6 + 16 + 4), textColor.Value * 0.9f * alpha);
                    num6 += (int)font.MeasureString(text).Y + 4;
                }
            }

            if (craftingIngredients != null)
            {
                //craftingIngredients.drawRecipeDescription(b, new Vector2(x + 16, num6 - 8), num, additional_craft_materials);
                //num6 += craftingIngredients.getDescriptionHeight(num - 8);

                drawKnowegeCostDescription(b, new Vector2(x + 16, num6 - 8), num, craftingIngredients);
            }

            if (healAmountToDisplay != -1)
            {
                int num9 = (hoveredItem as Object).staminaRecoveredOnConsumption();
                if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh)
                {
                    num6 += 8;
                }

                if (num9 >= 0)
                {
                    int num10 = (hoveredItem as Object).healthRecoveredOnConsumption();
                    if (num9 > 0)
                    {
                        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(x + 16 + 4, num6 + 16), new Rectangle((num9 < 0) ? 140 : 0, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, flipped: false, 0.95f);
                        Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", ((num9 > 0) ? "+" : "") + num9), font, new Vector2(x + 16 + 34 + 4, num6 + 16), Game1.textColor);
                        num6 += 34;
                    }

                    if (num10 > 0)
                    {
                        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(x + 16 + 4, num6 + 16), new Rectangle(0, 438, 10, 10), Color.White, 0f, Vector2.Zero, 3f, flipped: false, 0.95f);
                        Utility.drawTextWithShadow(b, (num10 >= 999) ? " 100%" : Game1.content.LoadString("Strings\\UI:ItemHover_Health", ((num10 > 0) ? "+" : "") + num10), font, new Vector2(x + 16 + 34 + 4, num6 + 16), Game1.textColor);
                        num6 += 34;
                    }
                }
                else if (num9 != -300)
                {
                    Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(x + 16 + 4, num6 + 16), new Rectangle(140, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, flipped: false, 0.95f);
                    Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", num9.ToString() ?? ""), font, new Vector2(x + 16 + 34 + 4, num6 + 16), Game1.textColor);
                    num6 += 34;
                }
            }

            if (buffIconsToDisplay != null)
            {
                num6 += 16;
                b.Draw(Game1.staminaRect, new Rectangle(x + 12, num6 + 6, num - ((craftingIngredients != null) ? 4 : 24), 2), new Color(207, 147, 103) * 0.8f);
                for (int k = 0; k < buffIconsToDisplay.Length; k++)
                {
                    if (buffIconsToDisplay[k].Equals("0") || !(buffIconsToDisplay[k] != ""))
                    {
                        continue;
                    }

                    if (k == 12)
                    {
                        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(x + 16 + 4, num6 + 16), new Rectangle(410, 501, 9, 9), Color.White, 0f, Vector2.Zero, 3f, flipped: false, 0.95f);
                        Utility.drawTextWithShadow(b, buffIconsToDisplay[k], font, new Vector2(x + 16 + 34 + 4, num6 + 16), Game1.textColor);
                    }
                    else
                    {
                        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(x + 16 + 4, num6 + 16), new Rectangle(10 + k * 10, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, flipped: false, 0.95f);
                        string text8 = ((Convert.ToDouble(buffIconsToDisplay[k]) > 0.0) ? "+" : "") + buffIconsToDisplay[k] + " ";
                        if (k <= 11)
                        {
                            text8 = Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + k, text8);
                        }

                        Utility.drawTextWithShadow(b, text8, font, new Vector2(x + 16 + 34 + 4, num6 + 16), Game1.textColor);
                    }

                    num6 += 39;
                }

                num6 -= 8;
            }

            if (hoveredItem != null && hoveredItem.attachmentSlots() > 0)
            {
                hoveredItem.drawAttachments(b, x + 16, num6 + 16);
                if (moneyAmountToDisplayAtBottom > -1)
                {
                    num6 += 68 * hoveredItem.attachmentSlots();
                }
            }

            if (moneyAmountToDisplayAtBottom > -1)
            {
                b.Draw(Game1.staminaRect, new Rectangle(x + 12, num6 + 22 - ((healAmountToDisplay <= 0) ? 6 : 0), num - ((craftingIngredients != null) ? 4 : 24), 2), new Color(207, 147, 103) * 0.5f);
                string text9 = moneyAmountToDisplayAtBottom.ToString();
                int num11 = 0;
                if ((buffIconsToDisplay != null && buffIconsToDisplay.Length > 1) || healAmountToDisplay > 0 || craftingIngredients != null)
                {
                    num11 = 8;
                }

                b.DrawString(font, text9, new Vector2(x + 16, num6 + 16 + 4 + num11) + new Vector2(2f, 2f), textShadowColor.Value);
                b.DrawString(font, text9, new Vector2(x + 16, num6 + 16 + 4 + num11) + new Vector2(0f, 2f), textShadowColor.Value);
                b.DrawString(font, text9, new Vector2(x + 16, num6 + 16 + 4 + num11) + new Vector2(2f, 0f), textShadowColor.Value);
                b.DrawString(font, text9, new Vector2(x + 16, num6 + 16 + 4 + num11), textColor.Value);
                switch (currencySymbol)
                {
                    case 0:
                        b.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + 16) + font.MeasureString(text9).X + 20f, num6 + 16 + 20 + num11), Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16), Color.White, 0f, new Vector2(8f, 8f), 4f, SpriteEffects.None, 0.95f);
                        break;
                    case 1:
                        b.Draw(Game1.mouseCursors, new Vector2((float)(x + 8) + font.MeasureString(text9).X + 20f, num6 + 16 - 5 + num11), new Rectangle(338, 400, 8, 8), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                        break;
                    case 2:
                        b.Draw(Game1.mouseCursors, new Vector2((float)(x + 8) + font.MeasureString(text9).X + 20f, num6 + 16 - 7 + num11), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                        break;
                    case 4:
                        b.Draw(Game1.objectSpriteSheet, new Vector2((float)(x + 8) + font.MeasureString(text9).X + 20f, num6 + 16 - 7 + num11), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 858, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                        break;
                }

                num6 += 48;
            }

            if (extraItemToShowIndex != null)
            {
                if (moneyAmountToDisplayAtBottom == -1)
                {
                    num6 += 8;
                }

                ParsedItemData dataOrErrorItem2 = ItemRegistry.GetDataOrErrorItem(extraItemToShowIndex);
                string displayName2 = dataOrErrorItem2.DisplayName;
                Texture2D texture = dataOrErrorItem2.GetTexture();
                Rectangle sourceRect2 = dataOrErrorItem2.GetSourceRect();
                string text10 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", extraItemToShowAmount, displayName2);
                float num12 = Math.Max(font.MeasureString(text10).Y + 21f, 96f);
                IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, num6 + 4, num + ((craftingIngredients != null) ? 21 : 0), (int)num12, Color.White);
                num6 += 20;
                b.DrawString(font, text10, new Vector2(x + 16, num6 + 4) + new Vector2(2f, 2f), textShadowColor.Value);
                b.DrawString(font, text10, new Vector2(x + 16, num6 + 4) + new Vector2(0f, 2f), textShadowColor.Value);
                b.DrawString(font, text10, new Vector2(x + 16, num6 + 4) + new Vector2(2f, 0f), textShadowColor.Value);
                b.DrawString(Game1.smallFont, text10, new Vector2(x + 16, num6 + 4), textColor.Value);
                b.Draw(texture, new Vector2(x + 16 + (int)font.MeasureString(text10).X + 21, num6), sourceRect2, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            }

            //if (craftingIngredients != null && Game1.options.showAdvancedCraftingInformation)
            //{
            //    Utility.drawTextWithShadow(b, craftingIngredients.getCraftCountText(), font, new Vector2(x + 16, num6 + 16 + 4), Game1.textColor, 1f, -1f, 2, 2);
            //    num6 += (int)font.MeasureString("T").Y + 4;
            //}
        }

        public virtual void drawKnowegeCostDescription(SpriteBatch b, Vector2 position, int width, SpellPartSkill knowlege)
        {
            int num = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ko) ? 8 : 0);
            
            if(knowlege.Cost().Any())
            {

                b.Draw(Game1.staminaRect, new Rectangle((int)(position.X + 8f), (int)(position.Y + 32f + Game1.smallFont.MeasureString("Ing!").Y) - 4 - 2 - (int)((float)num * 1.5f), width - 32, 2), Game1.textColor * 0.35f);
                Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567"), Game1.smallFont, position + new Vector2(8f, 28f), Game1.textColor * 0.75f);


                int num2 = -1;

                foreach (KeyValuePair<Item, int> cost in knowlege.Cost())
                {
                    num2++;

                    int ingredientAmount = cost.Value;
                    string ingredientID = cost.Key.QualifiedItemId;

                    int itemCount = Game1.player.Items.CountId(ingredientID);
                    int num3 = 0;
                    int num4 = ingredientAmount - itemCount;

                    string nameFromIndex;

                    ParsedItemData dataOrErrorItem1 = ItemRegistry.GetDataOrErrorItem(ingredientID);

                    if (dataOrErrorItem1 != null)
                    {
                        nameFromIndex = dataOrErrorItem1.DisplayName;
                    }
                    else 
                    { 
                        nameFromIndex = ItemRegistry.GetErrorItemName(); 
                    }

                    Color color = ((num4 <= 0) ? Game1.textColor : Color.Red);
                    ParsedItemData dataOrErrorItem2 = ItemRegistry.GetDataOrErrorItem(ingredientID);
                    Texture2D texture = dataOrErrorItem2.GetTexture();
                    Rectangle sourceRect = dataOrErrorItem2.GetSourceRect();
                    float num5 = 2f;

                    if (sourceRect.Width > 0 || sourceRect.Height > 0)
                    {
                        num5 *= 16f / (float)Math.Max(sourceRect.Width, sourceRect.Height);
                    }

                    b.Draw(texture, new Vector2(position.X + 16f, position.Y + 64f + (float)(num2 * 64 / 2) + (float)(num2 * 4) + 16f), sourceRect, Color.White, 0f, new Vector2(sourceRect.Width / 2, sourceRect.Height / 2), num5, SpriteEffects.None, 0.86f);
                    Utility.drawTinyDigits(ingredientAmount, b, new Vector2(position.X + 32f - Game1.tinyFont.MeasureString(ingredientAmount.ToString() ?? "").X, position.Y + 64f + (float)(num2 * 64 / 2) + (float)(num2 * 4) + 21f), 2f, 0.87f, Color.AntiqueWhite);
                    Vector2 vector = new Vector2(position.X + 32f + 8f, position.Y + 64f + (float)(num2 * 64 / 2) + (float)(num2 * 4) + 4f);
                    Utility.drawTextWithShadow(b, nameFromIndex, Game1.smallFont, vector, color);
                    if (Game1.options.showAdvancedCraftingInformation)
                    {
                        vector.X = position.X + (float)width - 40f;
                        b.Draw(Game1.mouseCursors, new Rectangle((int)vector.X, (int)vector.Y + 2, 22, 26), new Rectangle(268, 1436, 11, 13), Color.White);
                        Utility.drawTextWithShadow(b, (itemCount + num3).ToString() ?? "", Game1.smallFont, vector - new Vector2(Game1.smallFont.MeasureString(itemCount + num3 + " ").X, 0f), color);
                    }
                }
            }

            if (knowlege.Parents().Any())
            {
                learnedDescription.AppendLine("Learned:");

                b.Draw(Game1.staminaRect, new Rectangle((int)(position.X + 8f), (int)(position.Y + 76 + knowlege.Cost().Count * 36 + num + Game1.smallFont.MeasureString("Ing!").Y) - 4 - 2 - (int)((float)num * 1.5f), width - 32, 2), Game1.textColor * 0.35f);
                Utility.drawTextWithShadow(b, Game1.parseText("Learned:", Game1.smallFont, width - 8), Game1.smallFont, position + new Vector2(8f, 76 + knowlege.Cost().Count * 36 + num), Game1.textColor * 0.75f);

                int g = 0;

                var helper = parent.modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

                foreach (SpellPartSkill parentSkill in knowlege.Parents())
                {
                    //learnedDescription.AppendLine(parentSkill.GetId());
                    learnedDescription.AppendLine(parent.modEntry.Helper.Translation.Get($"spellpart.{parentSkill.GetId()}.name"));
                    Utility.drawTextWithShadow(b, Game1.parseText(parent.modEntry.Helper.Translation.Get($"spellpart.{parentSkill.GetId()}.name"), Game1.smallFont, width - 8), Game1.smallFont, position + new Vector2(8f, 76 + knowlege.Cost().Count * 36 + num + 50 + g), helper.Knows(parent.modEntry, Game1.player, parentSkill) ? Color.Green : Color.Red);
                    g += 20;
                }
            }
        }
    }
}
