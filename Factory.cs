using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages and calls a list of things to spawn
/// </summary>
public class Factory : MonoBehaviour {

    [System.Serializable]
    public class SpawnInfo
    {
        // what and where to spawn>
        public GameObject prefab,location;
        // seconds to wait between spawning, with some RNG
        public float period,periodWiggle;
        // bounding box for RNG to offset the spawn location
        public Vector3 spawnExtents;
        // an actual time, in game seconds
        float nextSpawnTime;
        /// <summary>
        /// Spawn if possible, comparing nextSpawnTime
        /// </summary>
        public bool AttemptSpawn()
        {
            bool shouldSpawn = Time.time > nextSpawnTime;
            if(shouldSpawn)
            {
                nextSpawnTime = Time.time + period + Random.value * periodWiggle;
                GameObject obj = Instantiate(prefab);
                Vector3 offset = new Vector3( RandomBalanced * spawnExtents.x, RandomBalanced * spawnExtents.y, RandomBalanced * spawnExtents.z );
                obj.transform.position = location.transform.position + offset;
            }
            return shouldSpawn;
        }
        // converts RNG[0,1] to RNG[-1,1], useful for the offset vector
        float RandomBalanced { get { return ( Random.value * 2 ) -1; } }
    }

    public SpawnInfo[] spawnList;

    void Update()
    {
        foreach(SpawnInfo si in spawnList)
        {
            si.AttemptSpawn();
        }
    }

}
