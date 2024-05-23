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
    }
}
