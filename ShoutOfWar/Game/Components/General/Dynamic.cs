using Microsoft.Xna.Framework;
using MonoGame.Extended;
using ShoutOfWar.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components.General
{
    class Dynamic : Component
    {
        public Vector2 Acceleration
        {
            set
            {
                if (maxAcceleration > 0.0f && value.Length() > maxAcceleration)
                {
                    value = Vector2.Normalize(value) * maxAcceleration;
                }
                acceleration = value;
            }
            get { return acceleration; }
        }
        private Vector2 acceleration;

        public Vector2 Velocity
        {
            set
            {
                if (maxVelocity > 0.0f && value.Length() > maxVelocity)
                {
                    value = Vector2.Normalize(value) * maxVelocity;
                }
                velocity = value;
            }
            get { return velocity; }
        }
        private Vector2 velocity;

        public float maxAcceleration;
        public float maxVelocity;
        public int colliderRadius;

        public Dynamic(float maxVelocity, float maxAcceleration = 0.0f)
        {
            this.maxAcceleration = maxAcceleration;
            this.maxVelocity = maxVelocity;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void DrawDebug(GameTime gameTime)
        {
            if (colliderRadius > 0.0f) Game1.Current.SpriteBatch.DrawCircle(parent.position, colliderRadius, 128, Color.Red, 3);
        }

        public override void Init()
        {
        }

        public bool CollideWithEntity(Vector2 position, float radius)
        {
            return (position - parent.position).Length() <= (radius + colliderRadius);
        }

        public bool Collide()
        {
            var collidableEntities = Game1.Current.EnabledEntities.FindAll(e => e.HasComponent<Dynamic>() && e != parent);
            foreach (Entity e in collidableEntities)
            {
                var dynamic = e.GetComponent<Dynamic>();
                if (CollideWithEntity(e.position, dynamic.colliderRadius))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!float.IsNaN(Acceleration.X) && !float.IsNaN(Acceleration.Y))
            {
                Velocity += Acceleration * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
            }

            if (Velocity.Length() > 0)
            {
                var previousPosition = parent.position;

                var collide = false;

                // x move
                parent.position.X += Velocity.X * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
                if (Collide())
                {
                    parent.position.X = previousPosition.X;
                    collide = true;
                }                                                

                parent.position.Y += Velocity.Y * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);                
                if (Collide())
                {
                    parent.position.Y = previousPosition.Y;
                    collide = true;               
                }

                
                if (collide)
                {
                    // shift 2 pixel in vector direction perpendicular to velocity
                    var reboundVector = Vector2.Normalize(new Vector2(-Velocity.Y, Velocity.X)) * 2.0f;
                    parent.position.X += reboundVector.X;
                    if (Collide()) parent.position.X = previousPosition.X;

                    parent.position.Y += reboundVector.Y;
                    if (Collide()) parent.position.Y = previousPosition.Y;
                }
            }            
        }
    }
}
