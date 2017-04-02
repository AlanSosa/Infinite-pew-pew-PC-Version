using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using ThridWolrdShooterGame;
using Microsoft.Xna.Framework;
using TechnicalData;

namespace GameStateManagement.Managers
{
    public static class SoundManager
    {
        private static SoundEffectInstance[] normalShot;
        private static SoundEffectInstance[] pickItem;
        private static SoundEffectInstance[] rocketShot;
        private static SoundEffect[] explosions;
        private static bool isMusicPlaying;
        private static Song theme;
        //private static SoundEffect normalShot;
        private static SoundEffect playerDead;
        private static SoundEffect spawnItem;
        private static SoundEffect spawnPlayer;
        private static SoundEffect spawnBonusEnemies;

        private static SoundEffect yay;
        private static SoundEffect applause;
        private static SoundEffect applauseLower;

        private static SoundEffect cashRegister;
        private static SoundEffect aww;
        private static SoundEffect slowDown;
        private static SoundEffect slowUp;
        

        public static SoundEffect SlowDown
        {
            set
            {
                slowDown = value;
            }
        }

        public static SoundEffect SlowUp
        {
            set
            {
                slowUp = value;
            }
        }

        public static SoundEffect[] Explosions
        {
            set
            {
                explosions = value;
            }
        }

        public static SoundEffect CashRegister
        {
            set
            {
                cashRegister = value;
            }
        }

        public static SoundEffect Aww
        {
            set
            {
                aww = value;
            }
        }

        public static SoundEffect Yay
        {
            set
            {
                yay = value;
            }
        }

        public static SoundEffect Applause
        {
            set
            {
                applause = value;
            }
        }

        public static SoundEffect ApplauseLower
        {
            set
            {
                applauseLower = value;
            }
        }

        public static SoundEffect SpawnBonusEnemies
        {
            set
            {
                spawnBonusEnemies = value;
            }
        }

        public static Song Theme
        {
            set
            {
                theme = value;
            }
        }

        public static SoundEffect NormalShot
        {
            set
            {
                normalShot = new SoundEffectInstance[30];
                for (int i = 0; i < normalShot.Length; i++)
                {
                    normalShot[i] = value.CreateInstance();
                }
                   // normalShot = value;
            }
        }

        public static SoundEffect PickItem
        {
            set
            {
                pickItem = new SoundEffectInstance[10];
                for (int i = 0; i < pickItem.Length; i++)
                {
                    pickItem[i] = value.CreateInstance();
                }

            }
        }

        public static SoundEffect RocketShot
        {
            set
            {
                rocketShot = new SoundEffectInstance[10];
                for (int i = 0; i < rocketShot.Length; i++)
                {
                    rocketShot[i] = value.CreateInstance();
                }

            }
        }

        public static SoundEffect PlayerDead
        {
            set
            {
                playerDead = value;
            }
        }

        

        public static SoundEffect SpawnItem
        {
            set
            {
                spawnItem = value;
            }
        }

        public static SoundEffect SpawnPlayer
        {
            set
            {
                spawnPlayer = value;
            }
        }

        public static float volume = .4f, pitch = 0, pan = 0;

        public static void PlaySlowDown()
        {
            if (!GameStatics.sounds)
                return;

            slowDown.Play(1f, pitch, pan);

        }

        public static void PlaySlowUp()
        {
            if (!GameStatics.sounds)
                return;

            slowUp.Play(1f, pitch, pan);
        }

        public static void PlayTheme()
        {
            if (!GameStatics.music)
                return;

            if(isMusicPlaying)
                return;

            MediaPlayer.Play(theme);
            MediaPlayer.IsRepeating = true;
            isMusicPlaying = true;
        }

        public static void StopTheme()
        {
            if(!isMusicPlaying)
                return;

            MediaPlayer.Stop();
            isMusicPlaying = false;
        }

        public static void PlayCashRegister()
        {
            if (!GameStatics.sounds)
                return;

            cashRegister.Play(1f, pitch, pan);
        }


        public static void PlayYay()
        {
            if (!GameStatics.sounds)
                return;

            yay.Play(1f, pitch, pan);
        }

        public static void PlayApplause()
        {
            if (!GameStatics.sounds)
                return;

            applause.Play(1f, pitch, pan);
        }

        public static void PlayApplauseLower()
        {
            if (!GameStatics.sounds)
                return;

            applauseLower.Play(1f, pitch, pan);
        }

        public static void PlaySpawnBonusEnemies()
        {
            if (!GameStatics.sounds)
                return;

            spawnBonusEnemies.Play(volume, pitch, pan);
        }

        static int indexNormalShot = 0;
        static int indexRocketShot = 0;
        static int indexPickItem = 0;

        public static void PlayNormalShot()
        {
            if (!GameStatics.sounds)
                return;

            normalShot[indexNormalShot].Volume = volume;
            normalShot[indexNormalShot].Pitch = pitch;
            normalShot[indexNormalShot].Pan = pan;
            normalShot[indexNormalShot].Play();
           // normalShot.Play(volume, pitch, pan);

            indexNormalShot++;

            if (indexNormalShot > normalShot.Length - 1)
                indexNormalShot = 0;
        }

        public static void PlayRocketShot()
        {
            if (!GameStatics.sounds)
                return;

            rocketShot[indexRocketShot].Volume = volume;
            rocketShot[indexRocketShot].Pitch = pitch;
            rocketShot[indexRocketShot].Pan = pan;
            rocketShot[indexRocketShot].Play();
            // normalShot.Play(volume, pitch, pan);

            indexRocketShot++;

            if (indexRocketShot > rocketShot.Length - 1)
                indexRocketShot = 0;
        }

        public static void PlayPickItem()
        {
            if (!GameStatics.sounds)
                return;

            pickItem[indexPickItem].Volume = volume;
            pickItem[indexPickItem].Pitch = pitch;
            pickItem[indexPickItem].Pan = pan;
            pickItem[indexPickItem].Play();
            // normalShot.Play(volume, pitch, pan);

            indexPickItem++;

            if (indexPickItem > pickItem.Length - 1)
                indexPickItem = 0;
        }

        public static void PlayPlayerDead()
        {
            if (!GameStatics.sounds)
                return;

            playerDead.Play(volume, pitch, pan);
        }

        public static void PlaySpawnItem()
        {
            if (!GameStatics.sounds)
                return;

            spawnItem.Play(volume, pitch, pan);
        }

        public static void PlaySpawnPlayer()
        {
            if (!GameStatics.sounds)
                return;

            spawnPlayer.Play(1f, pitch, pan);
        }

        public static void PlayAww()
        {
            if (!GameStatics.sounds)
                return;

            aww.Play(1f, pitch, pan);
        }

        public static void Update()
        {
            //Stuff about souund and pitch modulation for slow mo effeect.
#if WINDOWS_PHONE
            MediaPlayer.Volume = MathHelper.Lerp(.5f, 0, (float)GameplayScreen.slowMoTransition / GameplayScreen.timeToSlowMo);
#else
            MediaPlayer.Volume = MathHelper.Lerp(1f, 0, (float)GameplayScreen.slowMoTransition / GameplayScreen.timeToSlowMo);
#endif
            pitch = MathHelper.Lerp(0, -1, (float)GameplayScreen.slowMoTransition / GameplayScreen.timeToSlowMo);           
        }

        public static void PlayExplosion()
        {
            if (!GameStatics.sounds)
                return;

            Random rand= new Random();
            explosions[rand.Next(explosions.Length)].Play(volume, pitch, pan);
        }

        public static void Dispose()
        {
            StopTheme();

            theme = null;
            normalShot = null;
            pickItem = null;
            playerDead = null;
            rocketShot = null;
            spawnItem = null; 
            spawnPlayer = null;
            spawnBonusEnemies = null;
            yay = null;
            applause = null;
            applauseLower = null;
            spawnBonusEnemies = null;
            cashRegister = null;
            aww = null;
            slowDown = null;
            slowUp = null;
            explosions = null;
        }
    }
}
