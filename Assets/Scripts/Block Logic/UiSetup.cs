using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSetup : MonoBehaviour
{

    public SpriteRenderer[] hostTileExamples;
    public SpriteRenderer[] clientTileExamples;
    public SpriteRenderer hostFighter;
    public SpriteRenderer clientFighter;
    public SpriteRenderer hostType;
    public SpriteRenderer clientType;

    public Sprite[] fighters;
    public TileSet[] sets;

    public void SetupUI(bool isHost, int figherIndex, int setIndex)
    {
        if (isHost)
        {
            for (int i = 0; i < 4; i++)
            {
                hostTileExamples[i].sprite = sets[setIndex].set[i];
            }
            hostFighter.sprite = fighters[figherIndex];
            hostType.sprite = sets[setIndex].set[AvatarStats.allFighters[figherIndex].element];
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                clientTileExamples[i].sprite = sets[setIndex].set[i];
            }
            clientFighter.sprite = fighters[figherIndex];
            clientType.sprite = sets[setIndex].set[AvatarStats.allFighters[figherIndex].element];
        }
    }
}
