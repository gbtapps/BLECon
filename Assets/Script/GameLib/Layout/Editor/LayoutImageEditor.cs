#if UNITY_EDITOR
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
using UnityEngine;

[CustomEditor(typeof(LayoutImage))]
public class LayoutImageEditor : Editor
{
	public override void OnInspectorGUI()
	{
		LayoutImage img = target as LayoutImage;

		//base.OnInspectorGUI();

		//img.g_Pos = EditorGUILayout.Vector2Field("Pos", img.g_Pos);


		img.Pos = EditorGUILayout.Vector2Field("Pos", img.Pos);
		img.WorldPos = EditorGUILayout.Vector2Field("WorldPos", img.WorldPos);
		//img.LayoutPos = EditorGUILayout.Vector2Field("LayoutPos", img.LayoutPos);
		img.Anchor = EditorGUILayout.Vector2Field("Anchor", img.Anchor);

		string label = img.label;
		if( label == null )
		{
			label = "(null)";
		}


		EditorGUILayout.TextField( "Label(ReadOnry)", label );

		/*
		img.load_file = EditorGUILayout.TextField("Load File", img.load_file);
		img.prefix = EditorGUILayout.TextField("File Prefix", img.prefix);

		img.auto_load = EditorGUILayout.Toggle("Auto Load", img.auto_load);
		img.set_sprite_size = EditorGUILayout.Toggle("読み出したスプライトのサイズを入れる", img.set_sprite_size);
		img.load_Transparent = EditorGUILayout.Toggle("ロードしても透明", img.load_Transparent);
		img.load_fade_in = EditorGUILayout.Toggle("ロード完了してフェードイン", img.load_fade_in);
		img.owner_scene = EditorGUILayout.ObjectField("Scene Base", img.owner_scene, typeof(UISceneBase), true) as UISceneBase;
		*/
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("load_file"), new GUIContent("load_file"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("prefix"), new GUIContent("prefix"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("auto_load"), new GUIContent("auto_load"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("multi_load"), new GUIContent("multi_load"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("load_Transparent"), new GUIContent("load_Transparent"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("owner_scene"), new GUIContent("AutoLoadOwnerScene"));
	}
}

//エディター上から追加するよう
#if false
namespace LayoutLib
{
	public class MenuExtendAsbImage
	{
		[MenuItem("GameObject/Layout/LayoutImage")]
		static void CreateCustomGameObject(MenuCommand menuCommand)
		{
			GameObject root = new GameObject();

			/*AsbImage img = */
			LayoutImage Img = root.AddComponent<LayoutImage>();
			/*UnityEngine.UI.Image btn_img = */
			//root.AddComponent<UnityEngine.UI.Image>();
			root.name = "LayoutImage";
			//Img.SetData( )

			if (Selection.activeGameObject != null)
			{
				root.transform.SetParent(Selection.activeGameObject.transform, false);
				Selection.activeGameObject = root;
			}
		}
	}
}
#endif

#endif
