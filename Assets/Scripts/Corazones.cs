using UnityEngine;
using System.Collections.Generic;

public class Corazones : MonoBehaviour
{
    private List<GameObject> corazones;

    private GameObject _jugador;
    private PlayerController _jugadorScript;
    void Start()
    {
        _jugador = GameObject.FindGameObjectWithTag("Player");
        if (_jugador == null)
        {
            Debug.LogError("[Corazones] No se encontró ningún GameObject con tag 'Player'.", this);
            return;
        }

        _jugadorScript = _jugador.GetComponent<PlayerController>();
        if (_jugadorScript == null)
        {
            Debug.LogError("[Corazones] El objeto con tag 'Player' no tiene PlayerController.", _jugador);
            return;
        }

        // Aquí ya puedes usar _jugadorScript.vidaActual, vidaMax, etc.
    }

    void Update()
    {
        // Ejemplo: actualizar UI según _jugadorScript.vidaActual
        // if (_jugadorScript == null) return;
    }
}