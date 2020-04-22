﻿// (C) 2016 ERAL
// Distributed under the Boost Software License, Version 1.0.
// (See copy at http://www.boost.org/LICENSE_1_0.txt)

using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UIExtention {
	public class Circle : Image {

		[SerializeField] [Range(3, 120)] private int m_VertexCount = 16;
		public int vertexCount {get{return m_VertexCount;} set{if (m_VertexCount != value) {m_VertexCount = value; SetVerticesDirty();}}}

		[SerializeField] private bool m_Distortion = false;
		public bool distortion {get{return m_Distortion;} set{if (m_Distortion!= value) {m_Distortion = value; SetVerticesDirty();}}}

		protected Circle() {
		}

		protected override void OnPopulateMesh(VertexHelper toFill) {
			toFill.Clear();

			var vert = UIVertex.simpleVert;
			vert.color = color;

			var pos = GetPixelAdjustedRect();
			var uvVector4 = ((overrideSprite != null)? UnityEngine.Sprites.DataUtility.GetOuterUV(overrideSprite): Vector4.zero);
			var uv = new Rect(uvVector4.x, uvVector4.y, uvVector4.z - uvVector4.x, uvVector4.w - uvVector4.y);

			System.Func<float, float> DistortionFunction;
			if (distortion) {
				DistortionFunction = x=>{
					x = Mathf.PingPong(x, Mathf.PI * 0.25f);
					return 1.0f / Mathf.Cos(x);
				};
			} else {
				DistortionFunction = x=>{
					return 1.0f;
				};
			}

			var centerVertex = vert;
			centerVertex.position = pos.center;
			centerVertex.uv0 = uv.center;
			toFill.AddVert(centerVertex);

			var halfSize = pos.size * 0.5f;
			var countToRad = Mathf.PI * 2.0f / vertexCount;
			var vertices = Enumerable.Range(0, vertexCount)
									.Select(x=>x * countToRad)
									.Select(x=>new{Sin = Mathf.Sin(x), Cos = Mathf.Cos(x), Distortion = DistortionFunction(x)})
									.Select(x=>{
										vert.position = new Vector3(x.Sin * halfSize.x, x.Cos * halfSize.y);
										vert.uv0 = new Vector2(x.Sin * 0.5f * uv.width * x.Distortion + uv.center.x, x.Cos * 0.5f * uv.height * x.Distortion + uv.center.y);
										return vert;
									});
			foreach (var vertex in vertices) {
				toFill.AddVert(vertex);
			}
			for (int i = 0, iMax = vertexCount - 1; i < iMax; ++i) {
				toFill.AddTriangle(0, i + 1, i + 2);
			}
			toFill.AddTriangle(0, vertexCount, 1);
		}

		public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
			Vector2 localPoint;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,screenPoint, eventCamera, out localPoint);
			var pos = GetPixelAdjustedRect();
			localPoint = new Vector2(localPoint.x / pos.width, localPoint.y / pos.height);
			return localPoint.sqrMagnitude <= 0.25f;
		}
	}
}
