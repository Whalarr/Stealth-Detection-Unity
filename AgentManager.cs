using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [Header("For WayPoint Patrols")]
    public GameObject[] WayPoints;

    private void Awake()
    {
        WayPoints = GameObject.FindGameObjectsWithTag("PatrolWayPoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
