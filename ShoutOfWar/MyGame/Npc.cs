using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
using ShoutOfWar.MyGame.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace ShoutOfWar.MyGame
{
    class Npc : IMyGameComponent
    {
        private SpriteRender spriteRender;        

        private const double AnimationDelay = 200.0;
        private const float MoveSpeed = 200.0f;

        private Animation currentAnimation;
        private Dictionary<Direction, Animation> animations = new Dictionary<Direction, Animation>();        
        private Vector2 speed;

        public Vector2 position;

        public Npc(SpriteSheet spriteSheet, SpriteRender spriteRender, Vector2 position)
        {            
            this.spriteRender = spriteRender;
            this.position = position;

            var moveRight = new Animation(AnimationDelay);
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_398));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_399));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_400));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_401));
            animations.Add(Direction.Right, moveRight);

            var moveLeft = new Animation(AnimationDelay);
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_402));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_403));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_404));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_405));
            animations.Add(Direction.Left, moveLeft);

            var moveUp = new Animation(AnimationDelay);
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_394));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_395));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_396));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_397));
            animations.Add(Direction.Up, moveUp);

            var moveDown = new Animation(AnimationDelay);
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_390));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_391));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_392));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_393));
            animations.Add(Direction.Down, moveDown);

            currentAnimation = animations[Direction.Down];
        }

        public void Draw(GameTime gameTime)
        {
            spriteRender.Draw(currentAnimation.GetFrame(), position, null, 0, 2);
        }

        public void Update(GameTime gameTime)
        {                        
            position += speed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
            if (speed.Length() > float.Epsilon)
            {
                var speedAngle = MathHelper.ToDegrees((float)Math.Atan2(speed.Y * (-1), speed.X));
                if (speedAngle < 0.0f) speedAngle += 360.0f;
                currentAnimation = animations[(Direction)((int)(speedAngle / 90.0f))];
                currentAnimation.Update(gameTime);
            }
            else
            {
                // Frame 1 is best for idle sprite for every direction
                currentAnimation.SetFrame(1);
            }
        }
    }
}
