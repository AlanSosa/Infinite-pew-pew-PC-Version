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
    class GamePadSplashScreen:GameScreen
    {
        ContentManager content;
        Texture2D GamePad;
        SoundEffect sfx;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GamePadSplashScreen()
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

            GamePad = content.Load<Texture2D>("Sprites/Game Controller");
        }

        private int timerSplashScreen;
        private bool playedSound;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            timerSplashScreen += gameTime.ElapsedGameTime.Milliseconds;

            if(timerSplashScreen > 4000){
                LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
                }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White * TransitionAlpha, 0, 0);

            SpriteFont font = ScreenManager.Font;
            string title = "Infinite pew pew";
            string message = "Works best with a gamepad";

            Vector2 titleSize = font.MeasureString(title);
            Vector2 messageSize = font.MeasureString(message);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, title, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (titleSize.X + (ScreenManager.GraphicsDevice.Viewport.Width - titleSize.X) / 2),
                 ScreenManager.GraphicsDevice.Viewport.Height - (this.GamePad.Height + (ScreenManager.GraphicsDevice.Viewport.Height - this.GamePad.Height) / 2)),
                 Color.Black * this.TransitionAlpha);

            spriteBatch.Draw(this.GamePad, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (this.GamePad.Width + (ScreenManager.GraphicsDevice.Viewport.Width - this.GamePad.Width) / 2), 
                 ScreenManager.GraphicsDevice.Viewport.Height - (this.GamePad.Height + (ScreenManager.GraphicsDevice.Viewport.Height - this.GamePad.Height) / 2)), 
                 Color.White * this.TransitionAlpha);

            spriteBatch.DrawString(font, message, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (messageSize.X + (ScreenManager.GraphicsDevice.Viewport.Width - messageSize.X) / 2),
                (ScreenManager.GraphicsDevice.Viewport.Height - (this.GamePad.Height + (ScreenManager.GraphicsDevice.Viewport.Height - this.GamePad.Height) /2)) + this.GamePad.Height - (messageSize.Y) ),
                 Color.Black * this.TransitionAlpha);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }
    }
}
