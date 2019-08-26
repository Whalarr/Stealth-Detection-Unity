using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    Transform DoorObj;
    [SerializeField]
    bool isOpen = false;
    public bool Requires_Key;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //open the door if closed.
        if(!isOpen)
        {
            isOpen = true;
            DoorObj.position += new Vector3(0, 3, 0);
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        //close the door if open.
        if (isOpen)
        {
            isOpen = false;
            DoorObj.position -= new Vector3(0, 3, 0);
        }
    }
}
