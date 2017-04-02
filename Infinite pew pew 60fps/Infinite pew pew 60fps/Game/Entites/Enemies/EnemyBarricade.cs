using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;
using System.Diagnostics;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    // This enemy move in way that it's path creates a cirlce. whiel spawning a certain amount of squares.
    public class EnemyBarricade:Enemy
    {
        private int elapsedTime;
        private int enemiesToSpawn;
        private float spanTimeToSpawnEnemies;

        public EnemyBarricade(Vector2 Position, int enemiesToSpawn, float spanTimeToSpawnEnemies)
        {
            this.texture = EntityManager.Resources.Seeker;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White;
            this.PointValue = 400;
            this.HP = 3;
            this.enemiesToSpawn = enemiesToSpawn;
            this.spanTimeToSpawnEnemies = spanTimeToSpawnEnemies;
            this.AddBehaviour(this.MoveInCircles(100 * .5f, 50f * .5f));
            this.timeUntilStart = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsActive)
                return;

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= spanTimeToSpawnEnemies)
            {
                elapsedTime = 0;

                if (EntityManager.CanSpawn)
                {
                    if (enemiesToSpawn > 0)
                    {
                        EntityManager.Add(new SquareMini(this.Position));
                        enemiesToSpawn--;
                    }
                }
            }

            if (EntityManager.CanSpawnEnemyBullets)
            {
                if ((GameplayScreen.GameTime.TotalGameTime.Milliseconds / 500) % 3 == 0)
                {
                    var aim = Vector2.Normalize((EntityManager.PlayerShip.Position + EntityManager.PlayerShip.VelocityAim * 2.5f) - this.Position);
                    Vector2 vel = MathUtil.FromPolar(aim.ToAngle(), (PlayerStatus.Level >= 15 ? 15f * .5f : 10f * .5f));
                    EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position, vel, Bullet.BULLETTYPE.ENEMY, 1));
                }
            }

            // rotate the spray direction
            sprayAngle -= MathHelper.Pi / (50f *.5f);
        }

        private float sprayAngle = 0;

        public override void Kill()
        {
            base.Kill();
        }
        public override void WasShot()
        {
            base.WasShot();
            if (HP <= 0)
                Kill();
        }
    }
}
