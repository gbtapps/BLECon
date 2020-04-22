using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonBackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            SceneFunc.BackScene();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
