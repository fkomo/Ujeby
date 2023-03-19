using Ujeby.Vectors;

namespace Ujeby.Alg
{
	/// <summary>
	/// finds shortest path between start and every other vertex
	/// 
	/// https://en.wikipedia.org/wiki/Breadth-first_search
	/// </summary>
	public class BreadthFirstSearch
	{
		public readonly v2i Size;
		public readonly v2i Start;

		private readonly int[,] _map;
		private readonly Func<v2i, v2i, int[,], bool> _connectionCheck;

		public bool[,] Visited { get; private set; }

		private Queue<v2i> _queue;
		private v2i _p;

		public v2i?[,] Prev { get; private set; }

		public BreadthFirstSearch(int[,] map, v2i start, Func<v2i, v2i, int[,], bool> connectionCheck)
		{
			Size = new v2i(map.GetLength(1), map.GetLength(0));
			Start = start;

			_map = map;
			_connectionCheck = connectionCheck;

			Visited = new bool[Size.Y, Size.X];
			_queue = new Queue<v2i>();

			Prev = new v2i?[Size.Y, Size.X];

			Visited[Start.Y, Start.X] = true;

			_p = Start;
		}

		public bool Step()
		{
			if (_queue.Any())
				_p = _queue.Dequeue();

			var rightDownLeftUpLength = v2i.RightDownLeftUp.Length;
			for (var vDir = 0; vDir < rightDownLeftUpLength; vDir++)
			{
				var p1 = _p + v2i.RightDownLeftUp[vDir];

				// visited and border check
				if (p1.X < 0 || p1.Y < 0 || p1.X == Size.X || p1.Y == Size.Y || Visited[p1.Y, p1.X])
					continue;

				if (_connectionCheck(p1, _p, _map) == false)
					continue;

				Visited[p1.Y, p1.X] = true;
				_queue.Enqueue(p1);
				Prev[p1.Y, p1.X] = _p;
			}

			if (_queue.Count <= 0)
				return false;

			return true;
		}

		public v2i[] Path(v2i end)
		{
			var path = new List<v2i>();
			var p = end;
			while (p.X != Start.X || p.Y != Start.Y)
			{
				path.Add(p);

				if (!Prev[p.Y, p.X].HasValue)
					return null;

				p = Prev[p.Y, p.X].Value;
			}

			path.Reverse();
			return path.ToArray();
		}

		public void StepFull()
		{
			while (Step())
			{
			}
		}
	}
}
