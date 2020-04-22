using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rect = transform.GetComponent<RectTransform>();
        var basex = -rect.sizeDelta.x/2;
        var basey = -rect.sizeDelta.y/2;

        //  グラフ描画
        for (int i=0;i < 31; i++)
        {
            var obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(basex+2+i*7+7,basey+2);
            obj.transform.localScale = Vector3.one;
            var line = obj.AddComponent<LineRenderer>();
            line.sortingOrder = 1;
#if UNITY_EDITOR
            line.material = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
#else
            line.material = Resources.GetBuiltinResource<Material>("Default-Line.mat");
#endif
            line.alignment = LineAlignment.TransformZ;
            line.useWorldSpace = false;
            line.startColor = Color.red;
            line.endColor = Color.red;
            line.startWidth = 4.0f;
            line.endWidth = 4.0f;
            line.positionCount = 2;
            line.SetPosition(0, new Vector3(0,0));
            line.SetPosition(1, new Vector3(0,Random.Range(10.0f,100.0f)));
        }

        //  折れ線
        {
            var obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(basex + 2 + 7, basey + 2);
            obj.transform.localScale = Vector3.one;
            var line = obj.AddComponent<LineRenderer>();
            line.sortingOrder = 1;
#if UNITY_EDITOR
            line.material = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
#else
            line.material = Resources.GetBuiltinResource<Material>("Default-Line.mat");
#endif
            line.alignment = LineAlignment.TransformZ;
            line.useWorldSpace = false;
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
            line.numCornerVertices = 1;
            line.startWidth = 2.0f;
            line.endWidth = 2.0f;
            line.positionCount = 31;
            for(int day = 0;day < 31; day++)
            {
                line.SetPosition(day, new Vector3(day*7, Random.Range(10.0f,100.0f)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
