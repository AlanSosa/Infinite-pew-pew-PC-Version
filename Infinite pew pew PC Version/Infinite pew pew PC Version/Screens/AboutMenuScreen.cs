using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameStateManagement.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class AboutMenuScreen : MenuScreen
    {

        ContentManager content;
        private Texture2D credits;
        private Vector2 Position;
        private float speed = 1.5f;
        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutMenuScreen()
            : base("About", 1)
        {
            Position = new Vector2(400 , 50);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            credits = content.Load<Texture2D>("Sprites/CreditsCanvas");

            base.LoadContent();
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            Position.Y -= speed;

            if (Position.Y <= -1220)
                this.Position.Y = 500;
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(this.credits, this.Position, Color.White * TransitionAlpha);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
