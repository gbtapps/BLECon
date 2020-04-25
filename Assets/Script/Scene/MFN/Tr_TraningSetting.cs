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


    }


    public static ConstData.EnumScene GetPlaytraining()
    {
        return playTraining;
    }


    ExButton btnStart;



    void Start()
    {
        CommonHeaderMfn.Instance.SetView(true);


        btnStart = gameObject.FindDescendant("Btn_Start").AddComponent<ExButton>();
        

    
    }
    
    


    private void Update()
    {

        if (btnStart.lastHit2)
        {
            SceneFunc.ChangeScene(playTraining, false);
        }
    }




}
