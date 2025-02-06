using ArsVenefici.Framework.Spell.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.Menus
{
    public class MagicAltarTabButton : ClickableComponent
    {
        private static readonly int SIZE = 22;
        private readonly int index;
        private readonly int xOffset;
        private readonly int yOffset;
        private readonly MagicAltarTab tab;
        public bool isHovered;

        MagicAltarMenu menu;

        public MagicAltarTabButton(int index, int x, int y, MagicAltarTab tab, string name, MagicAltarMenu menu) : base(new Rectangle(x, y, SIZE, SIZE), name)
        {
            this.index = index;
            this.xOffset = 0;
            this.yOffset = 0;
            this.tab = tab;
            
            this.menu = menu;
        }
        public MagicAltarTabButton(int index, int x, int y, int xOffset, int yOffset, MagicAltarTab tab, string name, MagicAltarMenu menu) : base(new Rectangle(x, y, SIZE, SIZE), name)
        {
            this.index = index;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.tab = tab;

            this.menu = menu;
        }

        public void Draw(SpriteBatch spriteBatch, int positionX, int positionY)
        {
            int scale = 2;

            int tabIndex = GetIndex();

            if (tabIndex == menu.activeTabIndex)
                IClickableMenu.drawTextureBox(spriteBatch, bounds.X - 5, bounds.Y - 5, tab.GetIcon().Width + 40, tab.GetIcon().Height + 40, Color.Aqua);
            else
                IClickableMenu.drawTextureBox(spriteBatch, bounds.X - 5, bounds.Y - 5, tab.GetIcon().Width + 40, tab.GetIcon().Height + 40, Color.White);
            
            spriteBatch.Draw(tab.GetIcon(), new Vector2(this.bounds.X + 2f * scale, this.bounds.Y + 2f * scale), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            
            //isHovered = bounds.Contains(positionX, positionY);

            if(isHovered)
            {
                string tabNameText = tab.GetName();

                int val1 = 272;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                    val1 = 384;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                    val1 = 336;

                int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(tabNameText == null ? "" : tabNameText).X);

                if (tabNameText != null && tabNameText != null)
                    IClickableMenu.drawToolTip(spriteBatch, tabNameText, null, null);
            }
        }
        public int GetIndex()
        {
            return index;
        }

        public void IsHovered(int positionX, int positionY)
        {
            Rectangle rectangle = new Rectangle(bounds.X - 5, bounds.Y - 5, tab.GetIcon().Width + 40, tab.GetIcon().Height + 40);

            if(rectangle.Contains(positionX, positionY))
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }
        }

        public MagicAltarTab GetTab()
        {
            return tab;
        }
    }
}
