using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Tiles;
using static ArsVenefici.Framework.Interfaces.Spells.ISpellPart;
using static StardewValley.Minigames.TargetGame;

namespace ArsVenefici.Framework.Spells
{
    public class SpellHelper : ISpellHelper
    {
        private static SpellHelper INSTANCE = new SpellHelper();

        private SpellHelper()
        {

        }

        public static SpellHelper Instance()
        {
            return INSTANCE;
        }

        public void SetSpell(SpellBook spellBook, ISpell spell)
        {
            int currentSpellIndex = spellBook.GetCurrentSpellIndex();
            spellBook.GetSpells()[currentSpellIndex] = spell;
        }

        public Character GetPointedCharacter(Character character, double range)
        {
            Vector2 from = character.getStandingPosition();
            //int view = character.FacingDirection;
            int view = (int)-Math.Atan2(character.getStandingPosition().Y - character.GetToolLocation(true).Y, character.GetToolLocation(true).X - character.getStandingPosition().X);
            Vector2 to = new Vector2((float)(from.X + (view * range)), (float)(from.Y + (view * range)));

            Rectangle aabb = new Rectangle(character.GetBoundingBox().X, character.GetBoundingBox().Y, (int)(character.GetBoundingBox().Width + (view * range)), (int)(character.GetBoundingBox().Height + (view * range)));

            //CharacterHitResult hit = ProjectileUtil.getEntityHitResult(entity, from, to, aabb, e-> !e.isSpectator() && e.isPickable(), range * range);
            CharacterHitResult hit = GameLocationUtils.GetCharacterHitResult(character, from, to, aabb, null, range * range);

            //return hit != null && from.distanceTo(hit.getLocation()) < range ? hit.getEntity() : null;
            return hit != null && Vector2.Distance(from, hit.GetLocation()) < range ? hit.GetCharacter() : null;

        }

        public HitResult Trace(ModEntry modEntry, Character entity, GameLocation level, double range, bool entities, bool mouseCursor)
        {
            if (entities)
            {
                Character pointed = GetPointedCharacter(entity, range);
                if (pointed != null) return new CharacterHitResult(pointed);

            }

            Vector2 fromTileVec;

            if (mouseCursor)
            {
                fromTileVec = Utils.ConvertToTilePos(Utility.clampToTile(new Vector2(Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y)));
            }
            else
            {
                fromTileVec = Utils.ConvertToTilePos(Utility.clampToTile(entity.GetToolLocation(true)));
            }

            TilePos fromTilePos = new TilePos(fromTileVec);
            TilePos toTilePos = new TilePos(Vector2.Multiply(fromTileVec, new Vector2((float)range, (float)range)));

            return GameLocationUtils.Clip(entity, fromTilePos.GetVector(), toTilePos.GetVector());
        }

        public float GetModifiedStat(float baseValue, ISpellPartStat stat, List<ISpellModifier> modifiers, ISpell spell, IEntity caster, HitResult target, int componentIndex)
        {
            componentIndex--;
            float modified = baseValue;

            foreach (ISpellModifier iSpellModifier in modifiers)
            {

                foreach (ISpellPartStat item in iSpellModifier.GetStatsModified())
                {
                    if (item.equals(stat))
                    {
                        ISpellPartStatModifier modifier = iSpellModifier.GetStatModifier(stat);
                        modified = modifier.Modify(baseValue, modified, spell, caster, target, componentIndex);
                    }
                }
            }

            return modified;
        }

        public SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, HitResult target, int castingTicks, int index, bool awardXp)
        {
            List<MutableKeyValuePair<ISpellPart, List<ISpellModifier>>> pwm = spell.PartsWithModifiers();

            if(pwm.Count > 0)
            {
                MutableKeyValuePair<ISpellPart, List<ISpellModifier>> pair = pwm[index];
                ISpellPart part = pair.Key;
                List<ISpellModifier> modifiers = pair.Value;

                //modEntry.Monitor.Log("modifiers : " + modifiers.Count.ToString(), StardewModdingAPI.LogLevel.Info);

                switch (part.GetType())
                {
                    case SpellPartType.COMPONENT:
                    {

                        ISpellComponent component = (ISpellComponent)part;
                        SpellCastResult result = new SpellCastResult(SpellCastResultType.EFFECT_FAILED);

                        if (target is CharacterHitResult characterHitResult)
                        {
                            result = component.Invoke(modEntry, spell, caster, level, modifiers, characterHitResult, index + 1, castingTicks);
                            //result = component.Invoke(spell, caster, level, modifiers, characterHitResult, index, castingTicks);

                            if (result.IsSuccess())
                            {
                                Vector2 location = target.GetLocation();
                            }
                        }

                        if (target is TerrainFeatureHitResult terrainFeatureHitResult)
                        {
                            result = component.Invoke(modEntry, spell, caster, level, modifiers, terrainFeatureHitResult, index + 1, castingTicks);

                            if (result.IsSuccess())
                            {
                                Vector2 location = target.GetLocation();
                            }
                        }

                        return result.IsFail() || index + 1 == pwm.Count() ? result : Invoke(modEntry, spell, caster, level, target, castingTicks, index + 1, awardXp);
                    }
                    case SpellPartType.SHAPE:
                    {
                        ISpellShape shape = (ISpellShape)part;
                        return shape.Invoke(modEntry, spell, caster, level, modifiers, target, castingTicks, index + 1, awardXp);
                    }
                    default:
                    {
                        return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                    }
                }
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public void NextShapeGroup(ISpell spell)
        {
            int index = spell.CurrentShapeGroupIndex() + 1;

            //long count = spell.ShapeGroups().stream().filter(e-> !e.isEmpty()).count();
            long count = spell.ShapeGroups().Where(e => !e.IsEmpty()).Count();

            if (index >= count)
            {
                index -= (int)count;
            }

            spell.CurrentShapeGroupIndex(index);

            SetSpell(Game1.player.GetSpellBook(), spell);
        }

        public void PrevShapeGroup(ISpell spell)
        {
            int index = spell.CurrentShapeGroupIndex() - 1;

            //long count = spell.ShapeGroups().stream().filter(e-> !e.isEmpty()).count();
            long count = spell.ShapeGroups().Where(e => !e.IsEmpty()).Count();

            if (index < 0)
            {
                index += (int)count;
            }

            spell.CurrentShapeGroupIndex(index);
            SetSpell(Game1.player.GetSpellBook(), spell);
        }
    }
}
