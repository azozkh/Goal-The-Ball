using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float speedMultiplier = 2.0f; 
    public float duration = 5.0f; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(player.SpeedBoost(speedMultiplier, duration)); 
                Destroy(gameObject); 
            }
        }
    }
}
