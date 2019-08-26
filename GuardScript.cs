using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : MonoBehaviour
{
    AgentControl Controller;
    AgentManager Manager;
    private int CurrentWaypoint;

    private void Start()
    {      
        Manager = FindObjectOfType<AgentManager>();
        Controller = GetComponent<AgentControl>();
    }

    void Patrol()
    {
        // Returns if no points have been set up
        if (Manager.WayPoints.Length == 0)
            return;
        // Set the agent to go to the currently selected destination.
        Controller.AgentGoTo(Manager.WayPoints[CurrentWaypoint].transform.position);
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        CurrentWaypoint = (CurrentWaypoint + 1) % Manager.WayPoints.Length;
    }
}
