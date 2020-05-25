using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
using MonoGame.Extended;
using System.Linq;
using System.Collections.Generic;

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

        public NpcLogic.Team team = NpcLogic.Team.Red;

        private int armyVisibilityCone = 0;

        public List<Entity> army { private set; get; } = new List<Entity>();

        private ShoutState shoutState = ShoutState.Idle;

        public override void Draw(GameTime gameTime)
        {            
            if (shoutState != ShoutState.Idle) DrawShout(gameTime);
        }

        public override void Init()
        {
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
            gamepadMovement = parent.GetComponent<GamepadMovement>();
        }

        public override void Update(GameTime gameTime)
        {        
            if (gamepadMovement == null)
            {
                gamepadMovement = parent.GetComponent<GamepadMovement>();
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
                if (shoutState == ShoutState.Charge) CallToArms();
            }

            // B pressed
            if (previousGamepadState.IsButtonUp(Buttons.B) && gamepadState.IsButtonDown(Buttons.B))
            {
                if (shoutState == ShoutState.Idle) ReleaseFromCommand(); 
            }

            // LB pressed
            if (previousGamepadState.IsButtonUp(Buttons.LeftShoulder) && gamepadState.IsButtonDown(Buttons.LeftShoulder))
            {
                if (army.Count > 0)
                {
                    armyVisibilityCone--;
                    if (armyVisibilityCone < 0) armyVisibilityCone = 3;
                    SetArmyCone();
                }
            }

            // RB pressed
            if (previousGamepadState.IsButtonUp(Buttons.RightShoulder) && gamepadState.IsButtonDown(Buttons.RightShoulder))
            {
                if (army.Count > 0)
                {
                    armyVisibilityCone++;
                    if (armyVisibilityCone > 3) armyVisibilityCone = 0;
                    SetArmyCone();
                }
            }

            UpdateShout(gameTime);
            previousGamepadState = gamepadState;            
        }
        
        void SetArmyCone()
        {
            army.ForEach(e =>
            {
                e.GetComponent<NpcLogic>().selectedVisibilityCone = armyVisibilityCone;
            });
        }

        void UpdateShout(GameTime gameTime)
        {
            switch (shoutState)
            {
                case ShoutState.Charge:
                    shoutRadius += chargeSpeed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);                    
                    Game1.Current.entities.ForEach(e =>
                    {
                        var npcLogic = e.GetComponent<NpcLogic>();
                        //if (npcLogic != null && !npcLogic.UnderCommand && Vector2.Distance(parent.position, npcLogic.parent.position) <= shoutRadius) npcLogic.UnderCommand = true;
                    });
                    if (shoutRadius >= MaxShoutRadius)
                    {
                        shoutRadius = MaxShoutRadius;
                        CallToArms();
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

        void ReleaseFromCommand()
        {
            army.ForEach(e =>
            {
                var npcLogic = e.GetComponent<NpcLogic>();
                if (npcLogic != null) npcLogic.ReleaseFromCommand();
            });
            army = new List<Entity>();
            armyVisibilityCone = 0;
        }

        void CallToArms()
        {
            shoutState = ShoutState.Scream;
            var newSoldiers  = Game1.Current.EnabledEntities
                .FindAll(e =>
                {
                    var npcLogic = e.GetComponent<NpcLogic>();
                    var relativePosition = e.position - parent.position;
                    return (npcLogic != null && npcLogic.team == team && relativePosition.Length() <= shoutRadius) && army.Find(a => a==e) == null;
                });
            newSoldiers.ForEach(a => a.GetComponent<NpcLogic>().FollowCommander(this.parent, armyVisibilityCone));
            army.AddRange(newSoldiers);
        }
        void DrawShout(GameTime gameTime)
        {            
            Game1.Current.SpriteBatch.DrawCircle(parent.position, shoutRadius, 128, Color.Salmon, 5);
        }

        public override void DrawDebug(GameTime gameTime)
        {
        }
    }
}
