using Unity.VisualScripting;
using UnityEngine;

public enum Mejoras
{
    Fuerza,
    VelocidadAtaque,
    Velocidad,
    Vida
}

public class Mejora : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private Mejoras tipomejora;
    private Sprite[] sprites;

    private void Start()
    {
        int numMejora = Random.Range(1, 4);
        tipomejora = (Mejoras)numMejora;
        sprites = Resources.LoadAll<Sprite>("Sprites/Mejoras");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[numMejora - 1];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerController = collision.gameObject.GetComponent<PlayerController>();
            _playerController.recibirMejora(tipomejora);
            Destroy(this.gameObject);
        }
    }
}