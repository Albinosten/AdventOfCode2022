using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AdventOfCode2022
{
	public class Puzzle19 : IPuzzle
	{
        private IList<string> allLines;
        public Puzzle19()
        {
            this.allLines = File.ReadAllLines(source);
        }
		public static string source => "input/EXAMPLE.txt";
		// public static string source => "input/Puzzle19.txt";
        public int FirstResult => 0;
        public long SecondResult => 0;

		public int Solve() 
		{
			var blueprints = new List<Blueprint>();
			foreach(var line in allLines)
			{
				//1,6,12,18,21,27,30
				var a = line.Split(' ');
				var blueprint = new Blueprint()
				{
					Number = int.Parse(a[1].Split(':').First()),
					OreRobotCost = int.Parse(a[6]),
					ClayRobotCost = int.Parse(a[12]),
					ObsidianRobotCost = (int.Parse(a[18]),int.Parse(a[21])),
					GeodeRobotCost = (int.Parse(a[27]),int.Parse(a[30])),
				};
				blueprints.Add(blueprint);
			}

			return this.getScoreFromBlueprint(blueprints[1]);
			// return blueprints.AsParallel().Sum(x => this.getScoreFromBlueprint(x) * x.Number);
		}

		private int getScoreFromBlueprint(Blueprint blueprint)
		{
			var map = new RoadMap();
			int maxScore = 0;
			var resources = new Resources();
			var machines = new Machines();
			while(map.HasNextMove)
			{
				var nextMove = map.GetNextMove();
				for(int i = 0; i < 24; i++)
				{
					resources.Ores += machines.OreRobots;
					resources.Clay += machines.ClayRobot;
					resources.Obsidian += machines.ObsidianRobot;
					resources.Geodes += machines.GeodeRobot;
					if(this.CanBuild(blueprint, nextMove, resources) && i < 23)
					{
						this.Build(blueprint, nextMove, resources, machines);
						nextMove = map.GetNextMove();
					}
					if(resources.Geodes > maxScore)
					{
						maxScore = resources.Geodes;
					}
				}
				resources = new Resources();
				machines = new Machines();
				map.Done();
			}

			return maxScore;
		}
		/*
	Blueprint 1:
  Each ore robot costs 4 ore.
  Each clay robot costs 2 ore.
  Each obsidian robot costs 3 ore and 14 clay.
  Each geode robot costs 2 ore and 7 obsidian.
*/
		private bool CanBuild(Blueprint blueprint, Choice nextMove, Resources recources)
		{
			switch (nextMove)
			{
				case Choice.OreRobot:
					if( blueprint.OreRobotCost <= recources.Ores) 
					{
						return true;
					}
					return false;
				case Choice.ClayRobot:
					if(blueprint.ClayRobotCost <= recources.Ores) 
					{
						return true;
					}
					return false;
				case Choice.ObsidianRobot:
					if(blueprint.ObsidianRobotCost.clay <= recources.Clay && blueprint.ObsidianRobotCost.ore <= recources.Ores)
					{
						return true;
					}
					return false;
				case Choice.GeodeRobot:
					if(blueprint.GeodeRobotCost.ore <= recources.Ores && blueprint.GeodeRobotCost.obsidian <= recources.Obsidian) 
					{
						return true;
					}
					return false;
				default:
					return false;
			}
		}
					/*
	Blueprint 1:
  Each ore robot costs 4 ore.
  Each clay robot costs 2 ore.
  Each obsidian robot costs 3 ore and 14 clay.
  Each geode robot costs 2 ore and 7 obsidian.
*/
		private void Build(Blueprint blueprint, Choice nextMove, Resources recources, Machines machines)
		{
			switch (nextMove)
			{
				case Choice.OreRobot:
					recources.Ores =- blueprint.OreRobotCost;
					machines.OreRobots++;
					break;
				case Choice.ClayRobot:
					recources.Ores =- blueprint.ClayRobotCost;
					machines.ClayRobot++;
				break;
				case Choice.ObsidianRobot:
					recources.Ores =- blueprint.ObsidianRobotCost.ore;
					recources.Clay =- blueprint.ObsidianRobotCost.clay;
					machines.ObsidianRobot++;
				break;
				case Choice.GeodeRobot:
					recources.Ores =- blueprint.GeodeRobotCost.ore;
					recources.Obsidian =- blueprint.GeodeRobotCost.obsidian;
					machines.GeodeRobot++;
				break;
				default:
					break;
			}
		}
        public long SolveNext()
        {
            return 0;
        }
	}

	public class Machines
	{
		public Machines()
		{
			OreRobots = 1;
		}
		public int OreRobots;
		public int ClayRobot;
		public int ObsidianRobot;
		public int GeodeRobot;
	}
	public class Resources
	{
		public int Ores;
		public int Clay;
		public int Obsidian;
		public int Geodes;
	}
	public class RoadMap
	{
		public RoadMap()
		{
			this.Path = new List<Choice>();
			this.index = 0;
			this.HasNextMove = true;

		}
		public bool HasNextMove;
		public Choice GetNextMove()
		{
			index++;

			if(this.Path.Count < index)
			{
				this.Path.Add(Choice.OreRobot);
			}
			return this.Path[index-1];
		}
		public int NumberOfTries;
		public void Done()
		{
			this.NumberOfTries++;
			if(this.Path.All(x => x == Choice.GeodeRobot))
			{
				this.HasNextMove = false;
			}

			this.Done(this.Path.Count-1);


			this.index = 0;

		}

		private void Done(int i)
		{
			this.Path[i]++;
			if((int)this.Path[i] == 4)
			{
				this.Path[i] = Choice.OreRobot;
				if(i>0)
				{
					this.Done(i-1);
				}
			}
		}
		int index;
		List<Choice> Path;

	}
	public enum Choice
	{
		OreRobot = 0,
		ClayRobot = 1,
		ObsidianRobot = 2,
		GeodeRobot = 3,
	}
	public class Blueprint
	{
		public int Number;
		public int OreRobotCost;
		public int ClayRobotCost;
		public (int ore,int clay) ObsidianRobotCost;
		public (int ore,int obsidian) GeodeRobotCost;
	}
}
