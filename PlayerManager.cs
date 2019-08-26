using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //This script keeps track of the players progress and state within the play space.

    public enum Player_Disg {None}
    public Player_Disg Current_Disguise;

    public int Current_Clearance;
    public bool isTrespassing;
}
