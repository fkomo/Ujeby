using Ujeby.Vectors;

namespace Ujeby.Alg
{
	/// <summary>
	/// finds shortest path between 2 vertices in weighted graph
	/// 
	/// https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
	/// </summary>
	public class Dijkstra
	{
		private readonly int[,] _weights;
		private readonly v2i Start;
		private readonly v2i End;

		public readonly v2i Size;

		public int[,] Prev { get; private set; }
		public long[,] Dist { get; private set; }
		public bool[,] Visited { get; private set; }

		private List<v2i> _unvisited;

		public Dijkstra(int[,] weights, v2i start, v2i end)
		{
			_weights = weights;
			Start = start;
			End = end;
			Size = new v2i(_weights.GetLength(1), _weights.GetLength(0));

			Prev = new int[Size.Y, Size.X]; // 0, 1, 2, 3 (RightDownLeftUp)
			Dist = new long[Size.Y, Size.X];

			Visited = new bool[Size.Y, Size.X];
			_unvisited = new List<v2i>();

			for (int i = 0, y = 0; y < Size.Y; y++)
				for (var x = 0; x < Size.X; x++, i++)
				{
					Dist[y, x] = long.MaxValue;
					Prev[y, x] = -1;

					_unvisited.Add(new(x, y));
				}

			Dist[Start.Y, Start.X] = 0;
		}

		public bool Step()
		{
			if (!_unvisited.Any())
				return false;

			var uMinIdx = -1;
			var uMinDist = long.MaxValue;
			for (var uIdx = 0; uIdx < _unvisited.Count; uIdx++)
			{
				var qq = _unvisited[uIdx];
				if (Dist[qq.Y, qq.X] < uMinDist)
				{
					uMinIdx = uIdx;
					uMinDist = Dist[qq.Y, qq.X];
				}
			}
			var u = _unvisited[uMinIdx];
			_unvisited.RemoveAt(uMinIdx);
			Visited[u.Y, u.X] = true;

			//if (u == End)
			//	return false;

			var rightDownLeftUpLength = v2i.RightDownLeftUp.Length;
			for (var vDir = 0; vDir < rightDownLeftUpLength; vDir++)
			{
				var v = u + v2i.RightDownLeftUp[vDir];

				// visited or out of bounds
				if (v.X < 0 || v.Y < 0 || v.X == Size.X || v.Y == Size.Y || Visited[v.Y, v.X])
					continue;

				var d = Dist[u.Y, u.X] + _weights[v.Y, v.X];
				if (d < Dist[v.Y, v.X])
				{
					Dist[v.Y, v.X] = d;
					Prev[v.Y, v.X] = (vDir + 2) % rightDownLeftUpLength;
				}
			}

			return true;
		}

		public void StepFull()
		{
			while (Step())
			{
			}
		}

		public v2i[] Path()
		{
			List<v2i> path = new()
			{
				End
			};

			while (path.Last() != Start)
				path.Add(path.Last() + v2i.RightDownLeftUp[Prev[path.Last().Y, path.Last().X]]);

			path.Reverse();
			return path.ToArray();
		}
	}
}
