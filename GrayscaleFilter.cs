/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;
	using System;

	/**
	 * Filter which converts an image to Grayscale using the <b>average</b> method.
	 * http://www.johndcook.com/blog/2009/08/24/algorithms-convert-color-grayscale/
	 **/
	public class GrayscaleFilter {
		private static float rWeight = 0.2989f;
		private static float gWeight = 0.5870f;
		private static float bWeight = 0.1140f;

		/**
		 * Applies an <i>average</i> grayscale filter to the given image.
		 * All transformations will be performed on the original image and no copy will be created
		 **/
		public static Image ApplyInPlace(Image image) {
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				image.Pixels[i] = ApplyToPixel(image.Pixels[i]);
			}

			return image;
		}

		/**
		 * Applies an <i>average</i> grayscale filter to the given image.
		 * All transformations will be performed on a copy of the image. The original image will not be altered.
		 **/
		public static Image Apply(Image image) {
			Color32[] newPixels = new Color32 [image.Pixels.Length];

			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				newPixels[i] = ApplyToPixel(image.Pixels[i]);
			}

			return new Image (newPixels, image.Width, image.Height);
		}

		private static Color32 ApplyToPixel(Color32 color) {
			double value = (rWeight * color.r + gWeight * color.g + bWeight * color.b) / 3.0f;
			byte rgb = (byte)Math.Round(value, MidpointRounding.AwayFromZero);

			return new Color32(rgb, rgb, rgb, color.a);
		}
	}
}
