﻿// (C) 2016 ERAL
// Distributed under the Boost Software License, Version 1.0.
// (See copy at http://www.boost.org/LICENSE_1_0.txt)

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace UIExtention {
	[CustomEditor(typeof(Circle), true)]
	[CanEditMultipleObjects]
	public class CircleEditor : GraphicEditor {

		SerializedProperty m_Sprite;
		GUIContent m_SpriteContent;
		SerializedProperty m_VertexCount;
		GUIContent m_VertexCountContent;
		SerializedProperty m_Distortion;
		GUIContent m_DistortionContent;

		protected override void OnEnable() {
			base.OnEnable();

			m_SpriteContent			= new GUIContent("Source Image");
			m_VertexCountContent	= new GUIContent("Vertex Count");
			m_DistortionContent		= new GUIContent("Distortion");

			m_Sprite		= serializedObject.FindProperty("m_Sprite");
			m_VertexCount	= serializedObject.FindProperty("m_VertexCount");
			m_Distortion	= serializedObject.FindProperty("m_Distortion");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
			AppearanceControlsGUI();
			EditorGUILayout.PropertyField(m_VertexCount, m_VertexCountContent);
			EditorGUILayout.PropertyField(m_Distortion, m_DistortionContent);

			serializedObject.ApplyModifiedProperties();
		}

		public override bool HasPreviewGUI() {
			return true;
		}

		public override void OnPreviewGUI(Rect rect, GUIStyle background) {
			var circle = target as Circle;
			var sprite = circle.sprite;
			if (sprite == null) {
				return;
			}
		}

		public override string GetInfoString() {
			var circle = target as Circle;
			var sprite = circle.sprite;

			var x = ((sprite != null)? Mathf.RoundToInt(sprite.rect.width): 0);
			var y = ((sprite != null)? Mathf.RoundToInt(sprite.rect.height): 0);

			return string.Format("Circle Size: {0}x{1}", x, y);
		}
	}
}
