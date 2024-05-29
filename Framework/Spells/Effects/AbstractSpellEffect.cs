using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
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
    public abstract class AbstractSpellEffect : IActiveEffect, IEntity
    {
        protected IEntity owner;
        protected readonly Vector2 pos;
        private Rectangle boundingBox;
        protected int duration;

        public object entity { get { return this; } }

        public AbstractSpellEffect(Vector2 pos, int dur)
        {
            this.pos = pos;
            this.duration = dur;
        }

        public abstract bool Update(UpdateTickedEventArgs e);

        public abstract void Draw(SpriteBatch spriteBatch);

        public IEntity GetOwner()
        { 
            return owner; 
        }

        public abstract void SetOwner(IEntity owner);

        public abstract int GetDuration();

        protected void ForAllInRange(int radius, bool skipOwner, Action<Character> consumer)
        {
            //double x = GetX(), y = GetY(), z = GetZ();
            float x = pos.X, y = pos.Y;

            //var aabb = new AxisAlignedBoundingBox(x - radius, y - radius, z - radius, x + radius, y + radius, z + radius);
            //var aabb = new Rectangle((int)(x - radius), (int)(y - radius), (int)(x + radius), (int)(y + radius));
            var aabb = new Rectangle((int)(x), (int)(y), (int)(radius), (int)(radius));
            
            foreach (var e in GameLocationUtils.GetCharacters(this, aabb))
            {
                //if (e == this) 
                //    continue;
                
                if (skipOwner && e == GetOwner()) 
                    continue;

                //if (e is AbstractSpellEffect) 
                //    continue;

                if (e is Character living)
                {
                    consumer.Invoke(living);
                }
            }
        }

        public GameLocation GetGameLocation()
        {
            return owner.GetGameLocation();
        }

        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }

        public void SetBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        public Vector2 GetPosition()
        {
            return pos;
        }

        public int GetHorizontalMovement()
        {
            return 1;
        }

        public int GetVerticalMovement()
        {
            return 1;
        }
    }
}
