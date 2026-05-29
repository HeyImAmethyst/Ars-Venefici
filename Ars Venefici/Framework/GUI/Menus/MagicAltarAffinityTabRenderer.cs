using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using ArsVenefici.Framework.API.Client;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Input;
using System.Text;
using StardewModdingAPI;

namespace ArsVenefici.Framework.GUI.Menus
{
    public class MagicAltarAffinityTabRenderer : MagicAltarTabRenderer
    {
        

        public MagicAltarAffinityTabRenderer(MagicAltarTab magicAltarTab, MagicAltarMenu parent) : base(magicAltarTab, parent)
        {
            //Init(magicAltarTab.GetWidth() / 2, magicAltarTab.GetHeight() / 2, parent.width, parent.height, parent.xPositionOnScreen, parent.yPositionOnScreen);
            Init((magicAltarTab.GetWidth() / 2), (magicAltarTab.GetHeight() / 2), parent.width, parent.height, parent.xPositionOnScreen - 150, parent.yPositionOnScreen - 65); 
        }

        protected override void RenderBg(SpriteBatch spriteBatch, int mouseX, int mouseY, float partialTicks)
        {
            int w = 0;
            int h = 0;

            float FULLSCREEN_WINDOW_SCALE = 1.7f;

            if (parent.isFullScreen)
            {

                w = magicAltarTab.GetWidth() / 2;
                h = magicAltarTab.GetHeight() / 2;

                bounds = new Rectangle(parent.xPositionOnScreen - 350, 100, (int)(w * FULLSCREEN_WINDOW_SCALE), (int)(h * FULLSCREEN_WINDOW_SCALE));
            }
            else
            {
                w = magicAltarTab.GetWidth() / 2;
                h = magicAltarTab.GetHeight() / 2;

                bounds = new Rectangle(parent.xPositionOnScreen - 150, parent.yPositionOnScreen - 65, w, h);
            }

            if (parent.isFullScreen)
            {
                IClickableMenu.drawTextureBox(spriteBatch, bounds.X - 10, bounds.Y - 10, bounds.Width + 20, bounds.Height + 20, Color.White);
            }
            else
                IClickableMenu.drawTextureBox(spriteBatch, bounds.X - 10, bounds.Y - 10, bounds.Width + 20, bounds.Height + 20, Color.White);
        }

        protected override void RenderFg(SpriteBatch spriteBatch, int mouseX, int mouseY, float partialTicks)
        {
            ModEntry modEntry = parent.modEntry;

            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var affinityHelper = api.GetAffinityHelper();

            var registry = Affinities.AFFINITIES.GetObjectList();

            float FULLSCREEN_WINDOW_SCALE = 1.7f;

            int affNum = registry.Count() - 1;

            int portion = (360 / affNum);
            int currentID = 0;

            //int cX = width / 2;
            //int cY = height / 2;

            //int cX = (width / 2) + 405;
            //int cY = (height / 2) + 5;

            int cX;
            int cY;

            List<string> drawString = new List<string>();
            List<Affinity> affinities = new List<Affinity>();

            foreach (var affinity in registry)
            {
                affinities.Add(affinity.Get());
            }

            Farmer player = Game1.player;

            foreach (Affinity aff in affinities)
            {
                if (aff.GetId().Equals(Affinity.NONE)) continue;

                double depth = affinityHelper.GetAffinityDepth(player, aff);

                if (parent.isFullScreen)
                {
                    Vector2 adjustedPos = MagicAltarMenu.GetAppropriateMenuPosition();

                    //cX = (int)(((magicAltarTab.GetWidth() / 2) + adjustedPos.X) * FULLSCREEN_WINDOW_SCALE);
                    //cY = (int)(((magicAltarTab.GetHeight() / 2) + adjustedPos.Y) * FULLSCREEN_WINDOW_SCALE);

                    cX = (int)(adjustedPos.X + 100);
                    cY = (int)(adjustedPos.Y + 190);

                    double var1 = Math.Cos((portion * currentID) * Utils.Deg2Rad);
                    double var2 = Math.Sin((portion * currentID) * Utils.Deg2Rad);

                    double var3 = (portion * currentID - portion / 2) * Utils.Deg2Rad;
                    double var4 = (portion * currentID + portion / 2) * Utils.Deg2Rad;

                    //double var3 = (portion * currentID - portion) * Utils.Deg2Rad;
                    //double var4 = (portion * currentID + portion) * Utils.Deg2Rad;

                    double affEndX = var1 * 70F + var1 * depth * 60F;
                    double affEndY = var2 * 70F + var2 * depth * 60F;

                    //double affStartX1 = Math.Cos(var3) * 10F;
                    //double affStartY1 = Math.Sin(var3) * 10F;

                    double affStartX1 = Math.Cos(var3) * 70F;
                    double affStartY1 = Math.Sin(var3) * 70F;

                    //double affStartX2 = Math.Cos(var4) * 10F;
                    //double affStartY2 = Math.Sin(var4) * 10F;

                    double affStartX2 = Math.Cos(var4) * 70F;
                    double affStartY2 = Math.Sin(var4) * 70F;

                    //Controlls the position of the Icons and Text

                    //double affDrawTextX = var1 * 80F - 7;
                    //double affDrawTextY = var2 * 80F - 7;

                    double affDrawTextX = var1 * 350F - 45;
                    double affDrawTextY = var2 * 350F - 15;

                    currentID++;

                    int lineWidth = 2;

                    int displace = (int)((Math.Max(affStartX1, affStartX2) - Math.Min(affStartX1, affStartX2) + Math.Max(affStartY1, affStartY2) - Math.Min(affStartY1, affStartY2)) / 2);
                    //int displace = 0;

                    if (depth > 0.01F)
                    {
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX1 + cX, affStartY1 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 0.8F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX2 + cX, affStartY2 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 0.8F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX1 + cX, affStartY1 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 1.1F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX2 + cX, affStartY2 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 1.1F, lineWidth);
                    }
                    else
                    {
                        RenderUtils.DrawLine(spriteBatch, new Vector2((float)affStartX1 + cX, (float)affStartY1 + cY), new Vector2((float)affEndX + cX, (float)affEndY + cY), aff.color, lineWidth);
                        RenderUtils.DrawLine(spriteBatch, new Vector2((float)affStartX2 + cX, (float)affStartY2 + cY), new Vector2((float)affEndX + cX, (float)affEndY + cY), aff.color, lineWidth);
                    }

                    //graphics.drawString(getFont(), "%.2f".formatted(depth), (int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY), aff.color());

                    string text = string.Format("{0:n2}", depth);
                    Utility.drawTextWithColoredShadow(spriteBatch, text, Game1.smallFont, new Vector2((int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY + 15)), aff.color, Color.Gray);

                    int xMovement = affDrawTextX > 0 ? 5 : -5;
                    xMovement = affDrawTextX == 0 ? 0 : xMovement;

                    int yMovement = affDrawTextY > 0 ? 5 : -5;
                    yMovement = affDrawTextY == 0 ? 0 : yMovement;

                    int drawX = (int)((affDrawTextX * 1.1) + cX + xMovement);
                    int drawY = (int)((affDrawTextY * 1.1) + cY + yMovement);

                    int drawXToolTip = drawX - 5;
                    int drawYToolTip = drawY + 30;

                    Item essenceForAffinity = affinityHelper.GetEssenceForAffinity(aff);
                    essenceForAffinity.drawInMenu(spriteBatch, new Vector2(drawX, drawY), 0.5f, 100f, 100, StackDrawType.Draw, Color.White, false);
                    //essenceForAffinity.drawTooltip(spriteBatch, ref drawX, ref drawY, Game1.smallFont, 100f, new StringBuilder().Append(aff.GetId()));
                    //essenceForAffinity.drawTooltip(spriteBatch, ref drawXToolTip, ref drawYToolTip, Game1.smallFont, 100f, new StringBuilder().Append(aff.GetId()));

                    //IClickableMenu.drawTooltip(spriteBatch, ref drawX, ref drawY, Game1.smallFont, 100f,new StringBuilder().Append(aff.GetId()));

                    //Utility.drawTextWithColoredShadow(spriteBatch, essenceForAffinity.ItemId, Game1.smallFont, new Vector2((int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY + 5)), aff.color, Color.Gray, 0.6f);

                    //if(essenceForAffinity.)
                    //IClickableMenu.drawToolTip(spriteBatch, aff.GetId(), null, null);

                    var input = modEntry.Helper.Input;

                    if (input.GetState(SButton.LeftShift) == SButtonState.Held)
                        Utility.drawTextWithColoredShadow(spriteBatch, ModEntry.INSTANCE.Helper.Translation.Get($"affinity.{aff.GetId()}.name"), Game1.smallFont, new Vector2((int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY + 1)), aff.color, Color.Gray, 0.8f);

                    if (mouseX <= drawX || mouseX >= drawX + 16 || mouseY <= drawY || mouseY >= drawY + 16) continue;
                }
                else
                {

                    //cX = (magicAltarTab.GetWidth() / 2) + 405;
                    //cY = (magicAltarTab.GetHeight() / 2) + 5;

                    Vector2 adjustedPos = MagicAltarMenu.GetAppropriateMenuPosition();

                    cX = (int)(adjustedPos.X + 100);
                    cY = (int)(adjustedPos.Y + 190);

                    double var1 = Math.Cos((portion * currentID) * Utils.Deg2Rad);
                    double var2 = Math.Sin((portion * currentID) * Utils.Deg2Rad);

                    double var3 = (portion * currentID - portion / 2) * Utils.Deg2Rad;
                    double var4 = (portion * currentID + portion / 2) * Utils.Deg2Rad;

                    //double var3 = (portion * currentID - portion) * Utils.Deg2Rad;
                    //double var4 = (portion * currentID + portion) * Utils.Deg2Rad;

                    double affEndX = var1 * 40F + var1 * depth * 60F;
                    double affEndY = var2 * 40F + var2 * depth * 60F;

                    //double affStartX1 = Math.Cos(var3) * 10F;
                    //double affStartY1 = Math.Sin(var3) * 10F;

                    double affStartX1 = Math.Cos(var3) * 40F;
                    double affStartY1 = Math.Sin(var3) * 40F;

                    //double affStartX2 = Math.Cos(var4) * 10F;
                    //double affStartY2 = Math.Sin(var4) * 10F;

                    double affStartX2 = Math.Cos(var4) * 40F;
                    double affStartY2 = Math.Sin(var4) * 40F;

                    //Controlls the position of the Icons and Text

                    //double affDrawTextX = var1 * 80F - 7;
                    //double affDrawTextY = var2 * 80F - 7;

                    double affDrawTextX = var1 * 200F - 25;
                    double affDrawTextY = var2 * 200F - 25;

                    currentID++;

                    int lineWidth = 2;

                    int displace = (int)((Math.Max(affStartX1, affStartX2) - Math.Min(affStartX1, affStartX2) + Math.Max(affStartY1, affStartY2) - Math.Min(affStartY1, affStartY2)) / 2);
                    //int displace = 0;

                    if (depth > 0.01F)
                    {
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX1 + cX, affStartY1 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 0.8F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX2 + cX, affStartY2 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 0.8F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX1 + cX, affStartY1 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 1.1F, lineWidth);
                        RenderUtils.FractalLine2dd(spriteBatch, affStartX2 + cX, affStartY2 + cY, affEndX + cX, affEndY + cY, aff.color, displace, 1.1F, lineWidth);
                    }
                    else
                    {
                        RenderUtils.DrawLine(spriteBatch, new Vector2((float)affStartX1 + cX, (float)affStartY1 + cY), new Vector2((float)affEndX + cX, (float)affEndY + cY), aff.color, lineWidth);
                        RenderUtils.DrawLine(spriteBatch, new Vector2((float)affStartX2 + cX, (float)affStartY2 + cY), new Vector2((float)affEndX + cX, (float)affEndY + cY), aff.color, lineWidth);
                    }

                    //graphics.drawString(getFont(), "%.2f".formatted(depth), (int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY), aff.color());

                    string text = string.Format("{0:n2}", depth);
                    Utility.drawTextWithColoredShadow(spriteBatch, text, Game1.smallFont, new Vector2((int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY + 15)), aff.color, Color.Gray);

                    int xMovement = affDrawTextX > 0 ? 5 : -5;
                    xMovement = affDrawTextX == 0 ? 0 : xMovement;

                    int yMovement = affDrawTextY > 0 ? 5 : -5;
                    yMovement = affDrawTextY == 0 ? 0 : yMovement;

                    int drawX = (int)((affDrawTextX * 1.1) + cX + xMovement);
                    int drawY = (int)((affDrawTextY * 1.1) + cY + yMovement);

                    int drawXToolTip = drawX - 5;
                    int drawYToolTip = drawY + 30;

                    Item essenceForAffinity = affinityHelper.GetEssenceForAffinity(aff);
                    essenceForAffinity.drawInMenu(spriteBatch, new Vector2(drawX, drawY), 0.5f, 100f, 100, StackDrawType.Draw, Color.White, false);
                    //essenceForAffinity.
                    //essenceForAffinity.drawTooltip(spriteBatch, ref drawX, ref drawY, Game1.smallFont, 100f, new StringBuilder().Append(aff.GetId()));
                    //essenceForAffinity.drawTooltip(spriteBatch, ref drawXToolTip, ref drawYToolTip, Game1.smallFont, 100f, new StringBuilder().Append(essenceForAffinity.ItemId));

                    //IClickableMenu.drawTooltip(spriteBatch, ref drawX, ref drawY, Game1.smallFont, 100f,new StringBuilder().Append(aff.GetId()));
                    
                    var input = modEntry.Helper.Input;

                    if (input.GetState(SButton.LeftShift) == SButtonState.Held)
                        Utility.drawTextWithColoredShadow(spriteBatch, ModEntry.INSTANCE.Helper.Translation.Get($"affinity.{aff.GetId()}.name"), Game1.smallFont, new Vector2((int)((affDrawTextX * 0.9) + cX), (int)((affDrawTextY * 0.9) + cY + 1)), aff.color, Color.Gray, 0.8f);

                    //if(essenceForAffinity.)
                    //IClickableMenu.drawToolTip(spriteBatch, essenceForAffinity.Name, null, null);
                    //essenceForAffinity.drawAttachments(spriteBatch, drawX, drawY);

                    if (mouseX <= drawX || mouseX >= drawX + 16 || mouseY <= drawY || mouseY >= drawY + 16) continue;
                }
            }
        }
        public override void MouseHover(float mouseX, float mouseY)
        {
            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            //positionSkillHoverRect(mouseX, mouseY, skillTree);
        }

        public override void MouseClicked(float mouseX, float mouseY)
        {
            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;
            
        }

        public override void MouseScroll(int direction)
        {
            ModEntry modEntry = parent.modEntry;
            Farmer player = Game1.player;
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            
        }
    }
}
