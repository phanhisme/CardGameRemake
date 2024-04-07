using UnityEngine;

[CreateAssetMenu(fileName ="NewSpeaker",menuName ="Speaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;
}
