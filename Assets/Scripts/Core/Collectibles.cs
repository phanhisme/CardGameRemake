using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private SpriteRenderer visualImage;
    private ParticleSystem collectParticle;
    private bool collected = false;

    private CollectiblesDisplay displayScript;

    private void Awake()
    {
        displayScript = FindObjectOfType<CollectiblesDisplay>();
        //visualImage = this.visualImage.GetComponentInChildren<SpriteRenderer>();
        //collectParticle = this.GetComponentInChildren<ParticleSystem>();
    }

    public void LoadData(GameData data)
    {
        data.collectibles.TryGetValue(id, out collected);
        if (collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData (ref GameData data)
    {
        if (data.collectibles.ContainsKey(id))
        {
            data.collectibles.Remove(id);
        }

        data.collectibles.Add(id, collected);
    }

    public void OnClickCheck()
    {
        displayScript.currentCollectibles++;
        collected = true;
        Debug.Log("clicked on " + id);
        Destroy(this.gameObject, 0.1f);
    }
}
