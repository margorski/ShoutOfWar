using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Engine
{
    interface IMyGameComponent
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
