using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ThridWolrdShooterGame
{
    static class ColorUtil
    {

        public static Color HSVToColor(float h, float s, float v)
        {
            if (h == 0 && s == 0)
                return new Color(v, v, v);

            float c = s * v;
            float x = c * (1 - Math.Abs(h % 2 - 1));
            float m = v - c;

            if (h < 1) return new Color(c + m, x + m, m);
            else if (h < 2) return new Color(x + m, c + m, m);
            else if (h < 3) return new Color(m, c + m, x + m);
            else if (h < 4) return new Color(m, x + m, c + m);
            else if (h < 5) return new Color(x + m, m, c + m);
            else return new Color(c + m, m, x + m);
        }

        public static Color LerpColor(Color a, Color b, float percentage)
        {
            return new Color(
                (byte)MathHelper.Lerp(a.R, b.R, percentage),
                (byte)MathHelper.Lerp(a.G, b.G, percentage),
                (byte)MathHelper.Lerp(a.B, b.B, percentage),
                (byte)MathHelper.Lerp(a.A, b.A, percentage));
        }

        public static Color HSVToRGB(float hue, float saturation , float value)
        {

            int i = 0;
            float f, p, q, t;

            if (saturation == 0 && hue == 0)
                return new Color(value, value, value);

            hue /= 60;
            i = (int)Math.Round(hue);
            f = hue - i;
            p = value * (1 - saturation);
            q = value * (1 - saturation * f);
            t = value * (1 - saturation * (1- f));

            if (i == 0) return new Color(value, p, t);
            else if (i == 1) return new Color(q, value, p);
            else if (i == 2) return new Color(p, value, t);
            else if (i == 3) return new Color(p, q, value);
            else if (i == 4) return new Color(t, p, value);
            else return new Color(value,p,q);
        }
    }
}
