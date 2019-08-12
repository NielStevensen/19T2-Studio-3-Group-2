using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallListners : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CustomNetworkManager>().AddListners();
    }
}
