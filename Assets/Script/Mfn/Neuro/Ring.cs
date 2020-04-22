using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ring
{
    bool isClearEffect;

    Image back;
    Image front;

    RectTransform rtfBack;
    RectTransform rtfFront;

    public bool isPerfect { get; private set; }

    public Vector3 pos { get; private set; }

    public Vector3 effectPos{
        get
        {
            return new Vector3(pos.x, pos.y + 100);
        }
        set { }
    }

    float cnt;

    float speed;

    public bool isDead { get; private set; }
    public bool isFailedCombo { get; private set; }

    public void Create(GameObject _back, GameObject _front, GameObject _backParent, GameObject _frontParent)
    {
        back = Object.Instantiate(_back, _backParent.transform).GetComponent<Image>();
        front = Object.Instantiate(_front, _frontParent.transform).GetComponent<Image>();
        rtfBack = back.gameObject.GetComponent<RectTransform>();
        rtfFront = front.gameObject.GetComponent<RectTransform>();

        back.gameObject.SetActive(true);
        front.gameObject.SetActive(true);

        isClearEffect = false;
        isDead = false;
        isFailedCombo = false;
        cnt = 0;

    }

    public void UpdateExec()
    {
        float _move = -Time.deltaTime * speed;
        AddPos(new Vector3(_move, 0));
        if (pos.x < -800)
        {
            isDead = true;
        }

        if (isClearEffect)
        {
            cnt += Time.deltaTime*1f;
            if (cnt >= 1) {
                cnt = 1;
                isDead = true;
            }
            SetAlpha(1 - cnt);
        }
    }

    public void SetPos(Vector3 _pos)
    {
        pos = _pos;
        back.transform.localPosition = new Vector3(pos.x, pos.y);
        front.transform.localPosition = new Vector3(pos.x, pos.y);
    }

    void AddPos(Vector3 _addVec)
    {
        pos += _addVec;
        back.transform.localPosition = new Vector3(pos.x, pos.y);
        front.transform.localPosition = new Vector3(pos.x, pos.y);
    }

    void SetAlpha(float _alpha)
    {
        back.color = new Color(back.color.r, back.color.g, back.color.b,_alpha);
        front.color = new Color(front.color.r, front.color.g, front.color.b, _alpha);
    }

    public bool HitCheck(Vector3 _airPlanePos)
    {
        isPerfect = false;
        //既にくぐった。
        if (isClearEffect)
        {
            return false;
        }

        //x軸
        if(pos.x>_airPlanePos.x-10 && pos.x < _airPlanePos.x + 10)
        {
            //y軸
            if (_airPlanePos.y > pos.y - 150 && _airPlanePos.y < pos.y+150)
            {
                isClearEffect = true;
                //パーフェクトチェック ±2%
                if (_airPlanePos.y > pos.y - 150 * 0.02f && _airPlanePos.y < pos.y + 150 * 0.02f)
                {
                    isPerfect = true;
                }
                return true;
            }
        }

        return false;
    }

    //通り過ぎた
    public bool OverAirPlane(float _limitX)
    {
        if(isClearEffect ==false && pos.x <= _limitX)
        {
            isFailedCombo = true;
            return true;
        }
        return false;
    }

    public void SetHeight(float _height)
    {
        rtfBack.sizeDelta = new Vector2(rtfBack.sizeDelta.x,_height);
        rtfFront.sizeDelta = new Vector2(rtfFront.sizeDelta.x,_height);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Destroy()
    {
        Object.Destroy(back.gameObject);
        Object.Destroy(front.gameObject);
    }
}