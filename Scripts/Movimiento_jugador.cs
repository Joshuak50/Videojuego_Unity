using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento_jugador : MonoBehaviour
{
    private readonly int maxHealth = 100;
    public int currentHealth;



    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;
    public float x, y;
    public AudioClip sonidob;
    public AudioClip sonidom;

    public GameObject cuboPuntos;
    public GameObject cuboVelocidad;

    public int puntos;  // Variable para almacenar los puntos
    public float limitMinX, limitMaxX, limitMinZ, limitMaxZ;

    private TextMeshProUGUI txtVelocidad;
    private TextMeshProUGUI txtPuntos;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        puntos = 0;  // Inicializamos los puntos en 0
        limitMinX = -100000;
        limitMaxX = 9000000.13f;
        limitMinZ = -1100000.15f;
        limitMaxZ = 1000000.59f;

        txtPuntos = GameObject.Find("txtPuntos").GetComponent<TextMeshProUGUI>();
        txtVelocidad = GameObject.Find("txtVelocidad").GetComponent<TextMeshProUGUI>();

        txtVelocidad.text = "Velocidad: " + velocidadMovimiento.ToString();
        txtPuntos.text = "Puntos: " + puntos.ToString();

        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        transform.Rotate(0, x * Time.deltaTime * velocidadRotacion, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocidadMovimiento);

        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 posicion = transform.position + transform.forward * 2f;
            Instantiate(cuboPuntos, posicion, transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 posicion = transform.position + transform.forward * 2f;
            Instantiate(cuboVelocidad, posicion, transform.rotation);
        }

        anim.SetFloat("Vel X", x);
        anim.SetFloat("Vel Y", y);


        LimitarPosicion();

    }
    private void LimitarPosicion()
    {
        float clampedX = Mathf.Clamp(transform.position.x, limitMinX, limitMaxX);
        float clampedZ = Mathf.Clamp(transform.position.z, limitMinZ, limitMaxZ);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos el tag del objeto con el que colisionamos
        switch (collision.gameObject.tag)
        {
            case "Rojos":
                // Si el objeto tiene el tag "Rojos", disminuimos la velocidad
                velocidadMovimiento -= 2;
                GetComponent<AudioSource>().PlayOneShot(sonidom);
                Destroy(collision.gameObject);
                txtVelocidad.text = "Velocidad: " + velocidadMovimiento.ToString();
                break;
            case "Verdes":
                // Si el objeto tiene el tag "Verdes", aumentamos la velocidad
                velocidadMovimiento += 5;
                GetComponent<AudioSource>().PlayOneShot(sonidob);
                Destroy(collision.gameObject);
                txtVelocidad.text = "Velocidad: " + velocidadMovimiento.ToString();
                break;
            case "Rosas":
                // Si el objeto tiene el tag "Rosas", sumamos puntos
                puntos += 5;
                GetComponent<AudioSource>().PlayOneShot(sonidob);
                Destroy(collision.gameObject);
                if (puntos >= 20)
                {
                    SceneManager.LoadScene(0);
                }
                txtPuntos.text = "Puntos: " + puntos.ToString();
                break;
            case "Naranja":
                puntos -= 2;
                Debug.Log("Puntos disminuidos a: " + puntos);
                GetComponent<AudioSource>().PlayOneShot(sonidom);
                Destroy(collision.gameObject);
                txtPuntos.text = "Puntos: " + puntos.ToString();
                break;
        }
    } 
    public void TakeDamage (int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

    }

    private void Die ()
    {
        Debug.Log("El jugador ha muerto");
    }

    
}
