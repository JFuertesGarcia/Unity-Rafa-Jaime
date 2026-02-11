using UnityEngine;

public enum EstadoPuerta
{
    Abierta, Cerrada, Pared
}

public class Door : MonoBehaviour
{
    [SerializeField] public EstadoPuerta estado = EstadoPuerta.Pared;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
        
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.simulated = true;
    }

    void Update()
    {
        if (_collider == null) return;

        if (estado == EstadoPuerta.Abierta)
        {
            // PRUEBA RADICAL: Si está abierta, el collider se apaga del todo
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
            _collider.isTrigger = false;
        }

        GetComponent<SpriteRenderer>().color = (estado == EstadoPuerta.Abierta) ? Color.green : Color.white;
    }
}