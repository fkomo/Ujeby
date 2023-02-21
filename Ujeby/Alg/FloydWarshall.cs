namespace Ujeby.Alg
{
	public class FloydWarshall
	{
		/// <summary>
		/// computes shortest path between every vertex pair
		/// 
		/// https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm
		/// </summary>
		/// <param name="dist">distances between places: NxN</param>
		/// <returns></returns>
		public static int[,] ShortestPath(int[,] dist)
		{
			if (dist.GetLength(0) != dist.GetLength(1))
				throw new Exception($"dist[,] must be square!");

			var size = dist.GetLength(0);
			for (var through = 0; through < size; through++)
			{
				for (var from = 0; from < size; from++)
				{
					for (var to = 0; to < size; to++)
					{
						if (dist[from, to] > dist[from, through] + dist[through, to])
							dist[from, to] = dist[from, through] + dist[through, to];
					}
				}
			}

			return dist;
		}
	}
}
