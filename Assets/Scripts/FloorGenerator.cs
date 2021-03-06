﻿using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class FloorGenerator
{
	private ItemSpawner spawner;

	// Used for connecting two rooms together via a pathway.
	// Stores the axis the pathway will exist on as well as positions 
	// for the pathway.
	private struct Connection
	{
		public Vec2i a, b;
		public bool xAxis;

		public Connection(Vec2i a, Vec2i b, bool xAxis)
		{
			this.a = a;
			this.b = b;
			this.xAxis = xAxis;
		}
	}

	private Floor floor;

	public FloorGenerator(Floor floor)
	{
		this.floor = floor;
		spawner = GameObject.FindWithTag("ItemSpawner").GetComponent<ItemSpawner>();
	}

	// Generate all tiles for the given room.
	private void BuildRoom(Room room, bool stairRoom, bool powerupRoom)
	{
		// Add top and bottom walls.
		for (int x = 1; x <= Room.LimX - 1; x++)
		{
			room.SetTile(x, Room.LimY, TileType.Wall);
			room.SetTile(x, 0, TileType.Wall);
		}

		// Add left and right walls.
		for (int y = 1; y <= Room.LimY - 1; y++)
		{
			room.SetTile(0, y, TileType.Wall);
			room.SetTile(Room.LimX, y, TileType.Wall);
		}

		// Add corner walls. These could be baked into the above two loops, but I left them separate
		// since it depends on the art complexity.
		room.SetTile(0, Room.LimY, TileType.Wall);
		room.SetTile(Room.LimX, Room.LimY, TileType.Wall);
		room.SetTile(0, 0, TileType.Wall);
		room.SetTile(Room.LimX, 0, TileType.Wall);

		// Add floor.
		for (int y = 1; y <= Room.LimY - 1; y++)
		{
			for (int x = 1; x <= Room.LimX - 1; x++)
                room.SetTile(x, y, TileType.Floor);
		}

		int obstacleCount = Random.Range(0, 6);

		// Add some random obstacles for collision testing.
		for (int i = 0; i < obstacleCount; i++)
		{
			int x = Random.Range(2, Room.LimX - 1);
			int y = Random.Range(2, Room.LimY - 1);
			room.SetTile(x, y, TileType.Wall);
		}

        if (stairRoom)
        {
            room.SetTile(Room.LimX / 2, Room.LimY / 2, TileType.Stair);
        }

		Vector2 wPos = room.WorldPos;

		if (powerupRoom)
			spawner.SpawnItem(wPos + RandomV2(3.0f, 3.0f, Room.LimX - 3.0f, Room.LimY - 3.0f));
	}

	// Returns the position of the next room to generate.
	private Vec2i GetNextPos(Vec2i current)
	{
		List<Vec2i> possibleRooms = new List<Vec2i>(4)
		{
			current + Vec2i.Directions[Direction.Front],
			current + Vec2i.Directions[Direction.Right]
		};

		return possibleRooms[Random.Range(0, possibleRooms.Count)];
	}

	private void AddConnections(List<Connection> connections)
	{
		for (int c = 0; c < connections.Count; c++)
		{
			Connection info = connections[c];

			Vec2i a = info.a * new Vec2i(Room.Width, Room.Height);
			Vec2i b = info.b * new Vec2i(Room.Width, Room.Height);

			if (info.xAxis)
			{
				if (b.x < a.x) Swap(ref a, ref b);

				int startX = a.x + Room.LimX, y = a.y + Room.HalfSizeY;

				for (int x = startX; x < startX + 2; x++)
					floor.SetTile(x, y, TileType.Floor);
			}
			else
			{
				if (b.y < a.y) Swap(ref a, ref b);

				int startY = a.y + Room.LimY, x = a.x + Room.HalfSizeX;

				for (int y = startY; y < startY + 2; y++)
					floor.SetTile(x, y, TileType.Floor);
			}
		}
	}

	/// <summary>
	/// Generates the specified number of rooms.
	/// </summary>
	public void Generate(int roomCount)
	{
		// Room position.
		Vec2i roomP = new Vec2i(0, 0);

		// Used to pair two rooms together. These two rooms are connected and will have a path between them.
		List<Connection> connections = new List<Connection>(roomCount);
        int stairRoom = Random.Range(roomCount / 2, roomCount);

		for (int i = 0; i < roomCount; i++)
		{
			bool powerupRoom = Random.value < 0.4f;

			Room room = floor.CreateRoom(roomP.x, roomP.y);
            if (i == stairRoom)
            {
                BuildRoom(room, true, powerupRoom);
            }
            else
            {
                BuildRoom(room, false, powerupRoom);
            }
			

			Vec2i next = GetNextPos(roomP);

			connections.Add(new Connection(roomP, next, roomP.x != next.x));
			roomP = next;
		}

		// Remove the last connection because it doesn't connect to a room (it's what would connect to
		// the next room if we added another).
		connections.RemoveAt(connections.Count - 1);

		AddConnections(connections);
	}
}
