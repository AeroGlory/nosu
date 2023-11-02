using System;
using System.Text.RegularExpressions;

namespace nosu
{
	public class Beatmap
	{
        public List<string> assets;
        public List<string> difficulties;
        public List<string> catagories;
        public Beatmap(string filePath)
        {
            Console.WriteLine(filePath + Path.GetExtension(filePath));
            if (Path.GetExtension(filePath) == ".osz")
            {
                try
                {
                    Directory.CreateDirectory(@"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps/" + filePath.Split('/')[filePath.Split('/').Length - 1].Split('.')[0]);   
                    System.IO.Compression.ZipFile.ExtractToDirectory(filePath, @"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps/" + filePath.Split('/')[filePath.Split('/').Length - 1].Split('.')[0]); //Splits the entire file path to create a folder named after the name of the .osz file
                    File.Delete(filePath);
                }
                catch
                {
                    Console.WriteLine("Uh-oh, it's broken.");
                }
            }
            else
            {
                try
                {
                    assets = Directory.GetFiles(filePath).ToList();
                    for (int i = 0; i < assets.Count; i++) {
                        if(Path.GetExtension(filePath) == ".osu")
                        {
                            difficulties.Add(assets[i]);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Uh-oh, it's broken.");
                }
            }


            //catagories = Regex.Split(file, @"\[[^\]]*\]").ToList();
        }

        public static List<string> GetBeatmaps() //Returns file path of all files (.osz compressed beatmaps) and folders (extracted, usable beatmap files) in the beatmaps folder
        {
            List<string> beatmaps = Directory.GetFiles(@"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps").ToList();
            beatmaps.AddRange(Directory.GetDirectories(@"/Users/aeroglory/Projects/nosu!/nosu!/Beatmaps").ToList());
            return beatmaps;
        }
    }
}

