using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOptionController : MonoBehaviour
{
    public OptionController option;
    private StoryManager storyManager;
    private RectTransform rectTransform;

    private Animator anim;

    private float labelHeight = -1;

    void Start()
    {
        storyManager = FindObjectOfType<StoryManager>();
        rectTransform = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }

    public void SetupChoose(ChooseScene scene)
    {
        DestroyLabel();
        anim.SetTrigger("Show");

        for(int index = 0; index < scene.labels.Count; index++)
        {
            OptionController newOption = Instantiate(option.gameObject,transform).GetComponent<OptionController>();
            //newOption.scene = scene.labels[index].nextScene;

            if (labelHeight == -1)
            {
                labelHeight = newOption.GetHeight();
            }

            newOption.SetUp(scene.labels[index], this, CalculateLabelPosition(index, scene.labels.Count));
        }

        Vector2 size = rectTransform.sizeDelta;
        size.y = (scene.labels.Count + 2) * labelHeight;
        rectTransform.sizeDelta = size;
    }

    public void PerformChoose(GameScene scene)
    {
        storyManager.PlayScene(scene);
        anim.SetTrigger("Hide");
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        if (labelCount % 2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else if (labelIndex > labelCount / 2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
                return 0;
        }

    }
    private void DestroyLabel()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
   
}
