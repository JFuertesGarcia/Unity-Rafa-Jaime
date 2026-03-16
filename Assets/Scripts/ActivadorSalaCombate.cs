using UnityEngine;

public class ActivadorSalaCombate : MonoBehaviour
{
    private GestorSalaCombate gestorSala;

    private void Awake()
    {
        gestorSala = GetComponentInParent<GestorSalaCombate>();
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;

        if (gestorSala != null)
            gestorSala.ActivarCombate();
    }
}