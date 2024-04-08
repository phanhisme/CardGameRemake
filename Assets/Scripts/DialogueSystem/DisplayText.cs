using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerName;

    private int sentenceIndex = -1;
    private StoryScene currentScene;

    private State state = State.COMPLETED;

    private Animator anim;
    private bool isHidden = false;

    private enum State
    {
        PLAYING, COMPLETED
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void HideDialogue()
    {
        //hide dialogue to chose the next scene (through option)

        if (!isHidden)
        {
            anim.SetTrigger("Hide");
            isHidden = true;
        }
    }

    public void ShowDialogue()
    {
        //show dialogue
        anim.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        //clear everything
        dialogueText.text = "";
    }

    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextScene();
    }

    public void PlayNextScene()
    {
        //type word by word effect
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));

        //set & change speaker name
        speakerName.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        //speakerName.color = currentScene.sentences[sentenceIndex].speaker.textColor;
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        //check if this is the last sentence of the scene
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            dialogueText.text += text[wordIndex];
            yield return new WaitForSeconds(0.05f);
            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }
}
