namespace UnityColorFilters {
	using UnityEngine;
	using System;

	/**
	 * Filter which conerts an image to Grayscale using the <b>average</b> method.
	 * http://www.johndcook.com/blog/2009/08/24/algorithms-convert-color-grayscale/
	 **/
	public class GrayscaleFilter {
		private static float rWeight = 0.2989f;
		private static float gWeight = 0.5870f;
		private static float bWeight = 0.1140f;

		public static Image ApplyInPlace(Image image) {
			// for each pixel
			for (int i = 0; i < image.Pixels.Length; i++) {
				image.Pixels[i] = ApplyToPixel(image.Pixels[i]);
			}

			return image;
		}

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