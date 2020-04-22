using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScaler : MonoBehaviour {

    public bool nowLocalMove=false;
    public bool nowMove = false;
    float time;
    Vector3 vec;
    Vector3 goalPos;

    Vector3 startPos;

    public Vector3 autoMoveVec = new Vector3(0,0,0);
    public bool isAutoMove = false;

    float timeCnt;

    //今の座標からx,y分移動
    public void MoveLocal(float x,float y,float _time=0)
    {
        vec = new Vector3(x,y,0);
        if(_time ==0)
        {
            gameObject.transform.localScale += vec;
        }
        else
        {
            time = _time;
            nowLocalMove = true;
            goalPos = gameObject.transform.localScale + vec;
        }
    }

    //x,yに移動
    public void Move(float x, float y, float _time = 0)
    {
        goalPos = new Vector3(x, y, 0);
        if (_time == 0)
        {
            gameObject.transform.localScale = new Vector3(x, y, 0);
        }
        else
        {
            time = _time;
            timeCnt = 0;
            nowMove = true;
            startPos = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            vec = goalPos-gameObject.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update () {
		if(nowLocalMove)
        {
            Vector3 frameVec = vec * Time.deltaTime/time;
            gameObject.transform.localScale += frameVec;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                gameObject.transform.localScale = goalPos;
                nowLocalMove = false;
            }
        }

        if (nowMove)
        {
            timeCnt += Time.deltaTime / time;
            gameObject.transform.localScale = startPos + vec* timeCnt;
            if (timeCnt >= 1)
            {
                gameObject.transform.localScale = new Vector3(goalPos.x, goalPos.y, goalPos.z);
                nowMove = false;
            }
        }

        if (isAutoMove)
        {
            gameObject.transform.localScale += autoMoveVec*Time.deltaTime;
        }
    }
}
