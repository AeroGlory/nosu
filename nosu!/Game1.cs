using Microsoft.Xna.Framework.Input;
using nosu;
using OsuParsers.Beatmaps.Objects;
using System.Collections;
using NetCoreAudio;
using System.Diagnostics.Metrics;

namespace nosu_;

public class Game1 : Game
{
    int currentBeatmap = 0;
    bool hit = false;
    int nextObject = 0;
    int currentDiff = 0;
    const double selectCooldown = 0.2;
    double prevPress = 0;
    double prevSelect;
    HitObject currentHitObject;
    double beatmapStart;
    bool initialized = false;
    bool finished = false;
    int score = 0;

    List<HitObject> hitObjects;
    List<string> beatmapLocations;
    List<BeatmapFiles> beatmaps;
    Vector2 mousePos;
    Vector2 approachCircleSize = new Vector2(0.5f, 0.5f);
    Vector2 approachCirclePos = new Vector2(0,0);
    Vector2 approachCircleSizeStart;

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
    Texture2D approachCircle;
    Player player = new();
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false; //To use custom "circle" cursor found in osu!
        _graphics.PreferredBackBufferWidth = 640; 
        _graphics.PreferredBackBufferHeight = 480;
        _graphics.IsFullScreen = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        beatmapLocations = BeatmapFiles.GetBeatmaps();
        beatmaps = new();
        for (int i = 0; i < beatmapLocations.Count; i++)
        {
            Console.WriteLine(beatmapLocations);
            BeatmapFiles beatmap = new BeatmapFiles(BeatmapFiles.GetBeatmaps()[i]);
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
        approachCircle = Content.Load<Texture2D>("approachcircle");
        beatmapName = Content.Load<SpriteFont>("BeatmapName");
        nextPageL = Content.Load<Texture2D>("nextPg");
        nextPageR = Content.Load<Texture2D>("nextPg");
        cursor = Content.Load<Texture2D>("cursor");
        hitCircle = Content.Load<Texture2D>("hitcircle");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            if (gameState == GameState.Gameplay )//&& (gameTime.TotalGameTime.TotalSeconds - prevPress) >= selectCooldown) //Switch statement? I barely know her!
            {
                finished = true;
                initialized = false;
                prevPress = gameTime.TotalGameTime.TotalSeconds;
            }
            else if (gameState == GameState.DiffSelect && (gameTime.TotalGameTime.TotalSeconds - prevPress) >= selectCooldown)
            {
                gameState = GameState.SongSelect;
                prevPress = gameTime.TotalGameTime.TotalSeconds;
            }
            else if ((gameTime.TotalGameTime.TotalSeconds - prevPress) >= selectCooldown)
            {
                Exit();
            }
        }
        switch (gameState)
        {
            case GameState.SongSelect:

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && currentBeatmap + 1 < beatmaps.Count && (gameTime.TotalGameTime.TotalSeconds - prevSelect) >= selectCooldown)
                {
                    currentBeatmap++;
                    prevSelect = gameTime.TotalGameTime.TotalSeconds;
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Left) && currentBeatmap - 1 >= 0 && (gameTime.TotalGameTime.TotalSeconds - prevSelect) >= selectCooldown)
                {
                    currentBeatmap--;
                    prevSelect = gameTime.TotalGameTime.TotalSeconds;
                }

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

                if (finished)
                {
                    gameState = GameState.SongSelect; 
                }

                if (!initialized)
                {
                    finished = false;

                    beatmapStart = gameTime.TotalGameTime.TotalMilliseconds; //TODO: Should not be in here, gets constantly reassigned

                    hitObjects = beatmaps[currentBeatmap].difficulties[0].HitObjects; //TODO: Should not be in here, gets constantly reassigned

                    nextObject = 0;

                    currentHitObject = hitObjects[nextObject];

                    initialized = true;
                }

                //if(beatmapStart == currentHitObject.StartTime) {

                //}
                //else
                //{
                //nextObject++;
                //}

                currentHitObject = hitObjects[nextObject];


                if (approachCircleSize.X > 0.5)
                {
                Console.WriteLine((float)((approachCircleSizeStart.X - 0.5) * (currentHitObject.StartTime / (gameTime.TotalGameTime.TotalMilliseconds - beatmapStart)) / 2 * -1));
                approachCircleSize.X -= (float)((approachCircleSizeStart.X - 0.5) / (currentHitObject.StartTime - (gameTime.TotalGameTime.TotalMilliseconds - beatmapStart)));
                approachCircleSize.Y = approachCircleSize.X; 
                //approachCircleSize.X -= 0.01f;
                //approachCircleSize.Y = approachCircleSize.X;

                //Console.WriteLine(approachCircleSize);
                }
                else
                {
                approachCircleSize.X = 0.5f;
                approachCircleSize.Y = approachCircleSize.X;
                }

                //Thread.Sleep(1000);

                if (nextObject + 1 <= hitObjects.Count - 1)
                {
                    if (currentHitObject.StartTime! <= gameTime.TotalGameTime.TotalMilliseconds - beatmapStart)
                    {
                        nextObject++;
                        hit = false;

                        approachCircleSizeStart = new Vector2((float)((approachCircleSizeStart.X - 0.5) * (currentHitObject.StartTime / gameTime.TotalGameTime.TotalMilliseconds - beatmapStart)));
                        
                        //approachCircleSize = new Vector2(2);

                    }
                }
                else
                {
                    finished = true;
                    initialized = false;
                }

                //Console.WriteLine(gameTime.TotalGameTime.TotalMilliseconds - beatmapStart);
                /*foreach(HitObject hitObject in hitObjects) {
                    Console.WriteLine(hitObject.x.ToString(), hitObject.y.ToString(), hitObject.time.ToString());
                }*/

                //player.Play(Path.GetFullPath(beatmaps[currentBeatmap].difficulties[currentDiff].GeneralSection.AudioFilename));

                //Console.WriteLine(beatmaps[currentBeatmap].fullPath + (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]);

                try
                {
                    if (!player.Playing) //TODO: make it work realrealrealrealrealrealreal
                    {
                        //Console.WriteLine(Path.GetFullPath(beatmaps[currentBeatmap].difficulties[0].GeneralSection.AudioFilename)); //C# hates absoulte file paths apparently

                        //player.Play(Path.GetFullPath((string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]));
                        
                        //player.Play(Path.GetFullPath(@"/Users/aeroglory/Getter\ Jaani\ -\ Rockefeller\ Street\ \(Nightcore\ Mix\).mp3"));
                        //Console.WriteLine(Path.GetFullPath(@"/Users/aeroglory/Getter\ Jaani\ -\ Rockefeller\ Street\ \(Nightcore\ Mix\).mp3")); 
                        //player.Play(beatmaps[currentBeatmap].fullPath + (string)((Hashtable)beatmaps[currentBeatmap].difficulties[0]["[General]"])["AudioFilename"]);
                    }
                    else
                    {
                        //Console.WriteLine("Already playing.");
                    }
                }
                catch //It's a await function idiot 
                {
                    gameState = GameState.SongSelect;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Z) || Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    prevPress = gameTime.TotalGameTime.TotalMilliseconds - beatmapStart;

                    if (prevPress >= (currentHitObject.StartTime + 100) || prevPress <= (currentHitObject.StartTime - 100) && hit == false) {

                        Console.WriteLine("PrevPress = " + prevPress + "      |      HitObjectStart = " + currentHitObject.StartTime);

                        hit = true;
                        score += 300;
                    }
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
                _spriteBatch.DrawString(beatmapName, beatmaps[currentBeatmap].difficulties[0].MetadataSection.Title,
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - (beatmapName.MeasureString(beatmaps[currentBeatmap].difficulties[0].MetadataSection.Title).X / 2f), _graphics.PreferredBackBufferHeight / 2.3f),
                    Color.Black);

                //Page buttons
                _spriteBatch.Draw(nextPageL, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(nextPageR, new Vector2(_graphics.PreferredBackBufferWidth - nextPageR.Width / 8.8f, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.None, 0f); //Why the frick is this divided by cursor.Width???????????????????????????????

                //Cursor
                _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                _spriteBatch.End();

                base.Draw(gameTime);
                break;

            case GameState.DiffSelect:
                GraphicsDevice.Clear(Color.LightPink);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(beatmapName, beatmaps[currentBeatmap].difficulties[currentDiff].MetadataSection.Version,
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - (beatmapName.MeasureString(beatmaps[currentBeatmap].difficulties[currentDiff].MetadataSection.Version).X / 2f), _graphics.PreferredBackBufferHeight / 2.3f),
                    Color.Black);

                //Page buttons
                _spriteBatch.Draw(nextPageL, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(nextPageR, new Vector2(_graphics.PreferredBackBufferWidth - nextPageR.Width / 8.8f, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(0.12f, 0.16f), SpriteEffects.None, 0f); //Why the frick is this divided by cursor.Width???????????????????????????????

                //Cursor
                _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                _spriteBatch.End();

                break;

            case GameState.Gameplay:
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                if (initialized)
                {
                    _spriteBatch.Draw(hitCircle, currentHitObject.Position, null, Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    _spriteBatch.Draw(approachCircle, currentHitObject.Position, null, Color.White, 0f, approachCirclePos, approachCircleSize, SpriteEffects.None, 0f);
                }

                _spriteBatch.Draw(cursor, mousePos, null, Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                _spriteBatch.End();
                break;
        }
    }
}