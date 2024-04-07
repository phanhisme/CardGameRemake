using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blessing", menuName = "Blessings")]
public class DabriaBlessings : ScriptableObject
{
    public Sprite icon;
    public string blessingName;

    public string description;
}
