using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideInObject : MonoBehaviour
{
    //how many agents can be stored here? We want to have the player hide in the object, but we want to have them maybe hide bodies and such.
    public int Capacity;
    public List<GameObject> contents;
    public bool ContainsPlayer;
    //where we transport the player during this.
    public Transform Inside_Spot;
    //the point at which the player leaves the object.
    public Transform Exit_Point;

    //if player enters the trigger, they in within interaction range of the object. 
    //Thus we allow them to hide inside of the object upon pressing a button. we use
    //trigger stay so we can check for when the player inputs.
    private void OnTriggerStay(Collider other)
    {
        //if the player is within the trigger
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown("space"))
            {
                //we want the player to hide in the box.
                if(!ContainsPlayer)
                {
                    PlayerHideIn(other.gameObject);
                }
                else
                {
                    //the box contains the player 
                    PlayerLeaveHiding(other.gameObject);
                }          
            }
        }
    }

    void PlayerHideIn(GameObject Player)
    {
        ContainsPlayer = true;
        //hide the players visible attributes.
        SkinnedMeshRenderer[] meshrender = Player.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < meshrender.Length; i++)
        {
            meshrender[i].enabled = false;
        }

        //disable player movement
        Player.GetComponent<MovementInput>().enabled = false;
        Player.transform.position = Inside_Spot.position;
    }

    void PlayerLeaveHiding(GameObject Player)
    {
        ContainsPlayer = false;

        SkinnedMeshRenderer[] meshrender = Player.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < meshrender.Length; i++)
        {
            meshrender[i].enabled = true;
        }

        //disable player movement
        Player.GetComponent<MovementInput>().enabled = true;
        Player.transform.position = Exit_Point.position;
    }
}
