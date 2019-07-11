using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class mobileButtonToggle : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button Bar = gameObject.GetComponent<Button>();
        Bar.enabled = false;
#if UNITY_EDITOR
        if (isLocalPlayer)
        {
            Bar.enabled = true;
        }
#endif   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
