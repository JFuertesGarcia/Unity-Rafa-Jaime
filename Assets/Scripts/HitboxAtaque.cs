using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    private PlayerController jugador;
    private Collider2D colisionador;

    private void Awake()
    {
        jugador = GetComponentInParent<PlayerController>();
        colisionador = GetComponent<Collider2D>();

        if (colisionador != null) colisionador.enabled = false; // OFF por defecto
    }

    public void ActivarHitbox(bool activa)
    {
        if (colisionador != null) colisionador.enabled = activa;
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