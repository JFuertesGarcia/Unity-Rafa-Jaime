using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento del personaje
    public float movementSpeed;

    // Velocidad máxima que el personaje puede alcanzar
    public float maxSpeed;
    
    //Tamaño del área para detectar el suelo
    public Vector3 areaSize;

    //Variacion respecto al posicionamiento del área
    public Vector3 areaOffset;
    
    //Si true, el personaje toca suelo
    public bool grounded;

    //Aquí se recogen todos los objetos que se encuentran dentro del área dedetección del suelo.
    //Si hay uno o más, el personaje está tocando suelo
    public Collider2D[] items;

    //Para que solo se detecte esteLayer
    public LayerMask groundLayer;
    
    //Fuerza de salto
    public float jumpForce;
    
    // Referencia al Rigidbody2D
    private Rigidbody2D _rB;
    // Referencia al Animator
    private Animator _anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Recuperación de referencias
        _rB = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

   
    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Jump();
        CheckGround();
    }

    void Move()
    {
        // Recuperamos el valor del eje horizontal
        float x = Input.GetAxisRaw("Horizontal");

        // Generamos el vector de fuerza a aplicar
        Vector2 force = new Vector2(x * movementSpeed, 0f);

        if (_rB.linearVelocity.magnitude < maxSpeed)
        {
            // Aplicamos la fuerza al Rigidbody2D
            _rB.AddForce(force, ForceMode2D.Impulse);
        }
        
        // Aplicamos un freno si no se está presionando un botón de dirección
        _rB.linearDamping = (x == 0 && grounded) ? 5f : 1f;
        
        //Ajustamos parámetro del animator
        _anim.SetBool("corriendo", x !=0);

        //Hacemos que el persona mire hacia donde se mueve
        FaceDirection(x);
    }
    
    void FaceDirection(float x){
        //Dependiendo del valor de desplazamiento en x ajustamos la escala del personaje en -1 o 1
        if(x<0){
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }else if(x>0){
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
    }
    
    private void OnDrawGizmos(){
        Gizmos.color = Color.magenta; //color para el área de detección
        //Pide un centro y un tamaño
        Gizmos.DrawCube(transform.position + areaOffset, areaSize);
    }
    
    void CheckGround(){
        //Creamos un área que recoge todos los objetos que detecta y los introduce en nuestro array
        items = Physics2D.OverlapBoxAll(transform.position + areaOffset, areaSize, 0f, groundLayer);
        //Si toca algo, al array es mayor que cero, porque contendría
        grounded = items.Length > 0;
    }
    
    void Jump(){
        //saltamos si hemos pulsado el botón de salto y si estamos tocando suelo
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            //Aplicamos una fuerza hacia arriba sobre el rigitdbody del personaje
            _rB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
