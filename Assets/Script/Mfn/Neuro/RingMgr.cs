using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingMgr
{
    public const int ONE_SET = 10;          //リングの群れの数
    float RING_PUT_INTERVAL = 1080 * 0.125f;

    float[] RING_SIZE =
    {
        0.063f,
        0.069f,
        0.075f,
        0.081f,
        0.088f,
        0.094f,
        0.100f,
        0.106f,
        0.113f,
        0.125f
    };


    List<Ring> rings = new List<Ring>();
    GameObject backParent;
    GameObject frontParent;
    GameObject back;
    GameObject front;

    //エフェクト
    GameObject effParent;
    GameObject effBase;
    List<GameObject> effList = new List<GameObject>();

    //パーティクル
    RenderTexture particleTexture;
    Camera particleCamera;
    GameObject particlePrefab;
    RawImage particleRawImage;
    ParticleSystem particle;
    GameObject particleParent;


    bool isUp = true;

    Combo combo;
    public int comboNum { get; private set; }

    float addCnt;

    public int level { get; private set; }

    public bool oneSetClear { get { return (clearNum >= ONE_SET); } private set { } }

    public int clearNum { get; private set;}


    int totalRings;
    int totalClear;
    public int totalperfect { get; private set; }

    public int score { get {
            if (totalRings>0)
            {
                return (int)(((float)totalClear / (float)totalRings)*100f);
            }
            else
            {
                return 0;
            }
        } private set { } }

    public void Init()
    {
        backParent = GameObject.Find("RingBackParent");
        frontParent = GameObject.Find("RingFrontParent");
        back = GameObject.Find("Ring_Back");
        front = GameObject.Find("Ring_Front");
        back.SetActive(false);
        front.SetActive(false);

        combo = GameObject.Find("Combo").GetComponent<Combo>();
        combo.Init();
        combo.gameObject.SetActive(false);

        effParent = GameObject.Find("ParticleParent");
        //effBase = GameObject.Find("ParticleRawImage");
        //effBase.SetActive(false);

        particleTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        particleTexture.height = 1920;
        particleTexture.width = 1080;
        particleTexture.Create();

        particleCamera = GameObject.Find("ParticleCamera").GetComponent<Camera>();
        particleCamera.targetTexture = particleTexture;

        particleParent = GameObject.Find("ParticleParent2");

        particlePrefab = Resources.Load<GameObject>("Effect/ef_kira001");
        particle = GameObject.Instantiate(particlePrefab, particleParent.transform).GetComponent<ParticleSystem>();
        particle.Pause();

        particleRawImage = GameObject.Find("ParticleRawImage").GetComponent<RawImage>();
        particleRawImage.texture = particleTexture;



        isUp = true;
        RING_PUT_INTERVAL = 1080 * 0.125f;

        addCnt = 1.0f;
        comboNum = 0;
        clearNum = 0;
        totalRings = 0;
        totalClear = 0;
        totalperfect = 0;
    }

    public void UpdateExec()
    {
        for (int i=0;i<rings.Count;i++)
        {
            rings[i].UpdateExec();
        }

        for (int i = 0; i < rings.Count; i++)
        {
            if (rings[i].isDead)
            {
                rings[i].Destroy();
                rings.RemoveRange(i, 1);
                i--;
            }
        }

        addCnt += Time.deltaTime;
        if (addCnt > 1.0f)
        {
            addCnt = 0;
            float _y = 1920 / 8;

            if (!isUp)
            {
                _y *= -1;
            }
            float _ringWidth = 144;
            Add(new Vector3(1080 / 2+_ringWidth, _y), level);
        }
    }

    void UpdateCombo()
    {

    }

    public void HitCheck(Vector3 _airPlanePos)
    {
        for (int i=0;i<rings.Count;i++)
        {
            //くぐった
            if (rings[i].HitCheck(_airPlanePos))
            {
                combo.gameObject.SetActive(true);
                combo.StartAnime(rings[i].effectPos);
                clearNum++;
                comboNum++;
                combo.SetComboNum(comboNum,rings[i].isPerfect);

                particleRawImage.transform.localPosition = new Vector3(rings[i].pos.x, rings[i].pos.y);
                particle.Play();


                if (rings[i].isPerfect)
                {
                    totalperfect++;
                }

                //GameObject _eff = GameObject.Instantiate(effBase,effParent.transform);
                //_eff.transform.localPosition = new Vector3(rings[i].pos.x,rings[i].pos.y);
                //_eff.SetActive(true);
                //effList.Add(_eff);

                totalClear++;
                totalRings++;
                break;
            }
            else if(!rings[i].isFailedCombo)
            {
                if(rings[i].OverAirPlane(_airPlanePos.x-10))
                {
                    comboNum = 0;
                    totalRings++;
                }
            }
        }
    }


    public void SetNewGame(int _level)
    {
        if (comboNum >= 10)
        {
            isUp = !isUp;
        }
        addCnt = 1.0f;
        clearNum = 0;
        comboNum = 0;
        level = _level;
    }

    public void AddOneSet(int _level)
    {
        float _y =1920/8;

        if (comboNum >= 10)
        {
            isUp = !isUp;
        }
        if (!isUp)
        {
            _y *= -1;
        }

        for (int i=0;i<ONE_SET;i++)
        {
            Add(new Vector3(1080/2+i* RING_PUT_INTERVAL, _y),_level);
        }
        clearNum = 0;
        comboNum = 0;
    }

    void Add(Vector3 _pos,int _level)
    {
        Ring _ring = new Ring();
//        Object.Destroy(particle.gameObject);
        _ring.Create(back,front,backParent,frontParent);
        _ring.SetPos(_pos);
        _ring.SetSpeed(1080 * 0.125f);
        _ring.SetHeight(1920 * RING_SIZE[_level]);
        rings.Add(_ring);
    }

    public void Destroy()
    {
        for(int i=0;i<rings.Count;i++)
        {
            rings[i].Destroy();
        }
        rings.Clear();
        SetNewGame(-1);
    }

}
