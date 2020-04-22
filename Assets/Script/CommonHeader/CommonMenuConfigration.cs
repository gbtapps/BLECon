using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonMenuConfigration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Text_btn").GetComponent<Button>().onClick.AddListener(() => {
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
