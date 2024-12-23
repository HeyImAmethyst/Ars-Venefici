using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Interfaces.Magic;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Magic;
using ArsVenefici.Framework.Spell.Effects;
using ArsVenefici.Framework.Spell.Shape;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArsVenefici.Framework.Events
{
    public class DisplayEvents
    {
        ModEntry modEntryInstance;
        bool displayHitBoxes = false;

        public DisplayEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;
        }

        public void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu is SpellBookMenu)
            {
                string filePath = Path.Combine(modEntryInstance.Helper.DirectoryPath + "/Saves", $"{Constants.SaveFolderName}_spellbook_data.json");

                if (File.Exists(filePath))
                {
                    Game1.player.GetSpellBook().SyncSpellBook(modEntryInstance);
                }

                Game1.player.GetSpellBook().CreateSpells(modEntryInstance);
            }
        }

        public void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                return;

            GameLocation location = Game1.currentLocation;

            if (displayHitBoxes)
            {
                foreach (Character character in location.characters)
                {
                    DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, character.GetBoundingBox()), Color.Red, 1);
                }

                foreach (Farmer farmer in location.farmers)
                {
                    DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, farmer.GetBoundingBox()), Color.Red, 1);
                }

                foreach (IActiveEffect effect in modEntryInstance.ActiveEffects)
                {
                    if (effect != null && effect is AbstractSpellEffect abstractSpellEffect)
                        DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, abstractSpellEffect.GetBoundingBox()), Color.Red, 1);
                }

                foreach (Debris debris in location.debris)
                {
                    foreach (Chunk chunk in debris.Chunks)
                    {
                        //DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, new Rectangle(chunk.GetVisualPosition().ToPoint(), new Point(Game1.tileSize, Game1.tileSize))), Color.Red, 1);

                        //Vector2 position = chunk.position.Value;
                        //Vector2 snapPosition = Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport, position));

                        //DrawSprite.DrawRectangle(e.SpriteBatch, new Rectangle(snapPosition.ToPoint(), new Point((int)(Game1.tileSize * chunk.scale), (int)(Game1.tileSize * chunk.scale))), Color.Red, 1);

                        //Vector2 local = Vector2.One;
                        //Texture2D texture = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");

                        //Vector2 absoluteClampedMousePos = Utility.clampToTile(chunk.GetVisualPosition());
                        //local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        //e.SpriteBatch.Draw(texture, local, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, ((float)(debris.chunkFinalYLevel + 32) + local.X / 10000f));

                        //Vector2 position = chunk.GetVisualPosition();
                        //Vector2 snapPosition = Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport, position));

                        //Texture2D texture = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");

                        //local = Utils.AbsolutePosToScreenPos(snapPosition);

                        ParsedItemData itemData = ItemRegistry.GetDataOrErrorItem(debris.itemId.Value);
                        Texture2D texture = itemData.GetTexture();

                        Rectangle sourceRect = debris.debrisType.Value == Debris.DebrisType.RESOURCE ? itemData.GetSourceRect(chunk.randomOffset) : itemData.GetSourceRect();
                    }
                }
            }

            RenderBeam(e.SpriteBatch);
            RenderTouchIndicator(e.SpriteBatch);

            //draw active effects
            foreach (IActiveEffect effect in modEntryInstance.ActiveEffects)
                effect.Draw(e.SpriteBatch);
        }

        public void OnRenderedStep(object sender, RenderedStepEventArgs e)
        {
            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                return;
        }

        public void RenderBeam(SpriteBatch spriteBatch)
        {
            if (!modEntryInstance.LearnedWizardy || !ModEntry.SpellCastingMode)
                return;

            SpellBook spellBook = Game1.player.GetSpellBook();
            //ISpell spell = spellBook.GetCurrentSpell();
            ISpell spell = spellBook.GetSpells()[spellBook.GetCurrentSpellIndex()];

            if (spell != null)
            {
                if (Game1.activeClickableMenu == null && !Game1.eventUp && Game1.player.IsLocalPlayer && /*Game1.player.CurrentTool != null &&*/ (Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && /*this.CurrentTool.doesShowTileLocationMarker() &&*/ (!Game1.options.hideToolHitLocationWhenInMotion || !Game1.player.isMoving()))
                {
                    Vector2 local = Vector2.One;
                    Texture2D texture = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/beam/beam.png");
                    Texture2D texture2 = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");

                    if (spell.FirstShape(spell.CurrentShapeGroupIndex()) != null && spell.FirstShape(spell.CurrentShapeGroupIndex()) is Beam && modEntryInstance.buttonEvents.spellKeyHoldTime > 0)
                    {
                        //Vector2 mousePos = Utility.PointToVector2(Game1.getMousePosition()) + new Vector2(Game1.viewport.X, Game1.viewport.Y);
                        //Vector2 absoluteClampedMousePos = Utility.clampToTile(mousePos);

                        //local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        ICursorPosition cursorPosition = modEntryInstance.Helper.Input.GetCursorPosition();

                        Vector2 absoluteClampedMousePos = Utility.clampToTile(cursorPosition.AbsolutePixels);

                        //local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);
                        //local = Utils.AbsolutePosToScreenPos(Utility.clampToTile(Game1.player.GetToolLocation(true)));
                        local = Utils.AbsolutePosToScreenPos(Game1.player.getStandingPosition());

                        //Vector2 dPos = local - Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);
                        //Vector2 dPos = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos) - local;

                        var screenMousePos = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);


                        var localMouse = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        spriteBatch.Draw(texture2, localMouse, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);

                        //var rotation = (float)Math.Atan2(dPos.Y, dPos.X);
                        var rotation = (float)-Math.Atan2(local.Y - screenMousePos.Y, screenMousePos.X - local.X);

                        float width = screenMousePos.X - local.X;
                        float height = screenMousePos.Y - local.Y;

                        var size = ExpandToBound(new Rectangle((int)local.X, (int)local.Y, 198, 22), new Rectangle((int)local.X, (int)local.Y, (int)width, (int)height));

                        spriteBatch.Draw(texture, local, new Rectangle(0, 0, 198, 22), Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);


                    }
                }
            }
        }

        private static double ExpandToBound(Rectangle image, Rectangle boundingBox)
        {
            double widthScale = 0, heightScale = 0;
            if (image.Width != 0)
                widthScale = boundingBox.Width / (double)image.Width;
            if (image.Height != 0)
                heightScale = boundingBox.Height / (double)image.Height;

            double scale = Math.Min(widthScale, heightScale);

            //Rectangle result = new Rectangle((int)(image.Width * scale),
            //                    (int)(image.Height * scale));
            return scale;
        }

        public void RenderTouchIndicator(SpriteBatch spriteBatch)
        {

            if (!modEntryInstance.LearnedWizardy || !ModEntry.SpellCastingMode)
                return;

            SpellBook spellBook = Game1.player.GetSpellBook();
            //ISpell spell = spellBook.GetCurrentSpell();
            ISpell spell = spellBook.GetSpells()[spellBook.GetCurrentSpellIndex()];

            if (spell != null)
            {
                if (Game1.activeClickableMenu == null && !Game1.eventUp && Game1.player.IsLocalPlayer && /*Game1.player.CurrentTool != null &&*/ (Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && /*this.CurrentTool.doesShowTileLocationMarker() &&*/ (!Game1.options.hideToolHitLocationWhenInMotion || !Game1.player.isMoving()))
                {
                    Vector2 local = Vector2.One;
                    Texture2D texture = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");

                    if (spell.FirstShape(spell.CurrentShapeGroupIndex()) != null && spell.FirstShape(spell.CurrentShapeGroupIndex()) is Touch)
                    {
                        local = Utils.AbsolutePosToScreenPos(Utility.clampToTile(Game1.player.GetToolLocation(true)));

                        spriteBatch.Draw(texture, local, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);
                    }
                    else if (spell.FirstShape(spell.CurrentShapeGroupIndex()) != null && spell.FirstShape(spell.CurrentShapeGroupIndex()) is EtherialTouch)
                    {
                        //Vector2 mousePos = Utility.PointToVector2(Game1.getMousePosition()) + new Vector2(Game1.viewport.X, Game1.viewport.Y);
                        //Vector2 absoluteClampedMousePos = Utility.clampToTile(mousePos);

                        //local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        ICursorPosition cursorPosition = modEntryInstance.Helper.Input.GetCursorPosition();

                        Vector2 absoluteClampedMousePos = Utility.clampToTile(cursorPosition.AbsolutePixels);
                        local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        spriteBatch.Draw(texture, local, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);
                    }
                }
            }
        }

        public void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {

        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree || !ModEntry.SpellCastingMode)
                return;

            if (!modEntryInstance.LearnedWizardy)
                return;

            int x = modEntryInstance.Config.Position.X;
            int y = modEntryInstance.Config.Position.Y;

            RenderSpellLable(x, y, Game1.player.GetSpellBook());
        }

        public void RenderSpellLable(int x, int y, SpellBook spellBook)
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;

            ClickableComponent spellNameLable = new ClickableComponent(new Rectangle(x, y, 64, 64), "Spell Name");
            //spellNameLable.draw(spriteBatch);

            int spellPageIndex = spellBook.GetCurrentSpellIndex() + 1;
            string spellName = "";
            int spellShapeGroupIndex = 0;

            int manaCost = 0;

            string spellText;

            if (spellBook.GetCurrentSpell().GetName() != null)
                spellName = spellBook.GetCurrentSpell().GetName();

            if (spellBook.GetCurrentSpell().IsValid())
            {
                spellShapeGroupIndex = spellBook.GetCurrentSpell().CurrentShapeGroupIndex() + 1;
                manaCost = spellBook.GetCurrentSpell().Mana();
            }

            if (spellName == "")
            {
                spellText = spellName;
            }
            else
            {
                spellText = spellName + " | " + modEntryInstance.Helper.Translation.Get("ui.spell_label.shape_group.name") + ": " + spellShapeGroupIndex + " | " + modEntryInstance.Helper.Translation.Get("ui.mana_cost.name") + ": " + manaCost;
            }

            string text = spellPageIndex + " : " + spellText;

            if (Game1.player.modData.ContainsKey(ContingencyHelper.ContingencyKey))
            {
                //modEntryInstance.Monitor.Log("Player has ContingencyKey", LogLevel.Info);

                ContingencyType contingencyType = ContingencyHelper.Instance().GetContingencyType(Game1.player);

                if (contingencyType == ContingencyType.DAMAGE)
                {
                    Texture2D sprite = modEntryInstance.spellPartIconManager.GetSprite("contingency_damage");
                    spriteBatch.Draw(sprite, new Vector2(x, y - 75), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, y / 10000f);
                }

                if (contingencyType == ContingencyType.HEALTH)
                {
                    Texture2D sprite = modEntryInstance.spellPartIconManager.GetSprite("contingency_health");
                    spriteBatch.Draw(sprite, new Vector2(x, y - 75), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, y / 10000f);
                }
            }

            if (!Game1.player.modData.ContainsKey(ContingencyHelper.ContingencyKey))
            {
                //modEntryInstance.Monitor.Log("Player has no ContingencyKey", LogLevel.Info);
            }

            //if (text != null)
            //{
            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, spellNameLable.bounds.Width + 100 + text.Length, spellNameLable.bounds.Height, Color.White);
            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, spellNameLable.bounds.Width + (int)Game1.smallFont.MeasureString(text).Y + 32 , spellNameLable.bounds.Height, Color.White);

            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, text.Length + 400, spellNameLable.bounds.Height, Color.White);
            //}

            //Color color = Game1.textColor;
            Color color = Color.White;

            //Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X, spellNameLable.bounds.Y - 30), color);
            Utility.drawTextWithColoredShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X, spellNameLable.bounds.Y - 30), color, Color.Gray);

            //if (text.Length > 0)
            //    Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, spellNameLable.bounds.Y + Game1.tileSize / 2), color);
        }
    }
}
