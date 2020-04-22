using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuCancel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.parent.Find("Common_Menu").gameObject.SetActive(false);
            gameObject.SetActive(false);
            if(WebView.Instance != null)
            {
                WebView.Instance.SetVisibility(true);
            }
        });   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
