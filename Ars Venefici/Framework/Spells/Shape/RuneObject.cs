using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StardewValley;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.API;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class RuneObject : StardewValley.Object
    {
        ModEntry modEntry;

        ISpell spell;
        HitResult hitResult;
        int index;

        GameLocation gameLocation;

        string itemID = "HeyImAmethyst.ArsVenefici_Rune";

        public RuneObject()
        {

        }

        public RuneObject(ModEntry modEntry, GameLocation gameLocation, HitResult hitResult, int index, ISpell spell, Vector2 tileLocation, int initialStack = 1, bool isRecipe = false, int price = -1, int quality = 0)
        {
            itemID = base.ValidateUnqualifiedItemId(itemID);
            base.stack.Value = initialStack;
            base.isRecipe.Value = isRecipe;
            base.quality.Value = quality;
            base.ItemId = itemID;
            base.ResetParentSheetIndex();

            this.canBeSetDown.Value = true;
            this.canBeGrabbed.Value = false;
            this.isSpawnedObject.Value = false;

            this.TileLocation = tileLocation;

            this.modEntry = modEntry;
            this.gameLocation = gameLocation;
            this.spell = spell;
            this.hitResult = hitResult;
            this.index = index;
            this.ParentSheetIndex = 0;
        }

        public void CastAttatchedSpell(Character who, HitResult hitResult, int index)
        {
            var spellHelper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();
            spellHelper.Invoke(modEntry, spell, new CharacterEntityWrapper(who), who.currentLocation, hitResult, 0, index, false);
        }

        public override void updateWhenCurrentLocation(GameTime time)
        {
            base.updateWhenCurrentLocation(time);

            foreach (Character character in gameLocation.characters.ToList())
            {
                if (gameLocation.objects.TryGetValue(this.TileLocation, out var obj))
                {
                    if (character != null)
                    {
                        Vector2 characterTilePos = character.Tile;

                        if (characterTilePos == TileLocation)
                        {
                            CastAttatchedSpell(Game1.player, new CharacterHitResult(character), index);
                            shakeTimer = 200;
                            gameLocation.objects.Remove(this.TileLocation);
                        }
                    }
                }
            }

            foreach (Farmer farmer in gameLocation.farmers.ToList())
            {
                if (gameLocation.objects.TryGetValue(this.TileLocation, out var obj))
                {
                    if (farmer != null)
                    {
                        Vector2 characterTilePos = farmer.Tile;

                        if (characterTilePos == TileLocation)
                        {
                            CastAttatchedSpell(farmer, new CharacterHitResult(farmer), index);
                            shakeTimer = 200;
                            gameLocation.objects.Remove(this.TileLocation);
                        }
                    }
                    
                }
            }
        }

        public override bool isPassable()
        {
            return true;
        }

        public override bool IsFloorPathItem()
        {
            return true;
        }

        public void SetModEntry(ModEntry modEntry)
        {
            this.modEntry = modEntry;
        }

        public void SetSpell(ISpell spell)
        {
            this.spell = spell;
        }

        public ISpell GetSpell()
        {
            return this.spell;
        }

        public void SetHitResult (HitResult hitResult)
        {
            this.hitResult = hitResult;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public void SetGameLocation(GameLocation gameLocation)
        {
            this.gameLocation = gameLocation;
        }

        public void SetItemID(string itemID)
        {
            this.itemID = itemID;
        }
    }
}
