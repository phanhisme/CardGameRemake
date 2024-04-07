using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBlessings : MonoBehaviour
{
    //this script is used to enable all the scriptable objects blessing into the gameplay
    private DabriaBlessings blessings;

    private void Start()
    {
        
    }

    private void Blessing (DabriaBlessings blessing)
    {
        blessings = blessing;
    }
}
