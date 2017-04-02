using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MyExtensions;
using System.Diagnostics;
using ThridWolrdShooterGame.Entites.Messages;
using ThridWolrdShooterGame.Managers;
using GameStateManagement.Managers;


namespace ThridWolrdShooterGame.Entites.Enemies
{
    public abstract class Enemy : Entity
    {
        protected Color newColor;
        protected float timeUntilStart = 30 ;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int PointValue { get; protected set; }
        public int HP { get; protected set; }
        protected Random rand = new Random();
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        public int EnemyID = 0;

        public override void Update(GameTime gameTime)
        {
            //elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

            if (timeUntilStart <= 0)
            {
                ApplyBehaviours();
            }
            else
            {
                timeUntilStart -= GameplayScreen.SlowMoMultiplier;
                this.Color = Color.White * (1 - timeUntilStart / 60f);
            }

            this.Velocity *= GameplayScreen.SlowMoMultiplier;
            this.Position += this.Velocity;
            //Debug.WriteLine("ID :"+ EnemyID + ", Position Before : " + this.Position +  ", Velocity Before : "+ this.Velocity);
            this.Position = Vector2.Clamp(Position, Size / 2, GameplayScreen.WorldSize - Size / 2);
            //Debug.WriteLine("ID :" + EnemyID + ", Position After : " + this.Position + ", Velocity After : " + this.Velocity);
            Velocity *= 0.8f;
        }

        protected void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture == null)
                return;
            if (timeUntilStart > 0)
            {
                // Draw an expanding, fading-out version of the sprite as part of the spawn-in effect.
                float factor = timeUntilStart / 60f;	// decreases from 1 to 0 as the enemy spawns in
                spriteBatch.Draw(texture, Position, null, Color.White * factor, Orientation, Size / 2f, 2f - factor, 0, 0);
            }

            base.Draw(spriteBatch);
        }

        public virtual void Kill()
        {
            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            //EntityManager.Resources.Explosion.Play();

            //EntityManager.ChangeColor(color1);
            //
            //Sound.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
            rand = new Random();
            this.IsExpired = true;
            //ColorManager.AddFactorValue();
            EntityManager.Add(new FadingOutMessage(this.Position, this.PointValue + "", 30 * 2));
            PlayerStatus.enemiesKilled++;
            PlayerStatus.AddPoints(this.PointValue);
            PlayerStatus.AddSlowMoPoints();
            if(!GameplayScreen.GameTime.IsRunningSlowly)
                for (int i = 0; i < 40; i++)
                {
                    float speed = 12f * (1f - 1 / rand.NextFloat(1f, 10f));

                    var state = new ParticleState()
                    {
                        Velocity = rand.NextVector2(speed, speed),
                        Type = ParticleType.ENEMY,
                        LengthMultiplier = 1f
                    };

                    Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                    GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.Particle, this.Position, color, 190f, 1.5f, state);
                }   
  
        }

        public virtual void WasShot()
        {
            SoundManager.PlayExplosion();
            GameplayScreen.Camera.Shake(6.5f, .2f);
            HP--;
        }

        #region BEHAVIOURS
        //Seeker & SeekerM1 Behaviour
        protected  IEnumerable<int> FollowPlayer(float acceleration)
        {
            while (true)
            {
                if ((EntityManager.PlayerShip.Position - this.Position).Length() > 1f)
                    this.Velocity += (EntityManager.PlayerShip.Position - this.Position).ScaleTo(acceleration);

                //Debug.WriteLine("We've got a problem houston!!  : " + (EntityManager.PlayerShip.Position - this.Position));
                //this.Velocity += Vector2.Normalize(EntityManager.PlayerShip.Position - this.Position) * acceleration;
                if (this.Velocity != Vector2.Zero)
                    this.Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        //SeekerFasterFar Behaviour
        protected IEnumerable<int> FollowFasterWhenFar()
        {
            while (true)
            {
                if ((EntityManager.PlayerShip.Position - this.Position).Length() > 1f)
                    this.Velocity += (EntityManager.PlayerShip.Position - this.Position) * (0.015f * .5f);
                //this.Velocity += Vector2.Normalize(EntityManager.PlayerShip.Position - this.Position) * acceleration;
                if (this.Velocity != Vector2.Zero)
                    this.Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }


        //Square Mini Behaviour
        protected IEnumerable<int> FollowPlayerWithNoOrientation(float acceleration)
        {
            while (true)
            {
                this.Velocity += (EntityManager.PlayerShip.Position - this.Position).ScaleTo(acceleration);
                //this.Velocity += Vector2.Normalize(EntityManager.PlayerShip.Position - this.Position) * acceleration;
                yield return 0;
            }
        }


        //Enemy Barricade Behaviour
        protected IEnumerable<int> MoveInCircles(float angleIncrementation, float speed)
        {
            float angle = 0;
            float angleIncrement = angleIncrementation;

            while (true)
            {
                this.Velocity.X = (float)Math.Cos(MathHelper.ToRadians(angle)) * speed;
                this.Velocity.Y = (float)Math.Sin(MathHelper.ToRadians(angle)) * speed;
                angle += angleIncrement;

                if (Velocity.Length() >= 1)
                    Orientation = Velocity.ToAngle();
                yield return 0;
            }
        }

        /*private int elapsedTime;
        protected IEnumerable<int> MoveInCirclesVariant(float angleIncrement, float speed, float timeSpanForAngle)
        {
            float angle = 0;
            while (true)
            {
                this.Velocity.X = (float)Math.Cos(MathHelper.ToRadians(angle)) * speed;
                this.Velocity.Y = (float)Math.Sin(MathHelper.ToRadians(angle)) * speed;

                
                if (Velocity.Length() >= 1)
                    Orientation = Velocity.ToAngle();

                angle += angleIncrement;
                
                elapsedTime += GameplayScreen.GameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime >= timeSpanForAngle)
                {
                    Velocity *= .1f;
                    elapsedTime = 0;
                }

                yield return 0;
            }
        }*/

        //Wanderer & Mobile Spawner Behaviour
        protected IEnumerable<int> MoveRandomly()
        {
            Random rand = new Random();
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);

                    var bounds = new Rectangle(0, 0, (int)GameplayScreen.WorldSize.X, (int)GameplayScreen.WorldSize.Y);
                    bounds.Inflate(-texture.Width, -texture.Height);

                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (new Vector2(GameplayScreen.graphicsWidth, GameplayScreen.graphicsHeight) / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);


                    Orientation -= 0.05f * GameplayScreen.SlowMoMultiplier;

                    yield return 0;
                }
            }
        }

        //Worm Head Behaviour
        protected IEnumerable<int> MoveRandomlyVariant()
        {
            Random rand = new Random();
            float direction = rand.NextFloat(0, MathHelper.TwoPi);
            direction += rand.NextFloat(-0.1f, 0.1f);
            direction = MathHelper.WrapAngle(direction);

            while (true)
            {
                for (int j = 0; j < 2; j++)
                {

                    direction += rand.NextFloat(-1f, 1f);
                    direction = MathHelper.WrapAngle(direction);

                    for (int i = 0; i < 6; i++)
                    {
                        Velocity += MathUtil.FromPolar(direction, 0.4f);

                        var bounds = new Rectangle(0, 0, (int)GameplayScreen.WorldSize.X, (int)GameplayScreen.WorldSize.Y);
                        bounds.Inflate(-texture.Width, -texture.Height);

                        if (!bounds.Contains(Position.ToPoint()))
                            direction = (new Vector2(GameplayScreen.graphicsWidth, GameplayScreen.graphicsHeight) / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);


                        if (Velocity.Length() >= 0)
                            Orientation = Velocity.ToAngle();

                        yield return 0;
                    }
                }
            }
        }

        // SeekerM2 Behaviour
        protected IEnumerable<int> FollowAndSpin(float acceleration, float spinAcceleration)
        {

            while (true)
            {
                if ((EntityManager.PlayerShip.Position - this.Position).Length() > 1f)
                    this.Velocity += (EntityManager.PlayerShip.Position - this.Position).ScaleTo(acceleration);
                //this.Velocity += Vector2.Normalize(EntityManager.PlayerShip.Position - this.Position) * acceleration;
                this.Orientation -= (spinAcceleration * .5f) * GameplayScreen.SlowMoMultiplier;

                yield return 0;
            }
        }

        //Bouncer, 360 Shooter & Spiral Shooter Behaviour
        protected IEnumerable<int> BounceAndSpin(float acceleration)
        {
            Random rand = new Random();
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                //direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, acceleration);

                    var bounds = new Rectangle(0, 0, (int)GameplayScreen.WorldSize.X, (int)GameplayScreen.WorldSize.Y);
                    bounds.Inflate(-texture.Width, -texture.Height);

                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (new Vector2(GameplayScreen.graphicsWidth, GameplayScreen.graphicsHeight) / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

                    Orientation -= 0.15f * GameplayScreen.SlowMoMultiplier;
                    yield return 0;
                }
            }
        }
        #endregion

        public void HandleCollision(Enemy other)
        {
            if (other is WormHead || other is WormTail)
                return;

            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

    }
}
