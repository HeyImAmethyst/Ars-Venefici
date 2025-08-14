using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class ArsShapes
    {
        public static ObjectRegister<AbstractShape> SHAPES = ObjectRegister<AbstractShape>.Create(ModEntry.ArsVenificiModId);

        public static ObjectHolder<AbstractShape> SELF = RegisterShape(new ObjectHolder<AbstractShape>(new Self()));
        public static ObjectHolder<AbstractShape> PROJECTILE = RegisterShape(new ObjectHolder<AbstractShape>(new Projectile()));
        public static ObjectHolder<AbstractShape> TOUCH = RegisterShape(new ObjectHolder<AbstractShape>(new Touch()));
        public static ObjectHolder<AbstractShape> ETHEREALTOUCH = RegisterShape(new ObjectHolder<AbstractShape>(new EtherealTouch()));
        public static ObjectHolder<AbstractShape> AOE = RegisterShape(new ObjectHolder<AbstractShape>(new AoE()));
        public static ObjectHolder<AbstractShape> ZONE = RegisterShape(new ObjectHolder<AbstractShape>(new Zone()));
        public static ObjectHolder<AbstractShape> WAVE = RegisterShape(new ObjectHolder<AbstractShape>(new Wave()));
        public static ObjectHolder<AbstractShape> BEAM = RegisterShape(new ObjectHolder<AbstractShape>(new Beam()));
        public static ObjectHolder<AbstractShape> WALL = RegisterShape(new ObjectHolder<AbstractShape>(new Wall()));
        public static ObjectHolder<AbstractShape> RUNE = RegisterShape(new ObjectHolder<AbstractShape>(new Shape.Rune()));
        public static ObjectHolder<AbstractShape> CHANNEL = RegisterShape(new ObjectHolder<AbstractShape>(new Channel()));
        public static ObjectHolder<AbstractShape> CONTINGENCY_HEALTH = RegisterShape(new ObjectHolder<AbstractShape>(new Contingency("contingency_health", ContingencyType.HEALTH)));
        public static ObjectHolder<AbstractShape> CONTINGENCY_DAMAGE = RegisterShape(new ObjectHolder<AbstractShape>(new Contingency("contingency_damage", ContingencyType.DAMAGE)));
        public static ObjectHolder<AbstractShape> CONE = RegisterShape(new ObjectHolder<AbstractShape>(new Cone()));

        public static ObjectHolder<AbstractShape> RegisterShape(ObjectHolder<AbstractShape> obj)
        {
            ObjectHolder<AbstractShape> toReturn = SHAPES.Register(obj);
            ModEntry.INSTANCE.Monitor.Log("Registered Shape " + obj.Get().GetId(), StardewModdingAPI.LogLevel.Info);
            return toReturn;
        }
    }
}
