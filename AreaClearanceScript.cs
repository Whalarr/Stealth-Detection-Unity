using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AreaClearanceScript : MonoBehaviour
{
    //to be attached to invisible trigger box colliders, if an ai agent enters the area, we check their clearence level.
    //if the agent hasnt got the clearence, we command them to relocate.
    //this also lets us designate spaces to particular job roles and functions so agents can navigate around the map to fulfill 
    //varying requirements.
    public string Area_Name;
    //equates to the int value in the agent control script. each value associates with a level of access.
    public int access_required;
    public GameObject AccessDeniedPrefab;

    private void OnTriggerEnter(Collider other)
    {
        //if the other object is a navagent.
       if(other.gameObject.GetComponent<AgentControl>())
        {
            AgentControl other_agent = other.gameObject.GetComponent<AgentControl>();
            //compare clearance of agent against the access level of this space.
            if(other_agent.clearance_level >= access_required)
            {
                //has clearance, we do nothing.
            }
            else
            {
                //doest have clearance. We tell the agent to go away.
                Debug.Log(other_agent.name + " is not authorised to access : " + Area_Name);
                //spawns an object to show visual feedback that an agent has been denied access.
                Instantiate(AccessDeniedPrefab, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 2.0f, other.gameObject.transform.position.z), Quaternion.identity, other.gameObject.transform);
                other_agent.AgentWander();
            }                     
        }

       //we want an event for the player as well to display whether or not they are allowed into a specific area. Works into the keycards/disguises to add later.
       //Display on player UI that they are tresspassing if they are by checking the players current credentials
       if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerManager>().Current_Clearance >= access_required)
            {
                //player has clearance at this moment in time.
                other.gameObject.GetComponent<PlayerManager>().isTrespassing = false;
            }
            else
            {
                //Player is currently trespassing.
                other.gameObject.GetComponent<PlayerManager>().isTrespassing = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Player has left the area and is no longer trespassing.
            other.gameObject.GetComponent<PlayerManager>().isTrespassing = false;         
        }
    }
}
