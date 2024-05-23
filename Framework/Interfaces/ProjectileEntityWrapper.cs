using ArsVenefici.Framework.Spells.Effects;
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
    }
}
