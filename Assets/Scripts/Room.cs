using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door topDoor;
    [SerializeField] private Door bottomDoor;
    [SerializeField] private Door leftDoor;
    [SerializeField] private Door rightDoor;
    
    
    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up && topDoor != null)
            topDoor.estado = EstadoPuerta.Abierta;
        else if (direction == Vector2Int.down && bottomDoor != null)
            bottomDoor.estado = EstadoPuerta.Abierta;
        else if (direction == Vector2Int.left && leftDoor != null)
            leftDoor.estado = EstadoPuerta.Abierta;
        else if (direction == Vector2Int.right && rightDoor != null)
            rightDoor.estado = EstadoPuerta.Abierta;
    }
}