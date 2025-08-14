using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Skill
{
    public class GenericProfession : SpaceCore.Skills.Skill.Profession
    {
        /*********
        ** Accessors
        *********/
        public string Name { get; set; }
        public string Description { get; set; }


        /*********
        ** Public methods
        *********/
        public GenericProfession(ArsVeneficiSkill skill, string theId)
            : base(skill, theId) { }

        public override string GetName()
        {
            return this.Name;
        }

        public override string GetDescription()
        {
            return this.Description;
        }

        public virtual T GetValue<T>()
        {
            return default(T);
        }
    }
}
