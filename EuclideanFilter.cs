namespace UnityColorFilters {
	using UnityEngine;
	using System;

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

		public EuclideanFilter(Color32 center, short radius) {
			this.center = center;
			this.radius = radius;
		}

		public Image ApplyInPlace(Image image) {
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				image.Pixels[i] = ApplyToPixel(image.Pixels[i]);
			}

			return image;
		}

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