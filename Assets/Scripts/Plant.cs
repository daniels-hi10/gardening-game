using UnityEngine;

public class Plant : MonoBehaviour
{
    public Player player; // player script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject CollidedObject = other.gameObject;

        if (CollidedObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
