using Ujeby.Vectors;

namespace Ujeby.Grid
{
	public class CharMap
	{
		/// <summary>
		/// fill empty space with specified tile
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="fillTile"></param>
		public static void FloodFill(char[][] map, v2i start, char fillTile,
			char emptyTile = '.')
		{
			if (start.X < 0 || start.Y < 0 || start.X == map[0].Length || start.Y == map.Length)
				return;

			if (map[start.Y][(int)start.X] == fillTile || map[start.Y][(int)start.X] != emptyTile)
				return;

			map[start.Y][(int)start.X] = fillTile;
			foreach (var near in v2i.PlusMinusOne)
				FloodFill(map, start + near, fillTile);
		}

		public static void FloodFillNonRec(char[][] map, v2i start, char fillTile,
			char emptyTile = '.')
		{
			var queue = new Queue<v2i>();
			queue.Enqueue(start);
			while (queue.Count > 0)
			{
				var p = queue.Dequeue();

				if (p.X < 0 || p.Y < 0 || p.Y >= map.Length || p.X >= map[p.Y].Length)
					continue;

				if (map[p.Y][(int)p.X] == fillTile || map[p.Y][(int)p.X] != emptyTile)
					continue;

				map[p.Y][(int)p.X] = fillTile;
				foreach (var near in v2i.PlusMinusOne)
					queue.Enqueue(p + near);
			}
		}
	}
}
