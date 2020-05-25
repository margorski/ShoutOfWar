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
        public float moveSpeed;

        private Dynamic dynamic;

        public GamepadMovement(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
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

            if (dynamic == null)
            {
                dynamic = parent.GetComponent<Dynamic>();
            }

            if (dynamic != null) {
                dynamic.Velocity = new Vector2(moveSpeed, -moveSpeed) * GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;                
            }
        }

        public void Disable()
        {
            dynamic.Velocity = Vector2.Zero;
            enabled = false;
        }
        public void Enable()
        {
            enabled = true;
        }

        public override void DrawDebug(GameTime gameTime)
        {
        }
    }
}
