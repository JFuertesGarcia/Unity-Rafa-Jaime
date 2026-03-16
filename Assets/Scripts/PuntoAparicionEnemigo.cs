using UnityEngine;

public class PuntoAparicionEnemigo : MonoBehaviour
{
    public TipoEnemigo tipoEnemigo;
}

public enum TipoEnemigo
{
    Slime,
    Murcielago,
    Tirador
}