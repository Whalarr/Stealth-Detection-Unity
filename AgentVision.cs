using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVision : MonoBehaviour {

    public Camera Enemy_viewcam;
    public float view_Radius;
    [Range(0,360)]
    public float view_Angle;

    public LayerMask targetMask;
    public LayerMask ObstacleMask;

    public List<Transform> visibleTargets;

    // Use this for initialization
    void Start()
    {
        Enemy_viewcam = gameObject.GetComponentInChildren<Camera>();
        StartCoroutine("FindTargets_Delay", .2f);
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, view_Radius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
        
            if (Vector3.Angle(transform.forward,dirToTarget) < view_Angle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, ObstacleMask))
                {
                    visibleTargets.Add(target);
                    //target layer is just the player, so they will be visible to this agent in this case.
                    Debug.Log("You have been spotted by: " + gameObject.name);
                }
                else
                {
                   
                }
            }
        }   
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    IEnumerator FindTargets_Delay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    } 	
}
