/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;

	/**
	 * Turns an image into an image which only contains black (rgb(0, 0, 0)) or white (rgb(255, 255, 255)) pixels
	 **/
	public class BinaryFilter {
		private byte thresholdValue;

		public byte ThresholdValue {
			get {
				return thresholdValue;
			}

			set {
				thresholdValue = value;
			}
		}

		public BinaryFilter(byte thresholdValue) {
			this.thresholdValue = thresholdValue;
		}

		/**
		 * Transform an image to a binary image.
		 * The transformation will overwrite the image passed as parameter.
		 **/
		public Image ApplyInPlace(Image image) {
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				image.Pixels[i] = ApplyToPixel(image.Pixels[i]);
			}
			
			return image;
		}

		/**
		 * Transform an image to a binary image.
		 * The transformation will return a new image.
		 **/
		public Image Apply(Image image) {
			Color32[] newPixels = new Color32[image.Pixels.Length];
			
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				newPixels[i] = ApplyToPixel(image.Pixels[i]);
			}
			
			return new Image (newPixels, image.Width, image.Height);
		}

		private Color32 ApplyToPixel(Color32 color) {
			int intensity = color.r + color.g + color.b;
			if(intensity >= thresholdValue * 3) {
				return new Color32(255, 255, 255, color.a);
			} else {
				return new Color32(0, 0, 0, color.a);
			}
		}
	}
}
