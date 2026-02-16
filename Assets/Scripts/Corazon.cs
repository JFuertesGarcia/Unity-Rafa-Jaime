using UnityEngine;

public class Corazon : MonoBehaviour
{
    private void Actualizar(int vida)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>("Sprites/Corazones" + vida);
    }
}
