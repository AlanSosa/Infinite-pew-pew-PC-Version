using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using System.Diagnostics;

namespace ThridWolrdShooterGame.Managers
{
    public static class ColorManager
    {
        private static Color startingColor;
        private static Color finalColor;
        private static float factor;
        private static float factorIncrement;
        public static Color Color;

        private static Random random = new Random();

        private static float hue1, hue2;
        public static void Init()
        {
            hue1 = random.NextFloat(0f, 360f);
            hue2 = (hue1 + random.NextFloat(0, 50));
            startingColor = ColorUtil.HSVToRGB(hue1, .67f,.85f);
            finalColor = ColorUtil.HSVToRGB(hue2, .67f, .85f);
            factor = 0;
        }

        public static void Update()
        {
            Color = Color.Lerp(startingColor, finalColor, factor);
        }

        public static void SetNewFactorIncrement(float newFactorIncrement)
        {
            factorIncrement = newFactorIncrement;
            startingColor = finalColor;
            hue1 = hue2;
            hue2 = (hue1 + random.NextFloat(0, 50));
            finalColor = ColorUtil.HSVToRGB(hue2, .67f, .85f);
            factor = 0;

            //Debug.WriteLine("Color INcrement");
        }

        public static void AddFactorValue()
        {
            factor += factorIncrement;
        }

    }

}
