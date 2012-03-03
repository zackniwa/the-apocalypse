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

namespace The_Apocalypse
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool isPaused = false, isMainMenu = true;
        Options options;
        MainMenu main;
        Level game;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 960;
            this.graphics.PreferredBackBufferHeight = 600;

            //D�finit si l'on voit la souris ou non. Nous pouvons la g�n�r�.
            this.IsMouseVisible = true;

            options = new Options((this.Window.ClientBounds.Width / 2),(this.Window.ClientBounds.Height / 2));
            main = new MainMenu();
            game = new Level();
            game.initialize(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            options.LoadTexture(Content, GraphicsDevice, graphics);
            main.LoadTexture(Content);
            game.LoadContent(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        KeyboardState oldState = Keyboard.GetState();
        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            if (oldState.IsKeyDown(Keys.Escape) && newState.IsKeyUp(Keys.Escape))
            {
                isPaused = !isPaused;
            }

            if (isPaused)
            {
                if (options.closeProgram)
                    this.Exit();
                options.update_buttons(gameTime);
            }
            else
            {
                if (isMainMenu)
                {
                    if (main.closeProgram)
                        this.Exit();
                    if (main.play)
                        this.isMainMenu = false;
                    main.update_buttons(gameTime);
                }
                //Update du jeu.
            }

            oldState = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            if (isMainMenu)
            {
                main.Draw(gameTime, spriteBatch);
            }
            else
            {
                //Draw du jeu. D�s que le play est appuyer on rentre ici.
            }
            if (isPaused)
            {
                options.setButtonData((this.Window.ClientBounds.Width / 2), (this.Window.ClientBounds.Height / 2));
                options.Draw(gameTime, spriteBatch);
            }
            else
            {
                //Control brightness/contrast, in else to desactivate it for option preview
                game.DrawContrastAndBrightness(spriteBatch);
            }
            
            base.Draw(gameTime);
        }
    }
}
