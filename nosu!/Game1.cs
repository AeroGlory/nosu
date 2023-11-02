using nosu;

namespace nosu_;

public class Game1 : Game
{
    Beatmap test = new Beatmap(Beatmap.GetBeatmaps()[1]);
    SpriteFont beatmapName;
        
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        beatmapName = Content.Load<SpriteFont>("BeatmapName");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkViolet);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        //_spriteBatch.DrawString(beatmapName, test.catagories[0], new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), Color.Black);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

