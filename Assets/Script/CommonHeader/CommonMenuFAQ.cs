using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuFAQ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Text_btn").GetComponent<Button>().onClick.AddListener(() => {
            SceneFunc.ChangeScene(ConstData.EnumScene.T_TitleSelect);
            transform.parent.parent.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
