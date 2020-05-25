using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ShoutOfWar.Game.Shared
{    
    public class Util
    {
        public static Texture2D pixelTexture { private set; get; } = new Texture2D(Game1.Current.GraphicsDevice, 1, 1);

        static Util()
        {
            pixelTexture.SetData(new Color[] { Color.White });
        }

        public static double GetVectorAngleRad(Vector2 vector, bool wrap = true)
        {
            var angle = Math.Atan2(vector.Y, vector.X);
            if (wrap && angle < 0.0f) angle += Math.PI*2;
            return angle;
        }
        
        public static bool IsWithinRadius(Vector2 v, float radius)
        {
            return v.Length() <= radius;
        }

        // Check if angle2 lies between angle1 +/- radiusRange/2. It is made in this form because 
        // it is easiest to use with facinAngle and visibilityRanges.
        public static bool IsInRadiusRange(double angle1, double visibilityRange, double angle2)
        {
            var startAngle = angle1 - visibilityRange / 2.0;
            var endAngle = angle1 + visibilityRange / 2.0;
            if (endAngle < startAngle) endAngle += Math.PI * 2;

            return (angle2 >= startAngle && angle2 <= endAngle);
        }
    }
}
