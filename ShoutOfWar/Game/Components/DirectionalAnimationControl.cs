using Microsoft.Xna.Framework;
using ShoutOfWar.Engine;
using ShoutOfWar.Game.Components.General;
using ShoutOfWar.Game.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace ShoutOfWar.Game.Components
{    
    class DirectionalAnimationControl : Component
    {
        public enum Direction
        {
            Right = 0,
            Up,
            Left,
            Down
        }

        private AnimatedSprite animatedSprite;
        private Dynamic dynamic;

        public Dictionary<Direction, Animation> animations = new Dictionary<Direction, Animation>();

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Init()
        {
            animatedSprite = parent.GetComponent<AnimatedSprite>();
            dynamic = parent.GetComponent<Dynamic>();

            if (animatedSprite != null) animatedSprite.animation = animations.FirstOrDefault().Value;
        }

        public override void Update(GameTime gameTime)
        {            
            if (animatedSprite == null)
            {
                animatedSprite = parent.GetComponent<AnimatedSprite>();
                if (animatedSprite == null) return;
            }
            if (dynamic == null)
            {
                dynamic = parent.GetComponent<Dynamic>();
                if (dynamic == null) return;
            }

            if (Vector2.Normalize(dynamic.Velocity).Length() > float.Epsilon)
            {
                // Y axis needs to be mirrored as on screen Y rise in down direction
                var speedAngle = (Util.GetVectorAngleRad(dynamic.Velocity * new Vector2(0.0f, -1.0f)) + (Math.PI/4));
                var animationFrame = (int)(speedAngle / (Math.PI / 2)) % 4;

                animatedSprite.animation = animations[(Direction)animationFrame];
                animatedSprite.animation.Update(gameTime);
            }
            else
            {
                // Frame 1 is best for idle sprite for every direction
                animatedSprite.animation.SetFrame(1);
            }
        }

        public void SetAnimationDelay(float animationDelay)
        {
            foreach (var animation in animations.Values)
            {
                animation.FramesDelay = animationDelay;
            }
        }

        public override void DrawDebug(GameTime gameTime)
        {
        }
    }
}
