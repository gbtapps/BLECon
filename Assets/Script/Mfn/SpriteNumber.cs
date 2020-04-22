using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//数字を画像で表示する用
public class SpriteNumber : MonoBehaviour
{

    [Tooltip("桁数")]
    public int digit = 2;
    [Tooltip("表示する数字")]
    public int number = 0;
    public void SetViewNum(int num_) { number = num_; now_count_num = 0; }
    public int GetViewNum() { return number; }
    public void AddViewNum(int num_ = 1) { number = +num_; }
    [Tooltip("そのまま表示するか、カウントアップで表示するか")]
    public bool is_count_up = false;
    [Tooltip("カウントアップを完了するのにかかる時間")]
    public float count_up_time = 1.0f;
    private float now_count_num = 0;
    [Tooltip("ゼロ埋めするか")]
    public bool is_zero_pad = false;
    [Tooltip("数字間隔")]
    public float sprite_interval = 0.0f;
    [Tooltip("数字画像")]
    public Sprite[] num_tex_source = new Sprite[10];

    bool isEnabled = true;

    float baseX;

    class NumberObjct
    {
        public List<GameObject> obj = new List<GameObject>();
        public List<Image> img = new List<Image>();
        public List<RectTransform> rect_trans = new List<RectTransform>();
    }

    NumberObjct num_obj = new NumberObjct();

    void Awake()
    {
        now_count_num = 0;
        if (!isEnabled) { return; }

        for (int i = 0; i < digit; i++)
        {
            //生成と登録
            GameObject obj = new GameObject("num_" + i.ToString());
            Image img = obj.AddComponent<Image>();
            obj.transform.SetParent(transform, false);

            //管理用リストに追加
            num_obj.obj.Add(obj);
            num_obj.img.Add(img);
            num_obj.rect_trans.Add(obj.GetComponent<RectTransform>());
        }

        for (int i = 0; i < num_obj.img.Count; i++)
        {

            num_obj.img[i].sprite = num_tex_source[0];
            num_obj.rect_trans[i].sizeDelta = new Vector2(num_tex_source[0].rect.width, num_tex_source[0].rect.height);
            num_obj.obj[i].transform.localPosition = new Vector3
            (
                //                num_obj.obj[i].transform.localPosition.x - (num_obj.rect_trans[i].sizeDelta.x * i),
                num_obj.obj[i].transform.localPosition.x - (sprite_interval * i),
                 num_obj.obj[i].transform.localPosition.y,
                num_obj.obj[i].transform.localPosition.z
            );

        }
        baseX = gameObject.transform.localPosition.x;

        AdjastViewNum();

    }

    // Use this for initialization
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) { return; }

        if (is_count_up)
        {
            if (now_count_num < number)
            {
                now_count_num += (number * (Time.deltaTime / count_up_time));
                if (now_count_num > number)
                {
                    now_count_num = number;
                    is_count_up = false;
                }
            }
        }
        AdjastViewNum();
    }

    void AdjastViewNum()
    {
        //表示する数字
        int view_num = number;
        if (is_count_up) { view_num = (int)now_count_num; }
        //実際の計算で使用する数字
        int cluc_num = view_num;

        int shortageDigit = digit - view_num.ToString().Length;


        for (int i = 0; i < num_obj.img.Count; i++)
        {
            int pow = (int)Mathf.Pow(10, i);

            //その桁の数字が存在するか確認
            if (view_num >= pow)
            {
                //存在する
                //位に応じた数字(1の位を削っていく方式)
                int digit_num = 0;
                digit_num = cluc_num % 10;
                cluc_num -= digit_num;
                cluc_num /= 10;

                num_obj.img[i].sprite = num_tex_source[digit_num];
                num_obj.img[i].enabled = true;
            }
            else
            {
                //存在しない
                //0で埋めるか非表示か？
                if (is_zero_pad)
                {
                    num_obj.img[i].sprite = num_tex_source[0];
                }
                else
                {
                    num_obj.img[i].enabled = false;
                }
            }
        }

        if(!is_zero_pad && shortageDigit >= 0)
        {
            gameObject.transform.localPosition = new Vector3(
                baseX- (shortageDigit-1)*sprite_interval,
                gameObject.transform.localPosition.y
            );
        }

    }

        public void Enabled(bool _enabled)
    {
        isEnabled = _enabled;
        for (int i = 0; i < num_obj.img.Count; i++)
        {
            num_obj.img[i].enabled = _enabled;
        }

    }

    public void SetColor(Color c)
    {
        if (c.a < 0) c.a = 0.1f;
        
        for (int i = 0; i < num_obj.img.Count; i++)
        {
            Debug.Log("CCCC: " + c);
            //num_obj.img[i].color = new Color(1,1,1,0.0f); //_color;
            num_obj.img[i].color = c;
        }
    }
}