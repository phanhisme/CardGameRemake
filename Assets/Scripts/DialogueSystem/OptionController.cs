using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionController : MonoBehaviour
{
    private StoryScene scene;
    private TextMeshProUGUI text;
    private ChooseOptionController controller;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public float GetHeight()
    {
        return text.rectTransform.sizeDelta.y * text.rectTransform.localScale.y;
    }

    public void SetUp(ChooseScene.ChooseLabel label,ChooseOptionController controller, float y)
    {
        scene = label.nextScene;
        text.text = label.text;

        this.controller = controller;

        Vector3 position = text.rectTransform.localPosition;
        position.y = y;
        text.rectTransform.localPosition = position;
    }
}
