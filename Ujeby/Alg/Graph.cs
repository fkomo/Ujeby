namespace Ujeby.Alg
{
	public static class Graph
	{
		/// <summary>
		/// when graph contains only from -> to connections, this will fix it, so also reverse (to -> from) connections are present
		/// </summary>
		/// <param name="graph">1way graph</param>
		/// <returns>2way graph</returns>
		public static Dictionary<int, (int Dest, long Weight)[]> Fix1WayAs2Way(this Dictionary<int, (int Dest, long Weight)[]> graph)
		{
			var fix = new Dictionary<int, HashSet<(int, long)>>();
			foreach (var from in graph)
			{
				if (!fix.ContainsKey(from.Key))
					fix.Add(from.Key, new());

				foreach (var connection in from.Value)
				{
					fix[from.Key].Add((connection.Dest, connection.Weight));

					if (!fix.ContainsKey(connection.Dest))
						fix.Add(connection.Dest, new());

					fix[connection.Dest].Add((from.Key, connection.Weight));
				}
			}

			return fix.ToDictionary(x => x.Key, x => x.Value.ToArray());
		}

		public static long CountReach(this Dictionary<int, (int Dest, long Weight)[]> graph, int source)
		{
			var counted = new HashSet<int>();

			var nodeQueue = new Queue<int>();
			nodeQueue.Enqueue(source);
			while (nodeQueue.Any())
			{
				var node = nodeQueue.Dequeue();
				if (!counted.Add(node))
					continue;

				foreach (var (Dest, Weight) in graph[node].Where(x => !counted.Contains(x.Dest)))
					nodeQueue.Enqueue(Dest);
			}

			return counted.Count;
		}
	}
}
