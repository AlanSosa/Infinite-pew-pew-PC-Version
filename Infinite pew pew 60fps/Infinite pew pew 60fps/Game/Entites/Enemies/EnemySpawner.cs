using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyExtensions;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Entites;
using ThridWolrdShooterGame.Entites.Enemies;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;
using ThridWolrdShooterGame.Entites.Messages;
using ThridWolrdShooterGame.Entites.Items;
using GameStateManagement.Managers;

namespace ThridWolrdShooterGame
{
    public static class EnemySpawner
    {
        private static int basket; // Enemies to spawn per batch
        public static bool IsSpawning
        {
            get;
            set;
        }
        private static int enemyLevel; // Enemy Level that unlocks new enemies.
        private static int maxEnemies; // Max count of enemies that the game can have on screen.
        private static int hordeLevel; // It's just a number that multiplied by the PLayerStatus.Level, defines the amount of enemies to spawn per horde. 
        private static int hordeCount; //The amount of enemies per horde.

        private static int enemyLevelSum;

        private static int enemyLevelMultiplicator;

        private enum SPAWNZONES
        {
            AMBUSH, NORTH, SOUTH,
            EAST, WEST, NORTHEAST,
            NORTHWEST, SOUTHEAST, SOUTHWEST, RANDOM
        };

        private static List<SPAWNZONES> spawnZones;

        private static SPAWNZONES[] spawningZonesSelector;

        private static Random random = new Random();
        private static float[] enemiesPerCategory;
        private static int enemiesPerZone;

        private static int index;
        private static int hordeBatchAcumulator;

        private static int[,] enemy;

        private static int elapsedTime = 0;
        private static bool IsBatchAcumulatorEqual;
        private static int categoryPicked;

        private static bool bonusEnemiesCreated;

        /*
         * private static Enemy[,] enemies = new Enemy[7, 2] { 
{new Seeker(), new Wanderer()}, 
{new SeekerM1(), new SeekerM2()}, 
{new SeekerFasterFar(), new Bouncer()}, 
{new SpinAnd180Shooter(), new SpinAnd360Shooter()},
{new SpiralShooter(), new SpiralShooter()},
{new MobileSpawner(), new EnemyBarricade()},
{new MobileSpawner(), new EnemyBarricade()}
};
         */
        private static int printInstruction;
        private static int instructionIncrement;
        private static int idInstruction;

        private static float counterGetReady;
        private static float counterGo;

        private static int bonusLevelCounter;
        public static void Reset()
        {

            BonusModeOff();
            bonusLevelCounter = 0;
            enemyLevel = 1;
            //maxEnemies = 60;
            hordeLevel = 15;
            spawningZonesSelector = new SPAWNZONES[] { SPAWNZONES.AMBUSH, 
                SPAWNZONES.NORTH, SPAWNZONES.SOUTH, 
                SPAWNZONES.EAST, SPAWNZONES.WEST, 
                SPAWNZONES.NORTHEAST, SPAWNZONES.NORTHWEST, 
                SPAWNZONES.SOUTHEAST, SPAWNZONES.SOUTHWEST,
                SPAWNZONES.RANDOM};
            enemiesPerCategory = new float[] { 3, 4, 4, 4f, 1f, 1f, 1f };
            //enemiesPerCategory = new float[] { 4, 4, 4, 2f, 2f, 2f, 2f };
            index = 0;
            basket = 0;
            hordeBatchAcumulator = 0;
            hordeCount = PlayerStatus.Level * hordeLevel;
            enemiesPerZone = 15;
            enemyLevelMultiplicator = enemyLevelSum = 3;

            bonusLevelCounter += random.Next(4, 6);
            bonusEnemiesCreated = false;
            BonusMode = false;
            mobileSpawnersCount = 0;
        }


        public static void Update(GameTime gameTime)
        {
            if (EntityManager.EnemiesCount <= 0 && IsBatchAcumulatorEqual)
            {
                levelUp();
            }

            if (IsSpawning)
            {
                if (!EntityManager.CanSpawn)
                    return;

                elapsedTime += GameplayScreen.GameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime < 50)
                    return;

                elapsedTime = 0;

                spawnNormalEnemies();


            }
            else if (BonusMode)
            {

                if (!bonusEnemiesCreated)
                {
                    timerToSpawnBonus += gameTime.ElapsedGameTime.Milliseconds;
                    counterGetReady += gameTime.ElapsedGameTime.Milliseconds;
                    counterGo += gameTime.ElapsedGameTime.Milliseconds;

                    if (timerToSpawnBonus >= 10500)
                    {
                        timerToSpawnBonus = 0;
                        spawnBonusEnemies();
                        bonusEnemiesCreated = true;
                    }


                    if (timerToSpawnBonus > printInstruction)
                    {

                        new Vector2(GameplayScreen.graphicsWidth / 2 - 130, (GameplayScreen.graphicsHeight / 2) - 150);

                        printInstruction += instructionIncrement;
                        switch (idInstruction)
                        {
                            case 0:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 400),
                                    "Win Prizes by killing", 240 * 2, 15 * 2));
                                break;
                            case 1:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 370),
                                    "all the enemies", 210 * 2, 15 * 2));
                                break;
                            case 2:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 340),
                                    "before the timer", 180 * 2 , 15 * 2));
                                break;
                            case 3:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 310),
                                    "reaches 0.", 150 * 2, 15 * 2));
                                break;
                            case 4:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 280),
                                    "Bonus level ends", 120 * 2, 15 * 2));
                                break;
                            case 5:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 250),
                                    "when an enemy", 90 * 2, 15 * 2));
                                break;
                            case 6:
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight - 220),
                                    "hurts you.", 60 * 2, 15 * 2));

                                break;
                        }
                        idInstruction++;
                    }

                    if (counterGetReady > 9000 && !printedGetReady)
                    {
                        HudManager.Add(new ShakingMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 150, GameplayScreen.graphicsHeight / 2 - 100), 1500, "Get Ready!"));
                        printedGetReady = true;
                        SoundManager.PlaySpawnBonusEnemies();
                    }

                    if (counterGo > 10500 && !printedGo)
                    {
                        HudManager.Add(new ShakingMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 20, GameplayScreen.graphicsHeight / 2 - 100), 1000, "Go!"));
                        printedGo = true;

                        ShieldAux = PlayerStatus.Shield;
                        printInstruction = 0;
                        idInstruction = 0;
                        timerToSpawnBonus = 0;
                    }
                    // Print Instrucctions
                }

                if (bonusEnemiesCreated && EntityManager.EnemiesCount > 0)
                {
                    if (ShieldAux != PlayerStatus.Shield)
                        HudManager.BonusClock.StopTiming();
                    else
                        HudManager.BonusClock.StartTiming();
                }

                //NO enemies let's qualify the player
                if (bonusEnemiesCreated && EntityManager.EnemiesCount == 0)
                {
                    timerToSpawnBonus += gameTime.ElapsedGameTime.Milliseconds;
                    printInstruction += gameTime.ElapsedGameTime.Milliseconds;
                    if (!exitMessageSpawn)
                    {
                        GameplayScreen.CanPause = false;
                        if (!printedBonusLevelFinished)
                        {
                            SoundManager.PlayCashRegister();
                            HudManager.Add(new SlideInMessage(new Vector2(-500, (GameplayScreen.graphicsHeight / 2) - 100),
                                "Bonus Level Finished!!", new Vector2(GameplayScreen.graphicsWidth / 2 - 250, (GameplayScreen.graphicsHeight / 2) - 150), 7000f));

                            printedBonusLevelFinished = true;
                        }

                        if (ShieldAux == PlayerStatus.Shield)
                        {
                            if (timerToSpawnBonus > 2000 && !printedResultMessage)
                            {
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 250, GameplayScreen.graphicsHeight / 2),
                                    "Result : ", 120 * 2, 15 * 2, 1.3f));

                                printedResultMessage = true;
                            }

                            if (HudManager.BonusClock.MilisecondsTimer > 4000 && !printedPrizeResultMessage && timerToSpawnBonus > 4000)
                            {
                                SoundManager.PlayYay();

                                HudManager.Add(new ShakingMessage(new Vector2(350, GameplayScreen.graphicsHeight - 320), 3000, "Awesome"));
                                prizesCount = 4;

                                printedPrizeResultMessage = true;
                                exitMessageSpawn = true;
                            }
                            else if (HudManager.BonusClock.MilisecondsTimer > 2000 && !printedPrizeResultMessage && timerToSpawnBonus > 4000)
                            {
                                SoundManager.PlayApplause();
                                HudManager.Add(new ShakingMessage(new Vector2(350, GameplayScreen.graphicsHeight - 320), 3000, "Incredible"));
                                prizesCount = 3;
                                printedPrizeResultMessage = true;
                                exitMessageSpawn = true;
                            }
                            else if (HudManager.BonusClock.MilisecondsTimer > 1000 && !printedPrizeResultMessage && timerToSpawnBonus > 4000)
                            {
                                SoundManager.PlayApplauseLower();
                                HudManager.Add(new ShakingMessage(new Vector2(350, GameplayScreen.graphicsHeight - 320), 3000, "Good"));
                                prizesCount = 2;
                                printedPrizeResultMessage = true;
                                exitMessageSpawn = true;
                            }
                            else if (!printedPrizeResultMessage && timerToSpawnBonus > 4000)
                            {
                                HudManager.Add(new ShakingMessage(new Vector2(350, GameplayScreen.graphicsHeight - 320), 3000, "Nice"));
                                prizesCount = 1;
                                printedPrizeResultMessage = true;
                                exitMessageSpawn = true;
                            }
                        }
                        else
                        {

                            if (timerToSpawnBonus > 2000 && !printedResultMessage)
                            {
                                HudManager.Add(new FadeInMessage(new Vector2(GameplayScreen.graphicsWidth / 2 - 250, GameplayScreen.graphicsHeight - 300),
                                    "Result : ", 120, 15, 1.3f));
                                printedResultMessage = true;
                            }

                            if (HudManager.BonusClock.MilisecondsTimer > 4000 && !printedPrizeResultMessage && timerToSpawnBonus > 4000)
                            {

                                HudManager.Add(new ShakingMessage(new Vector2(350, GameplayScreen.graphicsHeight - 320), 3000, "No Bonus!"));
                               // prizesCount = 4;

                                printedPrizeResultMessage = true;
                                exitMessageSpawn = true;

                                SoundManager.PlayAww();

                            }
                            /* HudManager.Add(new SlideInMessage(new Vector2(GameplayScreen.graphicsWidth + 500, (GameplayScreen.graphicsHeight / 2)),
                             "No Bonus.Remember......", new Vector2(20, (GameplayScreen.graphicsHeight / 2)), 5000f)
                             );

                             HudManager.Add(new SlideInMessage(new Vector2(GameplayScreen.graphicsWidth + 500, (GameplayScreen.graphicsHeight / 2) + 300),
                                 "Don't let them harm you!.", new Vector2(20, (GameplayScreen.graphicsHeight / 2) + 50), 5000f,Color.Yellow, .5f)
                                 );
                             exitMessageSpawn = true;*/
                        }

                        HudManager.BonusClock.StopTiming();
                    }

                    if (!itemSpawn)
                    {
                        if (timerToSpawnBonus >= 4000 && exitMessageSpawn && ShieldAux == PlayerStatus.Shield)
                        {
                            ItemSpawner.SpawnPrizes(prizesCount);
                            GameplayScreen.Camera.Shake(15f, .8f);
                            itemSpawn = true;
                        }
                    }

                    if (timerToSpawnBonus >= 7000)
                    {
                        BonusModeOff();
                    }

                }
            }

        }


        private static bool printedGetReady;
        private static bool printedGo;
        private static bool instructionsPrinted;
        private static bool printedResultMessage;
        private static bool printedBonusLevelFinished;
        private static bool printedPrizeResultMessage;

        public static void BonusModeOff()
        {
            BonusMode = false;
            IsSpawning = true;
            timerToSpawnBonus = 0;
            GameplayScreen.CanPause = true;
            HudManager.BonusClock.ResetTimer();
            counterGetReady = 0;
            counterGo = 0;

            idInstruction = 0;
            printInstruction = 1500;
            instructionIncrement = 1000;
            printedGetReady = false;
            printedResultMessage = false;
            printedGo = false;
            printedPrizeResultMessage = false;
            printedBonusLevelFinished = false;
            exitMessageSpawn = false;
            itemSpawn = false;
            instructionsPrinted = false;
            prizesCount = 0;
            bonusEnemiesCreated = false;
            instructionsPrinted = false;
        }

        private static bool itemSpawn;
        private static bool exitMessageSpawn;
        private static float timerToSpawnBonus = 0;
        private static int prizesCount;

        public static bool BonusMode
        {
            get;
            private set;
        }

        public static int ShieldAux
        {
            get;
            set;
        }

        private static int mobileSpawnersCount;

        private static void levelUp()
        {
            mobileSpawnersCount = 0;
            PlayerStatus.Level++;
            IsBatchAcumulatorEqual = false;
            IsSpawning = true;
            basket = 0;
            hordeCount = hordeLevel * PlayerStatus.Level;
            hordeBatchAcumulator = 0;
            ShieldAux = 0;

            if (PlayerStatus.Level == enemyLevelMultiplicator)
            {
                //Debug.WriteLine("PLayer level : " + PlayerStatus.Level + " BonusLevelcoming at : " + bonusLevelCounter);
                enemyLevelMultiplicator += enemyLevelSum;
                enemyLevel++;
            }

            if (PlayerStatus.Level == bonusLevelCounter)
            {
                //Bonus Level Activation
                BonusMode = true;
                IsSpawning = false;
                SoundManager.PlayCashRegister();
                GameplayScreen.CanPause = false;
                HudManager.Add(new SlideInMessage(new Vector2(-800, (GameplayScreen.graphicsHeight / 2) - 150),
                    "Bonus Level",
                    new Vector2(GameplayScreen.graphicsWidth / 2 - 130, (GameplayScreen.graphicsHeight / 2) - 150),
                    8000f));

                printInstruction = 500;
                //Bonus Level Activation
                bonusLevelCounter += random.Next(3, 9);
            }
        }

        #region SPAWNING METHODS
        private static void spawnNormalEnemies()
        {
            //If the last batch has been deployed, let's start a new one.
            if (basket <= 0)
            {
                //if the sum of all batches it's equal to the horde count, it means that we have already deployed the amount of enemies per horde.
                if (hordeBatchAcumulator >= hordeCount)
                {
                    IsBatchAcumulatorEqual = true;
                    return;
                }
                configureSpawn();
            }

            Vector2 position = Vector2.Zero;

            if (index > spawnZones.Count() - 1)
                index = 0;

            switch (spawnZones[index])
            {
                case SPAWNZONES.NORTH:
                    position = spawnNorth();
                    break;
                case SPAWNZONES.SOUTH:
                    position = spawnSouth();
                    break;
                case SPAWNZONES.EAST:
                    position = spawnEast();
                    break;
                case SPAWNZONES.WEST:
                    position = spawnWest();
                    break;
                case SPAWNZONES.NORTHEAST:
                    position = spawnNorthEast();
                    break;
                case SPAWNZONES.NORTHWEST:
                    position = spawnNorthWest();
                    break;
                case SPAWNZONES.SOUTHEAST:
                    position = spawnSouthEast();
                    break;
                case SPAWNZONES.SOUTHWEST:
                    position = spawnSouthWest();
                    break;
                case SPAWNZONES.RANDOM:
                    position = spawnRandom();
                    break;
                case SPAWNZONES.AMBUSH:
                    position = spawnAmbush();
                    break;
            }

            if (basket > 0)
            {
                Enemy enemyToSpawn;

                switch (enemy[0, 0])
                {
                    case 0:

                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new Wanderer(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new Wanderer(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }

                        break;
                    case 1:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new Seeker(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new SeekerM1(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }

                        break;
                    case 2:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new SeekerM2(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new SeekerFasterFar(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }
                        break;
                    case 3:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new Bouncer(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new Bouncer(position);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }

                        break;
                    case 4:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                if (mobileSpawnersCount < 2)
                                {
                                    mobileSpawnersCount++;
                                    enemyToSpawn = new SpinAnd360Shooter(position, 40);
                                    EntityManager.Add(enemyToSpawn);
                                }
                                break;
                            case 1:
                                if (mobileSpawnersCount < 2)
                                {
                                    mobileSpawnersCount++;
                                    enemyToSpawn = new SpinAnd180Shooter(position, 10);
                                    EntityManager.Add(enemyToSpawn);
                                }
                                break;
                        }
                        break;
                    case 5:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new SpiralShooter(position, 40, 4, 5);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new SpiralShooter(position, 40, 4, 5);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }
                        break;
                    case 6:
                        switch (enemy[0, 1])
                        {
                            case 0:
                                enemyToSpawn = new MobileSpawner(position, 20, 7);
                                EntityManager.Add(enemyToSpawn);
                                break;
                            case 1:
                                enemyToSpawn = new MobileSpawner(position, 20, 7);
                                EntityManager.Add(enemyToSpawn);
                                break;
                        }
                        break;

                }
            }
            basket--;
        }

        private static void spawnBonusEnemies()
        {
            SoundManager.PlaySpawnBonusEnemies();
            GameplayScreen.Camera.Shake(15f, .8f);
            GameplayScreen.CanPause = true;
            for (int i = 0; i < 2; i++)
            {
                int numID = random.Next(1, 3);
                Vector2 position;
                switch (numID)
                {
                    case 1:
                        if (i == 0)
                            position = spawnSouth();
                        else
                            position = spawnNorth();
                        EntityManager.Add(new MobileSpawner(position, 60, random.Next(1, 7), true));
                        break;
                    case 2:
                        if (i == 0)
                            position = spawnSouth();
                        else
                            position = spawnNorth();

                        EntityManager.Add(new EnemyBarricade(position, 30, 50));
                        break;
                }
            }
        }

        #endregion

        private static void configureSpawn()
        {
            spawnZones = new List<SPAWNZONES>();
            categoryPicked = random.Next(0, (enemyLevel > enemiesPerCategory.Count() ? enemiesPerCategory.Count() : enemyLevel));

            if (enemiesPerCategory[categoryPicked] * PlayerStatus.Level + hordeBatchAcumulator < hordeCount)
            {
                hordeBatchAcumulator += basket = (int)Math.Round((enemiesPerCategory[categoryPicked] * PlayerStatus.Level));
            }
            else
            {
                hordeBatchAcumulator += basket = hordeCount - hordeBatchAcumulator;
            }

            if (basket > 15)
            {
                int idselector = random.Next(0, 3);
                switch (idselector)
                {
                    case 0:
                        spawnZones.Add(spawningZonesSelector[random.Next(0, spawningZonesSelector.Count())]);
                        break;
                    case 1:
                        int auxDouble = random.Next(0, 4);
                        //Just in case ;)
                        if (auxDouble == 4)
                            auxDouble = 3;
                        //Just in case ;)

                        if (auxDouble == 0)
                        {
                            spawnZones.Add(SPAWNZONES.NORTHWEST);
                            spawnZones.Add(SPAWNZONES.SOUTHEAST);
                        }
                        if (auxDouble == 1)
                        {
                            spawnZones.Add(SPAWNZONES.NORTH);
                            spawnZones.Add(SPAWNZONES.SOUTH);
                        }
                        if (auxDouble == 2)
                        {
                            spawnZones.Add(SPAWNZONES.NORTHEAST);
                            spawnZones.Add(SPAWNZONES.SOUTHWEST);
                        }
                        if (auxDouble == 3)
                        {
                            spawnZones.Add(SPAWNZONES.EAST);
                            spawnZones.Add(SPAWNZONES.WEST);
                        }
                        break;
                    case 2:
                        int auxQuadruple = random.Next(0, 2);
                        if (auxQuadruple == 2)
                            auxQuadruple = 1;

                        if (auxQuadruple == 0)
                        {
                            spawnZones.Add(SPAWNZONES.NORTH);
                            spawnZones.Add(SPAWNZONES.EAST);
                            spawnZones.Add(SPAWNZONES.SOUTH);
                            spawnZones.Add(SPAWNZONES.WEST);
                        }

                        if (auxQuadruple == 1)
                        {
                            spawnZones.Add(SPAWNZONES.NORTHEAST);
                            spawnZones.Add(SPAWNZONES.SOUTHEAST);
                            spawnZones.Add(SPAWNZONES.SOUTHWEST);
                            spawnZones.Add(SPAWNZONES.NORTHWEST);
                        }
                        break;
                }
            }
            else
                spawnZones.Add(spawningZonesSelector[random.Next(0, spawningZonesSelector.Count())]);

            enemy = new int[1, 2] { { categoryPicked, random.Next(0, 2) } };

            enemiesPerZone = 15;
        }

        #region POSITIONING_METHODS

        private static Vector2 getSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(random.Next((int)GameplayScreen.WorldSize.X * 2), random.Next((int)GameplayScreen.WorldSize.Y * 2));
            }
            while (Vector2.DistanceSquared(pos, EntityManager.PlayerShip.Position) < 250 * 250);

            return pos;
        }

        private static Vector2 getSpawnPositionClosePlayer()
        {
            Vector2 pos;
            Rectangle worldBounds = new Rectangle(0, 0, (int)GameplayScreen.WorldSize.X, (int)GameplayScreen.WorldSize.Y);
            int radius = 150;
            do
            {
                pos = new Vector2(random.Next((int)EntityManager.PlayerShip.Position.X - radius, (int)EntityManager.PlayerShip.Position.X + radius),
                    random.Next((int)EntityManager.PlayerShip.Position.Y - radius, (int)EntityManager.PlayerShip.Position.Y + radius));
            }
            while (Vector2.DistanceSquared(pos, EntityManager.PlayerShip.Position) < radius * radius || !worldBounds.Contains(pos.ToPoint()));
            return pos;
        }

        private static Vector2 spawnAmbush()
        {
            Vector2 position;

            position = getSpawnPositionClosePlayer();
            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                index++;
                enemiesPerZone = 15;
            }

            return position;
        }

        private static Vector2 spawnRandom()
        {
            Vector2 position;

            position = getSpawnPosition();
            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                index++;
                enemiesPerZone = 15;
            }
            return position;
        }

        private static Vector2 spawnNorth()
        {
            Vector2 position;
            Vector2 startingPosition = new Vector2(60, 0);

            position = startingPosition * enemiesPerZone;

            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return position;
        }

        private static Vector2 spawnSouth()
        {
            Vector2 position;

            Vector2 startingPosition = new Vector2(60, GameplayScreen.WorldSize.Y);

            position = new Vector2(startingPosition.X * enemiesPerZone, GameplayScreen.WorldSize.Y);

            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return position;
        }

        private static Vector2 spawnEast()
        {
            Vector2 position;
            Vector2 startingPosition = new Vector2(GameplayScreen.WorldSize.X, 60);

            position = new Vector2(GameplayScreen.WorldSize.X, startingPosition.Y * enemiesPerZone);

            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return position;
        }

        private static Vector2 spawnWest()
        {
            Vector2 position;
            Vector2 startingPosition = new Vector2(0, 60);

            position = new Vector2(0, startingPosition.Y * enemiesPerZone);

            enemiesPerZone--;

            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return position;
        }

        private static Vector2 spawnNorthEast()
        {
            enemiesPerZone--;
            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return new Vector2(GameplayScreen.WorldSize.X, 0);
        }

        private static Vector2 spawnNorthWest()
        {
            enemiesPerZone--;
            if (enemiesPerZone == 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return Vector2.Zero;
        }

        private static Vector2 spawnSouthEast()
        {
            if (enemiesPerZone <= 0)
            {
                index++;
                enemiesPerZone = 15;
            }

            enemiesPerZone--;

            return GameplayScreen.WorldSize;
        }

        private static Vector2 spawnSouthWest()
        {
            enemiesPerZone--;
            if (enemiesPerZone <= 0)
            {
                enemiesPerZone = 15;
                index++;
            }

            return new Vector2(0, GameplayScreen.WorldSize.Y);
        }

        #endregion
    }
}