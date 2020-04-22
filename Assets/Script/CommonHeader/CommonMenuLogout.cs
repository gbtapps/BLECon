using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuLogout : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Text_btn").GetComponent<Button>().onClick.AddListener(() => {
            transform.parent.parent.gameObject.SetActive(false);
            transform.parent.parent.parent.Find("LogoutBG").gameObject.SetActive(true);
//            SceneFunc.ChangeScene(ConstData.EnumScene.S_1_FirstLogin);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
