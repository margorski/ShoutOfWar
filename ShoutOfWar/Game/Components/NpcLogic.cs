using Microsoft.Xna.Framework;
using ShoutOfWar.Engine;
using ShoutOfWar.Game.Components.General;
using ShoutOfWar.Game.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components
{
    class NpcLogic : Component
    {
        private Velocity velocity;

        public int randomMoveDelay;
        public int randomMoveMaxLength;
        public float maxSpeed;

        private Vector2? targetPosition = null;
        private double randomMoveCurrentTime = 0.0;
        private double? previousAngle = null;
        public NpcLogic(float maxSpeed, int randomMoveDelay, int randomMoveMaxLength)
        {
            this.maxSpeed = maxSpeed;
            this.randomMoveDelay = randomMoveDelay;
            this.randomMoveMaxLength = randomMoveMaxLength;
            randomMoveCurrentTime = Game1.Current.Random.Next(this.randomMoveDelay);
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Init()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (velocity == null)
            {
                velocity = parent.GetComponent<Velocity>().FirstOrDefault();
                if (velocity == null) return;
            }

            if (targetPosition != null)
            {
                var deltaPosition = targetPosition.Value - parent.position;
                var deltaAngle = Util.GetVectorAngleRad(deltaPosition);
                if (previousAngle != null && Math.Abs(previousAngle.Value - deltaAngle) >= Math.PI * 0.9)
                {
                    parent.position = targetPosition.Value;
                    targetPosition = null;
                    velocity.value = Vector2.Zero;
                }
                else
                {
                    velocity.value = maxSpeed * Vector2.Normalize(deltaPosition);
                }
                previousAngle = deltaAngle;
            }
            else
            {
                randomMoveCurrentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (randomMoveCurrentTime >= randomMoveDelay)
                {
                    randomMoveCurrentTime = 0.0;
                    previousAngle = null;
                    targetPosition = new Vector2(
                        Game1.Current.Random.Next((int)parent.position.X - randomMoveMaxLength, (int)parent.position.X + randomMoveMaxLength),
                        Game1.Current.Random.Next((int)parent.position.Y - randomMoveMaxLength, (int)parent.position.Y + randomMoveMaxLength)
                    );
                }
            }          
        }
    }
}
