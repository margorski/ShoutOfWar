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
using MonoGame.Extended;
using ShoutOfWar.Game.Components;

namespace ShoutOfWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Current { private set; get; }

        public GraphicsDeviceManager Graphics { private set; get; }
        public SpriteBatch SpriteBatch { private set; get; }
        public SpriteRender SpriteRender { private set; get; }
        public Random Random { private set; get; } = new Random();

        // to be moved to Scene
        public List<Entity> entities = new List<Entity>();
        public EntityFactory entityFactory;

        public bool drawDebug = false;

        private KeyboardState keyboardState;
        private MouseState mouseState;

        public List<Entity> EnabledEntities
        {
            get
            {
                return entities.FindAll(e => e.enabled);
            }
        }

        public Game1()
        {
            if (Game1.Current != null) throw new Exception("GameObject exists already. That means that something went wrong or someone explicitly try to create Game1 object.");

            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            Graphics.PreferredBackBufferHeight = Config.ScreenHeight;
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
            keyboardState = Keyboard.GetState();

            entities.Add(entityFactory.createPlayer(new Vector2(Config.ScreenWidth / 2, Config.ScreenHeight / 2)));
            
            var teams = new NpcLogic.Team[] { NpcLogic.Team.Red, NpcLogic.Team.Blue };
            for (var i = 0; i < 2; i++)
            {            
                for (var j = 0; j < Config.NumberOfNpcs; j++)
                {
                    entities.Add(entityFactory.createNPC(new Vector2(500.0f + i * 1000, 400.0f + j * 30.0f), $"player_{i}{j}", teams[i]));
                }
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

            var currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Tab) && keyboardState.IsKeyUp(Keys.Tab)) drawDebug = !drawDebug;
            keyboardState = currentKeyboardState;

            var currentMouseState = Mouse.GetState();
            if (currentMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                entities.Add(entityFactory.createNPC(new Vector2(currentMouseState.Position.X, currentMouseState.Position.Y), $"npc_{entities.Count+1}", NpcLogic.Team.Red));
            }
            if (currentMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
            {
                entities.Add(entityFactory.createNPC(new Vector2(currentMouseState.Position.X, currentMouseState.Position.Y), $"npc_{entities.Count + 1}", NpcLogic.Team.Blue));
            }
            mouseState = currentMouseState;

            foreach (var entity in entities)
            {
                if (entity.enabled) entity.Update(gameTime);
            }

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

            foreach (var entity in entities)
            {
                if (entity.enabled)
                {
                    entity.Draw(gameTime);
                    if (drawDebug) entity.DrawDebug(gameTime);
                }
            }

            this.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
