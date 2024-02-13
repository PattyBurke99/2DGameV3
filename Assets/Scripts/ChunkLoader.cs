using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChunkLoader : NetworkBehaviour {

    private Transform world;
    private Transform[,] tiles;
    private int worldSize = 27;

    private Dictionary<NetworkInstanceId, Transform> playerTransforms;
    private Dictionary<NetworkInstanceId, Vector2>  playerChunks;

    public List<Transform> loadedChunks;

	void Start () {
        world = GameObject.Find("World").transform;
        tiles = new Transform[worldSize, worldSize];

        playerTransforms = new Dictionary<NetworkInstanceId, Transform>();
        playerChunks = new Dictionary<NetworkInstanceId, Vector2>();

        for (int y = 0; y < worldSize; y++)
        {
            Transform row = world.transform.Find("Row" + y);
            for (int x = 0; x < worldSize; x++)
            {
                tiles[x, y] = row.Find("Tile" + x);
                tiles[x, y].gameObject.SetActive(false);
            }
        }
    }
	
	void Update () {

		foreach (KeyValuePair<NetworkInstanceId, Transform> pair in playerTransforms)
        {
            if (playerChunks[pair.Key] != GetPlayerChunk(pair.Value))
            {
                Vector2 oldChunk = playerChunks[pair.Key];
                Vector2 newChunk = GetPlayerChunk(pair.Value);

                playerChunks[pair.Key] = newChunk;
                Vector2 change = newChunk - oldChunk;

                if (change.x != 0)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        DisableTile((int)(oldChunk.x - change.x), (int)(oldChunk.y + y));
                        EnableTile((int)(newChunk.x + change.x),(int)(oldChunk.y + y));
                    }
                }

                if (change.y != 0)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        DisableTile((int)(oldChunk.x + x), (int)(oldChunk.y - change.y));
                        EnableTile((int)(oldChunk.x + x), (int)(newChunk.y + change.y));
                    }
                }
            }
        }
	}

    public void RegisterPlayer(Transform player)
    {
        Vector2 currentChunk = GetPlayerChunk(player);

        playerTransforms.Add(player.GetComponent<NetworkIdentity>().netId, player);
        playerChunks.Add(player.GetComponent<NetworkIdentity>().netId, currentChunk);
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
               EnableTile((int)currentChunk.x + x, (int)currentChunk.y + y);
            }
        }
    }

    Vector2 GetPlayerChunk(Transform player)
    {
        return new Vector2(worldSize+Mathf.Floor((player.position.x - 540)/40), worldSize + Mathf.Floor((player.position.y - 540)/40));
    }

    void EnableTile(int x, int y)
    {
        Transform tile = tiles[x, y];
        if (!loadedChunks.Contains(tile))
            tile.gameObject.SetActive(true);

        loadedChunks.Add(tile);
        RpcEnableTile(x, y);
    }

    void DisableTile(int x, int y)
    {
        Transform tile = tiles[x, y];
        loadedChunks.Remove(tile);
        if (!loadedChunks.Contains(tile))
            tile.gameObject.SetActive(false);

        RpcDisableTile(x, y);
    }

    [ClientRpc]
    void RpcEnableTile(int x, int y)
    {
        Transform tile = tiles[x, y];
        if (!loadedChunks.Contains(tile))
            tile.gameObject.SetActive(true);
    }

    [ClientRpc]
    void RpcDisableTile(int x, int y)
    {
        Transform tile = tiles[x, y];
        if (!loadedChunks.Contains(tile))
            tile.gameObject.SetActive(false);
    }
}
