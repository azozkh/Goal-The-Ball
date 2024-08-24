using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healthAmount = 20; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerCollisions player = other.GetComponent<PlayerCollisions>();
            if (player != null)
            {
                player.RestoreHealth(healthAmount); 
                Destroy(gameObject); 
            }
        }
    }
}
