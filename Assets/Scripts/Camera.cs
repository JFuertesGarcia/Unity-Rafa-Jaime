using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float smoothing = 5f; // Velocidad de movimiento de la cámara
    
    private Transform playerTransform;
    private Vector3 targetPosition;

    void Start()
    {
        // Buscamos al jugador por su Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        
        // La cámara empieza donde esté al iniciar
        targetPosition = transform.position;
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // Detectar en qué sala está el jugador basándonos en el tamaño de tus salas
        // Usamos las variables que definiste en RoomManager: 24 (ancho) y 15 (alto)
        float roomWidth = 24f;
        float roomHeight = 15f;

        // Calculamos el centro de la sala actual redondeando la posición del jugador
        float posX = Mathf.Round(playerTransform.position.x / roomWidth) * roomWidth;
        float posY = Mathf.Round(playerTransform.position.y / roomHeight) * roomHeight;

        // Mantener la Z de la cámara (usualmente -10)
        targetPosition = new Vector3(posX, posY, transform.position.z);

        // Movimiento suave hacia el centro de la sala
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}