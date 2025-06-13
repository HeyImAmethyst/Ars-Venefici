using ArsVenefici.Framework.Spells.Effects;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Interfaces
{
    public class ProjectileEntityWrapper : IEntity
    {
        private object _entity;
        public object entity { get { return _entity; } }

        public ProjectileEntityWrapper(SpellProjectile spellProjectile)
        {
            _entity = spellProjectile;
        }

        public GameLocation GetGameLocation()
        {
            return ((SpellProjectile)entity).GetGameLocation();
        }

        public Vector2 GetPosition()
        {
            return ((SpellProjectile)entity).position.Get();
        }

        public Rectangle GetBoundingBox()
        {
            return ((SpellProjectile)_entity).getBoundingBox();
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
