using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Commands
{
    public abstract class Command
    {
        protected ModEntry modEntry;

        public Command(ModEntry modEntry) 
        {
            this.modEntry = modEntry;
        }
    }
}
