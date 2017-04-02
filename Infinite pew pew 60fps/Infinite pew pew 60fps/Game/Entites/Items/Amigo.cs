using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using System.Diagnostics;
using ThridWolrdShooterGame.Entites.Messages;
using ThridWolrdShooterGame.Managers;
using Geometry_Wars_Rip_Off;

namespace ThridWolrdShooterGame.Entites.Items
{
    public class Amigo:Entity
    {
        private float cooldownFrames;
        private float cooldownRemaining = 0;
        private Random random;

        private int timer;
        private int timeAlive;

        private bool messageCreated;

        public Vector2 Target
        {
            get;
            set;
        }

        public Amigo(Vector2 Position)
        {
            this.Position = Position;
            this.texture = EntityManager.Resources.Amigo;
            cooldownFrames = 3 * 2;
            timer = timeAlive = (int)PlayerStatus.AmigoTime;
            messageCreated = false;
        }

        public virtual void Kill()
        {
            this.IsExpired = true;
        }

        public override void Update(GameTime gametime)
        {
            this.Velocity += ((Target - this.Position) * 0.04f) *  GameplayScreen.SlowMoMultiplier;
            Vector2 aim;
            if (Input.GetAimDirection() != Vector2.Zero)
                this.Orientation = Input.GetAimDirection().ToAngle();
            aim = Input.GetAimDirection();

            if(!GameplayScreen.GameTime.IsRunningSlowly)
                if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
                {
                
                    random = new Random();
                    cooldownRemaining = cooldownFrames / GameplayScreen.SlowMoMultiplier;
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = random.NextFloat(-0.04f, 0.04f) + random.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 50f * .5f);

                    Vector2 offset = Vector2.Transform(new Vector2(25, -1), aimQuat);
                    EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER,1));

                }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            this.Position += Velocity;

            Velocity *= 0.8f;

            this.Position = Vector2.Clamp(Position, Size / 2, GameplayScreen.WorldSize - Size / 2);

            if (Vector2.Distance(this.Position, EntityManager.PlayerShip.Position) > 75)
                if(!GameplayScreen.GameTime.IsRunningSlowly)
                    makeExhaustFire();

            timer -= gametime.ElapsedGameTime.Milliseconds;

            if (timer <= 3000 && timer >= 2000)
            {
                if (!messageCreated)
                {
                    messageCreated = true;
                    EntityManager.Add(new FadingOutMessage(this.Position, "3"));
                }
            }
            else if (timer <= 2000 && timer >= 1000)
            {
                if (messageCreated)
                {
                    messageCreated = false;
                    EntityManager.Add(new FadingOutMessage(this.Position, "2"));
                }
            }
            else if( timer <= 1000)
            {
                if (!messageCreated)
                {
                    messageCreated = true;
                    EntityManager.Add(new FadingOutMessage(this.Position, "1"));
                }
            }


            if (timer <= 0)
            {
                this.IsExpired = true;
                EntityManager.Add(new FadingOutMessage(this.Position, "ADIOS"));
            }
            else if (EntityManager.PlayerShip.IsDead)
                this.IsExpired = true;


        }

        private void makeExhaustFire()
        {
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
                Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.6f * (float)Math.Sin(t * 10));
                Color sideColor = new Color(200, 38, 9);	// deep red
                Color midColor = new Color(255, 187, 30);	// orange-yellow
                Vector2 pos = Position + Vector2.Transform(new Vector2(-25, 0), rot);	// position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel + rand.NextVector2(0, 1);

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, midColor * alpha, 40f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.ENEMY));

                // side particle streams
                Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, sideColor * alpha, 40f, new Vector2(0.5f, 1),
                    new ParticleState(vel1, ParticleType.ENEMY));
                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, sideColor * alpha, 40f, new Vector2(0.5f, 1),
                    new ParticleState(vel2, ParticleType.ENEMY));
            }
        
        }

    }
}
