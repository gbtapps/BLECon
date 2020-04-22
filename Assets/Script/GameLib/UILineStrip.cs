using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//  uGUIでマスク有効な線を引く(ラインレンダラーだとマスクできない)

[ExecuteInEditMode]
public class UILineStrip : MaskableGraphic
{
    public int CornerDivisionCount = 1; // コーナー部分の接続部の分割数...大きくするとなめらかになるが、頂点数が増える
    public float LineWidth = 1.0f; // 線の太さ

    [SerializeField]
    Vector2[] m_positions;
    // これらの点を結んだ折れ線を描く(原点はRectTransformの左下隅)
    public Vector2[] Positions
    {
        get {
            return m_positions;
        }
        set {
            m_positions = value;
            SetVerticesDirty();
        }
    }

#if UNITY_EDITOR

    [MenuItem("GameObject/UI/Line Strip")] // メニューの「GameObject」→「UI」→「Line Strip」でオブジェクト作成
    public static void CreateLineStrip()
    {
        // Canvasを探し、もしなければ作成
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            canvas = new GameObject("Canvas", typeof(Canvas)).GetComponent<Canvas>();
            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        // EventSystemを探し、もしなければ作成
        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem));
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        // LineStripを作成し、Canvasの子にする
        var lineStrip = new GameObject("LineStrip", typeof(CanvasRenderer), typeof(UILineStrip));
        lineStrip.transform.SetParent(canvas.transform, false);
        // 作成されたLineStripを選択
        Selection.activeGameObject = lineStrip;
    }

#endif

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        this.CornerDivisionCount = Mathf.Max(this.CornerDivisionCount, 1);
        var pivot = this.rectTransform.pivot;
        var size = this.rectTransform.rect.size;
        var origin = Vector2.Scale(-pivot, size); // RectTransformの左下隅を原点とする
        vh.Clear(); // 以前に描いた頂点は削除
        var cornerCount = this.Positions == null ? 0 : this.Positions.Length;
        if (cornerCount < 2)
        {
            return; // コーナーの数が2未満だと線を引けないため、何もしない
        }

        var segmentCount = cornerCount - 1; // コーナーをつなぐ線分の数
        for (var i = 0; i < segmentCount; i++)
        {
            var a = origin + this.Positions[i];
            var b = origin + this.Positions[i + 1];
            // コーナー間に線分を追加
            AddSegment(
                vh,
                a,
                this.color,
                this.LineWidth,
                b,
                this.color,
                this.LineWidth);
            // 細い線なら気にならないでしょうが、太い線だと線分のつなぎ目が目立つので接続部を追加しました
            // ですが、1回AddSegmentまたはAddJointを行うごとに頂点数+4、三角形数+2のオーダーで形状が複雑化します
            // (本当は、折れ線全体が一つの三角形ストリップになるようにすれば頂点数増加を+2に抑えられるはずですが、手抜きしました...)
            // もし細い線しか使わない、あるいは大量のコーナーを持つ折れ線を描くので描画負荷を下げたい、といったことがありましたら
            // 以下のifブロックは削除してしまうのがいいでしょう
            if (i < (segmentCount - 1))
            {
                var c = origin + this.Positions[i + 2];
                var tangentFrom = (b - a).normalized;
                var tangentTo = (c - b).normalized;
                var tangentCount = this.CornerDivisionCount + 1;
                var tangentAngle = -Vector2.SignedAngle(tangentFrom, tangentTo) * Mathf.Deg2Rad;
                var tangents = Enumerable.Range(0, tangentCount)
                    .Select(
                        index =>
                        {
                            var angle = Mathf.Lerp(0.0f, tangentAngle, index / (tangentCount - 1.0f));
                            var cosAngle = Mathf.Cos(angle);
                            var sinAngle = Mathf.Sin(angle);
                            return new Vector2(
                                (cosAngle * tangentFrom.x) + (sinAngle * tangentFrom.y),
                                (cosAngle * tangentFrom.y) - (sinAngle * tangentFrom.x));
                        })
                    .ToArray();
                for (var j = 0; j < this.CornerDivisionCount; j++)
                {
                    AddJoint(vh, b, tangents[j], tangents[j + 1], this.color, this.LineWidth);
                }
            }
        }
    }

    // p点に接続部を追加
    // tangentFromは入っていく線分の向き
    // tangentToは出ていく線分の向き
    private static void AddJoint(
        VertexHelper vh,
        Vector2 p,
        Vector2 tangentFrom,
        Vector2 tangentTo,
        Color color,
        float width)
    {
        var dFrom = width * 0.5f * new Vector2(-tangentFrom.y, tangentFrom.x);
        var dTo = width * 0.5f * new Vector2(-tangentTo.y, tangentTo.x);
        var offset = vh.currentVertCount;
        var vertex = UIVertex.simpleVert;
        vertex.position = p - dFrom;
        vertex.color = color;
        vh.AddVert(vertex);
        vertex.position = p + dFrom;
        vh.AddVert(vertex);
        vertex.position = p + dTo;
        vh.AddVert(vertex);
        vertex.position = p - dTo;
        vh.AddVert(vertex);
        vh.AddTriangle(offset + 0, offset + 1, offset + 2);
        vh.AddTriangle(offset + 2, offset + 3, offset + 0);
    }

    // a、b点間に線分を追加
    private static void AddSegment(
        VertexHelper vh,
        Vector2 a,
        Color colorA,
        float widthA,
        Vector2 b,
        Color colorB,
        float widthB)
    {
        var tangent = (b - a).normalized;
        var normal = new Vector2(-tangent.y, tangent.x);
        var dA = widthA * 0.5f * normal;
        var dB = widthB * 0.5f * normal;
        var offset = vh.currentVertCount;
        var vertex = UIVertex.simpleVert;
        vertex.position = a - dA;
        vertex.color = colorA;
        vh.AddVert(vertex);
        vertex.position = a + dA;
        vh.AddVert(vertex);
        vertex.position = b + dB;
        vertex.color = colorB;
        vh.AddVert(vertex);
        vertex.position = b - dB;
        vh.AddVert(vertex);
        vh.AddTriangle(offset + 0, offset + 1, offset + 2);
        vh.AddTriangle(offset + 2, offset + 3, offset + 0);
    }
}
