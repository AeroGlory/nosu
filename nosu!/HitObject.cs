using System;
namespace nosu
{
	public class HitObject
	{
		public HitObject(string hitObject)
		{


			public int x = 0;
			public int y = 0;
			public int time = 0;
			public int type = 0;	


			x = int.Parse(hitObject.Split(',')[0]);
			y = int.Parse(hitObject.Split(',')[1]);
			time = int.Parse(hitObject.Split(',')[2]);
			type = int.Parse(hitObject.Split(',')[3]); //TODO: This is actually a bitflag, but I'm too lazy to figure those out right now, so come back to this
			//TODO: Implement all aspects of HitObjects and add sliders
			
			Console.WriteLine("Whoops");
		}
	}
}
		
 
