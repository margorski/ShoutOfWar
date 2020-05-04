using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
    class GamepadMovement : Component
    {
        public float maxSpeed;

        private Velocity velocity;

        public GamepadMovement(float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Init()
        {            
        }

        public override void Update(GameTime gameTime)
        {
            if (!enabled) return;

            if (velocity == null)
            {
                velocity = parent.GetComponent<Velocity>().FirstOrDefault();
            }

            if (velocity != null) {
                velocity.value = new Vector2(maxSpeed, -maxSpeed) * GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            }
        }

        public void Disable()
        {
            velocity.value = Vector2.Zero;
            enabled = false;
        }
        public void Enable()
        {
            enabled = true;
        }
    }
}
