﻿// (C) 2016 ERAL
// Distributed under the Boost Software License, Version 1.0.
// (See copy at http://www.boost.org/LICENSE_1_0.txt)

using UnityEngine;
using UnityEngine.UI;

namespace UIExtention {
	public class UICollider : Graphic, ICanvasRaycastFilter {

		protected override void Awake() {
			base.Awake();
			color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}

		protected override void OnPopulateMesh(VertexHelper toFill) {
			toFill.Clear();
		}

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
			return true;
		}
	}
}
