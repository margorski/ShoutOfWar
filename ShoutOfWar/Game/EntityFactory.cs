using Microsoft.Xna.Framework;
using ShoutOfWar.Engine;
using ShoutOfWar.Game.Components;
using ShoutOfWar.Game.Components.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace ShoutOfWar.Game
{
    class EntityFactory
    {
        SpriteSheet spriteSheet;

        public EntityFactory(SpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public Entity createPlayer(Vector2 position, string idModifier = "")
        {            
            var entity = new Entity($"player{idModifier}");
            entity.position = position;
            entity.scale = 2;
            entity.AddComponent(new Velocity());
            entity.AddComponent(new GamepadMovement(200.0f));

            var animationController = new DirectionalAnimationControl();
            var animationDelay = 200.0f;
            var moveRight = new Animation(animationDelay);
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_87));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_88));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_89));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_90));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Right, moveRight);

            var moveLeft = new Animation(animationDelay);
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_91));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_92));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_93));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_94));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Left, moveLeft);

            var moveUp = new Animation(animationDelay);
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_83));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_84));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_85));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_86));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Up, moveUp);

            var moveDown = new Animation(animationDelay);
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_79));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_80));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_81));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_82));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Down, moveDown);
            entity.AddComponent(animationController);

            var animatedSprite = new AnimatedSprite();
            animatedSprite.animation = moveDown;
            entity.AddComponent(animatedSprite);

            return entity;
        }

        public Entity createNPC(Vector2 position, string idModifier = "")
        {
            var entity = new Entity($"player{idModifier}");
            entity.position = position;
            entity.scale = 2;
            entity.AddComponent(new Velocity());
            entity.AddComponent(new NpcLogic(200.0f, 4000, 150));

            var animationController = new DirectionalAnimationControl();
            var animationDelay = 200.0f;
            var moveRight = new Animation(animationDelay);
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_398));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_399));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_400));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_401));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Right, moveRight);

            var moveLeft = new Animation(animationDelay);
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_402));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_403));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_404));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_405));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Left, moveLeft);

            var moveUp = new Animation(animationDelay);
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_394));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_395));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_396));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_397));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Up, moveUp);

            var moveDown = new Animation(animationDelay);
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_390));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_391));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_392));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_393));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Down, moveDown);
            entity.AddComponent(animationController);

            var animatedSprite = new AnimatedSprite();
            animatedSprite.animation = moveDown;
            entity.AddComponent(animatedSprite);

            return entity;
        }
    }
}
