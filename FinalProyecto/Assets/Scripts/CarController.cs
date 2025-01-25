using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    public Transform[] waypoints; // Puntos de control
    private float speed = 18.0f;
    private float turnSpeed = 10.0f;
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private int collisions = 0;
    private Puntaje pointSys;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float valor = 30.0f;
    private Cronometer time;
    public AudioClip choque;
    private AudioSource audioSonido;
    void Start() {
        //speed = Random.Range(15.0f, 20.0f);
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        audioSonido = GetComponent<AudioSource>();
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++) {
            waypoints[i] = waypointObjects[i].transform;
        }
        pointSys = FindObjectOfType<Puntaje>();
        time = FindObjectOfType<Cronometer>();
    }
    
    void Update() {
        if (isMoving && !time.isGameOver)
        {
            if (waypoints.Length == 0) return;
        
            Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized; // Dirección hacia el punto actual
        
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Rotar hacia el punto actual
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        
            transform.position += transform.forward * speed * Time.deltaTime; // Mover hacia el punto actual
        
            // Verificar si alcanzó el punto actual
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.5f) {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Ir al siguiente punto
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isMoving = false;
            collisions++;
            audioSonido.PlayOneShot(choque);
            if (collisions == 3)
            {
                int num = Random.Range(0, 10);
                if (num == 2 || num == 6)
                {
                    Instantiate(explosionParticle, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(gameObject);
            }
            pointSys.SumarPuntos(valor, 0);
        }
    }
}
