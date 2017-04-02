using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SpinAnd180Shooter : Enemy
    {
        private int bulletsToShoot;

        public SpinAnd180Shooter() { }

        public SpinAnd180Shooter(Vector2 position, int bulletsToShoot)
        {
            this.texture = EntityManager.Resources.Shooter180;
            this.Position = position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 200;
            this.HP = 12;
            this.AddBehaviour(this.BounceAndSpin(1.2f * .5f));
            this.bulletsToShoot = bulletsToShoot;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Kill()
        {
            this.rand = new Random();

            var aim = Vector2.Normalize(EntityManager.PlayerShip.Position - this.Position);
            float direction = 0;
            float increment = 180 / this.bulletsToShoot;
            float aimAngle;

            aimAngle = aim.ToAngle();
            Vector2 vel = MathUtil.FromPolar(aimAngle, 15f *.5f);
            EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position, vel, Bullet.BULLETTYPE.ENEMY,1));
            if (EntityManager.CanSpawnEnemyBullets)
            {
                for (int i = 0; i < bulletsToShoot; i++)
                {
                    direction += increment;
                    aimAngle = aim.ToAngle() - (direction * 0.005f);
                    vel = MathUtil.FromPolar(aimAngle, 15f * .5f);
                    EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position, vel, Bullet.BULLETTYPE.ENEMY, 1));

                    aimAngle = aim.ToAngle() + (direction * 0.005f);
                    vel = MathUtil.FromPolar(aimAngle, 15f * .5f);
                    EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position, vel, Bullet.BULLETTYPE.ENEMY, 1));
                }
            }
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
