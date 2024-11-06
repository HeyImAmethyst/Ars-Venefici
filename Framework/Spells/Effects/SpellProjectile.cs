using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Tiles;

namespace ArsVenefici.Framework.Spells.Effects
{
    public class SpellProjectile : Projectile , IEntity
    {
        private ModEntry modEntry;

        public object entity { get { return this; } }

        private IEntity Source;
        private ISpell spell;

        private NetFloat Direction = new();
        private NetFloat Velocity = new();
        private NetInt Index = new();

        private Texture2D Tex;
        //private readonly NetString TexId = new();
        private static readonly Random Rand = new();

        private SpellProjectile(ModEntry modEntry, IEntity source)
        {
            this.modEntry = modEntry;
            Source = source;

            NetFields.AddField(Direction);
            NetFields.AddField(Velocity);
            NetFields.AddField(Index);
            //this.NetFields.AddField(this.TexId);

            Tex = modEntry.Helper.ModContent.Load<Texture2D>("assets/projectile/projectile.png");

            damagesMonsters.Value = true;
        }

        public SpellProjectile(ModEntry modEntry, IEntity source, ISpell spell, float direction, int index, float velocity)
            : this(modEntry, source)
        {
            Source = source;
            this.spell = spell;
            Index.Value = index;
            Direction.Value = direction;
            Velocity.Value = velocity;

            if(source.entity is Character character)
            {
                theOneWhoFiredMe.Set(character.currentLocation, character);

                position.Value = character.getStandingPosition();
                position.X += character.GetBoundingBox().Width;
                position.Y += character.GetBoundingBox().Height;
            }

            rotation = direction;
            xVelocity.Value = (float)Math.Cos(Direction.Value) * Velocity.Value;
            yVelocity.Value = (float)Math.Sin(Direction.Value) * Velocity.Value;

            //this.Tex = Content.LoadTexture($"magic/{this.Spell.ParentSchoolId}/{this.Spell.Id}/projectile.png");
            //this.TexId.Value = Content.LoadTextureKey($"magic/{this.Spell.ParentSchoolId}/{this.Spell.Id}/projectile.png");

            //this.Tex = modEntry.Helper.ModContent.Load<Texture2D>("assets/projectile/rock.png");
            //this.TexId.Value = modEntry.Helper.ModContent.($"magic/{this.Spell.ParentSchoolId}/{this.Spell.Id}/projectile.png");
        }

        //
        // Summary:
        //     Handle the projectile hitting an obstacle.
        //
        // Parameters:
        //   location:
        //     The location containing the projectile.
        //
        //   target:
        //     The target player or monster that was hit, if applicable.
        //
        //   terrainFeature:
        //     The terrain feature that was hit, if applicable.
        private void behaviorOnCollision(GameLocation location, Character target, TerrainFeature terrainFeature)
        {
            bool flag = true;
            Farmer farmer = target as Farmer;
            if (farmer == null)
            {
                NPC nPC = target as NPC;
                if (nPC != null)
                {
                    if (!nPC.IsInvisible)
                    {
                        behaviorOnCollisionWithMonster(nPC, location);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (terrainFeature != null)
                {
                    behaviorOnCollisionWithTerrainFeature(terrainFeature, terrainFeature.Tile, location);
                }
                else
                {
                    behaviorOnCollisionWithOther(location);
                }
            }
            else
            {
                behaviorOnCollisionWithPlayer(location, farmer);
            }

            if (flag && piercesLeft.Value <= 0 && hasLit && Utility.getLightSource(lightSourceId) != null)
            {
                Utility.getLightSource(lightSourceId).fadeOut.Value = 3;
            }
        }

        private void behaviorOnCollision(GameLocation location, Character target, StardewValley.Object obj)
        {
            bool flag = true;
            Farmer farmer = target as Farmer;
            if (farmer == null)
            {
                NPC nPC = target as NPC;
                if (nPC != null)
                {
                    if (!nPC.IsInvisible)
                    {
                        behaviorOnCollisionWithMonster(nPC, location);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (obj != null)
                {
                    behaviorOnCollisionWithObject(obj, location);
                }
                else
                {
                    behaviorOnCollisionWithOther(location);
                }
            }
            else
            {
                behaviorOnCollisionWithPlayer(location, farmer);
            }

            if (flag && piercesLeft.Value <= 0 && hasLit && Utility.getLightSource(lightSourceId) != null)
            {
                Utility.getLightSource(lightSourceId).fadeOut.Value = 3;
            }
        }

        private void behaviorOnCollision(GameLocation location, Character target, ResourceClump clump)
        {
            bool flag = true;
            Farmer farmer = target as Farmer;
            if (farmer == null)
            {
                NPC nPC = target as NPC;
                if (nPC != null)
                {
                    if (!nPC.IsInvisible)
                    {
                        behaviorOnCollisionWithMonster(nPC, location);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else if (clump != null)
                {
                    behaviorOnCollisionWithResourceClump(clump, clump.Tile, location);
                }
                else
                {
                    behaviorOnCollisionWithOther(location);
                }
            }
            else
            {
                behaviorOnCollisionWithPlayer(location, farmer);
            }

            if (flag && piercesLeft.Value <= 0 && hasLit && Utility.getLightSource(lightSourceId) != null)
            {
                Utility.getLightSource(lightSourceId).fadeOut.Value = 3;
            }
        }

        public bool isColliding(GameLocation location, out Character target, out StardewValley.Object obj)
        {
            target = null;
            obj = null;
            Microsoft.Xna.Framework.Rectangle boundingBox = getBoundingBox();
            if (!ignoreCharacterCollisions.Value)
            {
                if (damagesMonsters.Value)
                {
                    Character character = location.doesPositionCollideWithCharacter(boundingBox);
                    if (character != null)
                    {
                        if (character is NPC && (character as NPC).IsInvisible)
                        {
                            return false;
                        }

                        target = character;
                        return true;
                    }
                }
                else if (Game1.player.currentLocation == location && Game1.player.GetBoundingBox().Intersects(boundingBox))
                {
                    target = Game1.player;
                    return true;
                }
            }

            foreach (Vector2 item in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(boundingBox))
            {
                if (location.objects.TryGetValue(item, out var value) && !value.isPassable())
                {
                    obj = value;
                    return true;
                }
            }

            if (!location.isTileOnMap(position.Value / 64f) || (!ignoreCharacterCollisions.Value && location.isCollidingPosition(boundingBox, Game1.viewport, isFarmer: false, 0, glider: true, theOneWhoFiredMe.Get(location), pathfinding: false, projectile: true)))
            {
                return true;
            }

            return false;
        }

        public bool isColliding(GameLocation location, out Character target, out ResourceClump clump)
        {
            target = null;
            clump = null;
            Rectangle boundingBox = getBoundingBox();
            if (!ignoreCharacterCollisions.Value)
            {
                if (damagesMonsters.Value)
                {
                    Character character = location.doesPositionCollideWithCharacter(boundingBox);
                    if (character != null)
                    {
                        if (character is NPC && (character as NPC).IsInvisible)
                        {
                            return false;
                        }

                        target = character;
                        return true;
                    }
                }
                else if (Game1.player.currentLocation == location && Game1.player.GetBoundingBox().Intersects(boundingBox))
                {
                    target = Game1.player;
                    return true;
                }
            }

            foreach (Vector2 item in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(boundingBox))
            {
                ICollection<ResourceClump> clumps = location.resourceClumps;

                if (location is Woods woods)
                    clumps = woods.resourceClumps;

                if (clumps != null)
                {
                    foreach (var rc in clumps)
                    {
                        if(rc != null)
                        {
                            if (new Rectangle((int)rc.Tile.X, (int)rc.Tile.Y, rc.width.Value, rc.height.Value).Contains(item.X, item.Y))
                            {
                                clump = rc;
                                return true;
                            }
                        }
                    }
                }
            }

            if (!location.isTileOnMap(position.Value / 64f) || (!ignoreCharacterCollisions.Value && location.isCollidingPosition(boundingBox, Game1.viewport, isFarmer: false, 0, glider: true, theOneWhoFiredMe.Get(location), pathfinding: false, projectile: true)))
            {
                return true;
            }

            return false;
        }

        public override void behaviorOnCollisionWithMonster(NPC npc, GameLocation location)
        {
            if (npc is not Monster)
                return;

            CharacterHitResult hitResult = new CharacterHitResult(npc);

            SpellHelper spellHelper = SpellHelper.Instance();
            spellHelper.Invoke(modEntry, spell, Source, location, hitResult, 0, Index.Value, false);
            
            //Disappear(location);
        }

        public override void behaviorOnCollisionWithOther(GameLocation location)
        {
            //Disappear(location);
        }

        public override void behaviorOnCollisionWithPlayer(GameLocation location, Farmer player)
        {
            CharacterHitResult hitResult = new CharacterHitResult(player);

            SpellHelper spellHelper = SpellHelper.Instance();
            spellHelper.Invoke(modEntry, spell, Source, location, hitResult, 0, Index.Value, false);

            //Disappear(location);
        }

        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
        {
            TerrainFeatureHitResult hitResult = new TerrainFeatureHitResult(tileLocation, Direction.Value, new TilePos(tileLocation), false);

            SpellHelper spellHelper = SpellHelper.Instance();
            spellHelper.Invoke(modEntry, spell, Source, location, hitResult, 0, Index.Value, false);

            //Disappear(location);
        }

        public virtual void behaviorOnCollisionWithObject(StardewValley.Object obj, GameLocation location)
        {

            TerrainFeatureHitResult hitResult = new TerrainFeatureHitResult(obj.TileLocation, Direction.Value, new TilePos(obj.TileLocation), false);

            SpellHelper spellHelper = SpellHelper.Instance();
            spellHelper.Invoke(modEntry, spell, Source, location, hitResult, 0, Index.Value, false);


            //Disappear(location);

        }

        public virtual void behaviorOnCollisionWithResourceClump(ResourceClump clump, Vector2 tileLocation, GameLocation location)
        {
            TerrainFeatureHitResult hitResult = new TerrainFeatureHitResult(tileLocation, Direction.Value, new TilePos(tileLocation), false);

            SpellHelper spellHelper = SpellHelper.Instance();
            spellHelper.Invoke(modEntry, spell, Source, location, hitResult, 0, Index.Value, false);

            //Disappear(location);
        }

        public override Microsoft.Xna.Framework.Rectangle getBoundingBox()
        {
            return new((int)(position.X - Game1.tileSize), (int)(position.Y - Game1.tileSize), Game1.tileSize / 2, Game1.tileSize / 2);
        }

        
        public override bool update(GameTime time, GameLocation location)
        {

            if (Game1.isTimePaused)
            {
                return false;
            }

            if (Game1.IsMasterGame && hostTimeUntilAttackable > 0f)
            {
                hostTimeUntilAttackable -= (float)time.ElapsedGameTime.TotalSeconds;
                if (hostTimeUntilAttackable <= 0f)
                {
                    ignoreMeleeAttacks.Value = false;
                    hostTimeUntilAttackable = -1f;
                }
            }

            if (light.Value)
            {
                if (!hasLit)
                {
                    hasLit = true;
                    lightSourceId = $"{GetType().Name}_{Game1.random.Next()}";
                    if (location.Equals(Game1.currentLocation))
                    {
                        Game1.currentLightSources.Add(new LightSource(lightSourceId, 4, position.Value + new Vector2(32f, 32f), 1f, new Color(Utility.getOppositeColor(color.Value).ToVector4() * alpha.Value), LightSource.LightContext.None, 0L, location.NameOrUniqueName));
                    }
                }
                else
                {
                    LightSource lightSource = Utility.getLightSource(lightSourceId);
                    if (lightSource != null)
                    {
                        lightSource.color.A = (byte)(255f * alpha.Value);
                    }

                    Utility.repositionLightSource(lightSourceId, position.Value + new Vector2(32f, 32f));
                }
            }

            alpha.Value += alphaChange.Value;
            alpha.Value = Utility.Clamp(alpha.Value, 0f, 1f);
            rotation += rotationVelocity.Value;
            travelTime += time.ElapsedGameTime.Milliseconds;
            if (scaleGrow.Value != 0f)
            {
                localScale += scaleGrow.Value;
            }

            Vector2 value = position.Value;
            updatePosition(time);
            updateTail(time);
            travelDistance += (value - position.Value).Length();
            if (maxTravelDistance.Value >= 0)
            {
                if (travelDistance > (float)(maxTravelDistance.Value - 128))
                {
                    alpha.Value = ((float)maxTravelDistance.Value - travelDistance) / 128f;
                }

                if (travelDistance >= (float)maxTravelDistance.Value)
                {
                    if (hasLit)
                    {
                        Utility.removeLightSource(lightSourceId);
                    }

                    return true;
                }
            }

            if ((travelTime > 100 || ignoreTravelGracePeriod.Value))
            {

                if(isColliding(location, out var target, out TerrainFeature terrainFeature) && ShouldApplyCollisionLocally(location))
                {
                    if (bouncesLeft.Value <= 0 || target != null)
                    {
                        behaviorOnCollision(location, target, terrainFeature);
                        return piercesLeft.Value <= 0;
                    }

                    bouncesLeft.Value--;
                    bool[] array = Utility.horizontalOrVerticalCollisionDirections(getBoundingBox(), theOneWhoFiredMe.Get(location), projectile: true);
                    if (array[0])
                    {
                        xVelocity.Value = 0f - xVelocity.Value;
                    }

                    if (array[1])
                    {
                        yVelocity.Value = 0f - yVelocity.Value;
                    }

                    if (!string.IsNullOrEmpty(bounceSound.Value))
                    {
                        location?.playSound(bounceSound.Value);
                    }
                }
                else if(isColliding(location, out var target1, out StardewValley.Object obj) && ShouldApplyCollisionLocally(location))
                {
                    if (bouncesLeft.Value <= 0 || target != null)
                    {
                        behaviorOnCollision(location, target1, obj);
                        return piercesLeft.Value <= 0;
                    }

                    bouncesLeft.Value--;
                    bool[] array = Utility.horizontalOrVerticalCollisionDirections(getBoundingBox(), theOneWhoFiredMe.Get(location), projectile: true);
                    if (array[0])
                    {
                        xVelocity.Value = 0f - xVelocity.Value;
                    }

                    if (array[1])
                    {
                        yVelocity.Value = 0f - yVelocity.Value;
                    }

                    if (!string.IsNullOrEmpty(bounceSound.Value))
                    {
                        location?.playSound(bounceSound.Value);
                    }
                }
                else if (isColliding(location, out var target2, out ResourceClump clump) && ShouldApplyCollisionLocally(location))
                {
                    if (bouncesLeft.Value <= 0 || target != null)
                    {
                        behaviorOnCollision(location, target2, clump);
                        return piercesLeft.Value <= 0;
                    }

                    bouncesLeft.Value--;
                    bool[] array = Utility.horizontalOrVerticalCollisionDirections(getBoundingBox(), theOneWhoFiredMe.Get(location), projectile: true);
                    if (array[0])
                    {
                        xVelocity.Value = 0f - xVelocity.Value;
                    }

                    if (array[1])
                    {
                        yVelocity.Value = 0f - yVelocity.Value;
                    }

                    if (!string.IsNullOrEmpty(bounceSound.Value))
                    {
                        location?.playSound(bounceSound.Value);
                    }
                }

            }

            return false;
        }


        public override void updatePosition(GameTime time)
        {

            //this.position.X += this.xVelocity.Value;
            //this.position.Y += this.yVelocity.Value;

            xVelocity.Value += acceleration.X;
            yVelocity.Value += acceleration.Y;
            if ((double)maxVelocity.Value != -1.0 && Math.Sqrt((double)xVelocity.Value * (double)xVelocity.Value + (double)yVelocity.Value * (double)yVelocity.Value) >= (double)maxVelocity.Value)
            {
                xVelocity.Value -= acceleration.X;
                yVelocity.Value -= acceleration.Y;
            }
            position.X += xVelocity.Value;
            position.Y += yVelocity.Value;
        }

        public override void draw(SpriteBatch b)
        {
            //this.Tex ??= Game1.content.Load<Texture2D>(this.TexId.Value);
            Vector2 drawPos = Game1.GlobalToLocal(new Vector2(getBoundingBox().X + getBoundingBox().Width / 2, getBoundingBox().Y + getBoundingBox().Height / 2));
            b.Draw(Tex, drawPos, new Microsoft.Xna.Framework.Rectangle(0, 0, Tex.Width, Tex.Height), Color.White, Direction.Value, new Vector2(Tex.Width / 2, Tex.Height / 2), 2, SpriteEffects.None, (float)((position.Y + (double)(Game1.tileSize * 3 / 2)) / 10000.0));
            //Vector2 bdp = Game1.GlobalToLocal(new Vector2(getBoundingBox().X, getBoundingBox().Y));
            //b.Draw(Mod.instance.manaFg, new Rectangle((int)bdp.X, (int)bdp.Y, getBoundingBox().Width, getBoundingBox().Height), Color.White);
        }

        private void Disappear(GameLocation loc)
        {
            //if (this.Spell?.SoundHit != null)
            //    loc.LocalSoundAtPixel(this.Spell.SoundHit, this.position.Value);

            //Game1.createRadialDebris(loc, 22 + rand.Next( 2 ), ( int ) position.X / Game1.tileSize, ( int ) position.Y / Game1.tileSize, 3 + rand.Next(5), false);
            //Game1.createRadialDebris(loc, this.TexId.Value, Game1.getSourceRectForStandardTileSheet(Projectile.projectileSheet, 0), 4, (int)this.position.X, (int)this.position.Y, 6 + SpellProjectile.Rand.Next(10), (int)(this.position.Y / (double)Game1.tileSize) + 1, new Color(255, 255, 255, 8 + SpellProjectile.Rand.Next(64)), 2.0f);
            //Game1.createRadialDebris(loc, tex, new Rectangle(0, 0, tex.Width, tex.Height), 0, ( int ) position.X, ( int ) position.Y, 3 + rand.Next(5), ( int ) position.Y / Game1.tileSize, Color.White, 5.0f);
            
            destroyMe = true;

            loc.projectiles.RemoveWhere((Func<Projectile, bool>)(projectile =>
            {
                return projectile.destroyMe;
            }));
        }

        public GameLocation GetGameLocation()
        {
            return Source.GetGameLocation();
        }

        public Vector2 GetPosition()
        {
            return position.Get();
        }

        public Rectangle GetBoundingBox()
        {
            return getBoundingBox();
        }

        public int GetHorizontalMovement()
        {
            return 1;
        }

        public int GetVerticalMovement()
        {
            return 1;
        }
    }
}
