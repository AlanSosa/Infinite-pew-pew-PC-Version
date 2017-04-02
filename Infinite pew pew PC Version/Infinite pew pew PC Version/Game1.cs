using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;
using GameStateManagement.Screens;
using GameStateManagement.ScreensUtils;
using TechnicalData;
using BloomPostprocess;

namespace Infinite_pew_pew_PC_Version
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        #endregion

        #region Initialization

        /// <summary>
        /// The main game constructor.
        /// </summary>

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = true;

            this.Window.Title = "Infinite pew pew";

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            InitializeLandscapeGraphics();

            // Create the screen manager component.
            screenManager = new ScreenManager(this,graphics);

            GameStatics.PlayersHighScores = FileHandler.LoadHighScores();

            ConfigData configuration = FileHandler.GetConfigData();
            GameStatics.music = configuration[0];
            GameStatics.sounds = configuration[1];
            graphics.IsFullScreen = configuration[2];

            Components.Add(screenManager);
            // Activate the first screens.
            LoadingScreen.Load(screenManager, true, null, new SplashScreen());

        }

        protected override void OnExiting(object sender, System.EventArgs args)
        {
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// Helper method to the initialize the game to be a portrait game.
        /// </summary>
        private void InitializePortraitGraphics()
        {
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Helper method to initialize the game to be a landscape game.
        /// </summary>
        private void InitializeLandscapeGraphics()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        #endregion


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

namespace TechnicalData
{
    public static class GameStatics
    {
        public static bool music = true;
        public static bool sounds = true;
        public static bool fullscreen = true;

        public static PlayersList PlayersHighScores;
    }
}
