#region Using Statements
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Managers;
using MyExtensions;
using ThridWolrdShooterGame;
using System.Collections.Generic;
using MenuScreen;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        private Texture2D pixel;
        private Color backgroundColor;
        private Color decorationsColor;

        private List<MenuDecoration> decorations;
        private int decorationTimer;
        private int timeToDecorate;
        private int timeLimit = 500;

        private Random random = new Random();

        private float spawnOffset = 100;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            decorations = new List<MenuDecoration>();

            decorationTimer = 0;
            timeToDecorate = random.Next(0, timeLimit);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            pixel = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            float backgroundHue = random.NextFloat(0, 6);
            backgroundColor = ColorUtil.HSVToColor(backgroundHue, .5f, .6f);

            float decorationsHue = (backgroundHue + random.NextFloat(0, 2)) % 6f;
            decorationsColor = ColorUtil.HSVToColor(decorationsHue, .5f, 1f);

        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            decorations = new List<MenuDecoration>();
            decorations = null;
            content.Unload();
            pixel.Dispose();
        }


        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (IsExiting)
                return;

            decorationTimer += gameTime.ElapsedGameTime.Milliseconds;
            foreach (var decoration in decorations)
            {
                if (IsExiting)
                    break;
                decoration.Update(gameTime);
            }

            decorations = decorations.Where(decoration => !decoration.IsExpired).ToList();

            if (decorationTimer >= timeToDecorate)
            {
                decorationTimer = 0;
                timeToDecorate = random.Next(0, timeLimit);

                float directionSouth = random.NextFloat(MathHelper.ToRadians(300f), MathHelper.ToRadians(360));
                float directionNorth = random.NextFloat(0, MathHelper.ToRadians(75));
                float directionSouthTwo = random.NextFloat(MathHelper.ToRadians(190), MathHelper.ToRadians(250));
                float directionNorthTwo = random.NextFloat(MathHelper.ToRadians(120), MathHelper.ToRadians(160));

                Vector2 VelocitySouth = MathUtil.FromPolar(directionSouth, random.NextFloat(.5f, 8f));
                Vector2 VelocityNorth = MathUtil.FromPolar(directionNorth, random.NextFloat(.5f, 8f));
                Vector2 VelocityNorthTwo = MathUtil.FromPolar(directionNorthTwo, random.NextFloat(.5f, 8f));
                Vector2 VelocitySouthTwo = MathUtil.FromPolar(directionSouthTwo, random.NextFloat(.5f, 8f));

                VelocityNorthTwo.ScaleTo(-1f);
                VelocitySouthTwo.ScaleTo(-1f);

                Color decorationNewColor = Color.Lerp(backgroundColor, decorationsColor, random.NextFloat(.65f, 1));
                if (decorations.Count < 100)
                {
                    decorations.Add(new MenuDecoration(spawnOffset,
                        new Vector2(0 - spawnOffset, ScreenManager.GraphicsDevice.Viewport.Height - spawnOffset),
                        VelocitySouth,
                        this.pixel, decorationNewColor,
                        ScreenManager.GraphicsDevice.Viewport.Bounds, this));

                    decorations.Add(new MenuDecoration(spawnOffset,
                        new Vector2(0 - spawnOffset, 0 - spawnOffset),
                        VelocityNorth, this.pixel, decorationNewColor, ScreenManager.GraphicsDevice.Viewport.Bounds, this));

                    decorations.Add(new MenuDecoration(spawnOffset,
                        new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + spawnOffset, 0 - spawnOffset),
                        VelocityNorthTwo,
                        this.pixel,
                        decorationNewColor,
                        ScreenManager.GraphicsDevice.Viewport.Bounds, this));

                    decorations.Add(new MenuDecoration(spawnOffset,
                        new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + spawnOffset, ScreenManager.GraphicsDevice.Viewport.Height + spawnOffset),
                        VelocitySouthTwo,
                        this.pixel,
                        decorationNewColor,
                        ScreenManager.GraphicsDevice.Viewport.Bounds, this));
                }

            }
            
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(this.pixel, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), backgroundColor * TransitionAlpha);

            foreach (var decoration in decorations)
                decoration.Draw(spriteBatch);

            spriteBatch.End();
        }


        #endregion
    }
}
