using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SpiralShooter : Enemy
    {
        private int bulletsToShoot;
        private int elapsedTime;
        private bool dying = false;
        private float direction;
        private float increment;
        private int timeSpanBetweenBullets;

        public SpiralShooter(Vector2 position, int bulletsToShoot, int laps ,int timeSpanBetweenBullets)
        {
            this.texture = EntityManager.Resources.CoreFollowerSmall;
            this.Position = position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 500;
            this.HP = 12;
            this.AddBehaviour(this.BounceAndSpin(1.2f * .5f));
            this.bulletsToShoot = bulletsToShoot *laps;
            increment = 360 / bulletsToShoot;
            direction = 0;
            this.timeSpanBetweenBullets = timeSpanBetweenBullets;
        }

        

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime <= timeSpanBetweenBullets)
                return;

            elapsedTime = 0;
            if (EntityManager.CanSpawnEnemyBullets)
            {
                if (dying)
                {
                    Velocity = Vector2.Zero;
                    Vector2 vel = MathUtil.FromPolar(direction, 15f * .5f);
                    float aimAngle = vel.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                    Vector2 offSet = Vector2.Transform(new Vector2(40, 40), aimQuat);

                    EntityManager.Add(new Bullet(EntityManager.Resources.EnemyBullet, Position + offSet, vel + Velocity, Bullet.BULLETTYPE.ENEMY, 1));
                    direction += MathHelper.ToRadians(increment);
                    //float aimAngle = MathHelper.WrapAngle(direction);
                    bulletsToShoot--;
                }
            }
            else
                bulletsToShoot = 0;

            if (bulletsToShoot == 0)
            {
                base.Kill();
            }
        }

        public override void Kill()
        {
            HP = 12;
            if (dying)
            {
                base.Kill();
            }
            dying = true;


        }

        public override void WasShot()
        {
            base.WasShot();
            if (HP <= 0)
                Kill();
        }
    }
}
