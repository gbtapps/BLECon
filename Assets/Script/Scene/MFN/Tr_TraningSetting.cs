using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tr_TraningSetting : SceneBase
{
    //実行するトレーニング
    static ConstData.EnumScene playTraining;
    public static void SetPlayTraining(ConstData.EnumScene _playTraining)
    {
        playTraining = _playTraining;

        if(_playTraining == ConstData.EnumScene.Tr_TrainingNeuro)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Neuro;
        }
        if(_playTraining == ConstData.EnumScene.Tr_TrainingHeartRate)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Heart;
        }
        if(_playTraining == ConstData.EnumScene.Tr_TrainingBreath)
        {
            CommonData.resultType = (int)R_ResultLog.ContentType.Breath;
        }
    }

    public static ConstData.EnumScene GetPlaytraining()
    {
        return playTraining;
    }

    //トレーニング時間
    public static float timeSec { get; private set; }

    Text textOverview;
    Slider slider;
    ExButton btnStart;



    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);
        textOverview = gameObject.FindDescendant("Text_Notice01").GetComponent<Text>();
        slider = gameObject.FindDescendant("Slider").GetComponent<Slider>();
        btnStart = gameObject.FindDescendant("Btn_Start").AddComponent<ExButton>();

        //初期値5分
        timeSec = 60 * 5;
        slider.onValueChanged.AddListener((float value)=>{
            timeSec = 60 + (value * 9 * 60); //1分から10分
        });


        if (playTraining == ConstData.EnumScene.Tr_TrainingNeuro)
        {
            textOverview.text = "\nこのトレーニングで「ストレス耐性」が高くなる可能性が研究で示されています。" +
                "\n\n□トレーニング方法" +
                "\n\nセンサーは額の外側（右利きの人は左、左利きの人は右）に付けます。" +
                "\n\n脳活動が大きくなると、飛行機が上昇、小さくなると下降します。飛行機の動きを見ながら、コントロールするように集中してください。" +
                "\n\nトレーニング時間の目安は1日5～10分です。" +
                "\n\n最初はうまくできないのが普通です。徐々にうまく集中してできるようになります。\n";
        }
        else if (playTraining == ConstData.EnumScene.Tr_TrainingHeartRate)
        {
            textOverview.text = "\nこのトレーニングで「ストレス耐性」が高くなる可能性が研究で示されています。" +
                "\n\n□トレーニング方法" +
                "\n\nセンサーは額の中央または外側に付けます。" +
                "\n\n脈拍数が高くなると、飛行機が上昇、低くなると下降します。飛行機の動きを見ながら、コントロールするように集中してください。" +
                "\n\nトレーニング時間の目安は1日5～10分です。" +
                "\n\n最初はうまくできないのが普通です。徐々にうまく集中してできるようになります。\n";
        }
        else if (playTraining == ConstData.EnumScene.Tr_TrainingBreath)
        {
            textOverview.text = "\n□トレーニング方法" +
                "\n\nセンサーは額の中央に付けます。" +
                "リラックスして脳活動が落ち着くと、飛行機が下降します。" +
                "\n\nゆっくり自分のペースで呼吸しながら、呼吸の数を数えてください。"+
                "\n「ひとつ」の時は、息を吐くときに「ひと～……」と伸ばし、次に息を吸いながら「……つ～」と数え、"+
                "次の息を吐くときは「ふた～……」、吸う時に「……つ～」と数えます。"+
                "「十（とお）」までいったらまた「一（ひとつ）」に戻りましょう。"+
                "\n\nトレーニング時間の目安は1日5～10分です。" +
                "\n\n最初はうまくできないのが普通です。徐々にうまく集中してできるようになります。\n";
        }
    }
          
    private void Update()
    {

        if (btnStart.lastHit2)
        {
            SceneFunc.ChangeScene(playTraining, false);
        }
    }
}
