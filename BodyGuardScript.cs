using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGuardScript : MonoBehaviour
{
    AgentControl Controller;
    [Header("A reference to the VIP this agent is guarding.")]
    public VIPScript Protect_Client;

    //the vicinity of which the guard can be from the vip.
    int min_Dist;
    int max_Dist;

    //The bodyguards role is to remain within the nearby vicinity of their allocated VIP or to safely escort the VIP around the map/protect them from the player.
    //Bodyguards must also be able to designate safe spaces to hide the vip or attempt to escort the vip out of the map.

    //currently, vip's are just normal civillians and will wonder around the map and go to specific locations on chance. VIP's also have master level access and can go anywhere.
    //in the case a VIP doesnt have access to the entire map, we make sure the bodyguards clearance matches that of their VIP.

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<AgentControl>();
        //pick a vip.
        if(GameController.GameManager.Target_Agents.Count >0)
        {
            Protect_Client = PickVIP(GameController.GameManager.Target_Agents);
        }    
        min_Dist = Random.Range(2, 4);
        max_Dist = Random.Range(5, 7);


        //under normal circumstances, the bodyguard should update their knowledge of the vips whereabouts every so often.  
        StartCoroutine(VIP_Track(7.0f));
    }

    //sets the vip for the agent.
    VIPScript PickVIP(List<GameObject> VIPs)
    {
       int i= Random.Range(0, VIPs.Count);
       VIPScript chosenVip = VIPs[i].GetComponent<VIPScript>();
       Controller.Agent_Clearance = chosenVip.gameObject.GetComponent<AgentControl>().Agent_Clearance;
       return chosenVip;
    }

    //updates the bodyguards knowledge of where their vip currently is. We dont want this to be constant in some cases so there are points where the bodyguard
    //may not be as close to the vip.
    public void FindVIP()
    {
        Vector3 VIPPos = Protect_Client.transform.position;
        Debug.DrawLine(transform.position, VIPPos, Color.yellow);         
        //Now that we know where the vip is, we want to check the distance between the bodyguard and the vip, if its too far, we reduce the distance between them.
        //We also want to space the bodyguards out if possible so they position roughly in a circle around the vip. The size of this circle should change
        //depending on how crowded an area is as well as in relation to the players pursuit of the vip. For now, we randomise the min/max distances the guard
        //can be from the vip.
        float dist = Vector3.Distance(transform.position, VIPPos);
        //if dist is less than the max, we are already within the circle. We also want to make sure the minimum distance is being met.
        if(dist > max_Dist)
        {
            //vip is too far away from the guards maximum allowed distance, we move the guard towards the vip until they are within range.
            Vector3 fromOriginToObj = transform.position - VIPPos;
            fromOriginToObj *= max_Dist / dist; //mult by rad , div by dist.
            Controller.AgentGoTo((VIPPos + fromOriginToObj));
        }

        if(dist < min_Dist)
        {
            //bodyguard is too close to the vip, set the bodyguard to wander to a random destination.
            Controller.SetRandomDestination();
        }
    }

    IEnumerator VIP_Track(float delay)
    {
        if(Protect_Client == null)
        {
            Protect_Client = PickVIP(GameController.GameManager.Target_Agents);
        }

        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVIP();
        }
    }
}
