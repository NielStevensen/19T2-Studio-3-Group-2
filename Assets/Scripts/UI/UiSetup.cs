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


    public Text[] hostInfo;
    public Text[] clientInfo;
    public Sprite[] fighters;
    public TileSet[] sets;

    public void SetupUI(bool isHost, bool shouldUseHost, int figherIndex, int setIndex)
    {
		//whether or not the host's fighter and type are updated
		if (isHost)
        {
            hostFighter.sprite = fighters[figherIndex];
            hostType.sprite = sets[setIndex].set[AvatarStats.allFighters[figherIndex].element];
        }
        else
        {
            clientFighter.sprite = fighters[figherIndex];
            clientType.sprite = sets[setIndex].set[AvatarStats.allFighters[figherIndex].element];
        }

		//whether or not the host's type chart is affected
		if (shouldUseHost)
		{
			for (int i = 0; i < 4; i++)
			{
				hostTileExamples[i].sprite = sets[setIndex].set[i];
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				if (clientTileExamples[i] != null)
				{
					clientTileExamples[i].sprite = sets[setIndex].set[i];
				}
			}
		}
    }
}
