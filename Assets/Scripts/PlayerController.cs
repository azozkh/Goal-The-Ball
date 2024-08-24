using System.Collections;
using UnityEngine;
using Sample; 

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; 
    private float originalSpeed; 
    private Rigidbody rb; 

    private bool canKillGhosts = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed; 
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    public IEnumerator SpeedBoost(float multiplier, float duration)
    {
        speed = originalSpeed * multiplier; 
        yield return new WaitForSeconds(duration); 
        speed = originalSpeed; 
    }

    public IEnumerator KillGhostPower(float duration)
    {
        canKillGhosts = true; 

        GhostScript[] ghosts = FindObjectsOfType<GhostScript>();
        foreach (GhostScript ghost in ghosts)
        {
            ghost.RunAway(transform.position); 
        }

        yield return new WaitForSeconds(duration); 

        canKillGhosts = false; 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canKillGhosts && collision.gameObject.CompareTag("Ghost"))
        {
            Destroy(collision.gameObject); 
        }
    }
}