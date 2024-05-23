using StardewValley.TerrainFeatures;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ArsVenefici.Framework.Spells.Effects;

namespace ArsVenefici.Framework.Util
{
    public static class GameLocationUtils
    {

        public static HitResult GetHitResult(Vector2 from, Vector2 to, Character entity)
        {
            HitResult hitResult = Clip(entity, from, to);

            if (hitResult.GetHitResultType() != HitResult.HitResultType.MISS)
            {
                to = hitResult.GetLocation();
            }

            Rectangle rectangle = entity.GetBoundingBox();
            rectangle.Inflate(entity.getHorizontalMovement(), entity.getVerticalMovement());

            //entity.GetBoundingBox().expandTowards(entity.getDeltaMovement()).inflate(1)
            HitResult entityHitResult = GetCharacterHitResult(entity, from, to, rectangle, e => true, 0.3);

            if (entityHitResult != null)
            {
                hitResult = entityHitResult;
            }

            return hitResult;
        }

        public static CharacterHitResult GetCharacterHitResult(Character entity, Vector2 vec3, Vector2 vec32, Rectangle aABB, Predicate<Character> predicate, double d)
        {
            GameLocation level = entity.currentLocation;

            double e = d;
            Character entity2 = null;
            Vector2 vec33 = new Vector2();

            var var12 = GetCharacters(entity, aABB, predicate).GetEnumerator();

            while (true)
            {
                while (var12.MoveNext())
                {
                    Character entity3 = var12.Current;

                    Rectangle aABB2 = entity3.GetBoundingBox();
                    aABB2.Inflate(0, 0);

                    Vector2 intersectionPoint = new Vector2();
                    //Optional<Vec3> optional = aABB2.clip(vec3, vec32);

                    bool optional = LineIntersectsRect(vec3, vec32, aABB2, out intersectionPoint);

                    if (aABB2.Contains(vec3))
                    {
                        if (e >= 0.0)
                        {
                            entity2 = entity3;
                            //vec33 = (Vec3)optional.orElse(vec3);
                            vec33 = intersectionPoint == Vector2.Zero ? vec3 : intersectionPoint;
                            e = 0.0;
                        }
                    }
                    else if (optional)
                    {
                        //Vector2 vec34 = (Vec3)optional.get();
                        Vector2 vec34 = intersectionPoint;

                        //double f = vec3.distanceToSqr(vec34);
                        double f = Vector2.DistanceSquared(vec3, vec34);

                        if (f < e || e == 0.0)
                        {
                            entity2 = entity3;
                            vec33 = vec34;
                            e = f;
                        }
                    }
                }

                if (entity2 == null)
                {
                    return null;
                }

                return new CharacterHitResult(entity2, vec33);
            }
        }

        public static TerrainFeatureHitResult Clip(Character entity, Vector2 from, Vector2 to)
        {
            GameLocation level = entity.currentLocation;

            VectorLine vectorLine = new VectorLine(from, to);
            Vector2[] points = vectorLine.GetPoints(100);

            float dir = (float)-Math.Atan2(entity.getStandingPosition().Y - to.Y, to.X - entity.getStandingPosition().X);

            foreach (var item in points)
            {
                if (level.terrainFeatures.TryGetValue(item, out TerrainFeature terrainFeature))
                {
                    return new TerrainFeatureHitResult(from, dir, new TilePos(item), false);
                }

                if (level.objects.TryGetValue(item, out StardewValley.Object obj))
                {
                    return new TerrainFeatureHitResult(from, dir, new TilePos(item), false);
                }
            }

            return TerrainFeatureHitResult.Miss(from, dir, new TilePos(from));
        }

        public static List<Character> GetCharacters(Character entity, Rectangle aABB, Predicate<Character> predicate)
        {
            List<Character> list = new List<Character>();

            GameLocation location = entity.currentLocation;

            if (location.isCollidingPosition(entity.GetBoundingBox(), Game1.viewport, entity))
            {
                Character character = isCollidingWithCharacter(aABB, location);

                if (character != entity && predicate != null && predicate.Invoke(character))
                {
                    list.Add(character);
                }
            }

            return list;
        }

        public static Character isCollidingWithCharacter(Rectangle box, GameLocation location)
        {
            foreach (Character character in location.characters)
            {
                if (character.GetBoundingBox().Intersects(box))
                    return character;
            }

            return null;
        }

        public static bool LineIntersectsRect(Vector2 p1, Vector2 p2, Rectangle r, out Vector2 intersectionPoint)
        {
            return LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), out intersectionPoint) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), out intersectionPoint) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height), out intersectionPoint) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y), out intersectionPoint) ||
                   (r.Contains(p1) && r.Contains(p2));
        }

        private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2, out Vector2 intersectionPoint)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                intersectionPoint = default;
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                intersectionPoint = default;
                return false;
            }

            Vector2 t0 = l1p2 - l1p1;
            Vector2 t1 = l2p2 - l2p1;
            Vector2 t2 = l2p1 - l1p1;

            float dotDPerp = t0.X * t1.Y - t0.Y * d;
            float t = (t2.X * t1.Y - t2.Y * t1.X) / dotDPerp;

            Vector2 intersection = l1p1 + t * t0;

            intersectionPoint = intersection;

            return true;
        }
    }
}
