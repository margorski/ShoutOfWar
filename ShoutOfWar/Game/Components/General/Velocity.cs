using Microsoft.Xna.Framework;
using ShoutOfWar.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components.General
{
    class Velocity : Component
    {
        public Vector2 value;

        public Velocity()
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Init()
        {
        }

        public override void Update(GameTime gameTime)
        {
            parent.position += value * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
        }
    }
}
