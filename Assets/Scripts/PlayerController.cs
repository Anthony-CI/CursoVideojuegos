using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 10f;
    public float fuerzaSalto = 12.5f;
    
    public GameObject kunaiPrefab;
    public int kunaisDisponibles = 5;

    public Transform groundCheck;
    public LayerMask groundLayer;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    private bool isGrounded = true;
    private string direccion = "Derecha";
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    //private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;


    [Header("Parámetros de salto")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;


    //definimos atributos de tipo texto para poder ser usados
    //creamos dos varible para inicializarlas una en 0 y ota en 3, el 3 depende de la cantidad de vidas que poseamos
    public Text enemigosMuertosText;
    public int enemigosMuertosCount = 0;

    public Text puntosVidaText;
    public int puntosVidaCount = 3;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Iniciando PlayerController");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        enemigosMuertosText = GameObject.Find("EnemigoMuertosText").GetComponent<Text>();

        defaultGravityScale = rb.gravityScale;

        //usamos las variables para que inicialicen con el juego
        enemigosMuertosText.text = $"Enemigos muertos: {enemigosMuertosCount}";
        puntosVidaText.text = $"Vidas: {puntosVidaCount}";

    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupMoverseVertical();
        SetupSalto();
        SetUpLanzarKunai();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            //Debug.Log($"Colision con Enemigo: ${zombie.puntosVida}");
            //dentro de la accion de colision con el enemigo le decimos a puntosVidaCount disminuya en 1
            puntosVidaCount--;
            //luego puntosVidaCount se visualizara en el texto con su debida reducion por colicion
            puntosVidaText.text = $"Vidas: {puntosVidaCount}";
            //Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }


    void SetupMoverseHorizontal() {
        
        Debug.Log($"isGrounded: {isGrounded}, {rb.linearVelocityY}");
        if (isGrounded && rb.linearVelocityY == 0) {
            animator.SetInteger("Estado", 0);
        }

        rb.linearVelocityX = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = velocidad;
            sr.flipX = false;
            direccion = "Derecha";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -velocidad;
            sr.flipX = true;
            direccion = "Izquierda";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
    }

    void SetupMoverseVertical() {
        
        if (!puedeMoverseVerticalMente) return;
        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = velocidad;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -velocidad;
        }
    }

    void SetupSalto() {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // --- Coyote Time ---
        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        }
        else {
            coyoteTimeCounter -= Time.deltaTime;
            if (rb.linearVelocityY > 5f)
                animator.SetInteger("Estado", 3);
            
        }

        


        // --- Jump Buffer ---
        if (Input.GetButtonDown("Jump")) {
            jumpBufferCounter = jumpBufferTime;
        }
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- Ejecutar salto ---
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferCounter = 0f;
        }

        // --- Ajuste de gravedad para caída más rápida ---
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
            
        }
    }

    void SetUpLanzarKunai() {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;
        if (Input.GetKeyUp(KeyCode.K))
        {
            GameObject kunai = Instantiate(kunaiPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            kunai.GetComponent<KunaiController>().SetDirection(direccion);
            kunaisDisponibles -= 1;

            //al lanzar el kunai tenemos que usar al GameObject creado en la clase kunaicontroller "public GameObject Ninja;"
            kunai.GetComponent<KunaiController>().Ninja = this.gameObject;

        }
    }

    // Visualiza el groundCheck en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }
}
