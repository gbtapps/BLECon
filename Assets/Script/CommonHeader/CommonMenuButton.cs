using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.parent.Find("Common_Menu").gameObject.SetActive(true);
            transform.parent.Find("Cancel").gameObject.SetActive(true);
            if (WebView.Instance != null)
            {
                WebView.Instance.SetVisibility(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
