using Ink.Parsed;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class NPCManager : MonoBehaviour
{
    public static NPCInstance NPCInstance { get; private set; }
    public NPCData npcData;
    public List<Transform> interestPoints;
    public bool patrol = false; // Flag to indicate if the NPC is patrolling

    public Animator Animator; // Reference to the Animator component for animations
    public CharacterController characterController; // Reference to the CharacterController for movement


    private void Start()
    {
        // Initialize the NPC instance
        NPCInstance = new NPCInstance(npcData, this);
        NarrativeManager.Instance.RegisterNPC(NPCInstance);

        Animator = GetComponentInChildren<Animator>();
        Animator = transform.GetChild(0).GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
       
    }

    private void Update()
    {
        if(characterController.velocity.magnitude==0)
        {
            Animator.SetBool("watching", true);
        }
        else
        {
            Animator.SetFloat("speed", characterController.velocity.magnitude);
        }


        if (patrol && GetComponent<CharacterController>().velocity.magnitude ==0 )
        {
            // If the NPC is patrolling, set a random interest point as the target
            if (interestPoints.Count > 0)
            {
                Transform targetPoint = interestPoints.Random<Transform>();
                GetComponent<AIDestinationSetter>().target = targetPoint;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MoveTo(Transform point)
    {
        if(point == null)
        {
            GetComponent<AIDestinationSetter>().target = interestPoints.Random<Transform>();
            return;
        }
        GetComponent<AIDestinationSetter>().target = point;
    }

    public void StartPatrol()
    {
        patrol = true;
    }
}

public enum NPC
{
    Commander,
    Scientist,
    Pilot1,
    Pilot2,
    Pilot3,
    Soldier
    // Add more characters as needed
}
