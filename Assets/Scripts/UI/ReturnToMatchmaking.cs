using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReturnToMatchmaking : NetworkBehaviour
{
    // Start is called before the first frame update
    public void BackToMenu()
    {
        if (isClient)
        {
            NetworkManager.singleton.StopClient();
        }
        else
        {
            NetworkManager.singleton.StopHost();
        }
    }
}
