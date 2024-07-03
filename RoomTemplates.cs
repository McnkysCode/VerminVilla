using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms = new List<GameObject>();
    public GameObject roomParent;
    public int maxRooms = 20;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;

    // Dictionary to track grid positions and room occupancy
    public Dictionary<Vector2Int, GameObject> roomGrid = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        if (waitTime <= 0 && !spawnedBoss)
        {
            if (rooms.Count > 0)
            {
                Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
                spawnedBoss = true;
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
