using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivillianScript : MonoBehaviour
{
    AgentControl Controller;
    AgentManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        Manager = FindObjectOfType<AgentManager>();
        Controller = GetComponent<AgentControl>();
    }

    private void Update()
    {
        //if we are idle, we do behaviour for moving
        if (Controller.Move_State == AgentControl.Agent_State.Idle)
        {
            Controller.AgentWander();
        }
    }
}
