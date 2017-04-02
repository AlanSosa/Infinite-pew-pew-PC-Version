using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace GameStateManagement.Screens
{
    class SplashScreen:GameScreen
    {
        ContentManager content;
        Texture2D SplashScreenTexture;
        SoundEffect sfx;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SplashScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            timerSplashScreen = 0;
            playedSound = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            SplashScreenTexture = content.Load<Texture2D>("Sprites/SplashScreen2");
            sfx = content.Load<SoundEffect>("Sounds/Sfx/SpawnPlayer");
        }

        private int timerSplashScreen;
        private bool playedSound;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            timerSplashScreen += gameTime.ElapsedGameTime.Milliseconds;

            if(timerSplashScreen > 4000){
                LoadingScreen.Load(ScreenManager, false, null, new GamePadSplashScreen());
                }
            if (timerSplashScreen > 1000 && !playedSound)
            {
                sfx.Play();
                playedSound = true;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White * TransitionAlpha, 0, 0);

            spriteBatch.Begin();
            spriteBatch.Draw(this.SplashScreenTexture, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (this.SplashScreenTexture.Width + (ScreenManager.GraphicsDevice.Viewport.Width - this.SplashScreenTexture.Width) / 2), 
                 ScreenManager.GraphicsDevice.Viewport.Height - (this.SplashScreenTexture.Height + (ScreenManager.GraphicsDevice.Viewport.Height - this.SplashScreenTexture.Height) / 2)), 
                 Color.White * this.TransitionAlpha);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }
    }
}
