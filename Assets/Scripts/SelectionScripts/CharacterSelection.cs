using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private ChooseCharacter chosenChar;
    
    private bool dabriaChosen = false;
    private bool daellaChosen = false;
    private bool asifChosen = false;
    private bool amiasChosen = false;
    private bool maeveChosen = false;

    private void Start()
    {
        chosenChar = FindObjectOfType<ChooseCharacter>();
    }
    public void DabriaChosen()
    {
        dabriaChosen = true;
        Debug.Log("Chosen: Dabria. Starting game with " + chosenChar.charName);
    }

    public void DaellaChosen()
    {
        daellaChosen = true;
        Debug.Log("Chosen: Daella. Starting game with " + chosenChar.charName);
    }

    public void AsifChosen()
    {
        asifChosen = true;
        Debug.Log("Chosen: Asif. Starting game with " + chosenChar.charName);
    }

    public void AmiasChosen()
    {
        amiasChosen = true;
        Debug.Log("Chosen: Amias. Starting game with " + chosenChar.charName);
    }

    public void MaeveChosen()
    {
        maeveChosen = true;
        Debug.Log("Chosen: Maeve. Starting game with " + chosenChar.charName);
    }
}
