using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Interfaces
{
    public interface IEntity
    {
        object entity { get; }

        GameLocation GetGameLocation();

        Vector2 GetPosition();

        Rectangle GetBoundingBox();

        int GetHorizontalMovement();

        int GetVerticalMovement();
    }
}
