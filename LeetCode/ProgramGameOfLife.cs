using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
	public static class ProgramGameOfLife
	{
		private class Life
		{
			private static readonly (int x, int y)[] _neighbors = new (int x, int y)[] 
			{
				(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)
			};

			private class CellsEnumerable : IReadOnlySet<(int x, int y)>
			{
				public int Count => _cells.Count;

				private readonly HashSet<(int x, int y)> _cells;

				public CellsEnumerable(HashSet<(int x, int y)> cells) => _cells = cells;

				IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
				public IEnumerator<(int x, int y)> GetEnumerator() => GetEnumerator();

				public bool Contains((int x, int y) item) => _cells.Contains(item);

				public bool IsProperSubsetOf(IEnumerable<(int x, int y)> other) => _cells.IsProperSubsetOf(other);

				public bool IsProperSupersetOf(IEnumerable<(int x, int y)> other) => _cells.IsProperSupersetOf(other);

				public bool IsSubsetOf(IEnumerable<(int x, int y)> other) => _cells.IsSubsetOf(other);

				public bool IsSupersetOf(IEnumerable<(int x, int y)> other) => _cells.IsSupersetOf(other);

				public bool Overlaps(IEnumerable<(int x, int y)> other) => _cells.Overlaps(other);

				public bool SetEquals(IEnumerable<(int x, int y)> other) => _cells.SetEquals(other);
			}

			private class CellComparer : EqualityComparer<(int x, int y)>
			{
				public static readonly CellComparer comparer = new();

				private CellComparer() { }

				public override bool Equals((int x, int y) x, (int x, int y) y) => x.x == y.x && x.y == y.y;

				public override int GetHashCode((int x, int y) obj) => obj.x ^ obj.y;
			}

			public IReadOnlySet<(int x, int y)> Cells { get; }

			private readonly HashSet<(int x, int y)> _cells = new(CellComparer.comparer);
			private readonly Dictionary<(int x, int y), bool> _changes = new(CellComparer.comparer);

			public Life(IEnumerable<(int x, int y)> cells)
			{
				// Set cells
				Set(cells);
				// Set cells
				Cells = new CellsEnumerable(_cells);
			}

			public void Set(IEnumerable<(int x, int y)> cells)
			{
				// Clear cells
				_cells.Clear();
				// Check if cells exists
				if (cells != null)
				{
					// Run through cells
					foreach (var cell in cells)
					{
						// Add cell
						_cells.Add(cell);
					}
				}
			}

			public void Mutate()
			{
				// Run through cells
				foreach (var (x, y) in _cells)
				{
					// Capture change
					_CaptureChange(x, y);
					// Run through neighbors
					for (int i = 0; i < _neighbors.Length; i++)
					{
						// Get offset
						var (xOffset, yOffset) = _neighbors[i];
						// Capture change
						_CaptureChange(x + xOffset, y + yOffset);
					}
				}
				// Run through changes
				foreach (var (cell, added) in _changes)
				{
					// Check if added
					if (added)
					{
						// Add cell
						_cells.Add(cell);
					}
					else
					{
						// Remove cell
						_cells.Remove(cell);
					}
				}
				// Clear changes
				_changes.Clear();
			}

			private void _CaptureChange(int x, int y)
			{
				// Create neighbors
				var neighbors = 0;
				// Run through neighbors
				for (int i = 0; i < _neighbors.Length; i++)
				{
					// Get offset
					var (xOffset, yOffset) = _neighbors[i];
					// Check if neighbor exists
					if (_cells.Contains((x + xOffset, y + yOffset)))
					{
						// Increase neighbors
						neighbors++;
					}
				}
				// Get cell
				var cell = (x, y);
				// Check if cell exists
				if (_cells.Contains(cell))
				{
					// Check neighbors
					if (neighbors != 2 && neighbors != 3)
					{
						// Add change to remove cell
						_changes[cell] = false;
					}
				}
				else
				{
					// Check neighbors
					if (neighbors == 3)
					{
						// Add change to add cell
						_changes[cell] = true;
					}
				}
			}
		}

		public static Task Run()
		{
			// Create cells
			var cells = new (int x, int y)[]
			{
				// R-Pentomino
				(0, 0),
				(0, 1),
				(0, -1),
				(-1, 0),
				(1, 1),
				// Simple glider
				//(0, 0),
				//(1, 0),
				//(2, 0),
				//(0, 1),
				//(1, 2),
				// Simple oscillator
				//(0, 0),
				//(0, 1),
				//(0, 2),
				// Vertical line
				//(0, 0),
				//(0, 1),
				//(0, -1),
				//(0, 2),
				//(0, -2),
				//(0, 3),
				//(0, -3),
				//(0, 4),
				//(0, -4),
				//(0, 5),
				//(0, -5),
			};
			// Create life
			var life = new Life(cells);
			// Create center
			var center = (x: 0, y: 0);
			// Create paused
			var paused = false;
			// Execute render
			return Task.WhenAll
				(
					_GetInputAsync
						(
							frameDelay: TimeSpan.FromSeconds(0.1),
							(
								ConsoleKey.UpArrow,
								() => center.y += 1
							),
							(
								ConsoleKey.DownArrow,
								() => center.y -= 1
							),
							(
								ConsoleKey.LeftArrow,
								() => center.x -= 1
							),
							(
								ConsoleKey.RightArrow,
								() => center.x += 1
							),
							(
								ConsoleKey.R,
								() => life = new Life(cells)
							),
							(
								ConsoleKey.P,
								() => paused = !paused
							)
						),
					_RenderAsync
						(
							frameDelay: TimeSpan.FromSeconds(0.1),
							horizontal: 50,
							vertical: 30,
							getLife: () => life,
							getCenter: () => center,
							getPaused: () => paused
						)
				);
		}

		private static async Task _GetInputAsync(TimeSpan frameDelay, params (ConsoleKey key, Action action)[] keysAndActions)
		{
			// Create key to action mappings
			var keyToActionMappings = new Dictionary<ConsoleKey, Action>();
			// Run through keys and actions
			for (int i = 0; i < keysAndActions.Length; i++)
			{
				var (key, action) = keysAndActions[i];
				// Add key to action mapping
				keyToActionMappings.Add(key, action);
			}
			// Run get input
			while (true)
			{
				// Check if key is available
				while (Console.KeyAvailable)
				{
					// Read key
					var key = Console.ReadKey(intercept: true).Key;
					// Check if action exists for key
					if (keyToActionMappings.TryGetValue(key, out var action))
					{
						// Execute action
						action();
					}
				}
				// Wait for delay
				await Task.Delay(frameDelay);
			}
		}

		private static async Task _RenderAsync(TimeSpan frameDelay, int horizontal, int vertical, Func<Life> getLife, Func<(int x, int y)> getCenter, Func<bool> getPaused)
		{
			// Get life
			var life = getLife();
			// Create iterations
			var iterations = 0;
			// Create render
			var render = new StringBuilder();
			// Run life
			while (true)
			{
				// Get new life
				var lifeNew = getLife();
				// Check if life changed
				if (life != lifeNew)
				{
					// Set life
					life = lifeNew;
					// Set iterations
					iterations = 0;
					// Clear console
					Console.Clear();
				}
				// Clear render
				render.Clear();
				// Get center
				var (xCenter, yCenter) = getCenter();
				// Render life
				for (int y = vertical; y >= -vertical; y--)
				{
					for (int x = -horizontal; x <= horizontal; x++)
					{
						// Get edge
						var edge = Math.Abs(x) == horizontal || Math.Abs(y) == vertical;
						// Get cell
						var xCell = x + xCenter;
						var yCell = y + yCenter;
						// Get character
						var character = life.Cells.Contains((xCell, yCell)) || edge ? "[]" : "  ";
						// Add character
						render.Append(character);
					}
					// Add new line
					render.AppendLine();
				}
				// Add iteration
				render.Append("iteration: ");
				render.Append(iterations);
				// Set position
				Console.SetCursorPosition(0, 0);
				// Write line
				Console.WriteLine(render.ToString());
				// Wait for delay
				await Task.Delay(frameDelay);
				// Check if paused
				if (getPaused())
				{
					// Skip mutation
					continue;
				}
				// Mutate life
				life.Mutate();
				// Increase iterations
				iterations++;
			}
		}
	}
}
