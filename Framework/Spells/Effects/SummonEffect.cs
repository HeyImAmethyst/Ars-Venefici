using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Effects
{
    //Class code from https://github.com/spacechase0/StardewValleyMods/blob/main/Magic/Framework/Spells/Effects/SpiritEffect.cs
    public class SummonEffect : AbstractSpellEffect
    {
        ModEntry modEntry;

        private ISpell spell;
        private int index;
        private readonly Texture2D Tex;

        private GameLocation PrevSummonerLoc;
        private int AttackTimer;
        private int AnimTimer;
        private int AnimFrame;

        public SummonEffect(ModEntry modEntry, IEntity source, ISpell spell, Vector2 pos, int dur) : base(modEntry, pos, dur)
        {
            this.modEntry = modEntry;

            this.spell = spell;

            this.Tex = Game1.content.Load<Texture2D>("Characters\\Junimo");
            this.pos = source.GetPosition();

            this.PrevSummonerLoc = source.GetGameLocation();
        }

        public override void Update(UpdateTickedEventArgs e)
        {
            if (Game1.activeClickableMenu == null && Game1.game1.IsActive)
                isActive = --this.duration > 0 || GetOwner() == null;

            if (this.PrevSummonerLoc != Game1.currentLocation)
            {
                this.PrevSummonerLoc = Game1.currentLocation;
                this.pos = this.GetOwner().GetPosition();
            }

            float nearestDist = float.MaxValue;
            Monster nearestMob = null;

            foreach (var character in this.GetOwner().GetGameLocation().characters)
            {
                if (character is Monster mob)
                {
                    float dist = Utility.distance(mob.GetBoundingBox().Center.X, this.GetOwner().GetPosition().X, mob.GetBoundingBox().Center.Y, this.GetOwner().GetPosition().Y);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestMob = mob;
                    }
                }
            }

            if (this.AttackTimer > 0)
                --this.AttackTimer;

            //ModEntry.INSTANCE.Monitor.Log(AttackTimer.ToString(), StardewModdingAPI.LogLevel.Info);

            if (nearestMob != null)
            {
                Vector2 unit = nearestMob.Position - this.pos;
                unit.Normalize();

                this.pos += unit * 4;

                if (Utility.distance(nearestMob.Position.X, this.pos.X, nearestMob.Position.Y, this.pos.Y) < Game1.tileSize)
                {
                    if (this.AttackTimer <= 0)
                    {
                        if(this.GetOwner().entity is Farmer farmer)
                        {
                            this.AttackTimer = 45;
                            int baseDmg = 20 * (farmer.CombatLevel + 1);
                            var oldPos = farmer.Position;
                            farmer.Position = new Vector2(nearestMob.GetBoundingBox().Center.X, nearestMob.GetBoundingBox().Center.Y);
                            farmer.currentLocation.damageMonster(nearestMob.GetBoundingBox(), (int)(baseDmg * 0.75f), (int)(baseDmg * 1.5f), true, 1, 0, 0.1f, 2, false, farmer);
                            farmer.Position = oldPos;
                        }
                    }
                }
            }

            if (!isActive)
            {
                modEntry.ActiveEffects.Remove(this);
            }
        }

        public override void OneSecondUpdate(OneSecondUpdateTickingEventArgs e)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (++this.AnimTimer >= 6)
            {
                this.AnimTimer = 0;
                if (++this.AnimFrame >= 12)
                    this.AnimFrame = 0;
            }

            int tx = (this.AnimFrame % 8) * 16;
            int ty = (this.AnimFrame / 8) * 16;
            spriteBatch.Draw(this.Tex, Game1.GlobalToLocal(Game1.viewport, this.pos), new Rectangle(tx, ty, 16, 16), new Color(1f, 1f, 1f, this.AttackTimer > 0 ? 0.1f : 1f), 0, new Vector2(8, 8), 2, SpriteEffects.None, 1);
        }

        public override int GetDuration()
        {
            return this.duration;
        }

        public override void SetOwner(IEntity owner)
        {
            this.owner = owner;
        }

    }
}
