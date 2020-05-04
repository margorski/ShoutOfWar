using Microsoft.Xna.Framework;
using System;

namespace ShoutOfWar.Game.Shared
{    
    public class Util
    {
        public static double GetVectorAngleRad(Vector2 vector)
        {
            var angle = Math.Atan2(vector.Y * (-1), vector.X);
            if (angle < 0.0f) angle += Math.PI*2;
            return angle;
        }
    }
}
