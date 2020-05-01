using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace ShoutOfWar.Engine
{
    class Animation
    {
        private double framesDelay;
        private double currentTime;
        private List<SpriteFrame> frames = new List<SpriteFrame>();
        private int currentFrameIndex;
        
        public Animation(double framesDelay)
        {
            this.framesDelay = framesDelay;
        }

        public void AddFrame(SpriteFrame spriteFrame)
        {
            frames.Add(spriteFrame);
        }
        
        public SpriteFrame GetFrame()
        {
            if (frames.Count == 0) throw new Exception("Animation is empty. Please add frames to animation class.");

            return frames[currentFrameIndex];
        }

        public void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentTime >= framesDelay)
            {
                currentTime = 0.0;
                currentFrameIndex++;
                if (currentFrameIndex >= frames.Count) currentFrameIndex = 0;
            }
        }
    }
}
