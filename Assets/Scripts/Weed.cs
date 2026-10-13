using UnityEngine;

public class Weed : MonoBehaviour
{
    public Player player; // player script
    public Spawner spawner; // spawner script
    private Vector3 pos; // the weed's position

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        spawner = FindAnyObjectByType<Spawner>();
        pos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject CollidedObject = other.gameObject;

        if (CollidedObject.CompareTag("Player"))
        {
            if (spawner != null)
            {
                spawner.FreeUpSpot(pos);
            }

            player.Score -= 1;
            Debug.Log("Score: " + player.Score);
            Destroy(gameObject);
        }
    }
}
