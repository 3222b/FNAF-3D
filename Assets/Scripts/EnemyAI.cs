using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Clock clock;
    public bool active = false;
    public Player target;

    public float targetRange = 5f;
    public int firstActiveNight = 1;
    public double activeTime = 0.5f;

    public Transform jumpscareCameraPosition;
    public Animator animator;
    public Light jumpscareLight;
    public AudioClip jumpScareSound;
    public Transform[] patrolPositions;
    public AudioClip[] walkSounds;
    public float patrolSpeed = 0.675f;
    public float chaseSpeed = 1.25f;

    private NavMeshAgent agent;
    private bool killedPlayer;
    private int patrolDestination;
    private float killTimer;

    private AudioSource walkAudioSource;
    private AudioSource jumpscareAudioSource;

    void NewPatrolDestination() => patrolDestination = Random.Range(0, patrolPositions.Length);

    void Start()
    {
        // Add Components
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.angularSpeed = 0f;
        agent.radius = 2f;
        agent.height = 4.8f;

        walkAudioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource.volume = 0.2f;
        walkAudioSource.spatialBlend = 1f;
        walkAudioSource.minDistance = 1f;
        walkAudioSource.maxDistance = 7f;

        jumpscareAudioSource = gameObject.AddComponent<AudioSource>();
        jumpscareAudioSource.clip = jumpScareSound;

        // Set initial patrol destination
        NewPatrolDestination();
    }

    void KillPlayer()
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        animator.SetBool("Jumpscare", true);
        jumpscareAudioSource.Play();
        agent.enabled = false;
        target.dead = true;
        killedPlayer = true;
        jumpscareLight.enabled = true;
    }

    private bool inPlayerBounds = false;
    private bool followingPlayer = false;
    private float walkSoundTimer;
    void Update()
    {
        if (clock.getTime() > 0.5f)
        {
            active = true;
        }

        animator.SetBool("Moving", agent.velocity.magnitude > 0.1);
        if (!animator.GetBool("Moving"))
            animator.speed = 1f;
        else
            animator.speed = Mathf.Max(0.1f, agent.velocity.magnitude);

        if (!agent.hasPath)
        {
            if (inPlayerBounds && followingPlayer)
            {
                followingPlayer = false;
            }
        }

        agent.enabled = active;

        animator.SetBool("Angry", followingPlayer);

        if (agent.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.velocity), Time.deltaTime * 8f);
            walkSoundTimer += Time.deltaTime * (animator.speed*2f);
            if (walkSoundTimer >= 1)
            {
                walkAudioSource.PlayOneShot(walkSounds[Random.Range(0, walkSounds.Length - 1)]);
                walkSoundTimer = 0;
            }
        }

        if (!target.dead && agent.enabled)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            var lineOfSight = transform.position;
            var targetLineOfSight = target.transform.position;
            targetLineOfSight.y += 0.675f;
            lineOfSight.y += 0.675f;
            Debug.DrawRay(lineOfSight, targetLineOfSight - lineOfSight, Color.white);
            if (Vector3.Distance(target.transform.position, transform.position) < targetRange)
            {
                RaycastHit raycastHit;
                Physics.Raycast(lineOfSight, targetLineOfSight - lineOfSight, out raycastHit, targetRange+1);
                if (raycastHit.transform.tag == "Player")
                {
                    followingPlayer = true;
                    inPlayerBounds = true;
                }
                if (Vector3.Distance(target.transform.position, transform.position) < 0.225f)
                {
                    KillPlayer();
                    return;
                }
            }
            else
            {
                followingPlayer = false;
                if (inPlayerBounds)
                {
                    inPlayerBounds = false;
                    NewPatrolDestination();
                }
            }

            if (inPlayerBounds && followingPlayer)
            {
                agent.speed = chaseSpeed;
                agent.SetDestination(target.transform.position);
                Vector3 targetLocation = target.transform.position;
                targetLocation.y += 0.5f;
            }
            else if (!followingPlayer)
            {
                agent.speed = patrolSpeed;
                // Debug.Log(patrolPositions[patrolDestination].name);
                agent.SetDestination(patrolPositions[patrolDestination].transform.position);
                if (Vector3.Distance(transform.position, patrolPositions[patrolDestination].transform.position) < 1f)
                {
                    NewPatrolDestination();
                }
            }
        }
        else if (killedPlayer)
        {
            var _direction = (jumpscareCameraPosition.position - target.cameraHolder.transform.position).normalized;
            var _lookRotation = Quaternion.LookRotation(_direction);
            target.cameraHolder.rotation = Quaternion.Lerp(target.cameraHolder.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * 12f);
            target.cameraHolder.position = jumpscareCameraPosition.position + (0.25f * transform.forward);
            killTimer += Time.deltaTime;
            if (killTimer >= 0.675)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
