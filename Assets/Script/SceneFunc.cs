using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneFunc
{
    //  シーンのロードと共通ヘッダの切り替え
    //  pushがfalseならヒストリに記録しない
    static public void ChangeScene(ConstData.EnumScene scene,bool push = true)
    {
        if (push)
        {
            CommonData.SceneHistory.Push(CommonData.NowScene);
        }
        CommonData.NowScene = scene;
        SceneManager.LoadScene(scene.ToString());
        CommonHeaderMfn.Instance.ChangeScene(scene);
        
    }

    //  前のシーンに戻る
    static public void BackScene()
    {
        ChangeScene(CommonData.SceneHistory.Pop(),false);
    }
}
