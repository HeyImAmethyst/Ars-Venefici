using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ArsVenefici.Framework.Spells.Effects
{
    public class ZoneEffect : AbstractSpellEffect
    {
        ModEntry modEntry;

        private float radius;
        private ISpell spell;
        private int index;
        private readonly Texture2D Tex;

        public ZoneEffect(ModEntry modEntry, ISpell spell, Vector2 pos, float radius, int dur) : base(pos, dur) 
        {
            this.modEntry = modEntry;

            this.spell = spell;
            this.radius = radius;

            //this.Tex = modEntry.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");
            this.Tex = new Texture2D(Game1.graphics.GraphicsDevice, (int)radius, (int)radius);


            Color manaCol = new Color(0, 48, 255);
            Tex.SetData(new[] { manaCol });

            //SetBoundingBox(new Rectangle((int)(pos.X), (int)(pos.Y), 1, 1);
            //SetBoundingBox(new Rectangle((int)(this.pos.X - Game1.tileSize), (int)(this.pos.Y - Game1.tileSize), Game1.tileSize / 2, Game1.tileSize / 2));
            SetBoundingBox(new Rectangle((int)(this.pos.X), (int)(this.pos.Y), (int)radius, (int)radius));
        }

        public override bool Update(UpdateTickedEventArgs e)
        {
            IEntity owner = GetOwner();

            int index = GetIndex();
            float radius = GetRadius();
            ISpell spell = GetSpell();

            //forAllInRange(radius, false, e->ArsMagicaAPI.get().getSpellHelper().invoke(spell, owner, level, new EntityHitResult(e), tickCount, index, true));

            var spellHelper = SpellHelper.Instance();

            ForAllInRange((int)radius, false, e => spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new CharacterHitResult(e), 0, index, true));

            List<Vector2> list = new List<Vector2>();

            for (int x = (int)-radius; x <= (int)radius; x++)
            {
                for (int y = -GetBoundingBox().Height; y <= GetBoundingBox().Height; y++)
                {
                    list.Add(new Vector2(pos.X + x, pos.Y + y));
                }
            }

            foreach (Vector2 vec2 in list)
            {
                HitResult result = GameLocationUtils.GetHitResult(vec2, Vector2.Add(vec2, Vector2.One), this);
                //modEntry.Monitor.Log("Effect", StardewModdingAPI.LogLevel.Info);
                spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), result, 0, index, true);
            }

            return --this.duration > 0 || GetOwner() == null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)radius, (int)radius);

            //r = Game1.GlobalToLocal(Game1.viewport, r);
            //Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((int)r.X, (int)r.Y));
            Vector2 tilePos = new Vector2((int)r.X, (int)r.Y);
            Vector2 absolutePos = new Vector2(tilePos.X * Game1.tileSize, tilePos.Y * Game1.tileSize);

            Vector2 localPos = Game1.GlobalToLocal(absolutePos);

            //Vector2 toolLocation = ((Character)GetOwner().entity).getStandingPosition();
            //float rot = (float)-Math.Atan2(this.pos.Y - toolLocation.Y, toolLocation.X - this.pos.X);

            //modEntry.Monitor.Log(localPos.ToString(), StardewModdingAPI.LogLevel.Info);

            //spriteBatch.Draw(tex, new Vector2(r.X, r.Y), new Rectangle(0, 0, (int)radius, (int)radius), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, r.Y / 10000f);
            //spriteBatch.Draw(tex, localPos, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, localPos.Y / 10000f);
            spriteBatch.Draw(Tex, localPos, null, Color.Red);
        }

        public override int GetDuration()
        {
            return this.duration;
        }

        public void SetDuration(int duration)
        {
            this.duration = duration;
        }

        public override void SetOwner(IEntity owner)
        {
            this.owner = owner;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float radius)
        {
            this.radius = radius;
        }

        public ISpell GetSpell()
        {
            return spell;
        }

        public void SetSpell(ISpell spell)
        {
            this.spell = spell;
        }
    }
}
