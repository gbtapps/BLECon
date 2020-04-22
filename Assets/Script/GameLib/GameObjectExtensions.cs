using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// 自分自身を含むすべての子オブジェクトのレイヤーを設定します
    /// </summary>
    public static void SetLayerRecursively(
        this GameObject self,
        int layer
    )
    {
        self.layer = layer;

        foreach (Transform n in self.transform)
        {
            SetLayerRecursively(n.gameObject, layer);
        }
    }

    /// <summary>
    /// 親を辿り指定のクラスを取得する
    /// </summary>
    public static T FindParentObject<T>( this GameObject self )
    {
        GameObject Obj = self;
		T BtlObj = default(T);
        while( Obj != null && BtlObj == null )
        {
            BtlObj = Obj.GetComponent<T>();
            if (Obj.transform.parent != null)
            {
                Obj = Obj.transform.parent.gameObject;
            }
            else
            {
                Obj = null;
            }
        }

        return BtlObj;

    }

    /// <summary>
    /// 子孫から指定の名前をしたオブジェクトを探す
    /// </summary>
    public static GameObject FindDescendant(this GameObject self, string Name)
    {
        Transform t = self.transform.Find(Name);
        if (t != null)
        {
            return t.gameObject;
        }


        foreach (Transform child in self.transform)
        {
            GameObject o = child.gameObject.FindDescendant(Name);
            if (o != null)
            {
                return o;
            }
        }

        return null;
    }

    /// <summary>
    /// 子孫から指定の名前をした指定のクラスを探す
    /// </summary>
    public static T FindDescendant<T>(this GameObject self, string Name)
    {
        // 自分の子供から検索
        foreach (Transform child in self.transform)
        {
            if (child.name.Equals(Name))
            {
                T s = child.GetComponent<T>();
                if( s != null )
                {
                    return s;
                }
            }
        }

        foreach (Transform child in self.transform)
        {
            T o = child.gameObject.FindDescendant<T>( Name );
            if( o != null )
            {
                return o;
            }
        }

        return default(T);
    }
}