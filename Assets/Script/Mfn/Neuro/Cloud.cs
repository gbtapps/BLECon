using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public int yTop = 1920/2;
    public int yBottom = -1920 / 2;
    public float speed = 100;

    float width;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(Random.Range(0, 1080), Random.Range(yBottom, yTop));
        RectTransform rectTransform = GetComponent<RectTransform>();
        width = rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x-Time.deltaTime* speed, transform.localPosition.y);
        if(transform.localPosition.x<-540- width / 2)
        {
            transform.localPosition = new Vector3(540+ width / 2+100, Random.Range( yBottom, yTop));
        }
    }
}
