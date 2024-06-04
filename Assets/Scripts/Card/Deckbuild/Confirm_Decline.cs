using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confirm_Decline : MonoBehaviour
{
    private Deckbuilding deck;

    private void Start()
    {
        deck = FindObjectOfType<Deckbuilding>();
    }

    public void ClearAllCards()
    {
        deck.tempList.Clear();
    }

    public void ConfirmDeck()
    {
        //change scene
        SceneManager.LoadScene("01_Gameplay", LoadSceneMode.Additive);
    }
}
