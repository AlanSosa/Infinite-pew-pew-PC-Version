using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThridWolrdShooterGame.Entites;
using ThridWolrdShooterGame.Managers;
using ThridWolrdShooterGame.Entites.Items;
using System.Diagnostics;
using GameStateManagement.ScreensUtils;

namespace ThridWolrdShooterGame
{
    public static class PlayerStatus
    {

        private static int lives;
        private static int level;
        private static int points;
        private static int shield;
        public static int enemiesKilled= 0;
        private static float slowMoCounter;

        public static float AmigoTime
        {
            get;
            private set;
        }

        public static float PewPewTime
        {
            get;
            private set;
        }

        public static float PewTime
        {
            get;
            private set;
        }

        public static float RocketsTime
        {
            get;
            private set;
        }

        public static float InvincibilityTime
        {
            get;
            private set;
        }

        public static float BoingyTime
        {
            get;
            private set;
        }

        public static float SlowMoTime;

        public static int Lives
        {
            get
            {
                return lives;
            }
        }

        public static int Shield
        {
            get
            {
                return shield;
            }
        }

        public static bool IsGameOver
        {
            get
            {
                return lives <= 0;
            }
        }

        public static void Init()
        {
            /*if (points > HighScore)
                FileHandler.WriteDataToFile(points);*/
            lives = 3;
            points = 0;
            shield = 3;
            enemiesKilled = 0;
            slowMoCounter = 0;
            SlowMoTime = 5000;
            level = 1;
            AmigoTime = 30000;

            //This time it's defined in Fps not Miliseconds!
            PewTime = 90f;
            PewPewTime = 60f;
            RocketsTime = 90f;
            InvincibilityTime = 60f;
            BoingyTime = 45f;
        }

        public static float SlowMoCounter
        {
            get
            {
                return slowMoCounter;
            }
        }
        public static int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
            }
        }

        public static int Points
        {
            get
            {
                return points;
            }
        }

        public static void AddLives()
        {
            lives++;
        }

        public static void RemoveLife()
        {
            lives--;
        }

        public static void RemoveShield()
        {
            shield--;
        }

        public static void AddShield()
        {
            shield++;
        }

        public static void ResetPlayer()
        {
            shield = 3;
        }

        public static void AddSlowMoPoints()
        {
            slowMoCounter += .35f;
        }

        public static void AddSlowMoBonus()
        {
            SlowMoTime += 350f;
        }

        public static void AddPoints(int pointvalue)
        {
            points += pointvalue;
        }

        public static void AddAmigoBonus()
        {
            AmigoTime += 8000;
        }
        public static void AddAmmoBonus()
        {
            PewTime += 15f;
            RocketsTime += 15f;
            PewPewTime += 10f;
            BoingyTime += 10f;
        }

        public static void Update()
        {
            if (slowMoCounter > 100)
            {
                slowMoCounter = 0;
                GameplayScreen.SlowGameBy(SlowMoTime);
            }
        }

        public static int HighScore
        {
            get
            {
                return 10;
            }

        }
    }
}
