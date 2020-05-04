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
        private Velocity velocity;

        public Dictionary<Direction, Animation> animations = new Dictionary<Direction, Animation>();

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Init()
        {
            animatedSprite = parent.GetComponent<AnimatedSprite>().FirstOrDefault();
            velocity = parent.GetComponent<Velocity>().FirstOrDefault();

            if (animatedSprite != null) animatedSprite.animation = animations.FirstOrDefault().Value;
        }

        public override void Update(GameTime gameTime)
        {            
            if (animatedSprite == null)
            {
                animatedSprite = parent.GetComponent<AnimatedSprite>().FirstOrDefault();
                if (animatedSprite == null) return;
            }
            if (velocity == null)
            {
                velocity = parent.GetComponent<Velocity>().FirstOrDefault();
                if (velocity == null) return;
            }

            if (Vector2.Normalize(velocity.value).Length() > float.Epsilon)
            {
                var speedAngle = Util.GetVectorAngleRad(velocity.value);
                animatedSprite.animation = animations[(Direction)((int)(speedAngle / (Math.PI / 2)))];
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
    }
}
