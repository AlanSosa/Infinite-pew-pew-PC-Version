#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.IO;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using GameStateManagement.Screens;
using Third_World_Shooter.Screens;
using System.Diagnostics;
#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif
using GameStateManagement.ScreensUtils;
using TechnicalData;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        static int menuEntryPadding = 20;

        ContentManager content;
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Infinite\npew pew", 0)
        {
            // Create our menu entries.
            TouchableEntry playGameMenuEntry = new TouchableEntry("Play Game",menuEntryPadding);
            TouchableEntry optionsMenuEntry = new TouchableEntry("Options",menuEntryPadding);
            TouchableEntry highscoresMenuEntry = new TouchableEntry("High Scores",menuEntryPadding);
            TouchableEntry aboutMenuEntry = new TouchableEntry("About",menuEntryPadding);
            TouchableEntry exitMenuEntry = new TouchableEntry("Exit Game", menuEntryPadding);

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            highscoresMenuEntry.Selected += StatsMenuEntrySelected;
            exitMenuEntry.Selected += ExitMenuEntrySelected;
            aboutMenuEntry.Selected += AboutMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(highscoresMenuEntry);
            MenuEntries.Add(aboutMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }
        #endregion

        #region Handle Input

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            Texture2D WindowsPhoneLogo = content.Load<Texture2D>("Sprites/WindowsPhoneIco");
            
            int xPos = 0;
            int imageWidth = 60;
            /*ImageTouchableEntry facebookIco = new ImageTouchableEntry(ScreenManager.FacebookIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - imageWidth, ScreenManager.GraphicsDevice.Viewport.Height - imageWidth /2), 30);
            xPos += 5;
            ImageTouchableEntry twitterIco = new ImageTouchableEntry(ScreenManager.TwitterIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (imageWidth + xPos + imageWidth), ScreenManager.GraphicsDevice.Viewport.Height - imageWidth /2), 30);
            xPos += 5;
            ImageTouchableEntry websiteIco = new ImageTouchableEntry(ScreenManager.WebsiteIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (imageWidth + xPos + imageWidth * 2), ScreenManager.GraphicsDevice.Viewport.Height - imageWidth /2), 30);
            */
            ImageTouchableEntry facebookEntry = new ImageTouchableEntry(ScreenManager.FacebookIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (imageWidth + xPos + imageWidth * 2) - 150, ScreenManager.GraphicsDevice.Viewport.Height - imageWidth / 2), 30);
            xPos += 15;
            ImageTouchableEntry twitterEntry = new ImageTouchableEntry(ScreenManager.TwitterIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - (imageWidth + xPos + imageWidth) -120, ScreenManager.GraphicsDevice.Viewport.Height - imageWidth / 2), 30);
            xPos += 15;
            ImageTouchableEntry websiteEntry = new ImageTouchableEntry(ScreenManager.WebsiteIco, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - imageWidth - 120 , ScreenManager.GraphicsDevice.Viewport.Height - imageWidth / 2), 30);
            xPos += 15;
            ImageTouchableEntry windowsPhoneStore = new ImageTouchableEntry(WindowsPhoneLogo, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - imageWidth - 30, ScreenManager.GraphicsDevice.Viewport.Height - WindowsPhoneLogo.Height / 2), 30);

            twitterEntry.Selected += TwitterMenuEntrySelect;
            facebookEntry.Selected += FacebookMenuEntrySelect;
            websiteEntry.Selected += WebsiteMenuEntrySelect;
            windowsPhoneStore.Selected += WindowsStoreEntrySelect;

            MenuEntries.Add(facebookEntry);
            MenuEntries.Add(twitterEntry);
            MenuEntries.Add(websiteEntry);
            MenuEntries.Add(windowsPhoneStore);
        }

        void IndieDBMenuEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("http://www.indiedb.com/games/third-world-shooter");
        }

        void GameJoltMenuEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("http://gamejolt.com/games/arcade/third-world-shooter/35232/");
        }

        void WebsiteMenuEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("http://www.cubitomorado.blogspot.mx/");
        }

        void WindowsStoreEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("http://www.windowsphone.com/en-us/store/app/infinite-pew-pew/5b657144-c4c1-4514-a91a-dd11bdc068ae");
        }

        void TwitterMenuEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("https://www.twitter.com/alansosame");
        }

        void FacebookMenuEntrySelect(object sender, PlayerIndexEventArgs e)
        {
            Process.Start("https://www.facebook.com/cubitomorado");
        }

        void AboutMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();

            ScreenManager.AddScreen(new AboutMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playStartGame();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Stats menu entry is selected
        /// </summary>
        void StatsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();
            ScreenManager.AddScreen(new LeaderBoardsScreen(), e.PlayerIndex);
        }

        
        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Exit menu entry is selected.
        /// </summary>
        void ExitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

            confirmQuitMessageBox.AcceptedEntry = new TouchableEntry("A: Yes", 20);
            confirmQuitMessageBox.CancelledEntry = new TouchableEntry("B: No", 20);
            confirmQuitMessageBox.AcceptedEntry.Selected += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// When the user cancels the main menu, we exit the game.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Wanna exit the game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

            confirmQuitMessageBox.AcceptedEntry = new TouchableEntry("A : Yes", 20);
            confirmQuitMessageBox.CancelledEntry = new TouchableEntry("B : No", 20);
            confirmQuitMessageBox.AcceptedEntry.Selected += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {

            ConfigData configuration = new ConfigData();
            configuration.Add(GameStatics.music);
            configuration.Add(GameStatics.sounds);
            configuration.Add(GameStatics.camera);
            configuration.Add(GameStatics.fullscreen);

            FileHandler.WriteConfigFile(configuration);
            FileHandler.SaveHighScores(GameStatics.PlayersHighScores);
            ScreenManager.Game.Exit();

        }


        #endregion
    }
}
