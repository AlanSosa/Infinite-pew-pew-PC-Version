using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Entites.Items;
using System.Diagnostics;
using ThridWolrdShooterGame.Entites.Enemies;
using ThridWolrdShooterGame.Managers;
using ThridWolrdShooterGame.Entites.Messages;
using Geometry_Wars_Rip_Off;
using GameStateManagement.ScreensUtils;
using GameStateManagement.Managers;

namespace ThridWolrdShooterGame.Entites
{
    public class PlayerShip : Entity
    {
        private float cooldownFrames;
        private float cooldownRemaining = 0;
        private Random random = new Random();
        int framesUntilRespawn = 0;
        private const float speed = 13;
        public bool IsDead { get { return framesUntilRespawn > 0; } }
        private int timeUntilActive = 90;
        public bool IsActive { get { return timeUntilActive <= 0; } }
        private Random rand = new Random();
        private ItemSpawner.ITEMS equippedItem;

        public ItemSpawner.ITEMS EquippedItem
        {
            get
            {
                return this.equippedItem;
            }
        }

        private List<Type> types;
        
        public int AmigoCount
        {
            get
            {
                return Amigos.Count(amigo => amigo != null);
            }

        }

        public Vector2 VelocityAim
        {
            get
            {
                return Velocity * speed;
            }
        }
        public Amigo[] Amigos
        {
            get;
            private set;
        }

        public PlayerShip()
        {
            this.texture = EntityManager.Resources.Playership;
            this.Radius = 3;
            this.Position = GameplayScreen.WorldSize / 2;
            this.Color = Color.White;

            cooldownFrames = 3;

            types = new List<Type>(new Type[]{ 
                typeof(Rockets),
                typeof(Bullet),
                typeof(Wanderer),
                typeof(Seeker),
                typeof(SeekerM1),
                typeof(SeekerM2),
                typeof(Bouncer),
                typeof(SeekerFasterFar),
                typeof(SpinAnd180Shooter),
                typeof(SpinAnd360Shooter),
                typeof(SpiralShooter),
                typeof(SquareMini),
                typeof(MobileSpawner),
                typeof(EnemyBarricade)
            });

            Amigos = new Amigo[2];
        }


        int soundStupidCount = 0;
        public override void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            if (PlayerStatus.IsGameOver)
            {
                return;
            }

            if (IsDead)
            {
                framesUntilRespawn--;
                if (framesUntilRespawn == 1)
                {
                    GameplayScreen.CanPause = true;
                    SoundManager.PlaySpawnPlayer();
                    timeUntilActive = 100;
                    Position = GameplayScreen.WorldSize / 2;
                    PlayerStatus.ResetPlayer();
                    this.Velocity = Vector2.Zero;
                }
                return;
            }
            timeUntilActive--;

            
            const float friction = 0.05f;

            if ( Input.GetMovementDirection() != Vector2.Zero)
                Velocity = Input.GetMovementDirection() * speed;
#if WINDOWS_PHONE
            if (VirtualThumbsticks.LeftThumbstick != Vector2.Zero)
                Velocity = VirtualThumbsticks.LeftThumbstick * speed;
#endif
            else
            {
                Vector2 i = Velocity;
                Velocity = i -= friction * i;
            }

            Velocity *= GameplayScreen.SlowMoMultiplier;

            this.Position += Velocity;

            
#if WINDOWS_PHONE
            if(VirtualThumbsticks.RightThumbstick != Vector2.Zero)
                this.Orientation = VirtualThumbsticks.RightThumbstick.ToAngle();
#else
            if (Input.GetAimDirection() != Vector2.Zero)
                this.Orientation = Input.GetAimDirection().ToAngle();
#endif

            if (!GameplayScreen.GameTime.IsRunningSlowly)
                if (Velocity.Length() >= speed-1)
                    makeExhaustFire();

            if (Amigos[0] != null)
            {
                Quaternion offsetQuat = Quaternion.CreateFromYawPitchRoll(10, .5f, Orientation);
                Vector2 offset = Vector2.Transform(new Vector2(10, 50), offsetQuat);
                Amigos[0].Target = this.Position + offset;

                if (Amigos[0].IsExpired)
                {
                    Amigos[0] = null;
                    ItemSpawner.AmigosCount--;
                }
            }

            if (Amigos[1] != null)
            {
                Quaternion offsetQuat = Quaternion.CreateFromYawPitchRoll(10, .5f, Orientation);
                Vector2 offset = Vector2.Transform(new Vector2(10, -50), offsetQuat);
                Amigos[1].Target = this.Position + offset;

                if (Amigos[1].IsExpired)
                {
                    Amigos[1] = null;
                    ItemSpawner.AmigosCount--;
                }
            }
            Vector2 aim = Vector2.Zero;

            if (equippedItem == ItemSpawner.ITEMS.WOOHOO)
            {

                aim.X = 0.1f * (float)Math.Sin((10 * GameplayScreen.SlowMoMultiplier) * GameplayScreen.GameTime.TotalGameTime.TotalSeconds);
                aim.Y = 0.1f * (float)Math.Cos((10 * GameplayScreen.SlowMoMultiplier) * GameplayScreen.GameTime.TotalGameTime.TotalSeconds);
                GameplayScreen.Camera.Shake(8f, .2f);
            }
            else
            {
                aim = Input.GetAimDirection();
#if WINDOWS_PHONE
                aim = VirtualThumbsticks.RightThumbstick;
#endif
            }

            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                if (equippedItem != ItemSpawner.ITEMS.NONE)
                {
                    switch(equippedItem)
                    {
                        case ItemSpawner.ITEMS.PEW:
                            cooldownFrames = 2;
                            break;
                        case ItemSpawner.ITEMS.BOINGY_BOINGY:
                            cooldownFrames = 2;
                            break;
                        case ItemSpawner.ITEMS.PEW_PEW:
                            cooldownFrames = 1;
                            break;
                        case ItemSpawner.ITEMS.WOAH:
                            cooldownFrames = 6;
                            break;
                        case ItemSpawner.ITEMS.WOOHOO:
                            cooldownFrames = 1;
                            break;

                    }

                    if (!HudManager.CounterMeter.IsActive)
                    {
                        cooldownFrames = 3;
                        equippedItem = ItemSpawner.ITEMS.NONE;
                    }
                }
                random = new Random();
                cooldownRemaining = cooldownFrames / GameplayScreen.SlowMoMultiplier;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = random.NextFloat(-0.04f, 0.04f) + random.NextFloat(-0.04f, 0.04f);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 50f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -1), aimQuat);

                if (equippedItem != ItemSpawner.ITEMS.WOAH )
                {
                    if (equippedItem == ItemSpawner.ITEMS.PEW)
                    {
                        double t = GameplayScreen.GameTime.TotalGameTime.TotalSeconds;

                        Vector2 baseVel = aim.ScaleTo(5);

                        Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (1.5f * (float)Math.Sin(t * 10));
                        Color midColor = new Color(255, 187, 30);
                        Vector2 velMid = baseVel + rand.NextVector2(0, 1);

                        const float alpha = 0.7f;

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, midColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(velMid, ParticleType.ENEMY));

                        // side particle streams
                        Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                        Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, midColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(vel1, ParticleType.ENEMY));

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, midColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(vel2, ParticleType.ENEMY));
                    }
                    else if (equippedItem == ItemSpawner.ITEMS.PEW_PEW)
                    {
                        double t = GameplayScreen.GameTime.TotalGameTime.TotalSeconds;

                        Vector2 baseVel = aim.ScaleTo(7);

                        Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (1.5f * (float)Math.Sin(t * 10));
                        Color sideColor = new Color(200, 38, 9);
                        Color midColor = new Color(255, 187, 30);
                        Vector2 velMid = baseVel + rand.NextVector2(0, 1);

                        const float alpha = 0.7f;

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, sideColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(velMid, ParticleType.ENEMY));

                        // side particle streams
                        Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                        Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, midColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(vel1, ParticleType.ENEMY));

                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, this.Position + offset, midColor * alpha, 15f, new Vector2(0.5f, 1),
                        new ParticleState(vel2, ParticleType.ENEMY));
                    }

                    if (equippedItem != ItemSpawner.ITEMS.PEW && equippedItem != ItemSpawner.ITEMS.BOINGY_BOINGY)
                    {
                        

                        if (random.Next(6) % 2 == 0)
                        {
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1));
                        }
                        else
                        {
                            aimAngle = aim.ToAngle() - 0.1f;
                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1));

                            aimAngle = aim.ToAngle() + 0.1f;
                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1));
                        }
                    }
                    else
                    {
                        if (equippedItem == ItemSpawner.ITEMS.PEW)
                        {
                           
                            offset = Vector2.Transform(new Vector2(25, -10), aimQuat);

                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1));


                            offset = Vector2.Transform(new Vector2(25, 10), aimQuat);

                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1));
                        }
                        else if (equippedItem == ItemSpawner.ITEMS.BOINGY_BOINGY)
                        {
                           
                            offset = Vector2.Transform(new Vector2(25, -1), aimQuat);

                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1,true));


                            offset = Vector2.Transform(new Vector2(55, -1), aimQuat);

                            vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                            EntityManager.Add(new Bullet(EntityManager.Resources.Bullet, Position + offset, vel, Bullet.BULLETTYPE.PLAYER, 1,true));
                        }

                    }

                    if (cooldownFrames == 1)
                    {
                        stupidSoundAux++;

                        if(stupidSoundAux % 2 == 0)
                            SoundManager.PlayNormalShot();
                    }
                    else
                    {
                        stupidSoundAux = 0;
                        SoundManager.PlayNormalShot();
                    }
                }
                else
                {
                    SoundManager.PlayRocketShot();
                    EntityManager.Add(new Rockets(EntityManager.Resources.Bullet, Position + offset, MathUtil.FromPolar(aimAngle + randomSpread, 4f), Bullet.BULLETTYPE.PLAYER, 3));
                    aimAngle = aim.ToAngle() - 0.1f;
                    vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                    EntityManager.Add(new Rockets(EntityManager.Resources.Bullet, Position + offset, MathUtil.FromPolar(aimAngle + randomSpread, 4f), Bullet.BULLETTYPE.PLAYER, 3));
                    aimAngle = aim.ToAngle() + 0.1f;
                    vel = MathUtil.FromPolar(aimAngle + randomSpread, 40f);
                    EntityManager.Add(new Rockets(EntityManager.Resources.Bullet, Position + offset,MathUtil.FromPolar(aimAngle + randomSpread, 4f), Bullet.BULLETTYPE.PLAYER,3));

                    GameplayScreen.Camera.Shake(8f, .2f);
                }
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;
            Position = Vector2.Clamp(Position, Size / 2, GameplayScreen.WorldSize - this.Size / 2);

            

            if (lastSlowMoState != GameplayScreen.IsSlowMotion)
            {
                stupidAuxForSlowMoMessage++;

                if (stupidAuxForSlowMoMessage == 1)
                {
                    
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(0, 60), "Chingon", 120));
                    EntityManager.Add(new FadingOutImageMessage(this.Position + new Vector2(110, 60), EntityManager.Resources.SlowTimeMessageIco, 120));
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(170, 60), "Time", 120 ));
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(80, 90), "On", 120));
                    
                }
                else if (stupidAuxForSlowMoMessage == 2)
                {
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(0, 60), "Chingon", 120));
                    EntityManager.Add(new FadingOutImageMessage(this.Position + new Vector2(110, 60), EntityManager.Resources.SlowTimeMessageIco, 120));
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(170, 60), "Time", 120));
                    EntityManager.Add(new FadingOutMessage(this.Position + new Vector2(80, 90), "Off", 120));

                    stupidAuxForSlowMoMessage = 0;
                }
                
            }

            lastSlowMoState = GameplayScreen.IsSlowMotion;
        }

        private int stupidAuxForSlowMoMessage = 0;
        private bool lastSlowMoState;
        private int stupidSoundAux = 0;
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                if (timeUntilActive > 0)
                {
                    float factor = timeUntilActive / 60f;
                    spriteBatch.Draw(texture, Position, null, Color.White * factor, Orientation, Size / 2f, 3 - factor, 0, 0);
                }

                base.Draw(spriteBatch);
            }
        }


        public void WasDamaged()
        {
            EntityManager.Add(new FadingOutMessage(this.Position, "OUCH!", 60));
            SoundManager.PlayPlayerDead();
            if (EnemySpawner.BonusMode)
            {
                EntityManager.RemoveEnemies();
                EnemySpawner.ShieldAux--;
                return;
            }

            PlayerStatus.RemoveShield();
            timeUntilActive = 30;

            if (PlayerStatus.Shield <= 0 )
            {
                this.Kill();
            }
        }

        public void Kill()
        {

            GameplayScreen.CanPause = false;
            framesUntilRespawn = 120;
            timeUntilActive = 100;
            Color yellow = new Color(0.8f, 0.8f, 0.4f);

            for (int i = 0; i < 5000; i++)
            {
                float speed = 25f * (1f - 1 / rand.NextFloat(1f, 10f));
                Color color = Color.Lerp(Color.White, yellow, rand.NextFloat(0, 1));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.NONE,
                    LengthMultiplier = 1
                };

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.Particle, Position, color, 100, 1.5f, state);
            }

            GameplayScreen.FreezeTime = 30;

            PlayerStatus.RemoveLife();
            //Main.Camera.Shake(30f, 3f);
        }


        internal void HandleCollision(Items.Item item)
        {
            SoundManager.PlayPickItem();
            if (item.type == Items.ItemSpawner.ITEMS.SLOW_MO_EXTEND)
            {
                PlayerStatus.AddSlowMoBonus();
            }
            else if (item.type == Items.ItemSpawner.ITEMS.SHIELD_EXTEND)
            {
                PlayerStatus.AddShield();

                if (EnemySpawner.BonusMode)
                    EnemySpawner.ShieldAux++;
            }
            else if (item.type == Items.ItemSpawner.ITEMS.AMIGO_EXTEND)
            {
                PlayerStatus.AddAmigoBonus();
            }
            else if (item.type == Items.ItemSpawner.ITEMS.WEAPON_EXTEND)
            {
                PlayerStatus.AddAmmoBonus();
            }
            else if (item.type == Items.ItemSpawner.ITEMS.PEW)
            {
                equippedItem = item.type;
                HudManager.CounterMeter.setWeapontime(PlayerStatus.PewTime);
            }
            else if (item.type == Items.ItemSpawner.ITEMS.PEW_PEW)
            {
                equippedItem = item.type;
                HudManager.CounterMeter.setWeapontime(PlayerStatus.PewPewTime);
            }
            else if (item.type == Items.ItemSpawner.ITEMS.WOAH)
            {
                equippedItem = item.type;
                HudManager.CounterMeter.setWeapontime(PlayerStatus.RocketsTime);

            }
            else if (item.type == ItemSpawner.ITEMS.WOOHOO)
            {
                equippedItem = item.type;
                HudManager.CounterMeter.setWeapontime(PlayerStatus.InvincibilityTime);
            }
            else if (item.type == ItemSpawner.ITEMS.BOINGY_BOINGY) 
            {
                equippedItem = item.type;
                HudManager.CounterMeter.setWeapontime(PlayerStatus.BoingyTime);
            }
            else if (item.type == ItemSpawner.ITEMS.AMIGO)
            {
                Amigo amigo = new Amigo(Vector2.Zero);

                if (Amigos[0] == null)
                {
                    Amigos[0] = amigo;
                }
                else
                {
                    Amigos[1] = amigo;
                }
                EntityManager.Add(amigo);
            }
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

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, sideColor * alpha, 15f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.ENEMY));
                ;

                // side particle streams
                Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, midColor * alpha, 10f, new Vector2(0.5f, 1),
                    new ParticleState(vel1, ParticleType.ENEMY));

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, pos, midColor * alpha, 10f, new Vector2(0.5f, 1),
                    new ParticleState(vel2, ParticleType.ENEMY));

            }

        }
    }
}
