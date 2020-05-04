using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
//using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components
{
    class PlayerLogic : Component
    {
        private GamePadState previousGamepadState;

        private float shoutSpeed = 10.0f;
        private float shoutRadius = 0.0f;
        private float maxShoutRadius = 100.0f;
        private bool screaming = false;

        public override void Draw(GameTime gameTime)
        {            
        }

        public override void Init()
        {
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public override void Update(GameTime gameTime)
        {            
            var gamepadState = GamePad.GetState(PlayerIndex.One);

            // A pressed
            if (previousGamepadState.IsButtonUp(Buttons.A) && gamepadState.IsButtonDown(Buttons.A))
            {
                if (!screaming) screaming = true;
            }

            // A released
            if (previousGamepadState.IsButtonDown(Buttons.A) && gamepadState.IsButtonUp(Buttons.A))
            {

            }
            if (screaming) UpdateShout(gameTime);

            previousGamepadState = gamepadState;
        }

        void UpdateShout(GameTime gameTime)
        {
            shoutRadius += shoutSpeed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
            if (shoutRadius >= maxShoutRadius)
            {
                shoutRadius = 0.0f;
                screaming = false;
            }
        }

        void DrawShout(GameTime gameTime)
        {            
         //   Game1.Current.SpriteBatch.DrawCircle(parent.position, shoutRadius, 0, Color.White);
        }
    }
}
