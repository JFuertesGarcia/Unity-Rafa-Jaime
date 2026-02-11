using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] int MaxRooms = 30;
    [SerializeField] int MinRooms = 20;
    
    int roomWidth = 24;
    int roomHeight = 15;
    
    int gridSizeX = 20;
    int gridSizeY = 20;
    
    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    
    private int[,] roomGrid;

    private int roomCount;
    
    private bool generationComplete = false;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();
        
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }
    
    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < MaxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;
            
            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
        }
        else if (roomCount <= MinRooms)
        {
            Debug.Log("RoomCount menos que el minimo de salas, volviendo a intentar");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation Complete, {roomCount} rooms created");
            generationComplete = true;
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room {roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        
        if (x >= gridSizeX || y >= gridSizeY || x < 0 || y < 0) return false;
        if(roomCount >= MaxRooms) return false;
        if(UnityEngine.Random.value < 0.5f && roomIndex!= Vector2Int.zero) return false;
        if (CountAdjacentRooms(roomIndex) > 1) return false;
        if (roomGrid[x, y] != 0) return false;
        
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;
        
        
        var NewRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        NewRoom.GetComponent<Room>().RoomIndex = roomIndex;
        NewRoom.name = $"Room-{roomCount}";
        roomObjects.Add(NewRoom);
        
        OpenDoors(NewRoom, x, y);
        
        return true;
    }

    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;
        
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }
    
    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();
        Vector2Int currentPos = new Vector2Int(x, y);

        // Definimos las 4 direcciones
        Vector2Int[] dirs = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

        foreach (Vector2Int dir in dirs)
        {
            Vector2Int neighborPos = currentPos + dir;

            // Si la posición vecina está dentro del grid y tiene una sala (roomGrid != 0)
            if (neighborPos.x >= 0 && neighborPos.x < gridSizeX && 
                neighborPos.y >= 0 && neighborPos.y < gridSizeY && 
                roomGrid[neighborPos.x, neighborPos.y] != 0)
            {
                Room neighbor = GetRoomScriptAt(neighborPos);
                if (neighbor != null)
                {
                    // Abrimos la puerta de la sala que acabamos de crear
                    newRoomScript.OpenDoor(dir);
                    // Abrimos la puerta de la sala vecina (en la dirección opuesta)
                    neighbor.OpenDoor(-dir);
                }
            }
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;
        
        if (x > 0 && roomGrid[x - 1, y] != 0) count++;
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++;
        if (y > 0 && roomGrid[x, y - 1] != 0) count++;
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++;
        
        return count;
    }
    
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2),
            roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;
        
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(new Vector3(position.x,position.y), new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}
