using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Engine
{    
    abstract class Component
    {
        public Entity parent = null;
        public abstract void Draw(GameTime gameTime);
        public abstract void Init();
        public abstract void Update(GameTime gameTime);
    }
}
