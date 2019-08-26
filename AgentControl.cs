using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    //type of agent
    public enum Agent_Category {Civillian, Guard};
    //what the agent should be doing.
    public enum Agent_Role {None, Worker, BodyGuard, VIP};
    //any specific tasks this agent has to attend or places this agent should commonly be.
    public enum Agent_Job {None ,Chef , Gardener, Scientist};
    //Where is the agent allowed to go.
    public enum Agent_Credentials {None, Basic, Advanced, Master};

    public Agent_Category Agent_Cat;
    public Agent_Role Agent_Rol;
    public Agent_Job Agent_Occupation;
    public Agent_Credentials Agent_Clearance;
    //makes for quicker cred checks, we convert the clearence to an int value.
    public int clearance_level;

    NavMeshAgent Agent;
    //this will keep track of general information that can be assigned to each agents knowledge and goals.
    AgentManager Manager;

    public enum Agent_State {Idle ,Walk , PendingWalk };   
    public Agent_State Move_State;

    public float Wander_Radius;
   
    private bool isDead;
    public GameObject Ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        switch (Agent_Clearance)
        {
            case Agent_Credentials.None:
                clearance_level = 0;
                break;

            case Agent_Credentials.Basic:
                clearance_level = 1;
                break;

            case Agent_Credentials.Advanced:
                clearance_level = 2;
                break;

            case Agent_Credentials.Master:
                clearance_level = 3;
                break;
        }

        Agent = this.GetComponent<NavMeshAgent>();
        Manager = FindObjectOfType<AgentManager>();
        Move_State = Agent_State.Idle;
        ConfigureAgent(Agent_Cat,Agent_Rol,Agent_Occupation,Agent_Clearance);
    }

    //this script will generate agents properties depending on the enums listed above, saying what role the agent is and what they need to be doing. as well
    //as loading/generating any other properties.
    void ConfigureAgent(Agent_Category cat, Agent_Role role, Agent_Job job, Agent_Credentials cred)
    {
        if(cat == Agent_Category.Civillian)
        {
            gameObject.AddComponent<CivillianScript>();

            if (role == Agent_Role.VIP)
            {
                //we either run the VIP commands or we give the agent a script with the VIP commands and rules.
                gameObject.AddComponent<VIPScript>();
            }

        }

        if(cat == Agent_Category.Guard)
        {
            //anything outside of the bodyguard scope is considered a regular guard, they should either hold ground or patrol an area.
            gameObject.AddComponent<GuardScript>();
            //do guard stuff.
            if (role == Agent_Role.BodyGuard)
            {
                //bodyguards must stay within close range of their VIP at all times. Since we can have more than one VIP at a time, we have the guards select a vip
                //Only if the VIP allows for guards.
                gameObject.AddComponent<BodyGuardScript>();
            }
           
        }
        
    }

    private void Update()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    //agent has reached destination
                    Move_State = Agent_State.Idle;
                }
            }
        }

      
    }

    //the agent will pick a destination depending on their agent type, civillians will pick an interactible object within their vicinity or wander around until they find something.
    public void AgentWander()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Wander_Radius, 1<<10);
        if(hitColliders.Length > 0)
        {
            //we want the agent to avoid repeatingly going to the same object once they first find it. So we make it a chance that they will engage with the item,
            //otherwise they will just default to more wandering for now. giving 1 in 10 that they will go to the object and not wander.
            int Engage_Chance = Random.Range(0, 10);
            if(Engage_Chance == 10)
            {
                int pick = Random.Range(0, hitColliders.Length);
                Transform DestTrans = hitColliders[pick].transform;
                AgentGoTo(DestTrans.position);
            }
            else
            {
                SetRandomDestination();
            }          
        }
        else
        {
            //for wander, we want the agent to either pick a place nearby to them to go, or continue wandering around picking random locations nearby and pathing to them.
            SetRandomDestination();
        }      
    }

    public void SetRandomDestination()
    {
        Vector3 NewDest = RandomNavSphere(transform.position, Wander_Radius, -1);
        AgentGoTo(NewDest);
    }

    Vector3 RandomNavSphere(Vector3 OriginPos,float dist, int layerMask)
    {
        Vector3 RandDir = Random.insideUnitSphere * dist;
        RandDir += OriginPos;
        NavMesh.SamplePosition(RandDir, out NavMeshHit NavHit, dist, layerMask);
        return NavHit.position;
    }

    //we have this coroutine send the agent to their destination and return a value once they have reached that goal.
    public void AgentGoTo(Vector3 Destination)
    {
        Agent.SetDestination(Destination);    
        Move_State = Agent_State.Walk;      
    }

    //for dead/unconscious agents we use a ragdoll.

    private void Agent_ToggleDead()
    {
        isDead = !isDead;

        if(isDead)
        {
            CopyAgentTransform(transform, Ragdoll.transform, Agent.velocity);
            Ragdoll.gameObject.SetActive(true);
            gameObject.SetActive(false);
            Agent.enabled = false;
        }
        else
        {
            //switch back to model and disable the ragdoll.
            Ragdoll.gameObject.SetActive(false);
            gameObject.SetActive(true);
            Agent.enabled = true;
        }
    }

    private void CopyAgentTransform(Transform sourceTrans, Transform destTrans, Vector3 velocity)
    {
        if(sourceTrans.childCount != destTrans.childCount)
        {
            Debug.Log("Failed copy transform!");
            return;
        }

        for (int i = 0; i < sourceTrans.childCount; i++)
        {
            var source = sourceTrans.GetChild(i);
            var dest = destTrans.GetChild(i);
            dest.position = source.position;
            dest.rotation = source.rotation;
            var rb = dest.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = velocity;
            }
            CopyAgentTransform(source, dest, velocity);
        }
    }

}
