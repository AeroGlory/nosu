using Microsoft.Xna.Framework.Input;
using nosu;
using System.Collections;
using NetCoreAudio;
using System.Diagnostics.Metrics;

namespace nosu_;

public class Game1 : Game
{
    int currentBeatmap = 0;
    int currentDiff = 0;
    int currentHitObject = 0;
    const double selectCooldown = 0.2;
    double prevPress;
    double prevSelect;
    double beatmapStart;

    List<HitObject> hitObjects;
    List<string> beatmapLocations;
    List<Beatmap> beatmaps;
    Vector2 mousePos;

    enum GameState
    {
        SongSelect,
        DiffSelect,
        Gameplay
    }
    GameState gameState = GameState.SongSelect;

    SpriteFont beatmapName;
    Texture2D cursor;
    Texture2D nextPageL;
    Texture2D nextPageR;
    Texture2D hitCircle;
    Player player = new();
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
        //hitCircle = Content.Load<Texture2D>("hitcircle");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        switch (gameState)
        {
            case GameState.SongSelect:
        
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && currentBeatmap + 1 < beatmaps.Count)
                currentBeatmap++;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && currentBeatmap - 1 >= 0)
                currentBeatmap--;

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    prevPress = gameTime.TotalGameTime.TotalSeconds;
                    gameState = GameState.DiffSelect;
                }

                break;

            case GameState.DiffSelect:
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && currentDiff + 1 < beatmaps[currentBeatmap].difficulties.Count && (gameTime.TotalGameTime.TotalSeconds - prevSelect) > selectCooldown) //Yandare Dev core
                {
                    prevSelect = gameTime.TotalGameTime.TotalSeconds;
                    currentDiff++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) && currentDiff - 1 >= 0 && (gameTime.TotalGameTime.TotalSeconds - prevSelect) > selectCooldown)
                {
                    prevSelect = gameTime.TotalGameTime.TotalSeconds;
                    currentDiff--;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (gameTime.TotalGameTime.TotalSeconds - prevPress) > selectCooldown)
                    gameState = GameState.Gameplay;
                break;

            case GameState.Gameplay:
                beatmapStart = gameTime.TotalGameTime.TotalMilliseconds; //TODO: Should not be in here, gets constantly reassigned

                hitObjects = (List<HitObject>)beatmaps[currentBeatmap].difficulties[0]["[HitObjects]"]; //TODO: Should not be in here, gets constantly reassigned\\

                foreach(HitObject hitObject in hitObjects) {
                    Console.WriteLine(hitObject.x.ToString(), hitObject.y.ToString(), hitObject.time.ToString());
                }


                //Console.WriteLine(beatmaps[currentBeatmap].fullPath + (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]);

                try
                {
                    if (!player.Playing)
                    {
                        Console.WriteLine(Path.GetFullPath((string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"])); //C# hates absoulte file paths apparently
                         
                        //player.Play(Path.GetFullPath((string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]));
                        //player.Play(beatmaps[currentBeatmap].fullPath + (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]);
                    }
                    else
                    {
                        Console.WriteLine("Already playing.");
                    }
                }
                catch //It's a await function idiot 
                {
                    gameState = GameState.SongSelect;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Z) || Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    prevPress = gameTime.TotalGameTime.TotalSeconds;
                }

                break;
        }

        mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (gameState)
        {
            case GameState.SongSelect:
                GraphicsDevice.Clear(Color.LightPink);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(beatmapName, (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["Title"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - (beatmapName.MeasureString((string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["Title"]).X / 2f), _graphics.PreferredBackBufferHeight / 2.3f),
                    Color.Black);

                //Page buttons
                _spriteBatch.Draw(nextPageL, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(nextPageR, new Vector2(_graphics.PreferredBackBufferWidth - nextPageR.Width / 8.8f, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.None, 0f); //TODO: Why the frick is this divided by cursor.Width???????????????????????????????

                //Cursor
                _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                _spriteBatch.End();

                base.Draw(gameTime);
                break;

            case GameState.DiffSelect:
                GraphicsDevice.Clear(Color.LightPink);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(beatmapName, (string)((Hashtable)beatmaps[currentBeatmap].difficulties[currentDiff]["[General]"])["Version"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - (beatmapName.MeasureString((string)((Hashtable)beatmaps[currentBeatmap].difficulties[currentDiff]["[General]"])["Version"]).X / 2f), _graphics.PreferredBackBufferHeight / 2.3f),
                    Color.Black);

                //Page buttons
                _spriteBatch.Draw(nextPageL, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(nextPageR, new Vector2(_graphics.PreferredBackBufferWidth - nextPageR.Width / 8.8f, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.None, 0f); //TODO: Why the frick is this divided by cursor.Width???????????????????????????????

                //Cursor
                _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                _spriteBatch.End();

                break;

            case GameState.Gameplay:
                GraphicsDevice.Clear(Color.Black);


                break;
        }
    }
}