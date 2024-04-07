using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public bool dabriaChosen = false;
    public bool daellaChosen = false;
    public bool asifChosen = false;
    public bool amiasChosen = false;
    public bool maeveChosen = false;

    public void DabriaChosen()
    {
        dabriaChosen = true;
    }

    public void DaellaChosen()
    {
        daellaChosen = true;
    }

    public void AsifChosen()
    {
        asifChosen = true;
    }

    public void AmiasChosen()
    {
        amiasChosen = true;
    }

    public void MaeveChosen()
    {
        maeveChosen = true;
    }
}
