using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;
using GameStateManagement.Managers;

namespace ThridWolrdShooterGame.Entites.Items
{
    public static class ItemSpawner
    {

        private static int pointsToSpawnItem;
        private static int pointsCounter;

        public enum ITEMS { NONE, SHIELD_EXTEND, SLOW_MO_EXTEND, WEAPON_EXTEND, AMIGO_EXTEND, PEW, PEW_PEW, WOAH, WOOHOO, BOINGY_BOINGY, AMIGO };

        private static Random random;

        private static int amigosMaxCount;
        public static int AmigosCount
        {
            get;
            set;
        }

        public static void Init()
        {
            random = new Random();
            pointsCounter = 0;
            pointsToSpawnItem = random.Next(15000, 30000);
            amigosMaxCount = 2;
        }

        public static void Update()
        {
            if (EnemySpawner.BonusMode)
                return;

            if (pointsCounter <= PlayerStatus.Points)
            {
                //Spawn Item
                pointsCounter += random.Next(15000, 30000);
                switch (random.Next(1, 8))
                //switch (4)
                {
                    case 1:
                        SoundManager.PlaySpawnItem();
                        Item itemC = new Item(getSpawnPositionClosePlayer(), ITEMS.PEW, EntityManager.Resources.WeaponItem);
                        ItemPointer itemPointerC = new ItemPointer(EntityManager.PlayerShip.Position, itemC);
                        EntityManager.Add(itemC);
                        EntityManager.Add(itemPointerC);
                        break;
                    case 2:
                        SoundManager.PlaySpawnItem();
                        Item itemD = new Item(getSpawnPositionClosePlayer(), ITEMS.PEW_PEW, EntityManager.Resources.WeaponItem);
                        ItemPointer itemPointerD = new ItemPointer(EntityManager.PlayerShip.Position, itemD);
                        EntityManager.Add(itemD);
                        EntityManager.Add(itemPointerD);
                        break;
                    case 3:
                        SoundManager.PlaySpawnItem();
                        Item itemE = new Item(getSpawnPositionClosePlayer(), ITEMS.WOAH, EntityManager.Resources.WeaponItem);
                        ItemPointer itemPointerE = new ItemPointer(EntityManager.PlayerShip.Position, itemE);
                        EntityManager.Add(itemE);
                        EntityManager.Add(itemPointerE);
                        break;
                    case 4:
                        SoundManager.PlaySpawnItem();
                        Item itemF = new Item(getSpawnPositionClosePlayer(), ITEMS.WOOHOO, EntityManager.Resources.WeaponItem);
                        ItemPointer itemPointerF = new ItemPointer(EntityManager.PlayerShip.Position, itemF);
                        EntityManager.Add(itemF);
                        EntityManager.Add(itemPointerF);
                        break;
                    case 5:

                        if (AmigosCount >= amigosMaxCount)
                            return;

                        AmigosCount++;
                        Item itemG = new Item(getSpawnPositionClosePlayer(), ITEMS.AMIGO, EntityManager.Resources.AmigoItem);
                        ItemPointer itemPointerG = new ItemPointer(EntityManager.PlayerShip.Position, itemG);
                        EntityManager.Add(itemG);
                        EntityManager.Add(itemPointerG);
                        SoundManager.PlaySpawnItem();
                        break;
                    case 6:
                        SoundManager.PlaySpawnItem();
                        Item itemA = new Item(getSpawnPositionClosePlayer(), ITEMS.SHIELD_EXTEND, "Shield + 1", EntityManager.Resources.ShieldItem);
                        ItemPointer itemPointerA = new ItemPointer(EntityManager.PlayerShip.Position, itemA);
                        EntityManager.Add(itemA);
                        EntityManager.Add(itemPointerA);
                        break;
                    case 7:
                        SoundManager.PlaySpawnItem();
                        Item itemH = new Item(getSpawnPositionClosePlayer(), ITEMS.BOINGY_BOINGY, EntityManager.Resources.WeaponItem);
                        ItemPointer itemPointerH = new ItemPointer(EntityManager.PlayerShip.Position, itemH);
                        EntityManager.Add(itemH);
                        EntityManager.Add(itemPointerH);
                        break;
                }
            }
        }

        private static Vector2 getSpawnPositionClosePlayer()
        {
            Vector2 pos;
            Rectangle worldBounds = new Rectangle(0, 0, (int)GameplayScreen.WorldSize.X, (int)GameplayScreen.WorldSize.Y);
            int radius = 200;
            do
            {
                pos = new Vector2(random.Next((int)EntityManager.PlayerShip.Position.X - radius, (int)EntityManager.PlayerShip.Position.X + radius),
                    random.Next((int)EntityManager.PlayerShip.Position.Y - radius, (int)EntityManager.PlayerShip.Position.Y + radius));
            }
            while (Vector2.DistanceSquared(pos, EntityManager.PlayerShip.Position) < radius * radius || !worldBounds.Contains(pos.ToPoint()));
            return pos;
        }

        internal static void SpawnPrizes(int prizes)
        {
            SoundManager.PlaySpawnItem();
            for (int i = 0; i < prizes; i++)
                switch (random.Next(0, 4))
                //(switch (6)
                {
                    case 0:
                        Item itemA = new Item(getSpawnPositionClosePlayer(), ITEMS.SHIELD_EXTEND, "Shield + 1 ", EntityManager.Resources.ShieldItem);
                        ItemPointer itemPointerA = new ItemPointer(EntityManager.PlayerShip.Position, itemA);
                        EntityManager.Add(itemA);
                        EntityManager.Add(itemPointerA);
                        break;

                    case 1:
                        Item itemB = new Item(getSpawnPositionClosePlayer(), ITEMS.SLOW_MO_EXTEND, "Badass Time\nincreased", EntityManager.Resources.SlowTimeBonus);
                        ItemPointer itemPointerB = new ItemPointer(EntityManager.PlayerShip.Position, itemB);
                        EntityManager.Add(itemB);
                        EntityManager.Add(itemPointerB);
                        break;

                    case 2:
                        Item itemC = new Item(getSpawnPositionClosePlayer(), ITEMS.WEAPON_EXTEND, "Weapon Time\nincreased", EntityManager.Resources.WeaponBonus);
                        ItemPointer itemPointerC = new ItemPointer(EntityManager.PlayerShip.Position, itemC);
                        EntityManager.Add(itemC);
                        EntityManager.Add(itemPointerC);
                        break;
                    case 3:
                        Item itemD = new Item(getSpawnPositionClosePlayer(), ITEMS.AMIGO_EXTEND, "Amigo Time\nincreased", EntityManager.Resources.AmigoBonus);
                        ItemPointer itemPointerD = new ItemPointer(EntityManager.PlayerShip.Position, itemD);
                        EntityManager.Add(itemD);
                        EntityManager.Add(itemPointerD);
                        break;
                }
        }
    }
}
