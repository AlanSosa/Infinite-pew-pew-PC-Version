using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class MobileSpawner : Enemy
    {
        private float EnemiesToSpawn;
        private bool spawnEnemies = false;
        private int enemyType;
        private int baseHp;
        private float cooldownFrames;
        private float cooldownRemaining = 0;
        private bool shootBullets;

        public MobileSpawner(Vector2 Position , int enemiesToSpawn, int enemyType, bool shootBullets)
        {
            this.texture = EntityManager.Resources.CoreFollowerBig;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 400;
            this.HP = baseHp=12;
            this.EnemiesToSpawn = enemiesToSpawn;
            this.AddBehaviour(this.MoveRandomly());
            this.enemyType = enemyType;
            cooldownFrames = 4;
            this.shootBullets = shootBullets;
        }

        public MobileSpawner(Vector2 Position, int enemiesToSpawn, int enemyType)
        {
            this.texture = EntityManager.Resources.CoreFollowerBig;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 100;
            this.HP = baseHp=12;
            this.EnemiesToSpawn = enemiesToSpawn;
            this.AddBehaviour(this.MoveRandomly());
            this.enemyType = enemyType;
            cooldownFrames = 4;
            this.shootBullets = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (!EntityManager.CanSpawn)
                return;

            if (spawnEnemies)
            {
                switch (enemyType)
                {
                    case 1:
                        EntityManager.Add(new Wanderer(this.Position));
                        break;
                    case 2:
                        EntityManager.Add(new Seeker(this.Position));
                        break;
                    case 3:
                        EntityManager.Add(new Bouncer(this.Position));
                        break;
                    case 4:
                        EntityManager.Add(new SeekerM2(this.Position));
                        break;
                    case 5:
                        EntityManager.Add(new SeekerFasterFar(this.Position));
                        break;
                    case 6:
                        EntityManager.Add(new SeekerM1(this.Position));
                        break;
                    case 7:
                        EntityManager.Add(new SquareMini(this.Position));
                        break;
                }

                if (EntityManager.CanSpawnEnemyBullets)
                {
                    if (shootBullets)
                    {
                        var aim = Vector2.Normalize((EntityManager.PlayerShip.Position + EntityManager.PlayerShip.VelocityAim * 2) - this.Position);

                        if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
                        {
                            cooldownRemaining = cooldownFrames / GameplayScreen.SlowMoMultiplier;
                            float aimAngle;
                            aimAngle = aim.ToAngle();
                            Vector2 vel = MathUtil.FromPolar(aimAngle, (PlayerStatus.Level >= 15 ? 15f : 10f));
                            EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position, vel, Bullet.BULLETTYPE.ENEMY, 1));
                        }
                    }
                }

                if (cooldownRemaining > 0)
                    cooldownRemaining--;

                EnemiesToSpawn--;
                if (EnemiesToSpawn <= 0)
                    this.Kill();
            }
        }

        public override void Kill()
        {
            base.Kill();
        }

        public override void WasShot()
        {
            base.WasShot();
            if (HP <= 0)
            {
                if (spawnEnemies && HP <= 0)
                {
                    this.Kill();
                }

                HP = baseHp;
                spawnEnemies = true;
            }
        }
    }
}
