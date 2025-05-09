using UnityEngine;

public class KunaiController: MonoBehaviour
{
    private string direccion = "Derecha";

    Rigidbody2D rb;
    SpriteRenderer sr;
    //invovamos a nuestro personaje atraves del atributo Ninja
    public GameObject Ninja;

    void Start()
    {
        // Initialize the Kunai object
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

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
        // si al hacer colision con el enemigo vas a destruir al objeto de la colicion
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            //si el ninja es diferente de null
            if (Ninja != null)
            {
                //primero hacemos uso del componente enemigosMuertosCount del player controller el cual incrementara en 1
                Ninja.GetComponent<PlayerController>().enemigosMuertosCount++;
                //luego esa disminucion o actualizacion de enemigosMuertosCount lo guardamos en la varible n
                int n = Ninja.GetComponent<PlayerController>().enemigosMuertosCount;
                //al final mostraremos el valor de ese n el el componente enemigosMuertosText
                Ninja.GetComponent<PlayerController>().enemigosMuertosText.text = $"Enemigos muertos: {n}";
            }
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }
    
}
