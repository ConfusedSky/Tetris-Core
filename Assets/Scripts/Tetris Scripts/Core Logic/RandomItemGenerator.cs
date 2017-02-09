using System;
using System.Collections.Generic;

namespace Tetris
{
	public class RandomItemGenerator<T>
	{
		private Queue<T> blocks;
		// Number of elements to have in the queue
		private int size = 5;
		// Number of elements to look back to find duplicates
		private int lookback = 4;
		// Number of tries before adding a duplicate
		private int tries = 4;
		// List of items to generate from
		private T[] sourceList;

		public RandomItemGenerator (T[] sourceList)
		{
			Initialize (sourceList, 5, 4, 4);
		}

		public RandomItemGenerator (T[] sourceList, int size, int lookback, int tries)
		{
			Initialize (sourceList, size, lookback, tries);
		}

		public void Initialize (T[] sourceList, int size, int lookback, int tries)
		{
			if (sourceList.Length <= 0)
				throw new ArgumentException ("Source list must have at least one element");
			if (size <= 0)
				throw new ArgumentException ("Size must be greater than zero");
			blocks = new Queue<T> (size);
			this.sourceList = sourceList;
			this.size = size;
			this.lookback = lookback;
			this.tries = tries;
			refreshQueue ();
		}

		public void Reset ()
		{
			blocks = new Queue<T> (size);
			refreshQueue ();
		}

		private void refreshQueue ()
		{
			// if there aren't (size) blocks in the queue add more blocks until there are
			if (blocks.Count <= size) {
				int index = size - blocks.Count;
				for (int i = 0; i < index; i++) {
					addItem ();
				}
			}
		// Else there are too many and we must get rid of them
		else {
				Queue<T> temp = new Queue<T> (size);
				for (int i = 0; i < size; i++)
					temp.Enqueue (blocks.Dequeue ());
				blocks = temp;
			}
		}

		private void addItem ()
		{
			int blockNumber;
			T result = sourceList [UnityEngine.Random.Range (0, sourceList.Length)];
			bool valid;

			// Try to add a new element that isn't a duplicate 
			for (int i = 0; i < tries; i++) {
				result = sourceList [UnityEngine.Random.Range (0, sourceList.Length)];
				valid = true;

				blockNumber = 0;
				foreach (T t in blocks) {
					if (blockNumber >= lookback)
						break;

					if (result.Equals (t)) {
						valid = false;
						break;
					}
				
					blockNumber++;
				}

				if (valid) {
					break;
				}
			}

			blocks.Enqueue (result);
		}

		public T GetNextItem ()
		{
			T result = blocks.Dequeue ();
			addItem ();

			return result;
		}

		public T[] GetObjects ()
		{
			return blocks.ToArray ();
		}
	}
}