#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using ThridWolrdShooterGame.Effects;
using ThridWolrdShooterGame.Entites;
using System.Collections.Generic;
using ThridWolrdShooterGame.Entites.Items;
using ThridWolrdShooterGame.Managers;
using ThridWolrdShooterGame.Entites.Messages;
using MyExtensions;
using GameStateManagement.Screens;
using Geometry_Wars_Rip_Off;
using GameStateManagement.ScreensUtils;
using System.Diagnostics;
using GameStateManagement.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using Microsoft.Xna.Framework.GamerServices;
using System.Text.RegularExpressions;
using TechnicalData;
using BloomPostprocess;
#endregion

namespace ThridWolrdShooterGame
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        BloomComponent bloom;
        ContentManager content;

        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        SpriteBatch spriteBatch;

        Random random = new Random();

        public static Camera Camera { get; private set; }

        public static Vector2 WorldSize { get; private set; }
        public static Viewport World { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static ParticleManager<ParticleState> ParticleManager { get; private set; }
        // desired width of backbuffer
        public static int graphicsWidth{private set; get;}

        // desired height of backbuffer
        public static int graphicsHeight { private set; get; }

        public static int FreezeTime
        {
            get;
            set;
        }

        public static bool IsFreezing
        {
            get
            {
                return FreezeTime > 0;
            }
        }

        public Vector2 ScreenSize {private set; get;}

        public static bool IsSlowMotion;

        public static float SlowMoMultiplier;
        public static float timeToSlowMo = 30f;
        public static float slowMoTransition = 0;

        private static float normalMo = 1;
        private static float slowMo = .5f;

        private List<Layer> layers;

        private int totalFrames = 0;
        private float elapsedTime = 0;
        private int fps = 0;
        private bool paused = false;
        private int slowMoTimer;
        private int aux = 0;
        float pauseAlpha;

        double timeToDisplayGameOver;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        private Resources resources;


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            resources = new Resources(content, ScreenManager.GraphicsDevice);

            graphicsWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            graphicsHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            ScreenSize = new Vector2(graphicsWidth, graphicsHeight);
            bloom = new BloomComponent(ScreenManager.Game);
            ScreenManager.Game.Components.Add(bloom);
            bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);

            ParticleManager = new ParticleManager<ParticleState>(1000 * 5, ParticleState.UpdateParticle);
            WorldSize = new Vector2(1000, 800);//800,600, 800, 480 , 1000, 800
            World = new Viewport(0, 0, (int)WorldSize.X, (int)WorldSize.Y);
            EntityManager.Init(new QuadTree(0, new Rectangle(0, 0, (int)WorldSize.X, (int)WorldSize.Y)));
            EntityManager.SetResources(resources);
            EntityManager.Add(new PlayerShip());

            HudManager.Init();
            HudManager.Add(new BonusClock());
            HudManager.Add(new CounterMeter());
            //EntityManager.Add(Core.Instance);
            
            PlayerStatus.Init();
            ColorManager.Init();
            EnemySpawner.Reset();
            EnemySpawner.IsSpawning = true;
            ItemSpawner.Init();
            IsSlowMotion = false;

            Camera = new Camera(ScreenManager.GraphicsDevice.Viewport);
            layers = new List<Layer>
            {
                //new Layer(Camera, new Rectangle(0,0,(int)WorldSize.X , (int)WorldSize.Y)) { Parallax = new Vector2(0.0f, 0.0f), Color = Color.Red },
                new FirstLayer( 5 ,5) { WorldBounds =  new Rectangle(-(int)WorldSize.X,-(int)WorldSize.Y,(int)WorldSize.X *3, (int)WorldSize.Y  * 3),
                    Camera = Camera, 
                    Parallax = new Vector2(0.15f, 0.15f), 
                    Color = Color.Tomato * 0.30f,
                    LineSize = 5}
                    ,
                new FirstLayer( 10 , 10 ) { WorldBounds =  new Rectangle(-(int)WorldSize.X,-(int)WorldSize.Y,(int)WorldSize.X *2, (int)WorldSize.Y  * 2),
                    Camera = Camera, 
                    Parallax = new Vector2(0.35f, 0.35f), 
                    Color = Color.CornflowerBlue * .30f,
                    LineSize = 3 }
                    ,
            };

            
            SoundManager.Theme = content.Load<Song>("Sounds/Music/GameMusic");
            SoundManager.NormalShot = content.Load<SoundEffect>("Sounds/Sfx/normalShot");
            SoundManager.PickItem = content.Load<SoundEffect>("Sounds/Sfx/PickItem");
            SoundManager.PlayerDead = content.Load<SoundEffect>("Sounds/Sfx/PlayerDead");
            SoundManager.RocketShot = content.Load<SoundEffect>("Sounds/Sfx/RocketsShot");
            SoundManager.SpawnItem = content.Load<SoundEffect>("Sounds/Sfx/SpawnItem");
            SoundManager.SpawnPlayer = content.Load<SoundEffect>("Sounds/Sfx/SpawnPlayer");
            SoundManager.Yay = content.Load<SoundEffect>("Sounds/Sfx/Yay");
            SoundManager.Applause = content.Load<SoundEffect>("Sounds/Sfx/applause");
            SoundManager.ApplauseLower = content.Load<SoundEffect>("Sounds/Sfx/applauseLower");
            SoundManager.SpawnBonusEnemies = content.Load<SoundEffect>("Sounds/Sfx/SpawnBonusEnemies");
            SoundManager.CashRegister = content.Load<SoundEffect>("Sounds/Sfx/cashRegister");
            SoundManager.Aww = content.Load<SoundEffect>("Sounds/Sfx/aww");
            SoundManager.Explosions = Enumerable.Range(1, 10).Select(x => content.Load<SoundEffect>("Sounds/Sfx/Hit_Hurt" + x)).ToArray();
            SoundManager.SlowDown = content.Load<SoundEffect>("Sounds/Sfx/slowdown");
            SoundManager.SlowUp = content.Load<SoundEffect>("Sounds/Sfx/slowup");
            SoundManager.PlayTheme();

            CanPause = true;
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
            messageTaken = false;
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            EntityManager.Unload();
            SoundManager.Dispose();
            content.Unload(); 
        }
        #endregion
        private bool lastSlowDownState = IsSlowMotion;
        int indexPlayer;
        bool displayInput = false;
        MessageBoxScreen confirmQuitMessageBox;
        bool messageTaken;
        InsertNameScreen insertNameMessageBox;

        bool bloomEffect;
        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

           
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);


            if (otherScreenHasFocus || coveredByOtherScreen)
                bloomEffect = false;
            else
                bloomEffect = true;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                

                elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                

                if (elapsedTime >= 1000f)
                {
                    fps = totalFrames;
                    totalFrames = 0;
                    elapsedTime = 0;
                }

                GameTime = gameTime;

                Input.Update();

                Camera.LookAt(EntityManager.PlayerShip.Position);
                Camera.Update(gameTime);

                if (PlayerStatus.IsGameOver)
                {
                    timeToDisplayGameOver += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (timeToDisplayGameOver > 4000)
                    {
                        var players = GameStatics.PlayersHighScores;
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (PlayerStatus.Points > players[i].Score)
                            {
                                displayInput = true;
                                indexPlayer = i;
                            }
                        } 

                        string textGameOver = "Game Over";

                        textGameOver += "\nYour Score : " + PlayerStatus.Points + "\n" + "Top Score by:\n" + GameStatics.PlayersHighScores.Last().Name + " : " + GameStatics.PlayersHighScores.Last().Score;

                        confirmQuitMessageBox = new MessageBoxScreen(textGameOver);
                        confirmQuitMessageBox.AcceptedEntry = new TouchableEntry("A: Play again", 20);
                        confirmQuitMessageBox.CancelledEntry = new TouchableEntry("B: Go to Menu", 20);

                        confirmQuitMessageBox.AcceptedEntry.Selected += ConfirmQuitMessageBoxAccepted;

                        confirmQuitMessageBox.CancelledEntry.Selected += CancelQuitMessageBoxAccepted;
                        ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
                        if (displayInput)
                        {
                            insertNameMessageBox = new InsertNameScreen("New_Highscore!\n");


                            insertNameMessageBox.AcceptedEntry = new TouchableEntry("A : Save", 20);
                            insertNameMessageBox.AcceptedEntry.Selected += ConfirmHighScoreInput;
                            ScreenManager.AddScreen(insertNameMessageBox, ControllingPlayer);
                        }

                    }
                }

                if (Input.WasKeyPressed(Keys.Space))
                    paused = !paused;
                if (!paused)
                {
                    if (FreezeTime > 0)
                    {
                        FreezeTime--;
                    }

                    if (IsSlowMotion)
                    {
                        slowMoTransition += (float)gameTime.ElapsedGameTime.Milliseconds * 0.01f;

                        slowMoTimer += gameTime.ElapsedGameTime.Milliseconds;
                        
                        if (slowMoTimer >= slowMoTime)
                        {
                            slowMoTimer = 0;
                            IsSlowMotion = false;
                        }

                        if (slowMoTransition > timeToSlowMo)
                            slowMoTransition = timeToSlowMo;
                    }
                    else
                    {

                        
                        
                        slowMoTransition -= (float)gameTime.ElapsedGameTime.Milliseconds * 0.01f;

                        if (slowMoTransition < 0)
                            slowMoTransition = 0;

                    }
                    if (lastSlowDownState != IsSlowMotion)
                        SoundManager.PlaySlowUp();

                    SlowMoMultiplier = MathHelper.Lerp(normalMo, slowMo, (float)slowMoTransition / timeToSlowMo);
                    Lightupdate(gameTime);
                    if (FreezeTime <= 0)
                    {
                        EntityManager.Update(gameTime);
                        if (IsSlowMotion)
                            aux++;
                        else
                            aux = 0;
                        if (aux % 2 == 0)
                            ParticleManager.Update();
                    }

                    lastSlowDownState = IsSlowMotion;

                }
            }
        }


        void ConfirmHighScoreInput(object sender, PlayerIndexEventArgs e)
        {
            string playerName;

            if (insertNameMessageBox.Text.Length == 0 || insertNameMessageBox.Text == null)
                playerName = "Shooter's Fan";
            else
                playerName = insertNameMessageBox.Text;

            for (int i = indexPlayer; i > 0; i--)
            {
                GameStatics.PlayersHighScores[indexPlayer - 1] = GameStatics.PlayersHighScores[indexPlayer];
            }

            GameStatics.PlayersHighScores[indexPlayer] = new Player { Name = playerName, Score = PlayerStatus.Points };
            FileHandler.SaveHighScores(GameStatics.PlayersHighScores);
            string textGameOver = "Game Over";
            textGameOver += "\nYour Score : " + PlayerStatus.Points + "\n" + "Top Score by:\n" + GameStatics.PlayersHighScores.Last().Name + " : " + GameStatics.PlayersHighScores.Last().Score;

            confirmQuitMessageBox.ReWriteMessage(textGameOver);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playStartGame();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            
            
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void CancelQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            if (GameStatics.sounds)
                ScreenManager.playBackToMenu();
            LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }



        private void Lightupdate(GameTime gameTime)
        {
            SoundManager.Update();
            ItemSpawner.Update();
            HudManager.Update(gameTime);
            EnemySpawner.Update(gameTime);
            PlayerStatus.Update();
        }

        private static float slowMoTime;

        public static void SlowGameBy(float miliseconds)
        {
            SoundManager.PlaySlowDown();
            slowMoTime = 5000;
            IsSlowMotion = true;
            timeToSlowMo = 30f;
        }

        public static void SlowGameInstantlyBy(float miliseconds)
        {
            if (IsSlowMotion)
                return;

            slowMoTime = miliseconds;
            IsSlowMotion = true;
            timeToSlowMo = 1f;
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                //LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                playerPosition += movement * 2;
            }
        }


        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = EntityManager.Resources.DemoFont.MeasureString(text).X;
            spriteBatch.DrawString(EntityManager.Resources.DemoFont, text, new Vector2(graphicsWidth - textWidth - 5, y), Color.White * .9f);
        }

        public static bool CanPause = false;
        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

           
            // This game has a blue background. Why? Because!

            if (bloomEffect)
            {
                bloom.Visible = true;
                bloom.BeginDraw();
            }
            else
            {
                bloom.Visible = false;
            }

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                             Color.Black, 0, 0);
            spriteBatch = ScreenManager.SpriteBatch;   

            foreach (Layer layer in layers)
                layer.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null,
                Camera.Transform);

            //spriteBatch.Begin();

            spriteBatch.DrawLine(new Vector2(0, 0), new Vector2(WorldSize.X, 0), Color.White, 5);
            spriteBatch.DrawLine(new Vector2(WorldSize.X, 0), new Vector2(WorldSize.X, WorldSize.Y), Color.White, 5);
            spriteBatch.DrawLine(new Vector2(WorldSize.X, WorldSize.Y), new Vector2(0, WorldSize.Y), Color.White, 5);
            spriteBatch.DrawLine(new Vector2(0, 0), new Vector2(0, WorldSize.Y), Color.White, 5);

            EntityManager.Draw(spriteBatch);
            ParticleManager.Draw(spriteBatch);
            spriteBatch.End();

            /*spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null,
                Camera.Transform);
            ParticleManager.Draw(spriteBatch);
            spriteBatch.End();

            

            */
            string fpsMessage = "FPS " + fps;

            string shieldMessage = "SHIELD " + (PlayerStatus.Shield - 1);
            string score = "SCORE " + PlayerStatus.Points;
            string lives = "LIVES " + (PlayerStatus.Lives <= 0 ? 0 : PlayerStatus.Lives - 1);
            //Draw UI 
            spriteBatch.Begin();
            //spriteBatch.DrawString(EntityManager.Resources.DemoFont, fpsMessage, new Vector2(100, 0), Color.White);
            spriteBatch.DrawString(EntityManager.Resources.DemoFont, shieldMessage,
                new Vector2(graphicsWidth - (EntityManager.Resources.DemoFont.MeasureString(score + shieldMessage).X),
                    EntityManager.Resources.DemoFont.MeasureString(score).Y) + new Vector2(-20, 0),
                Color.White * .9f);

            spriteBatch.DrawString(EntityManager.Resources.DemoFont, lives,
                new Vector2(graphicsWidth - (EntityManager.Resources.DemoFont.MeasureString(score + shieldMessage + lives).X),
                    EntityManager.Resources.DemoFont.MeasureString(score).Y) + new Vector2(-40, 0),
                Color.White * .9f);

            this.DrawRightAlignedString(score, EntityManager.Resources.DemoFont.MeasureString(shieldMessage).Y);

            HudManager.Draw(spriteBatch);

            totalFrames++;

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
           /* if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);*/

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
