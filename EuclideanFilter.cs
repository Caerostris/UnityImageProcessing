/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing {
	using UnityEngine;
	using System;

	/**
	 * A Euclidean color filter
	 **/
	public class EuclideanFilter {
		private short radius;
		private Color32 center;

		private Color32 fillColor = new Color32(0, 0, 0, 1);
		private bool fillOutside = true;

		public Color32 Center {
			get {
				return center;
			}

			set {
				center = value;
			}
		}

		public Color32 FillColor {
			get {
				return fillColor;
			}

			set {
				fillColor = value;
			}
		}

		public bool FillOutside {
			get {
				return fillOutside;
			}

			set {
				fillOutside = value;
			}
		}

		public short Radius {
			get {
				return radius;
			}

			set {
				radius = Math.Max ((short)0, Math.Min ((short)450, value));
			}
		}

		/**
		 * Create a EuclideanFilter
		 **/
		public EuclideanFilter(Color32 center, short radius) {
			this.center = center;
			this.radius = radius;
		}

		/**
		 * Apply the Euclidean filter the given image.
		 * All transformations will be performed on the original image and no copy will be created
		 **/
		public Image ApplyInPlace(Image image) {
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				image.Pixels[i] = ApplyToPixel(image.Pixels[i]);
			}

			return image;
		}

		/**
		 * Apply the Euclidean filter to the givn image.
		 * A copy of the image will be created and the original image will not be altered.
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
			int dR, dG, dB;

			dR = center.r - color.r;
			dG = center.g - color.g;
			dB = center.b - color.b;
			
			// calculate the distance
			Color32 newColor = color;
			if(dR * dR + dG * dG + dB * dB <= radius * radius) {
				if(!fillOutside) {
					newColor = fillColor;
				}
			} else {
				if(fillOutside) {
					newColor = fillColor;
				}
			}

			return newColor;
		}
	}
}
