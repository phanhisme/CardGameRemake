using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewStoryScene",menuName ="New Story Scene")]
[System.Serializable]
public class StoryScene : ScriptableObject
{
    public List<Sentence> sentences;

    public Sprite background;
    public GameScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}

public class GameScene : ScriptableObject { }
