using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SpinAnd360Shooter : Enemy
    {
        private int bulletsToShoot;


        public SpinAnd360Shooter(Vector2 position, int bulletsToShoot)
        {
            this.texture = EntityManager.Resources.Shooter360;
            this.Position = position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 200;
            this.HP = 12;
            this.AddBehaviour(this.BounceAndSpin(1.2f));
            this.bulletsToShoot = bulletsToShoot;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Kill()
        {
            float direction = 0;
            float increment = 360 / this.bulletsToShoot;

            if (EntityManager.CanSpawnEnemyBullets)
            {
                for (int i = 0; i < bulletsToShoot; i++)
                {
                    direction += MathHelper.ToRadians(increment);
                    //float aimAngle = MathHelper.WrapAngle(direction);
                    Vector2 vel = MathUtil.FromPolar(direction, 15f);
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
