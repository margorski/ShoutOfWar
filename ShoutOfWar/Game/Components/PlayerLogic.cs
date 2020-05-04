using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
using MonoGame.Extended;
using System.Linq;

namespace ShoutOfWar.Game.Components
{
    class PlayerLogic : Component
    {
        public enum ShoutState
        {
            Idle,
            Charge,
            Scream
        }

        private GamepadMovement gamepadMovement;

        private GamePadState previousGamepadState;

        private const float MaxShoutRadius = 500.0f;

        private float chargeSpeed = 200.0f;
        private float shoutSpeed = 1000.0f;
        private float shoutRadius = 0.0f;
        
        private ShoutState shoutState = ShoutState.Idle;

        public override void Draw(GameTime gameTime)
        {            
            if (shoutState != ShoutState.Idle) DrawShout(gameTime);
        }

        public override void Init()
        {
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
            gamepadMovement = parent.GetComponent<GamepadMovement>().FirstOrDefault();
        }

        public override void Update(GameTime gameTime)
        {        
            if (gamepadMovement == null)
            {
                gamepadMovement = parent.GetComponent<GamepadMovement>().FirstOrDefault();
            }

            var gamepadState = GamePad.GetState(PlayerIndex.One);

            // A pressed
            if (previousGamepadState.IsButtonUp(Buttons.A) && gamepadState.IsButtonDown(Buttons.A))
            {
                if (shoutState == ShoutState.Idle) shoutState = ShoutState.Charge;
                if (gamepadMovement != null) gamepadMovement.Disable();
            }

            // A released
            if (previousGamepadState.IsButtonDown(Buttons.A) && gamepadState.IsButtonUp(Buttons.A))
            {
                if (shoutState == ShoutState.Charge) shoutState = ShoutState.Scream;
            }

            UpdateShout(gameTime);
            previousGamepadState = gamepadState;
        }

        void UpdateShout(GameTime gameTime)
        {
            switch (shoutState)
            {
                case ShoutState.Charge:
                    shoutRadius += chargeSpeed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
                    if (shoutRadius >= MaxShoutRadius)
                    {
                        shoutRadius = MaxShoutRadius;
                        shoutState = ShoutState.Scream;
                    }
                    break;

                case ShoutState.Scream:
                    shoutRadius -= shoutSpeed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
                    if (shoutRadius <= 0.0f)
                    {
                        shoutRadius = 0.0f;
                        shoutState = ShoutState.Idle;
                        if (gamepadMovement != null) gamepadMovement.Enable();
                    }
                    break;
            }         
        }

        void DrawShout(GameTime gameTime)
        {            
            Game1.Current.SpriteBatch.DrawCircle(parent.position, shoutRadius, 128, Color.Salmon, 5);
        }
    }
}
