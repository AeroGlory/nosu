using System;
namespace nosu
{
	public class HitObject
	{
		public HitObject(string hitObject)
		{
			try
			{
				int x = int.Parse(hitObject.Split(',')[0]);
				int y = int.Parse(hitObject.Split(',')[1]);
				int time = int.Parse(hitObject.Split(',')[2]);
				int type = int.Parse(hitObject.Split(',')[3]); //TODO: This is actually a bitflag, but I'm too lazy to figure those out right now, so come back to this
				//TODO: Implement all aspects of HitObjects and add sliders

				//Console.Write($"X: {x}\nY: {y}\nTime: {time}\nType: {type}");
			}
			catch
			{
				Console.WriteLine("I can't get enough. Back with another milkshake. HELP! HELP! HELP ME! HEL-");
			}
        }
	}
}

