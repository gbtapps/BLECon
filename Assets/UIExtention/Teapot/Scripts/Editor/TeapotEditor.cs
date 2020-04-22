﻿// (C) 2016 ERAL
// Distributed under the Boost Software License, Version 1.0.
// (See copy at http://www.boost.org/LICENSE_1_0.txt)

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace UIExtention {
	[CustomEditor(typeof(Teapot), true)]
	[CanEditMultipleObjects]
	public class TeapotEditor : GraphicEditor {

		SerializedProperty m_Sprite;
		GUIContent m_SpriteContent;

		protected override void OnEnable() {
			base.OnEnable();

			m_SpriteContent			= new GUIContent("Source Image");

			m_Sprite		= serializedObject.FindProperty("m_Sprite");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
			AppearanceControlsGUI();

			serializedObject.ApplyModifiedProperties();
		}

		public override bool HasPreviewGUI() {
			return true;
		}

		public override void OnPreviewGUI(Rect rect, GUIStyle background) {
			var Teapot = target as Teapot;
			var sprite = Teapot.sprite;
			if (sprite == null) {
				return;
			}
		}

		public override string GetInfoString() {
			var Teapot = target as Teapot;
			var sprite = Teapot.sprite;

			var x = ((sprite != null)? Mathf.RoundToInt(sprite.rect.width): 0);
			var y = ((sprite != null)? Mathf.RoundToInt(sprite.rect.height): 0);

			return string.Format("Teapot Size: {0}x{1}", x, y);
		}
	}
}
