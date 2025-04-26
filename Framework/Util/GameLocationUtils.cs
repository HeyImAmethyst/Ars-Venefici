using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Spell.Components;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using xTile;
using static StardewValley.Minigames.TargetGame;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ArsVenefici.Framework.Util
{
    public static class GameLocationUtils
    {
        public static HitResult GetHitResult(Vector2 from, Vector2 to, IEntity entity)
        {
            HitResult hitResult = Clip(entity, from, to);

            if (hitResult.GetHitResultType() != HitResult.HitResultType.MISS)
            {
                to = hitResult.GetLocation();
            }

            Rectangle rectangle = entity.GetBoundingBox();
            //rectangle.Inflate(entity.GetHorizontalMovement(), entity.GetVerticalMovement());

            //entity.GetBoundingBox().expandTowards(entity.getDeltaMovement()).inflate(1)
            HitResult entityHitResult = GetCharacterHitResult(entity, from, to, rectangle, e => true, 0.3);

            if (entityHitResult != null)
            {
                hitResult = entityHitResult;
            }

            return hitResult;
        }

        public static CharacterHitResult GetCharacterHitResult(IEntity entity, Vector2 from, Vector2 to, Rectangle aABB, Predicate<Character> predicate, double d)
        {
            GameLocation level = entity.GetGameLocation();

            double e = d;
            Character character = null;
            Vector2 location = new Vector2();

            var characters = GetCharacters(entity, aABB);

            //ModEntry.INSTANCE.Monitor.Log(characters.Count().ToString(), StardewModdingAPI.LogLevel.Info);

            if (characters != null || characters.Count() > 0)
            {

                var var12 = characters.GetEnumerator();

                if (var12.MoveNext())
                {
                    Character currentCharacter = var12.Current;

                    Rectangle aABB2 = currentCharacter.GetBoundingBox();

                    //if (currentCharacter.Tile == new Vector2(aABB.X, aABB.Y))
                    if (currentCharacter.Tile == to)
                    {
                        character = currentCharacter;
                        location = from;
                    }

                    if (character == null)
                    {
                        return null;
                    }

                    return new CharacterHitResult(character, location);
                }
            }

            return null;

            // var var12 = GetCharacters(entity, aABB, predicate).GetEnumerator();

            //while (true)
            //{
            //    while (var12.MoveNext())
            //    {
            //        Character currentCharacter = var12.Current;

            //        Rectangle aABB2 = currentCharacter.GetBoundingBox();
            //        aABB2.Inflate(0, 0);

            //        Vector2 intersectionPoint = Vector2.Zero;
            //        //Optional<Vec3> optional = aABB2.clip(vec3, vec32);

            //        bool optional = LineIntersectsRect(from, to, aABB2, out intersectionPoint);

            //        if (aABB2.Contains(from))
            //        {
            //            if (e >= 0.0)
            //            {
            //                character = currentCharacter;
            //                //vec33 = (Vec3)optional.orElse(vec3);
            //                location = intersectionPoint == Vector2.Zero ? from : intersectionPoint;
            //                e = 0.0;
            //            }

            //        }
            //        else if (optional)
            //        {
            //            //Vector2 vec34 = (Vec3)optional.get();
            //            Vector2 lineIntersection = intersectionPoint;

            //            //double f = vec3.distanceToSqr(vec34);
            //            double f = Vector2.DistanceSquared(from, lineIntersection);

            //            if (f < e || e == 0.0)
            //            {
            //                character = currentCharacter;
            //                location = lineIntersection;
            //                e = f;
            //            }
            //        }
            //    }

            //    if (character == null)
            //    {
            //        return null;
            //    }

            //    return new CharacterHitResult(character, location);
            //}
        }

        public static TerrainFeatureHitResult Clip(IEntity entity, Vector2 from, Vector2 to)
        {
            GameLocation level = entity.GetGameLocation();

            VectorLine vectorLine = new VectorLine(from, to);
            Vector2[] points = vectorLine.GetPoints(100);

            //float dir = (float)-Math.Atan2(character.getStandingPosition().Y - toLocation.Y, toLocation.X - character.getStandingPosition().X);
            float dir = (float)-Math.Atan2(entity.GetPosition().Y - to.Y, to.X - entity.GetPosition().X);
            //float dir = (float)-Math.Atan2(to.Y - entity.GetPosition().Y, to.X - entity.GetPosition().X);

            //Vector2 dPos = from - to;
            //Vector2 dPos = to - from;
            //var dir = (float)Math.Atan2(dPos.Y, dPos.X);

            //var dir = Utils.GetDirectionFromVectors(from, to);

            foreach (var item in points)
            {
                if (level.terrainFeatures.TryGetValue(item, out TerrainFeature terrainFeature))
                {
                    //return new TerrainFeatureHitResult(from, dir, new TilePos(item), false);
                    return new TerrainFeatureHitResult(item, dir, new TilePos(item), false);
                }
                else if (level.objects.TryGetValue(item, out StardewValley.Object obj))
                {
                    //return new TerrainFeatureHitResult(from, dir, new TilePos(item), false);
                    return new TerrainFeatureHitResult(item, dir, new TilePos(item), false);
                }
                else 
                {
                    //return new TerrainFeatureHitResult(from, dir, new TilePos(item), false);
                    continue;
                }
            }

            //return TerrainFeatureHitResult.Miss(from, dir, new TilePos(to));
            return TerrainFeatureHitResult.Miss(to, dir, new TilePos(to));
        }

        //Code based on https://gamedev.stackexchange.com/questions/191285/generate-a-2d-field-of-view-cone
        public static List<HitResult> GetTilesInCone(IEntity entity, Vector2 from, int range)
        {
            List<HitResult> list = new List<HitResult>();

            Farmer farmer = entity.entity as Farmer;
            GameLocation level = entity.GetGameLocation();

            //int facingAngle = 0;

            //if (farmer.FacingDirection == 1) //right
            //{
            //    facingAngle = 0;
            //}

            //if (farmer.FacingDirection == 3) //left
            //{
            //    facingAngle = 180;
            //}

            //if (farmer.FacingDirection == 0) // up
            //{
            //    facingAngle = 90;
            //}

            //if (farmer.FacingDirection == 2) // down
            //{
            //    facingAngle = 270;
            //}

            //0 degrees points up, 90 degrees points right, 180 degrees points down, and 270 degrees points left

            int facingAngle = 90;

            if (farmer.FacingDirection == 1) //right
            {
                facingAngle = 90;
            }

            if (farmer.FacingDirection == 3) //left
            {
                facingAngle = 270;
            }

            if (farmer.FacingDirection == 0) // up
            {
                facingAngle = 0;
            }

            if (farmer.FacingDirection == 2) // down
            {
                facingAngle = 180;
            }

            //float minAngle = MathHelper.ToRadians(facingAngle - 45);
            //float maxAngle = MathHelper.ToRadians(facingAngle + 45);

            float minAngle = facingAngle - 45;
            float maxAngle = facingAngle + 45;

            //minAngle = Utils.WrapValue(minAngle, 0, 360);
            //maxAngle = Utils.WrapValue(maxAngle, 0, 360);

            //ModEntry.INSTANCE.Monitor.Log("minAngle:" + minAngle, StardewModdingAPI.LogLevel.Info);
            //ModEntry.INSTANCE.Monitor.Log("maxAngle:" + maxAngle, StardewModdingAPI.LogLevel.Info);

            if (minAngle > maxAngle)
            {
                float temp = minAngle;

                minAngle = maxAngle;
                maxAngle = temp;

                //ModEntry.INSTANCE.Monitor.Log("minAngleNew:" + minAngle, StardewModdingAPI.LogLevel.Info);
                //ModEntry.INSTANCE.Monitor.Log("maxAngleNew:" + maxAngle, StardewModdingAPI.LogLevel.Info);
            }

            List<Vector2> positionsToAffect = new List<Vector2>();

            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Vector2 sample = new Vector2(x, y);
                    float sampleDistance = sample.Length();

                    Vector2 position = sample + farmer.Tile;

                    //float theta = (float)Math.Acos(sample.X / sampleDistance);
                    float theta = (float)Math.Atan2(sample.X, -sample.Y);
                    float thetaDegrees = MathHelper.ToDegrees(theta);

                    thetaDegrees = Utils.WrapValue(thetaDegrees, 0, 360);

                    if(facingAngle == 0)
                    {
                        thetaDegrees = MathHelper.ToDegrees(theta);
                    }

                    //float normalizedAngle = Utils.RemapValue(thetaDegrees, minAngle, maxAngle, 0, 360);
                    //ModEntry.INSTANCE.Monitor.Log("thetaDegrees:" + thetaDegrees, StardewModdingAPI.LogLevel.Info);

                    if (sampleDistance <= range)
                    {
                        if (thetaDegrees >= minAngle && thetaDegrees <= maxAngle)
                        {
                            positionsToAffect.Add(position);
                        }
                    }
                }
            }

            foreach (Vector2 position in positionsToAffect)
            {
                float dir = (float)-Math.Atan2(entity.GetPosition().Y - position.Y, position.X - entity.GetPosition().X);

                if (level.terrainFeatures.TryGetValue(position, out TerrainFeature terrainFeature))
                {
                    TerrainFeatureHitResult terrainFeatureHitResult = new TerrainFeatureHitResult(position, dir, new TilePos(position), false);
                    list.Add(terrainFeatureHitResult);
                }
                else if (level.objects.TryGetValue(position, out StardewValley.Object obj))
                {
                    TerrainFeatureHitResult terrainFeatureHitResult = new TerrainFeatureHitResult(position, dir, new TilePos(position), false);
                    list.Add(terrainFeatureHitResult);
                }
                else
                {
                    foreach (Character character in level.characters)
                    {
                        if(character.Tile == position)
                        {
                            CharacterHitResult characterHitResult = new CharacterHitResult(character);
                            list.Add(characterHitResult);
                        }
                    }
                }
            }

            //ModEntry.INSTANCE.Monitor.Log("positionsToAffect:" + positionsToAffect.Count, StardewModdingAPI.LogLevel.Info);
            //ModEntry.INSTANCE.Monitor.Log("--------------------------", StardewModdingAPI.LogLevel.Info);

            return list;
        }

        public static List<Character> GetCharacters(IEntity entity, Rectangle aABB, Predicate<Character> predicate)
        {
            List<Character> list = new List<Character>();

            GameLocation location = entity.GetGameLocation();

            //if (location.isCollidingPosition(entity.GetBoundingBox(), Game1.viewport, entity))
            //{
            //    Character character = isCollidingWithCharacter(aABB, location);

            //    if (character != entity && predicate != null && predicate.Invoke(character))
            //    {
            //        list.Add(character);
            //    }
            //}

            if (location != null)
            {
                foreach (Character character in location.characters)
                {
                    if (character.GetBoundingBox().Intersects(aABB))
                    {

                        //if(entity.entity is Character)
                        //{
                        //    if (character != entity && predicate != null && predicate.Invoke(character))
                        //    {
                        //        list.Add(character);
                        //    }
                        //}
                        //else
                        //{
                        //    if (predicate != null && predicate.Invoke(character))
                        //    {
                        //        list.Add(character);
                        //    }
                        //}

                        if (predicate != null && predicate.Invoke(character))
                        {
                            list.Add(character);
                        }

                    }
                }

                foreach (Farmer farmer in location.farmers)
                {
                    if (farmer.GetBoundingBox().Intersects(aABB))
                    {

                        //if (entity.entity is Farmer)
                        //{
                        //    if (farmer != entity && predicate != null && predicate.Invoke(farmer))
                        //    {
                        //        list.Add(farmer);
                        //    }
                        //}
                        //else
                        //{
                        //    if (predicate != null && predicate.Invoke(farmer))
                        //    {
                        //        list.Add(farmer);
                        //    }
                        //}

                        if (predicate != null && predicate.Invoke(farmer))
                        {
                            list.Add(farmer);
                        }

                    }
                }
            }

            return list;
        }

        public static List<Character> GetCharacters(IEntity entity, Rectangle aABB)
        {
            List<Character> list = new List<Character>();

            GameLocation location = entity.GetGameLocation();

            if(location != null)
            {
                foreach (Character character in location.characters)
                {
                    if (character.GetBoundingBox().Intersects(aABB))
                    {
                        list.Add(character);
                    }
                }

                foreach (Farmer farmer in location.farmers)
                {
                    if (farmer.GetBoundingBox().Intersects(aABB))
                    {
                        list.Add(farmer);
                    }
                }
            }

            return list;
        }

        public static List<Character> GetCharacters(IEntity entity, Vector2 point)
        {
            List<Character> list = new List<Character>();

            GameLocation location = entity.GetGameLocation();

            if (location != null)
            {
                foreach (Character character in location.characters)
                {
                    if (character.Tile == point)
                    {
                        list.Add(character);
                    }
                }

                foreach (Farmer farmer in location.farmers)
                {
                    if (farmer.Tile == point)
                    {
                        list.Add(farmer);
                    }
                }
            }

            return list;
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
