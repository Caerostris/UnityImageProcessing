/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;

	public class Rectangle {
		private int topLeftX;
		private int topLeftY;
		private int bottomRightX;
		private int bottomRightY;
		private int strokeWidth = 1;
		private Color32 strokeColor = new Color32 (255, 0, 0, 1);

		public int StrokeWidth {
			get {
				return strokeWidth;
			}

			set {
				strokeWidth = value;
			}
		}

		public Color32 StrokeColor {
			get {
				return strokeColor;
			}

			set {
				strokeColor = value;
			}
		}

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

		public int GetSurfaceArea {
			get {
				return GetHeight * GetWidth;
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

		public Image drawInPlace(Image image) {
			// draw upper & lower border
			for (int x = TopLeftX; x <= bottomRightX; x++) {
				for(int yShift = 0; yShift < strokeWidth; yShift++) {
					int upperY = topLeftY + yShift;
					int lowerY = bottomRightY + yShift;

					if(upperY > 0 && upperY < image.Height) {
						image.setPixel(x, upperY, strokeColor);
					}

					if(lowerY > 0 && lowerY < image.Height) {
						image.setPixel(x, lowerY, strokeColor);
					}
				}
			}

			// draw left & right border
			for (int y = TopLeftY; y <= bottomRightY; y++) {
				for(int xShift = 0; xShift < strokeWidth; xShift++) {
					int upperX = topLeftX + xShift;
					int lowerX = bottomRightX + xShift;
					
					if(upperX > 0 && upperX < image.Width) {
						image.setPixel(upperX, y, strokeColor);
					}
					
					if(lowerX > 0 && lowerX < image.Width) {
						image.setPixel(lowerX, y, strokeColor);
					}
				}
			}

			return image;
		}
	}
}
