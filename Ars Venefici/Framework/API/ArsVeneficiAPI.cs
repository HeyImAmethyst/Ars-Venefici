using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ItemExtensions;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Util;

namespace ArsVenefici.Framework.API
{
    public interface ArsVeneficiAPI
    {

        ///// <returns>The API instance.</returns>
        //abstract ArsVeneficiAPI Get();

        /// <returns>The skill helper instance</returns>
        ISpellPartSkillHelper GetSpellPartSkillHelper();

        /// <returns>The skill helper instance</returns>
        IMagicHelper GetMagicHelper();

        /// <returns>The skill helper instance</returns>
        ISpellHelper GetSpellHelper();

        /// <returns>The contingency helper instance</returns>
        IContingencyHelper GetContingencyHelper();

        /// <summary>
        /// Opens the occulus gui for the given farmer.
        /// </summary>
        /// <param name="farmer">The player to open the gui for.</param>
        void OpenMagicAltarGui(Farmer farmer);

        /// <summary>
        /// Opens the spell book gui for the given farmer.
        /// </summary>
        /// <param name="modEntry"></param>
        void OpenSpellBookGui(ModEntry modEntry);

        /// <summary>
        /// Make an instance of ISpell.
        /// </summary>
        /// <param name="shapeGroups">The shape groups to use.</param>
        /// <param name="spellStack">The spell stack to use.</param>
        /// <returns>The spell instance.</returns>
        public ISpell MakeSpell(List<ShapeGroup> shapeGroups, SpellStack spellStack);

        /// <summary>
        /// Make an instance of ISpell.
        /// </summary>
        /// <param name="spellStack">The spell stack to use.</param>
        /// <param name="shapeGroups">The shape groups to use.</param>
        /// <returns>The spell instance.</returns>
        public ISpell MakeSpell(SpellStack spellStack, params ShapeGroup[] shapeGroups);

        //internal sealed class InstanceHolder
        //{
        //    private InstanceHolder() { }
        //    public static readonly Lazy<ArsVeneficiAPI> LAZY_INSTANCE = new Lazy<ArsVeneficiAPI>(() =>
        //    {
        //        var type = typeof(ArsVeneficiAPI);
        //        var types = GetTypesWithInterface(type.Assembly);
        //        var firstImplementations = types.Where(t => !types.Contains(t.BaseType));

        //        var impl = firstImplementations.FirstOrDefault();

        //        //ArsVeneficiAPI instance = ServiceLocator.Current.GetInstance<ArsVeneficiAPI>();

        //        if (impl != null)
        //        {
        //            return impl.;
        //        }
        //        else
        //        {
        //            ModEntry.INSTANCE.Monitor.Log("Unable to find implementation for ArsVeneficiAPI!", StardewModdingAPI.LogLevel.Info);
        //            return null;
        //        }
        //    });

        //    private static IEnumerable<Type> GetTypesWithInterface(Assembly asm)
        //    {
        //        var it = typeof(ArsVeneficiAPI);

        //        return asm.GetLoadableTypes().Where(it.IsAssignableFrom).ToList();
        //    }
        //}
    }
}
