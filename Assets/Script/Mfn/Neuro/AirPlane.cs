using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AirPlane : MonoBehaviour
{
    const float SPEED = 100;

    public bool isAppear{ get { return (Mathf.Abs(transform.localPosition.y-homePosition.y)>1); } private set { } }
    public Vector3 pos { get { return transform.localPosition;} private set { } }

    Image airPlane;
    Text airPlaneCnt;
    //SimpleMove move;
    Vector3 homePosition;
    float baseY;
    float toY;

    float apperMoveX;

    float sinAngle;
    float cnt;

    bool isView = false;

    public float maxHalfRange = 1920/2;

    public bool isFadeOut { get; set; }

    public void Init()
    {
        airPlane = GetComponent<Image>();
        airPlaneCnt = gameObject.FindDescendant<Text>("AirPlaneCnt");
        //move = GetComponent<SimpleMove>();
        baseY = transform.localPosition.y;
        toY = transform.localPosition.y;
        homePosition = new Vector3(transform.localPosition.x,transform.localPosition.y);
        transform.localPosition = new Vector3(-1080/2-125,transform.localPosition.y);
        isAppear = false;
        sinAngle = 0;
        cnt = 0;
        isFadeOut = false;
        isView = (Hot2gApplication.Instance.state2 == Hot2gApplication.eState.OnHead);
    }

    float toYnow = 0;
    public void UpdateExec()
    {
        ChangeColorOnHead();
        //紙飛行機が表示されていないときは動かない
        if (!isView) { return; }
                
        //移動量
        float moveY = Time.deltaTime * SPEED;        
        if(moveY >  Mathf.Abs(toY - toYnow))
        {
            moveY = Mathf.Abs(toY - toYnow);
        }
        
        //上下
        if (transform.localPosition.y>toY)
        {
            moveY *= -1;
        }

        toYnow += moveY;

        transform.localPosition = new Vector3(transform.localPosition.x, toYnow + Yswing()/10);
    }

    public void StartAppear()
    {
        apperMoveX = homePosition.x - transform.localPosition.x;
        isAppear = true;
        sinAngle = 0;
        //        move.Move(homePosition.x, homePosition.y, 5);
        //animator1();
    }

    private void animator1()
    {
        Sequence sequence = DOTween.Sequence();
        float y0 = transform.localPosition.y;
        y0 = 0;
        float dy = 50;
        float t = 1f;
                
        sequence.Append(transform.DOLocalMoveY(y0-dy, t).SetEase(Ease.OutCubic)).SetRelative();
        sequence.Append(transform.DOLocalMoveY(y0 + dy, t).SetEase(Ease.InCubic)).SetRelative();
        sequence.Append(transform.DOLocalMoveY(y0 + dy, t).SetEase(Ease.OutCubic)).SetRelative();
        sequence.Append(transform.DOLocalMoveY(y0 - dy, t).SetEase(Ease.InCubic)).SetRelative();
        sequence.SetLoops(-1);
        sequence.Play();

        //transform.DOLocalMoveY(transform.localPosition.y-50, 5).SetEase(Ease.InOutBounce);
    }

    private float Yswing()
    {
        float _speed = 2;
        float _shake = 20;
        sinAngle += Time.deltaTime * _speed;
        return Mathf.Sin(sinAngle) * _shake;
    }

    public void UpdateApper()
    {
        if (transform.localPosition.x < homePosition.x)
        {                        
            transform.localPosition = new Vector3(transform.localPosition.x + apperMoveX * Time.deltaTime / 5, homePosition.y + Yswing());
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, homePosition.y + Yswing());
        }
        /*         
        else
        {
            if (Mathf.Abs(homePosition.y - transform.localPosition.y) < 5)
            {
                transform.localPosition = new Vector3(transform.localPosition.x,homePosition.y);
            }
            else
            {
                sinAngle += Time.deltaTime * _speed;
                float _y = Mathf.Sin(sinAngle) * _shake;
                transform.localPosition = new Vector3(transform.localPosition.x + apperMoveX * Time.deltaTime / 5, homePosition.y + _y);
            }
        }
        */
        ChangeColorOnHead();
    }
    
    public void ChangeColorOnHead()
    {
        cnt += Time.deltaTime;
        if (Hot2gApplication.Instance.state2 == Hot2gApplication.eState.OnHead)
        {
            isView = true;
            airPlane.color = new Color(1, 1, 1, 1f);
        }
        //頭から離れてる
        if (Hot2gApplication.Instance.state2 == Hot2gApplication.eState.NotOnHead)
        {
            isView = false;
            airPlane.color = new Color(1, 1, 1, 0.4f);
        }
    }

    float time_MoveSet=0;
    
    public void SetXbValue(float _xbValue)
    {
        toY = baseY + _xbValue;
        toYnow = transform.localPosition.y;

        /*
        if (toY < -_xbValue * 0.1) toY = -_xbValue * 0.1f;
        if (toY > _xbValue * 1.1) toY = _xbValue * 1.1f;
        //float dtimemove = 1f;
        //if (Time.realtimeSinceStartup - time_MoveSet > dtimemove)
        {
            transform.DOLocalMoveY(toY, dtimemove).SetEase(Ease.InOutSine);
            //transform.DOLocalMoveY(toY, Mathf.Abs(_xbValue/maxHalfRange)*2f).SetEase(Ease.InOutCubic);
            //Debug.Log("AAAA: " + _xbValue+": "+maxHalfRange);
            time_MoveSet = Time.realtimeSinceStartup;
        }
        */
    }

    public void ResetPosY()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0);
        baseY = transform.localPosition.y;
        toY = transform.localPosition.y;
        homePosition = new Vector3(transform.localPosition.x, transform.localPosition.y);
    }

    public void StartBreathFadeOut()
    {
        //move.Move(1080,1920,5);
        transform.DOLocalMove(new Vector3(1080, 1920, 5), 2).SetEase(Ease.InCubic);

        cnt = 0;
        isFadeOut = true;
    }

    public void UpdateBreathFadeOut()
    {
        cnt += Time.deltaTime;
//        airPlane.color = new Color(1,1,1,cnt/3f);
        if (cnt > 3)
        {
            airPlane.color = new Color(1, 1, 1,1);
            isFadeOut = false;
        }
    }

    public void SetTextCnt(string _textCnt)
    {
        airPlaneCnt.text = _textCnt;
    }

    public void SetVisibleTextCnt(bool value)
    {
        airPlaneCnt.gameObject.SetActive(value);
    }
}
