using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GameManager;

    //all Targets will be listed here so we can track player progress.
    public List<GameObject> Target_Agents;

    private void Awake()
    {
        GameManager = this;
    }

}
