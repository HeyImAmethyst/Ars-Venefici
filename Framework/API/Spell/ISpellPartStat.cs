using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Spell
{
    /// <summary>
    /// Interface representing a spell part stat.
    /// </summary>
    public interface ISpellPartStat
    {

        /// <returns>The id of this spell part stat.</returns>
        string GetId();

        bool equals(ISpellPartStat other)
        {
            return GetId().Equals(other.GetId());
        }
    }
}
