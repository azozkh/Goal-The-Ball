using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    public class GhostScript : MonoBehaviour
    {
        private Animator Anim;
        private CharacterController Ctrl;
        private Vector3 MoveDirection = Vector3.zero;
        private Transform player;

        private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
        private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
        private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
        private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
        private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");

        [SerializeField] private SkinnedMeshRenderer[] MeshR;
        private float Dissolve_value = 1;
        private bool DissolveFlg = false;

        [SerializeField] private float Speed = 4;
        public int attackDamage = 10;
        public float attackRange = 2.0f;
        public float attackCooldown = 1.5f;
        public float runAwaySpeed = 6.0f; 

        private float lastAttackTime = 0;

        private const int Dissolve = 1;
        private const int Attack = 2;
        private const int Surprised = 3;
        private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
        {
            {Dissolve, false },
            {Attack, false },
            {Surprised, false },
        };

        private bool isRunningAway = false; 

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            Ctrl = this.GetComponent<CharacterController>();

            player = GameObject.FindWithTag("Player").transform;
            if (player != null)
            {
                Debug.Log("Player found: " + player.name);
            }
            else
            {
                Debug.LogError("Player not found! Ensure the player object is tagged as 'Player'.");
            }
        }

        void Update()
        {
            STATUS();
            ApplyGravity();

            if (player != null && !PlayerStatus.ContainsValue(true))
            {
                if (isRunningAway)
                {
                    RunAwayFromPlayer(); 
                }
                else
                {
                    MoveTowardsPlayer();
                }
            }
            else if (PlayerStatus.ContainsValue(true))
            {
                int status_name = 0;
                foreach (var i in PlayerStatus)
                {
                    if (i.Value == true)
                    {
                        status_name = i.Key;
                        break;
                    }
                }
                if (status_name == Dissolve)
                {
                    PlayerDissolve();
                }
                else if (status_name == Attack)
                {
                    RotateTowardsPlayer((player.position - transform.position).normalized);
                    PlayerAttack();
                }
            }

            if (DissolveFlg)
            {
                PlayerDissolve();
            }
        }

        private void STATUS()
        {
            if (DissolveFlg)
            {
                PlayerStatus[Dissolve] = true;
            }
            else
            {
                PlayerStatus[Dissolve] = false;
            }

            if (Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == SurprisedState)
            {
                PlayerStatus[Surprised] = true;
            }
            else
            {
                PlayerStatus[Surprised] = false;
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;

           
            direction += AvoidOtherGhosts();

            RotateTowardsPlayer(direction);

            MoveDirection = new Vector3(direction.x * Speed, MoveDirection.y, direction.z * Speed);
            Ctrl.Move(MoveDirection * Time.deltaTime);
            Anim.SetBool("isWalking", true);

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    PlayerAttack();
                    lastAttackTime = Time.time;
                }
            }
        }

        private void RunAwayFromPlayer()
        {
            Vector3 directionAway = (transform.position - player.position).normalized;

            RotateTowardsPlayer(-directionAway); 

            MoveDirection = new Vector3(directionAway.x * runAwaySpeed, MoveDirection.y, directionAway.z * runAwaySpeed);
            Ctrl.Move(MoveDirection * Time.deltaTime);

            Anim.SetBool("isRunning", true);
        }

        private void RotateTowardsPlayer(Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);
        }

        private void ApplyGravity()
        {
            if (Ctrl.enabled)
            {
                if (!CheckGrounded())
                {
                    MoveDirection.y -= 9.81f * Time.deltaTime;
                }
                else
                {
                    MoveDirection.y = 0;
                }
                Ctrl.Move(MoveDirection * Time.deltaTime);
            }
        }

        private bool CheckGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, Ctrl.height / 2 + 0.1f);
        }

        private void PlayerDissolve()
        {
            Dissolve_value -= Time.deltaTime;
            for (int i = 0; i < MeshR.Length; i++)
            {
                MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
            }
            if (Dissolve_value <= 0)
            {
                Ctrl.enabled = false;
            }
        }

        private void PlayerAttack()
        {
            PlayerCollisions playerCollisions = player.GetComponent<PlayerCollisions>();
            if (playerCollisions != null)
            {
                playerCollisions.playerHealth -= attackDamage;
                playerCollisions.UpdateHealthText();

                if (playerCollisions.playerHealth <= 0)
                {
                    playerCollisions.RestartGame();
                }
            }
        }

        public void RunAway(Vector3 playerPosition)
        {
            isRunningAway = true; // Set the flag to make the ghost run away
            StartCoroutine(StopRunningAfterDelay(5.0f)); // Stop running after 5 seconds
        }

        private IEnumerator StopRunningAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isRunningAway = false;
            Anim.SetBool("isRunning", false);
        }

        private Vector3 AvoidOtherGhosts()
        {
            Vector3 avoidance = Vector3.zero;
            float avoidanceRadius = 1.5f; 

            GhostScript[] allGhosts = FindObjectsOfType<GhostScript>();
            foreach (var otherGhost in allGhosts)
            {
                if (otherGhost != this)
                {
                    float distance = Vector3.Distance(transform.position, otherGhost.transform.position);
                    if (distance < avoidanceRadius)
                    {
                        avoidance += transform.position - otherGhost.transform.position;
                    }
                }
            }

            return avoidance.normalized * 0.5f; 
        }
    }
}
