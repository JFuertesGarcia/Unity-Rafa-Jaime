using UnityEngine;

public class Corazon : MonoBehaviour
{
    public Sprite agarrarSprite(int vida) // 0: lleno, 1: medio, 2: vacio, 3: medio contenedor lleno, 4: medio contenedor vacio
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Corazones");
        return sprites[vida];
    }
}
