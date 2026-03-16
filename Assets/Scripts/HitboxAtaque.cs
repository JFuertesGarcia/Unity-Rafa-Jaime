using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    private PlayerController jugador;

    private void Awake()
    {
        jugador = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Enemigo")) return;

        VidaEnemigo vidaEnemigo = otro.GetComponent<VidaEnemigo>();
        if (vidaEnemigo != null && jugador != null)
        {
            vidaEnemigo.RecibirDanio(jugador.ataque);
        }
    }
}