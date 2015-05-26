/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;

	/**
	 * Data structure for storing an image created from a Texture2D or a WebCamTexture in Unity
	 **/
	public class Image {
		// flattened 2D array containing the image left to right, bottom to top
		private Color32[] pixels;
		private int width;
		private int height;

		public Color32[] Pixels {
			get {
				return pixels;
			}

			set {
				pixels = value;
			}
		}

		public int Width {
			get {
				return width;
			}

			set {
				width = value;
			}
		}

		public int Height {
			get {
				return height;
			}

			set {
				height = value;
			}
		}

		/**
		 * Create a new Image given a Color32 array, image width, and image height
		 **/
		public Image(Color32[] pixels, int width, int height) {
			this.pixels = pixels;
			this.width = width;
			this.height = height;
		}

		/**
		 * Create a new Image given a WebCamTexture
		 **/
		public static Image FromWebCamTexture(WebCamTexture tex) {
			return new Image (tex.GetPixels32 (), tex.width, tex.height);
		}

		/**
		 * Create a new Image given a Texture2D
		 **/
		public static Image FromTexture2D(Texture2D tex) {
			return new Image (tex.GetPixels32 (), tex.width, tex.height);
		}

		/**
		 * Convert the Image to a Texture2D
		 **/
		public Texture2D GetTexture2D() {
			Texture2D tex2d = new Texture2D(width, height);
			tex2d.SetPixels32 (pixels);
			tex2d.Apply ();
			return tex2d;
		}

		/**
		 * Get a pixel of the image.
		 * Assumes a coordinate system where the upper left corner of the image is (0, 0)
		 **/
		public Color32 getPixel(int x, int y) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based

			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position

			return pixels [arrayPosition];
		}

		/**
		 * Set a pixel of the image
		 * Assumes a coordinate system where the upper left corner of the image is (0, 0)
		 **/
		public void setPixel(int x, int y, Color32 value) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based
			
			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position
			
			pixels [arrayPosition] = value;
		}
	}
}
