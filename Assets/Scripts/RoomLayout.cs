using System.Linq;
using UnityEngine;

public class RoomLayout
{
    public bool hasTop;
    public bool hasBottom;
    public bool hasLeft;
    public bool hasRight;
    public bool isStarting = false;
    public string key;
    

    public void CalcDoors(Room room)
    {
        this.hasTop = room.getDoor(Vector2Int.up).estado == EstadoPuerta.Abierta;
        this.hasBottom = room.getDoor(Vector2Int.down).estado == EstadoPuerta.Abierta;
        this.hasLeft = room.getDoor(Vector2Int.left).estado == EstadoPuerta.Abierta;
        this.hasRight = room.getDoor(Vector2Int.right).estado == EstadoPuerta.Abierta;
    }
    
    public void CalcLayoutKey()
    {
        string kei = "";
        if (hasTop) kei += "T";
        if (hasBottom) kei += "B";
        if (hasLeft) kei += "L";
        if (hasRight) kei += "R";
        kei += "_1";
        if (isStarting) kei = "Starting";
        this.key = kei;
    }
    public string getLayoutKey() { return key; }
}