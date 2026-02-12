using System.Linq;
using UnityEngine;

public class RoomLayout : MonoBehaviour
{
    [Header("Configuración de Puertas")]
    public bool hasTop;
    public bool hasBottom;
    public bool hasLeft;
    public bool hasRight;
    public bool isStarting = false;

    // Genera un código único como "T_B_L" o "T_R" para buscarlo
    public string GetLayoutKey()
    {
        string key = "";
        if (hasTop) key += "T";
        if (hasBottom) key += "B";
        if (hasLeft) key += "L";
        if (hasRight) key += "R";
        key += "_1";
        if (isStarting) key = "Starting";
        return key;
    }
}