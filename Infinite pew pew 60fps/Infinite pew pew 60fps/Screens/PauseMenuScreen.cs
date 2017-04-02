#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using TechnicalData;
#endregion

namespace GameStateManagement.Screens
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("Game\nPaused",1)
        {
            // Create our menu entries.
            TouchableEntry resumeGameMenuEntry = new TouchableEntry("Resume Game",20);
            TouchableEntry quitGameMenuEntry = new TouchableEntry("Quit Game",20);

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input

        MessageBoxScreen confirmQuitMessageBox;
        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Really?!!";

            confirmQuitMessageBox = new MessageBoxScreen(message);
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
            if (GameStatics.sounds)
                ScreenManager.BackToMenu.Play();
            LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void CancelQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
        }


        #endregion
    }
}
