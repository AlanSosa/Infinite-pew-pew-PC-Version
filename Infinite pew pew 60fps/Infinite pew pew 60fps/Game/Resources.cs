using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ThridWolrdShooterGame
{
    public class Resources:IDisposable
    {
        public Texture2D Playership;
        public Texture2D Bullet;
        public Texture2D Particle;
        public Texture2D Pixel;
        public Texture2D Core;
        public Texture2D Seeker;
        public Texture2D CoreFollowerBig;
        public Texture2D Wanderer;
        public Texture2D CoreFollowerSmall;
        public Texture2D EnemyBullet;
        public Texture2D BulletNew;
        public Texture2D Bouncer;
        public Texture2D SquareMini;
        public Texture2D EnemyItem;
        public Texture2D SquareParticle;
        public SpriteFont DemoFont;
        public SpriteFont AnimatedMessage;
        public Texture2D SeekerM1;
        public Texture2D SeekerM2;
        public Texture2D SeekerFar;
        public Texture2D Scanlines;
        public Texture2D Shooter360;
        public Texture2D Shooter180;
        public Texture2D blankTexture;
        public SpriteFont ItemFont;
        public Texture2D Amigo;

        public Texture2D Glow;

        public Texture2D WeaponItem;
        public Texture2D AmigoItem;
        public Texture2D ShieldItem;

        public Texture2D SlowTimeBonus;
        public Texture2D WeaponBonus;
        public Texture2D AmigoBonus;

        public Texture2D SlowTimeMessageIco; 
        public SpriteFont InstructionsBonusLevel;


        private static Random rand = new Random();

        private bool disposed = false;


        public Resources(ContentManager content, GraphicsDevice graphicsDevice) 
        {
            WeaponItem = content.Load<Texture2D>("Sprites/WeaponIco");
            ShieldItem = content.Load<Texture2D>("Sprites/ShieldIco");

            Glow = content.Load<Texture2D>("Sprites/Glow");

            WeaponBonus = content.Load<Texture2D>("Sprites/WeaponBonusIco");
            AmigoBonus = content.Load<Texture2D>("Sprites/AmigoBonusIco");
            SlowTimeBonus = content.Load<Texture2D>("Sprites/SlowTimeBonusIco");

            SlowTimeMessageIco = content.Load<Texture2D>("Sprites/SlowTimeMessageIco");

            Playership = content.Load<Texture2D>("Sprites/PlayerShip");
            AmigoItem = content.Load<Texture2D>("Sprites/AmigoIco");
            //Bullet = content.Load<Texture2D>("Sprites/Bullet");
            Particle = content.Load<Texture2D>("Sprites/Laser");
            Core = content.Load<Texture2D>("Sprites/Core");
            CoreFollowerBig = content.Load<Texture2D>("Sprites/CoreFollowerBig");
            CoreFollowerSmall = content.Load<Texture2D>("Sprites/CoreFollowerSmall");
            Seeker = content.Load<Texture2D>("Sprites/Seeker");
            Amigo = content.Load<Texture2D>("Sprites/Amigo");
            Wanderer = content.Load<Texture2D>("Sprites/Wanderer");
            EnemyBullet = content.Load<Texture2D>("Sprites/EnemyBullet");
            Bouncer = content.Load<Texture2D>("Sprites/Bouncer");
            SquareMini = content.Load<Texture2D>("Sprites/SquareArray");
            EnemyItem = content.Load<Texture2D>("Sprites/EnemyItem");
            DemoFont = content.Load<SpriteFont>("Fonts/Font");
            SeekerM1 = content.Load<Texture2D>("Sprites/SeekerM1");
            SeekerM2 = content.Load<Texture2D>("Sprites/SeekerM2");
            SeekerFar = content.Load<Texture2D>("Sprites/SeekerFar");
            Shooter360 = content.Load<Texture2D>("Sprites/threesixtyShooter");
            Shooter180 = content.Load<Texture2D>("Sprites/oneeightyShooter");
            AnimatedMessage = content.Load<SpriteFont>("Fonts/AnimatedMessage");
            ItemFont = content.Load<SpriteFont>("Fonts/ItemFont");

            InstructionsBonusLevel = content.Load<SpriteFont>("Fonts/InstructionsBonusLevel");

            blankTexture = content.Load<Texture2D>("Sprites/blank");

            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            SquareParticle = new Texture2D(graphicsDevice, 10, 10);
            Color[] data = new Color[10 * 10];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            SquareParticle.SetData(data);

            Particle = new Texture2D(graphicsDevice, 22, 4);
            Color[] dataParticle = new Color[22 * 4];
            for (int i = 0; i < dataParticle.Length; ++i) dataParticle[i] = Color.White;
            Particle.SetData(dataParticle); 

            Bullet = new Texture2D(graphicsDevice, 30, 5);
            Color[] dataBullet = new Color[30 * 5];
            for (int i = 0; i < dataBullet.Length; ++i) dataBullet[i] = Color.White;
            Bullet.SetData(dataBullet); 
        }

        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
