using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Interfaces
{
    public class CharacterEntityWrapper : IEntity
    {

        private object _entity;
        public object entity { get { return _entity; } }

        public CharacterEntityWrapper(Character character)
        {
            _entity = character;
        }

        public GameLocation GetGameLocation()
        {
            return ((Character)_entity).currentLocation;
        }

        public Vector2 GetPosition()
        {
            return ((Character)_entity).getStandingPosition();
        }

        public Rectangle GetBoundingBox()
        {
            return ((Character)_entity).GetBoundingBox();
        }

        public int GetHorizontalMovement()
        {
            return ((Character)_entity).getHorizontalMovement();
        }

        public int GetVerticalMovement()
        {
            return ((Character)_entity).getVerticalMovement();
        }
    }
}
