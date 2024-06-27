using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSlashes : MonoBehaviour
{
    public Animator slashAnim;
    public GameObject slashObject;

    public void DisableSlash()
    {
        Debug.Log("disable");
        slashAnim.SetTrigger("Empty");
        slashObject.SetActive(false);
    }
}
