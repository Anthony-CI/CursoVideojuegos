using UnityEngine;

public class KunaiController: MonoBehaviour
{
    private string direccion = "Derecha";

    Rigidbody2D rb;
    SpriteRenderer sr;
    GameRepository gameRepository;
    GameData gameData;

    public GameObject Ninja;
    public int Puntos = 0;
    public float Scale = 1;

    void Start()
    {
        // Initialize the Kunai object
        gameRepository = GameRepository.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gameData = gameRepository.GetData();

        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        // Update the Kunai object
        if (direccion == "Derecha")
        {
            rb.linearVelocityX = 15;
            sr.flipY = false;
            
        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocityX = -15;
            sr.flipY = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with the Kunai object
        /* if (collision.gameObject.CompareTag("Enemigo"))
         {
             Destroy(collision.gameObject);
             Destroy(this.gameObject);

             gameData.enemigosMuertos++;
             gameRepository.SaveData();
         }
        */

        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (Ninja != null)
            {
                Ninja.GetComponent<PlayerController>().enemigosMuertosCount++;
                int n = Ninja.GetComponent<PlayerController>().enemigosMuertosCount;
                Ninja.GetComponent<PlayerController>().enemigosMuertosText.text = $"Enemigos muertos: {n}";
            }

            Destroy(this.gameObject);
            ZombieController enemy = collision.gameObject.GetComponent<ZombieController>();

            if (this.Puntos >= enemy.puntosVida)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                enemy.puntosVida -= this.Puntos;
            }

            Debug.Log(Puntos);
        }
    }

    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }
    
}
