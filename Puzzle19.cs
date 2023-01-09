using System;
using System.Collections.Generic;
using System.Linq;
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
		public static string source => "input/Puzzle19.txt";
        public int FirstResult => 1653;
        public long SecondResult => 4212; //took 8:41...

		public int Solve() 
		{
			return this.GetBluePrints()
				.AsParallel()
				.Sum(x => this.GetScoreFromBlueprint(x, 24) * x.Number);
		}
    	public long SolveNext()
        {
			return this.GetBluePrints()
				.Take(3)
				.AsParallel()
				.Select(x => this.GetScoreFromBlueprint(x, 32))
				.Multiply();
        }
	
		private int GetScoreFromBlueprint(Blueprint blueprint, int time)
		{
			int totalRuns = 0;
			var maxScore = 0;

			var q = new Stack<(Resources resources, Machines machines, int time, Choice move)>();
			foreach(Choice move in (Choice[])  Enum.GetValues(typeof(Choice)))
			{
				q.Push((new Resources(), new Machines(), 1, move));
			}
			while(q.Any())
			{
				totalRuns++;
				var v = q.Pop();
				if(v.time > time)
				{
					continue;
				}
				
				if(this.CanBuild(blueprint, v.move, v.resources))
				{
					this.StartBuild(blueprint, v.move, v.resources);
					this.AddResources(v.resources, v.machines);
					this.DoneBuild(v.move, v.machines);

					foreach(Choice move in Enum.GetValues(typeof(Choice))
						.Cast<Choice>()
						.Where(x => Include(x, blueprint, v.machines))
						)
					{
						q.Push((v.resources.Clone(), v.machines.Clone(), v.time + 1, move));
					}
				}
				else
				{
					this.AddResources(v.resources, v.machines);
					q.Push((v.resources, v.machines, v.time + 1, v.move));
				}
				if(v.resources.Geodes > maxScore)
				{
					maxScore = v.resources.Geodes;
				}
			}
			return maxScore;
		}
		private void AddResources(Resources r, Machines m)
		{
			r.Ores += m.OreRobots;
			r.Clay += m.ClayRobot;
			r.Obsidian += m.ObsidianRobot;
			r.Geodes += m.GeodeRobot;
		}
		private bool Include(Choice move, Blueprint b, Machines machines) => move switch
		{
			Choice.OreRobot => new int[] { b.OreRobotCost, b.ClayRobotCost, b.ObsidianRobotCost.ore, b.GeodeRobotCost.ore }.Max() >= machines.OreRobots,
			Choice.ClayRobot => b.ObsidianRobotCost.clay >= machines.ClayRobot,
			Choice.ObsidianRobot => b.GeodeRobotCost.obsidian >= machines.ObsidianRobot,
			Choice.GeodeRobot => true,
			_ => false,
		};
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
		private void StartBuild(Blueprint blueprint, Choice nextMove, Resources recources)
		{
			switch (nextMove)
			{
				case Choice.OreRobot:
					recources.Ores -= blueprint.OreRobotCost;
					break;
				case Choice.ClayRobot:
					recources.Ores -= blueprint.ClayRobotCost;
				break;
				case Choice.ObsidianRobot:
					recources.Ores -= blueprint.ObsidianRobotCost.ore;
					recources.Clay -= blueprint.ObsidianRobotCost.clay;
				break;
				case Choice.GeodeRobot:
					recources.Ores -= blueprint.GeodeRobotCost.ore;
					recources.Obsidian -= blueprint.GeodeRobotCost.obsidian;
				break;
				default:
					break;
			}
		}
		private void DoneBuild(Choice nextMove, Machines machines)
		{
			switch (nextMove)
			{
				case Choice.OreRobot:
					machines.OreRobots++;
					break;
				case Choice.ClayRobot:
					machines.ClayRobot++;
				break;
				case Choice.ObsidianRobot:
					machines.ObsidianRobot++;
				break;
				case Choice.GeodeRobot:
					machines.GeodeRobot++;
				break;
				default:
					break;
			}
		}
    
		public IEnumerable<Blueprint> GetBluePrints()
		{
			foreach(var line in this.allLines)
			{
				var a = line.Split(' ');
				
				yield return new Blueprint()
				{
					Number = int.Parse(a[1].Split(':').First()),
					OreRobotCost = int.Parse(a[6]),
					ClayRobotCost = int.Parse(a[12]),
					ObsidianRobotCost = (int.Parse(a[18]),int.Parse(a[21])),
					GeodeRobotCost = (int.Parse(a[27]),int.Parse(a[30])),
				};
			}
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
		public Machines Clone()
		{
			return new Machines()
			{
				OreRobots = this.OreRobots,
				ClayRobot = this.ClayRobot,
				ObsidianRobot = this.ObsidianRobot,
				GeodeRobot = this.GeodeRobot,
			};
		}
	}
	public class Resources
	{
		public int Ores;
		public int Clay;
		public int Obsidian;
		public int Geodes;
		public Resources Clone()
		{
			return new Resources()
			{
				Ores = this.Ores,
				Clay = this.Clay,
				Obsidian = this.Obsidian,
				Geodes = this.Geodes,
			};
		}
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
