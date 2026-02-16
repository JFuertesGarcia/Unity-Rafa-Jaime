using UnityEditor.Build;
using UnityEngine;
public class HitboxAtaque : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            PlayerController parentScript = transform.parent.GetComponent<PlayerController>();
            float danio = parentScript.ataque;
            // pasarle el daño al enemigo
        }
    }
}
