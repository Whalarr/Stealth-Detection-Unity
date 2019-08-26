using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIPScript : MonoBehaviour
{
    AgentControl Controller;
    [Header("True if this VIP requires bodyguards.")]
    public bool Guarded;

    public int MaxGuardCount;
    List<BodyGuardScript> GuardList;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<AgentControl>();
        //for now, we have the vip request guarding based on random chance.
        Guarded = (Random.value > 0.5f);
        //Add the VIP to the target list.
        GameController.GameManager.Target_Agents.Add(gameObject);
        if(Guarded == true)
        {
            MaxGuardCount = Random.Range(1, 5);
        }
       
    }
}
