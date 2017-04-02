using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Entites.Enemies;
using ThridWolrdShooterGame.Entites.Items;
using ThridWolrdShooterGame.Entites;

namespace ThridWolrdShooterGame.Managers
{
    public static class EntityManager
    {
        private static List<Entity> entities;
        private static List<Entity> entitiesToAdd;
        private static bool isUpdating = false;
        private static QuadTree quadtreeMain;
        private static List<Enemy> enemies;
        private static List<Enemy> shooters;
        private static List<Enemy> spawnerMobile;

        private static List<Bullet> playerBullets;
        private static List<Bullet> enemyBullets;

        public static bool CanSpawnEnemyBullets
        {
            get
            {
                return enemyBullets.Count() < 120;
            }
        }

        public static bool CanSpawnPlayerBullets
        {
            get
            {
                return playerBullets.Count() < 40;
            }
        }

        public static bool CanSpawnShooter
        {
            get
            {
                return shooters.Count() < 3;
            }
        }

        public static bool CanSpawnSpawner
        {
            get
            {
                return spawnerMobile.Count() < 3;
            }
        }

        private static List<Type> types = new List<Type>();
        public static int AvoidDrawingCount
        {
            get
            {
                return types.Count();
            }
        }

        public static bool CanSpawn
        {
            get
            {
                return EnemiesCount <= 100;
            }
        }

        private static Rectangle blankTexture = new Rectangle(0, 0, GameplayScreen.graphicsWidth, GameplayScreen.graphicsHeight);

        public static int EnemiesCount
        {
            get
            {
                return enemies.Count();
            }
        }

        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }

        public static void Add(Entity entity)
        {
            if (entity is SpinAnd180Shooter ||
                entity is SpinAnd360Shooter ||
                entity is SpiralShooter)
                if (!CanSpawnShooter)
                    return;

            if (entity is MobileSpawner)
                if (!CanSpawnSpawner)
                    return;

            if (!isUpdating)
                addEntity(entity);
            else
                entitiesToAdd.Add(entity);
        }

        private static void addEntity(Entity entity)
        {
            if (entity is Enemy)
                enemies.Add(entity as Enemy);

            if (entity is PlayerShip)
                PlayerShip = entity as PlayerShip;

            if (entity is SpinAnd180Shooter ||
                entity is SpinAnd360Shooter ||
                entity is SpiralShooter)
                shooters.Add(entity as Enemy);

            if (entity is MobileSpawner)
                spawnerMobile.Add(entity as Enemy);

            if (entity is Bullet && (entity as Bullet).Type == Bullet.BULLETTYPE.ENEMY)
                enemyBullets.Add(entity as Bullet);
            else if ((entity is Bullet) && (entity as Bullet).Type == Bullet.BULLETTYPE.PLAYER)
                playerBullets.Add(entity as Bullet);

            entities.Add(entity);
        }

        public static void Unload()
        {
            entities = new List<Entity>();
            enemies = new List<Enemy>();
            entitiesToAdd = new List<Entity>();
            shooters = new List<Enemy>();
            spawnerMobile = new List<Enemy>();
            enemyBullets = new List<Bullet>();
            playerBullets = new List<Bullet>();
            Resources = null;
            PlayerShip = null;
        }

        public static Resources Resources
        {
            get;
            private set;
        }

        public static PlayerShip PlayerShip
        {
            get;
            private set;
        }

        public static void SetResources(Resources resources)
        {
            Resources = resources;
        }

        public static void Init(QuadTree quadtree)
        {
            quadtreeMain = quadtree;
            entities = new List<Entity>();
            enemies = new List<Enemy>();
            entitiesToAdd = new List<Entity>();

            shooters = new List<Enemy>();
            spawnerMobile = new List<Enemy>();

            enemyBullets = new List<Bullet>();
            playerBullets = new List<Bullet>();
        }

        public static void Update(GameTime gametime)
        {
            isUpdating = true;

            quadtreeMain.clear();
            entities.ForEach(x => quadtreeMain.insert(x));
            handleCollision();

            foreach (var entity in entities)
            {
                entity.Update(gametime);
            }

            isUpdating = false;

            foreach (var entity in entitiesToAdd)
                addEntity(entity);

            entitiesToAdd.Clear();

            shooters = shooters.Where(entity => !entity.IsExpired).ToList();
            spawnerMobile = spawnerMobile.Where(entity => !entity.IsExpired).ToList();
            enemyBullets = enemyBullets.Where(entity => !entity.IsExpired).ToList();
            playerBullets = playerBullets.Where(entity => !entity.IsExpired).ToList();

            entities = entities.Where(entity => !entity.IsExpired).ToList();
            enemies = enemies.Where(enemy => !enemy.IsExpired).ToList();
        }

        public static void RemoveEnemies()
        {
            enemies.ForEach(enemy => enemy.Kill());
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                if (types.Any(x => x == entity.GetType()))
                    continue;

                entity.Draw(spriteBatch);
            }
        }

        public static void RemoveDrawingCapabilities(Type type)
        {
            types.Add(type);
        }

        public static void RegainDrawingCapabilities(Type type)
        {
            types = types.Where(elements => elements.GetType() != type).ToList();
        }

        public static void RegainAllDrawingCapabilities()
        {
            types = new List<Type>();
        }

        private static void handleCollision()
        {

            List<Entity> returnObjects = new List<Entity>();

            for (int i = 0; i < entities.Count; i++)
            {

                returnObjects.Clear();
                quadtreeMain.retrieve(returnObjects, entities[i]);

                for (int x = 0; x < returnObjects.Count; x++)
                {

                    if (IsColliding(entities[i], returnObjects[x]))
                    {


                        if (entities[i] is Bullet && returnObjects[x] is Enemy)
                        {
                            (entities[i] as Bullet).WasDamaged();
                            (returnObjects[x] as Enemy).WasShot();
                        }

                        if (entities[i] is Enemy && returnObjects[x] is Enemy)
                        {
                            (entities[i] as Enemy).HandleCollision(returnObjects[x] as Enemy);
                            (returnObjects[x] as Enemy).HandleCollision(entities[i] as Enemy);
                        }

                        if (entities[i] is Item && returnObjects[x] is Item)
                        {
                            (entities[i] as Item).HandleCollision(returnObjects[x] as Item);
                            (returnObjects[x] as Item).HandleCollision(entities[i] as Item);
                        }

                        if (entities[i] is PlayerShip && returnObjects[x] is Enemy)
                        {
                            if ((entities[i] as PlayerShip).IsActive && (returnObjects[x] as Enemy).IsActive)
                            {
                                (entities[i] as PlayerShip).WasDamaged();
                                (returnObjects[x] as Enemy).WasShot();
                                //(returnObjects[x] as Enemy).Kill();
                            }
                        }

                        if (entities[i] is Bullet && returnObjects[x] is PlayerShip)
                        {
                            if ((entities[i] as Bullet).Type == Bullet.BULLETTYPE.ENEMY && (returnObjects[x] as PlayerShip).IsActive)
                            {
                                entities[i].IsExpired = true;
                                //(returnObjects[x] as PlayerShip).Kill();
                                (returnObjects[x] as PlayerShip).WasDamaged();
                            }
                        }

                        if (entities[i] is Item && returnObjects[x] is PlayerShip)
                        {
                            entities[i].IsExpired = true;
                            (returnObjects[x] as PlayerShip).HandleCollision(entities[i] as Item);
                        }

                        //Collide player with enemies
                        //Collide player with bullets
                    }
                }
            }
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }
    }
}
