using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using GameStateManagement.ScreensUtils;
using System.Reflection;
using TechnicalData;

namespace GameStateManagement.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class LeaderBoardsScreen : MenuScreen
    {
        #region Fields
        /*
        MenuEntry SubmitEntry;
        MenuEntry RefreshEntry;
        */

        TouchableEntry ScoresOne;
        TouchableEntry ScoresTwo;
        TouchableEntry ScoresThree;
        TouchableEntry ScoresFour;
        TouchableEntry ScoresFive;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public LeaderBoardsScreen()
            : base("High\nScores", 1)
        {/*
            // Create our menu entries.
            RefreshEntry = new MenuEntry("Refresh", 20);
            SubmitEntry = new MenuEntry("Submit", 20);

            // Hook up menu event handlers.
            SubmitEntry.Selected += SubmitMenuEntrySelected;
            RefreshEntry.Selected += RefreshMenuEntrySelected;


            // Add entries to the menu.
            MenuEntries.Add(SubmitEntry);
            MenuEntries.Add(RefreshEntry);*/
            ScoresOne = new TouchableEntry(GameStatics.PlayersHighScores[4].Name + "\n" + GameStatics.PlayersHighScores[4].Score, 20);
            ScoresOne.Number = 1;
            ScoresTwo = new TouchableEntry(GameStatics.PlayersHighScores[3].Name + "\n" + GameStatics.PlayersHighScores[3].Score, 20);
            ScoresTwo.Number = 2;
            ScoresThree = new TouchableEntry(GameStatics.PlayersHighScores[2].Name + "\n" + GameStatics.PlayersHighScores[2].Score, 20);
            ScoresThree.Number = 3;
            ScoresFour = new TouchableEntry(GameStatics.PlayersHighScores[1].Name + "\n" + GameStatics.PlayersHighScores[1].Score, 20);
            ScoresFour.Number = 4;
            ScoresFive = new TouchableEntry(GameStatics.PlayersHighScores[0].Name + "\n" + GameStatics.PlayersHighScores[0].Score, 20);
            ScoresFive.Number = 5;

            MenuEntries.Add(ScoresOne);
            MenuEntries.Add(ScoresTwo);
            MenuEntries.Add(ScoresThree);
            MenuEntries.Add(ScoresFour);
            MenuEntries.Add(ScoresFive);

            EntriesPosition = new Vector2(60, 50);
        }
        /*
        void SubmitMenuEntrySelected(object sender, PlayerIndexEventArgs player)
        {
            //Load Stat Screen
            Guide.BeginShowKeyboardInput(PlayerIndex.One, "Leaderboard crap", "insert your fucking name", "Shooter's Fan", new AsyncCallback(OnEndShowKeyboardInput), null);
        }

        private void OnEndShowKeyboardInput(IAsyncResult result)
        {
            string name = Guide.EndShowKeyboardInput(result);
        }

        void RefreshMenuEntrySelected(object sender, PlayerIndexEventArgs player)
        {
            // Refresh HighScores
        }*/


    }
}
