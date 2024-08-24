using UnityEngine;

public class KillGhostPowerUp : MonoBehaviour
{
    public float powerDuration = 5.0f; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(player.KillGhostPower(powerDuration)); 
                Destroy(gameObject); 
            }
        }
    }
}
