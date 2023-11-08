using nosu;
using System.Collections;
namespace nosu_;

public class Game1 : Game
{
    List<string> beatmapLocations;
    List<Beatmap> beatmaps;
    MouseState mouseState;
    Vector2 mousePos;

    SpriteFont beatmapName;
    Texture2D cursor;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false; //To use custom "circle" cursor found in osu!
        _graphics.IsFullScreen = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        beatmapLocations = Beatmap.GetBeatmaps();
        beatmaps = new();
        for (int i = 0; i < beatmapLocations.Count; i++)
        {
            Console.WriteLine(beatmapLocations);
            Beatmap beatmap = new Beatmap(Beatmap.GetBeatmaps()[i]);
            if (beatmap.assets != null) {
                beatmaps.Add(beatmap);
            }
        }
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        beatmapName = Content.Load<SpriteFont>("BeatmapName");
        cursor = Content.Load<Texture2D>("cursor");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mouseState = Mouse.GetState();

        mousePos = new Vector2(mouseState.X, mouseState.Y);

        // TODO: Add your update logic here


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        /*_spriteBatch.DrawString(beatmapName, (string)((Hashtable)beatmaps[0].difficulties[0]["[General]"])["Title"],
            new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
            Color.White);
        */
        _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width/2, cursor.Height/2), Vector2.One, SpriteEffects.None, 0f);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}