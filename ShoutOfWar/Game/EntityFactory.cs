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
    public class EntityFactory
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
            var dynamic = new Dynamic(200.0f);
            dynamic.colliderRadius = 15;
            entity.AddComponent(dynamic);
            entity.AddComponent(new GamepadMovement(200.0f));
            entity.AddComponent(new PlayerLogic());

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

        public Entity createNPC(Vector2 position, string idModifier = "", NpcLogic.Team team = NpcLogic.Team.None)
        {
            var entity = new Entity($"player{idModifier}");
            entity.position = position;
            entity.scale = 2;
            var dynamic = new Dynamic(150.0f);
            dynamic.colliderRadius = 10;
            entity.AddComponent(dynamic);
            entity.AddComponent(new NpcLogic(4000, 150, team));

            var animationController = new DirectionalAnimationControl();
            var animationDelay = 200.0f;
            var moveRight = new Animation(animationDelay);
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_435));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_436));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_437));
            moveRight.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_438));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Right, moveRight);

            var moveLeft = new Animation(animationDelay);
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_439));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_440));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_441));
            moveLeft.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_442));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Left, moveLeft);

            var moveUp = new Animation(animationDelay);
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_431));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_432));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_433));
            moveUp.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_434));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Up, moveUp);

            var moveDown = new Animation(animationDelay);
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_427));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_428));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_429));
            moveDown.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_430));
            animationController.animations.Add(DirectionalAnimationControl.Direction.Down, moveDown);
            entity.AddComponent(animationController);

            var animatedSprite = new AnimatedSprite();
            animatedSprite.animation = moveDown;
            entity.AddComponent(animatedSprite);

            return entity;
        }
    }
}
