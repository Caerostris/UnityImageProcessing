/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;

	/**
	 * Special datastructure for storing binary images.
	 * Instead of storing colors, boolean values are stored.
	 * A boolean true stands for white, false for black.
	 **/
	public class BinaryImage {
		// flattened 2D array containing the image left to right, bottom to top
		private bool[] pixels;
		private int width;
		private int height;
		
		public bool[] Pixels {
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
		 * Create a new BinaryImage
		 **/
		public BinaryImage(bool[] pixels, int width, int height) {
			this.pixels = pixels;
			this.width = width;
			this.height = height;
		}

		/**
		 * Generate a binary image from a given image.
		 * All completely black pixels will be black, all other pixels will be white
		 **/
		public static BinaryImage FromImage(Image image) {
			bool[] binImage = new bool[image.Pixels.Length];

			// generate a binary version of the image
			for (int i = 0; i < image.Pixels.Length; i++) {
				if (image.Pixels [i].r == 0 && image.Pixels [i].g == 0 && image.Pixels [i].b == 0) {
					binImage [i] = false;
				} else {
					binImage [i] = true;
				}
			}

			return new BinaryImage (binImage, image.Width, image.Height);
		}

		/**
		 * Convert a BinaryImage to an Image object, storing the rgb values for black and white
		 **/
		public Image GetImage() {
			Color32[] image = new Color32[pixels.Length];

			Color32 white = new Color32 (255, 255, 255, 1);
			Color32 black = new Color32 (0, 0, 0, 1);

			for (int i = 0; i < pixels.Length; i++) {
				if(pixels[i]) {
					image[i] = white;
				} else {
					image[i] = black;
				}
			}

			return new Image(image, width, height);
		}

		/**
		 * Get the value of a pixel in the BinaryImage.
		 * Assumes a coordinate system where the upper left corner of the image is (0, 0).
		 **/
		public bool getPixel(int x, int y) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based
			
			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position

			return pixels [arrayPosition];
		}

		/**
		 * Set the value of a pixel in the BinaryImage.
		 * Assumes a coordinate system where the upper left corner of the image is (0, 0).
		 **/
		public void setPixel(int x, int y, bool value) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based
			
			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position

			pixels [arrayPosition] = value;
		}

		/**
		 * Invert the binary image.
		 * This function will return a new BinaryImage and will not change the current object
		 **/
		public BinaryImage invert() {
			bool[] inverted = new bool[pixels.Length];

			// generate a binary mask from the binary map by inverting the image
			for (int i = 0; i < pixels.Length; i++) {
				inverted [i] = !pixels [i];
			}

			return new BinaryImage (inverted, width, height);
		}
	}
}
