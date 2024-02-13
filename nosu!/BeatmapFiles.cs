using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;

namespace nosu
{
    public class BeatmapFiles
    {
        public List<string> assets;
        public List<Beatmap> difficulties = new();
        private Beatmap newBeatmap;
        public string fullPath; 
        public BeatmapFiles(string filePath)
        {
            Console.WriteLine(filePath + Path.GetExtension(filePath));
            if (Path.GetExtension(filePath) == ".osz")
            {
                try
                {
                    
                    string folderPath = @"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps/" + filePath.Split('/')[filePath.Split('/').Length - 1].Split('.')[0];  //Splits the entire file path to create a folder named after the name of the .osz file
                    Directory.CreateDirectory(folderPath);
                    System.IO.Compression.ZipFile.ExtractToDirectory(filePath, folderPath);
                    File.Delete(filePath);
                    filePath = folderPath;
                    fullPath = filePath; //TODO: Broken still :3
                }
                catch
                {
                    Console.WriteLine("Uh-oh, our code, it's broken. A"); //TODO: Make these throw actually errors to be caught in Game1.cs
                }
            }
            try
            {
                assets = Directory.GetFiles(filePath).ToList();
                for (int i = 0; i < assets.Count; i++)
                {
                    if (Path.GetExtension(assets[i]) == ".osu")
                    {
                        Console.WriteLine("Retrieving beatmap data");

                        newBeatmap = BeatmapDecoder.Decode(assets[i]);

                        difficulties.Add(newBeatmap);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Uh-oh, our code, it's broken. B"); //TODO: Make these throw actually errors to be caught in Game1.cs
            }
        }

        public static List<string> GetBeatmaps() //Returns file path of all files (.osz compressed beatmaps) and folders (extracted, usable beatmap files) in the beatmaps folder
        {
            List<string> beatmaps = Directory.GetFiles(@"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps").ToList();
            beatmaps.AddRange(Directory.GetDirectories(@"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps").ToList());
            return beatmaps;
        }

        /*private Hashtable GetDiffInfo(string filePath) //TODO: Parse all beatmap data (as outlined here: https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29)
        {
            Hashtable diffData = new();
            Hashtable values = new();
            string[] sections;
            List<string> data;
            List<HitObject> hitObjects = new();

            Path.ChangeExtension(filePath, ".txt");

            sections = Regex.Matches(File.ReadAllText(filePath), @"\[[^\]]*\]").Cast<Match>().Select(m => m.Value).ToArray(); //LINQ /neg

            data = Regex.Split(File.ReadAllText(filePath), @"\[[^\]]*\]").ToList();
            data.RemoveAt(0);

            string a;
            string b;

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i] != "[Events]" && sections[i] != "[TimingPoints]" && sections[i] != "[HitObjects]") 
                {
                    for(int l = 0; l < data[i].Split("\n").Length; l++)
                    {
                        try
                        {
                            a = data[i].Split("\n")[l].Split(':')[0];//AAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHH
                            b = data[i].Split("\n")[l].Split(':')[1];//AAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHH
                        }
                        catch
                        {
                            //For whitespace or something else that causes the Split() to not work
                            
                            continue;
                        }
                                                 
                        //Console.WriteLine(a + "|" + b);
                        values.Add(a, b); 
                    }

                    diffData.Add(sections[i], values);
                }
                else
                {
                    if (sections[i] == "[HitObjects]")
                    {
                        for (int l = 0; l < data[i].Split("\n").Length; l++)
                        {
                            hitObjects.Add(new HitObject(data[i].Split("\n")[i]));
                        }

                        diffData.Add(sections[i], hitObjects);
                    }
                    else
                    {
                        Console.WriteLine("Not yet implemented!");
                    }
                }
            }

            Path.ChangeExtension(filePath, ".osu");

            return diffData;
        }

        */
        public static bool initGameplay(BeatmapFiles beatmap)
        {
            

            return false;
        }

    }
}

