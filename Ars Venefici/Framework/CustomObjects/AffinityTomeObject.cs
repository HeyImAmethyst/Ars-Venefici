using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.Spells.Registry;
using ItemExtensions;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Layers;

namespace ArsVenefici.Framework.CustomObjects
{
    public class AffinityTomeObject : StardewValley.Object, IAffinityObject
    {
        public AffinityTomeObject(string id)
        {
            string itemID = ValidateUnqualifiedItemId(id);
            ItemId = itemID;

            ResetParentSheetIndex();
            ParentSheetIndex = 0;
        }

        public override bool performUseAction(GameLocation location)
        {
            if (!Game1.player.canMove || this.isTemporarilyInvisible)
            {
                return false;
            }

            bool normal_gameplay = !Game1.eventUp && !Game1.isFestival() && !Game1.fadeToBlack && !Game1.player.swimming.Value && !Game1.player.bathingClothes.Value && !Game1.player.onBridge.Value;
            
            if (normal_gameplay && (base.Category == -102 || base.Category == -103))
            {
                ReadBook(location);
                return true;
            }

            return false;
        }

        public void ReadBook(GameLocation location)
        {
            var api = ModEntry.INSTANCE.arsVeneficiAPILoader.GetAPI();

            if (!api.GetMagicHelper().LearnedWizardy(Game1.player))
            {
                Game1.showGlobalMessage(ModEntry.INSTANCE.Helper.Translation.Get("tome.read.error.message"));
                return;
            }
            else
            {

                if (base.ItemId.StartsWith("HeyImAmethyst.ArsVenefici_") && base.ItemId.EndsWith("_Tome"))
                {

                    Game1.player.canMove = false;
                    Game1.player.freezePause = 1030;
                    Game1.player.faceDirection(2);
                    Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[1]
                    {
                        new FarmerSprite.AnimationFrame(57, 1000, secondaryArm: false, flip: false, Farmer.canMoveNow, behaviorAtEndOfFrame: true)
                        {
                            frameEndBehavior = delegate
                            {
                                location.removeTemporarySpritesWithID(1987654);
                                Utility.addRainbowStarExplosion(location, Game1.player.getStandingPosition() + new Vector2(-40f, -156f), 8);
                            }
                        }
                    });

                    Game1.MusicDuckTimer = 4000f;
                    Game1.playSound("book_read");

                    Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("LooseSprites\\Book_Animation", new Microsoft.Xna.Framework.Rectangle(0, 0, 20, 20), 10f, 45, 1, Game1.player.getStandingPosition() + new Vector2(-48f, -156f), flicker: false, flipped: false, Game1.player.getDrawLayer() + 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f)
                    {
                        holdLastFrame = true,
                        id = 1987654
                    });

                    Color? c = ItemContextTagManager.GetColorFromTags(this);

                    if (c.HasValue)
                    {
                        Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("LooseSprites\\Book_Animation", new Microsoft.Xna.Framework.Rectangle(0, 20, 20, 20), 10f, 45, 1, Game1.player.getStandingPosition() + new Vector2(-48f, -156f), flicker: false, flipped: false, Game1.player.getDrawLayer() + 0.0012f, 0f, c.Value, 4f, 0f, 0f, 0f)
                        {
                            holdLastFrame = true,
                            id = 1987654
                        });
                    }

                    var affinityHelper = api.GetAffinityHelper();
                    Affinity affinity = affinityHelper.GetAffinityForItem(this);

                    //float shift = (float)ModEntry.RandomGen.NextDouble();

                    float range = 0.1f;
                    //float shift = (float)((ModEntry.RandomGen.NextDouble() * range) - 1.0);
                    //float shift = (float)((ModEntry.RandomGen.NextDouble() * range) - 1.0);

                    //float shift = (float)((ModEntry.RandomGen.NextDouble() * (0.1 - 0.00001)) + 0.00001);
                    float shift = (float)((Game1.random.NextDouble() * (0.1 - 0.00001)) + 0.00001);

                    foreach (var affinityHolder in Affinities.AFFINITIES.GetObjectList())
                    {
                        Affinity aff = affinityHolder.Get();

                        if (aff.GetId() == Affinity.NONE) continue;

                        if (affinity == aff)
                        {
                            shift *= 1.05f;
                            affinityHelper.IncreaseAffinityDepth(Game1.player, affinity, shift);
                        }
                        
                        if(affinity != aff)
                        {
                            shift *= 1.05f;
                            affinityHelper.DecreaseAffinityDepth(Game1.player, aff, shift);
                        }
                    }

                    DelayedAction.functionAfterDelay(delegate
                    {
                        Game1.showGlobalMessage(ModEntry.INSTANCE.Helper.Translation.Get("tome.read.success.message") + " " + ModEntry.INSTANCE.Helper.Translation.Get($"affinity.{affinity.GetId()}.name"));
                    }, 1000);

                    affinityHelper.UpdateLock(Game1.player);
                }
            }
        }


    }
}

   
