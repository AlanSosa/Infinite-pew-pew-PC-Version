#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using GameStateManagement.ScreensUtils;
using Microsoft.Xna.Framework.Graphics;
using TechnicalData;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        TouchableEntry musicMenuEntry;
        TouchableEntry soundsMenuEntry;
        TouchableEntry screenEntry;
        TouchableEntry cameraEntry;
        TouchableEntry resetScores;
        bool scoresDeleted = false;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options",1)
        {
            // Create our menu entries.
            soundsMenuEntry = new TouchableEntry(string.Empty,20);
            musicMenuEntry = new TouchableEntry(string.Empty,20);
            screenEntry = new TouchableEntry(string.Empty, 20);
            cameraEntry = new TouchableEntry(string.Empty, 20);
            resetScores = new TouchableEntry("Reset Scores",20);


            SetMenuEntryText();

            // Hook up menu event handlers.
            musicMenuEntry.Selected += musicMenuEntrySelected;
            soundsMenuEntry.Selected += soundsMenuEntrySelected;
            screenEntry.Selected += controlsMenuEntrySelected;
            cameraEntry.Selected += cameraMenuEntrySelected;
            resetScores.Selected += resetScoresMenuEntrySelected;
            
            // Add entries to the menu.
            MenuEntries.Add(musicMenuEntry);
            MenuEntries.Add(soundsMenuEntry);
            MenuEntries.Add(cameraEntry);
            MenuEntries.Add(screenEntry);
            MenuEntries.Add(resetScores);

            EntriesPosition = new Vector2(30f , 50f);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            string message ="Scores\nDeleted!!";
            if(scoresDeleted){
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message,new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + font.MeasureString(message).X, 400), Color.White * TransitionAlpha, 0,
                                       origin, 1f, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            musicMenuEntry.Text = "Music: " + (GameStatics.music ? "On": "Off");
            soundsMenuEntry.Text = "Sounds: " + (GameStatics.sounds ? "On" : "Off");
            cameraEntry.Text = "Camera: " + (GameStatics.camera ? "World" : "Player");
            screenEntry.Text = "Fullscreen: " + (GameStatics.fullscreen ? "Yes" : "No");
        }


        #endregion

        #region Handle Input

        void cameraMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();

            GameStatics.camera = !GameStatics.camera;
            SetMenuEntryText();
        }

        void resetScoresMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();

            GameStatics.PlayersHighScores = new PlayersList();
            GameStatics.PlayersHighScores.Add(new Player { Name = "Fulano", Score = 6000});
            GameStatics.PlayersHighScores.Add(new Player { Name = "Compadre", Score = 7000 });
            GameStatics.PlayersHighScores.Add(new Player { Name = "AnotherDude", Score = 8000 });
            GameStatics.PlayersHighScores.Add(new Player { Name = "Dude", Score = 9000 });
            GameStatics.PlayersHighScores.Add(new Player { Name = "Bacon Lover", Score = 10000 });

            FileHandler.SaveHighScores(GameStatics.PlayersHighScores);
            scoresDeleted = true;
        }

        void controlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();

            ScreenManager.Graphics.ToggleFullScreen();

            GameStatics.fullscreen = !GameStatics.fullscreen;
            SetMenuEntryText();
        }

        void musicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();

            GameStatics.music = !GameStatics.music;
            SetMenuEntryText();
        }

        void soundsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameStatics.sounds = !GameStatics.sounds;
            SetMenuEntryText();

            if (GameStatics.sounds)
                ScreenManager.playSelect();
        }
        #endregion
    }
}
