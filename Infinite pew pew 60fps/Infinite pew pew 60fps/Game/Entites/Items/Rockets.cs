using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites
{
    public class Rockets:Bullet
    {
        private Vector2 acceleration;

        public Rockets(Texture2D texture, Vector2 position,Vector2 Acceleration,int bulletHp):base
            (texture, position, Acceleration, bulletHp)
        {
            this.acceleration = Acceleration;
        }

        public Rockets(Texture2D texture, Vector2 position,Vector2 Acceleration, BULLETTYPE type, int bulletHp):
            base(texture, position, Acceleration, type, bulletHp)
        {
            this.acceleration = Acceleration;
        }


        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            this.Velocity += (this.acceleration * GameplayScreen.SlowMoMultiplier) * .5f;

            if(!GameplayScreen.GameTime.IsRunningSlowly)
                makeBulletTrace();
        }

        private void makeBulletTrace()
        {/*
            if (Velocity.LengthSquared() > 0.1f)
            {
                Vector2 baseVel = Velocity.ScaleTo(-3);
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, Orientation);

                Vector2 pos = Position + Vector2.Transform(new Vector2(-15, 0f), rot);
                const float alpha = 0.7f;

                Main.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle,
                    pos,
                    Color.White * alpha,
                    30f,
                    new Vector2(0.5f, 1),
                    new ParticleState(baseVel, ParticleType.BULLET)
                    );
            }*/
            Random rand = new Random();

            if (Velocity.LengthSquared() > 0.1f)
            {
                // set up some variables
                // Orientation = Velocity.ToAngle();
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, Orientation);

                double t = GameplayScreen.GameTime.TotalGameTime.TotalSeconds;
                // The primary velocity of the particles is 3 pixels/frame in the direction opposite to which the ship is travelling.
                Vector2 baseVel = Velocity.ScaleTo(-3);
                // Calculate the sideways velocity for the two side streams. The direction is perpendicular to the ship's velocity and the
                // magnitude varies sinusoidally.
                Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.8f * (float)Math.Sin(t * 10));
                Color sideColor = new Color(255,0, 70);	// deep red
                Color midColor = new Color(255, 200, 50);	// orange-yellow
                Vector2 pos = Position + Vector2.Transform(new Vector2(-25, 0), rot);	// position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel + rand.NextVector2(0, 1);

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, midColor * alpha, 20f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.ENEMY));

                // side particle streams
                Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, sideColor * alpha, 10f, new Vector2(0.5f, 1),
                    new ParticleState(vel1, ParticleType.ENEMY));
                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, sideColor * alpha, 10f, new Vector2(0.5f, 1),
                    new ParticleState(vel2, ParticleType.ENEMY));
            }
        }

    }
}
