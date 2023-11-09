using Microsoft.Xna.Framework.Input;
using nosu;
using System.Collections;
namespace nosu_;

public class Game1 : Game
{
    int currentBeatmap = 0;
    List<string> beatmapLocations;
    List<Beatmap> beatmaps;
    Vector2 mousePos;
    Vector2 leftPageButtonArea;
    Vector2 rightPageButtonArea;

    enum GameState
    {
        SongSelect,
        Gameplay
    }
    GameState gameState = GameState.SongSelect;

    SpriteFont beatmapName;
    Texture2D cursor;
    Texture2D nextPageL;
    Texture2D nextPageR;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false; //To use custom "circle" cursor found in osu!
        //_graphics.PreferredBackBufferWidth = 500;
        //_graphics.PreferredBackBufferHeight = 500;
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
        nextPageL = Content.Load<Texture2D>("nextPg");
        nextPageR = Content.Load<Texture2D>("nextPg");
        cursor = Content.Load<Texture2D>("cursor");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Right) && currentBeatmap + 1 < beatmaps.Count)
            currentBeatmap++;

        if (Keyboard.GetState().IsKeyDown(Keys.Left) && currentBeatmap - 1 >= 0)
            currentBeatmap--;

       /* if (Mouse.GetState().LeftButton == ButtonState.Pressed && gameState == GameState.SongSelect && mousePos.X <= )
        {
            
        }*/

        mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MidnightBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        _spriteBatch.DrawString(beatmapName, (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["Title"],
            new Vector2(_graphics.PreferredBackBufferWidth / 2 - (beatmapName.MeasureString((string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["Title"]).X / 2f), _graphics.PreferredBackBufferHeight / 2.3f),
            Color.Black);

        //Page buttons
        _spriteBatch.Draw(nextPageL, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.FlipHorizontally, 0f);
        _spriteBatch.Draw(nextPageR, new Vector2(_graphics.PreferredBackBufferWidth - cursor.Width /0.327f, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.None, 0f); //TODO: Why the frick is this divided by cursor.Width???????????????????????????????

        //Cursor
        _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}