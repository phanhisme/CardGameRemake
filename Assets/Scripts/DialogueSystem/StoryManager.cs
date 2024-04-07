using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public GameScene currentScene;
    private DisplayText dialoguePanel;
    public ChooseOptionController chooseController;

    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    void Start()
    {
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;

            dialoguePanel = this.GetComponent<DisplayText>();
            dialoguePanel.PlayScene(storyScene);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == State.IDLE && dialoguePanel.IsCompleted())
            {
                if (dialoguePanel.IsLastSentence())
                {
                    PlayScene((currentScene as StoryScene).nextScene);
                }

                dialoguePanel.PlayNextScene();
            }
        }
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;

        currentScene = scene;
        dialoguePanel.HideDialogue();
        yield return new WaitForSeconds(1f);

        if(scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;

            //change background here (if needed)

            yield return new WaitForSeconds(1f);
            dialoguePanel.ClearText();
            dialoguePanel.ShowDialogue();
            yield return new WaitForSeconds(1f);
            dialoguePanel.PlayScene(storyScene);

            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }
}
