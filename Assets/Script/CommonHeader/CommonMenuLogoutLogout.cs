using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuLogoutLogout : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            transform.parent.parent.gameObject.SetActive(false);
            CommonData.logout = true;
            SceneFunc.ChangeScene(ConstData.EnumScene.T_TitleSelect);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
