using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerName;

    private int sentenceIndex = -1;
    public StoryScene currentScene;

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
        if (!isHidden)
        {
            anim.SetTrigger("Hide");
            isHidden = true;
        }
    }

    public void ShowDialogue()
    {
        anim.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        dialogueText.text = "";
    }

    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
    }

    public void PlayNextScene()
    {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
        speakerName.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        //speakerName.color = currentScene.sentences[sentenceIndex].speaker.textColor;
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
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
