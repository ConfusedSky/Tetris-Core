using System;
using System.Collections.Generic;

public class TetronimoQueue
{
	private Queue<TetronimoType> blocks;
	// Number of elements to have in the queue
	private int size = 5;
	// Number of elements to look back to find duplicates
	private int lookback = 4;
	// Number of tries before adding a duplicate
	private int tries = 4;

	public TetronimoQueue()
	{
		blocks = new Queue<TetronimoType>( size );
		refreshQueue();
	}

	public TetronimoQueue( int size, int lookback, int tries )
	{
		if( size <= 0 ) throw new ArgumentException( "Size must be greater than zero" );
		blocks = new Queue<TetronimoType>( size );
		Initialize( size, lookback, tries );
	}

	public void Initialize( int size, int lookback, int tries )
	{
		if( size <= 0 ) throw new ArgumentException( "Size must be greater than zero" );
		this.size = size;
		this.lookback = lookback;
		this.tries = tries;
		refreshQueue();
	}

	public void Reset()
	{
		blocks = new Queue<TetronimoType>( size );
		refreshQueue();
	}

	private void refreshQueue()
	{
		// if there aren't (size) blocks in the queue add more blocks until there are
		if( blocks.Count <= size )
		{
			int index = size - blocks.Count;
			for( int i = 0; i < index; i++ )
			{
				addBlock();
			}
		}
		// Else there are too many and we must get rid of them
		else
		{
			Queue<TetronimoType> temp = new Queue<TetronimoType>(size);
			for( int i = 0; i < size; i++ ) temp.Enqueue( blocks.Dequeue() );
			blocks = temp;
		}
	}

	private void addBlock()
	{
		int blockNumber;
		TetronimoType result = (TetronimoType)UnityEngine.Random.Range( 0, Tetronimo.TETRONIMO_COUNT );
		bool valid;

		// Try to add a new element that isn't a duplicate 
		for( int i = 0; i < tries; i++ )
		{
			result = (TetronimoType)UnityEngine.Random.Range( 0, Tetronimo.TETRONIMO_COUNT );
			valid = true;

			blockNumber = 0;
			foreach( TetronimoType t in blocks )
			{
				if( blockNumber >= lookback ) break;

				if( result == t )
				{
					valid = false;
					break;
				}
				
				blockNumber++;
			}

			if( valid )
			{
				break;
			}
		}

		blocks.Enqueue( result );
	}

	public TetronimoType GetNextBlock()
	{
		TetronimoType result = blocks.Dequeue();
		addBlock();

		return result;
	}

	public TetronimoType[] GetTypes()
	{
		return blocks.ToArray();
	}
}