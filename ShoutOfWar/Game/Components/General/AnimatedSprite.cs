using Microsoft.Xna.Framework;
using ShoutOfWar.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components.General
{
    class AnimatedSprite : Component
    {
        public Animation animation;
        public Color? color = null;

        public override void Draw(GameTime gameTime)
        {
            if (animation == null) return; 

            var spriteFrame = animation.GetFrame();
            if (spriteFrame == null) return;

            Game1.Current.SpriteRender.Draw(animation.GetFrame(), parent.position, color, parent.rotation, parent.scale);
        }

        public override void DrawDebug(GameTime gameTime)
        {
        }

        public override void Init()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (animation == null) return; 
            
            animation.Update(gameTime);
        }
    }
}
