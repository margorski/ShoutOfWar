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
    class Player : IMyGameComponent
    {
        private SpriteRender spriteRender;        

        private const double AnimationDelay = 200.0;
        private const float MoveSpeed = 200.0f;
        private readonly Vector2 StartPosition = new Vector2(300, 300);

        private Animation currentAnimation;
        private Dictionary<Direction, Animation> animations = new Dictionary<Direction, Animation>();
        private Vector2 position;
        private Vector2 speed;


        public Player(SpriteSheet spriteSheet, SpriteRender spriteRender)
        {            
            this.spriteRender = spriteRender;
            position = StartPosition;

            var moveRight = new Animation(AnimationDelay);
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_8));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_9));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_10));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_11));
            animations.Add(Direction.Right, moveRight);

            var moveLeft = new Animation(AnimationDelay);
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_12));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_13));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_14));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_15));
            animations.Add(Direction.Left, moveLeft);

            var moveUp = new Animation(AnimationDelay);
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_4));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_5));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_6));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_7));
            animations.Add(Direction.Up, moveUp);

            var moveDown = new Animation(AnimationDelay);
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_0));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_1));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_2));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_3));
            animations.Add(Direction.Down, moveDown);

            currentAnimation = animations[Direction.Down];
        }

        public void Draw(GameTime gameTime)
        {
            spriteRender.Draw(currentAnimation.GetFrame(), position, null, 0, 2);
        }

        public void Update(GameTime gameTime)
        {
            speed = new Vector2(MoveSpeed, -MoveSpeed) * GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
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
