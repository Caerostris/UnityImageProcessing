/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

﻿namespace UnityColorFilters {
	using UnityEngine;

	public class Rectangle {
		int topLeftX;
		int topLeftY;
		int bottomRightX;
		int bottomRightY;

		public int TopLeftX {
			get {
				return topLeftX;
			}

			set {
				topLeftX = value;
			}
		}

		public int TopLeftY {
			get {
				return topLeftY;
			}
			
			set {
				topLeftY = value;
			}
		}

		public int BottomRightX {
			get {
				return bottomRightX;
			}
			
			set {
				bottomRightX = value;
			}
		}

		public int BottomRightY {
			get {
				return bottomRightY;
			}
			
			set {
				bottomRightY = value;
			}
		}

		public int GetWidth {
			get {
				return bottomRightY - topLeftY;
			}
		}

		public int GetHeight {
			get {
				return bottomRightX - topLeftX;
			}
		}

		public Rectangle(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY) {
			this.topLeftX = topLeftX;
			this.topLeftY = topLeftY;
			this.bottomRightX = bottomRightX;
			this.bottomRightY = bottomRightY;
		}

		public bool ContainsCoordinate(int x, int y) {
			if (x >= topLeftX && x <= bottomRightX && y >= topLeftY && y <= bottomRightY) {
				return true;
			}

			return false;
		}
	}
}
