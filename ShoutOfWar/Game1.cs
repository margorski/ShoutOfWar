using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
using ShoutOfWar.Game;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;
using ShoutOfWar.Game.Shared;

namespace ShoutOfWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Current { private set; get; }

        public readonly Point ScreenSize = new Point(1280, 1024);
        public GraphicsDeviceManager Graphics { private set; get; }
        public SpriteBatch SpriteBatch { private set; get; }
        public SpriteRender SpriteRender { private set; get; }
        public Random Random { private set; get; } = new Random();

        // to be moved to Scene
        List<Entity> entities = new List<Entity>();
        EntityFactory entityFactory;

        public Game1()
        {
            if (Game1.Current != null) throw new Exception("GameObject exists already. That means that something went wrong or someone explicitly try to create Game1 object.");

            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = ScreenSize.X;
            Graphics.PreferredBackBufferHeight = ScreenSize.Y;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            Game1.Current = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            entities.Add(entityFactory.createPlayer(new Vector2(500, 500)));
            for (var i = 0; i < 50; i++)
            {
                var randomPosition = new Vector2(Random.Next(ScreenSize.X), Random.Next(ScreenSize.Y));
                entities.Add(entityFactory.createNPC(randomPosition, i.ToString()));                
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteRender = new SpriteRender(SpriteBatch);

            var spriteSheetLoader = new SpriteSheetLoader(Content, GraphicsDevice);
            var spriteSheet = spriteSheetLoader.Load(@"Spritesheets\mlm_armies\mlm_armies.png");
            entityFactory = new EntityFactory(spriteSheet);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var entity in entities) entity.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            foreach (var entity in entities) entity.Draw(gameTime);            

            this.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
