using UnityEngine;
using UnityEngine.AI;

public class DogWander : MonoBehaviour
{
    public float wanderRadius = 10f;    // Qué tan lejos puede ir del punto actual
    public float wanderTimer = 5f;      // Cada cuánto buscará un nuevo destino

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Cada cierto tiempo, busca una nueva posición aleatoria
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    // Genera una posición aleatoria válida dentro del NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
