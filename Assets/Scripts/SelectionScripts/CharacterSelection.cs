using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameManager gm;
    private GameObject selector;
    

    public enum Character { Dabria, Daella, Asif, Amias, Maeve };
    public Character character;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        selector = GameObject.Find("SELECTOR");
    }
    public void DabriaChosen(ChooseCharacter Dabria)
    {
        character = Character.Dabria;
        gm.selectedChar = Dabria;
        Debug.Log("Chosen: Dabria. Starting game with " + gm.selectedChar.charName);

        RemoveScene();
    }

    public void DaellaChosen(ChooseCharacter Daella)
    {
        character = Character.Daella;
        gm.selectedChar = Daella;
        Debug.Log("Chosen: Daella. Starting game with " + gm.selectedChar.charName);
    }

    public void AsifChosen(ChooseCharacter Asif)
    {
        character = Character.Asif;
        gm.selectedChar = Asif;
        Debug.Log("Chosen: Asif. Starting game with " + gm.selectedChar.charName);
    }

    public void AmiasChosen(ChooseCharacter Amias)
    {
       character = Character.Amias;
        gm.selectedChar = Amias;
        Debug.Log("Chosen: Amias. Starting game with " + gm.selectedChar.charName);
    }

    public void MaeveChosen(ChooseCharacter Maeve)
    {
        character = Character.Maeve;
        gm.selectedChar = Maeve;
        Debug.Log("Chosen: Maeve. Starting game with " + gm.selectedChar.charName);
    }

    public void CheckForRelic(Character chosenChar)
    {
        switch (chosenChar)
        {
            case Character.Dabria:
                DabriaStarterRelic dabrelic = FindObjectOfType<DabriaStarterRelic>();
                dabrelic.RelicUIUpdate();
                

                break;

            case Character.Daella:
                break;
        }
    }

    private void RemoveScene()
    {
        selector.SetActive(false);
        CheckForRelic(character);
        gm.PlayerTurn();
    }
}
