using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "XpData", menuName = "Create Xp Data", order = 0)]
public class XpData : ScriptableObject
{
    public int currentXp = 0;
    public int maxXp = 100;
    public int Level = 1;
}
