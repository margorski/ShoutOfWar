using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShoutOfWar.Engine;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace ShoutOfWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteRender spriteRender;

        // to be moved to Scene
        SpriteSheet spriteSheet;

        // To be moved to Entity        
        Animation playerAnimation = new Animation(200.0);

        public Game1()
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteRender = new SpriteRender(spriteBatch);

            var spriteSheetLoader = new SpriteSheetLoader(Content, GraphicsDevice);
            spriteSheet = spriteSheetLoader.Load(@"Spritesheets\mlm_armies\mlm_armies.png");

            playerAnimation.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_0));
            playerAnimation.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_1));
            playerAnimation.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_2));
            playerAnimation.AddFrame(spriteSheet.Sprite(mlm_armies.Mlm_armies_3));
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

            playerAnimation.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            this.spriteRender.Draw(
                playerAnimation.GetFrame(),
                new Vector2(300, 300),
                Color.White,
                0,
                1
                );
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
