using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLiveCost : MonoBehaviour
{
    [SerializeField] [Min(1)] private int m_liveCost = 1;
    
    public int LiveCost => m_liveCost;
}
