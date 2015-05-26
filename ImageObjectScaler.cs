namespace UnityColorFilters
{
	using System;
	using UnityEngine;

	public class ImageObjectScaler
	{
		private int strokeWidth;

		public int StrokeWidth {
			get {
				return strokeWidth * 2;
			}

			set {
				strokeWidth = (int)Math.Round ((double)Math.Abs (value) / 2, MidpointRounding.AwayFromZero);
			}
		}

		public ImageObjectScaler (int strokeWidth)
		{
			StrokeWidth = strokeWidth;
		}

		public BinaryImage Apply(BinaryImage image) {
			BinaryImage newImage = new BinaryImage (new bool[image.Pixels.Length], image.Width, image.Height);

			for(int y = 0; y < image.Height; y++) {
				for(int x = 0; x < image.Width; x++) {
					bool pixel = image.getPixel(x, y);
					newImage.setPixel (x, y, pixel);

					if(!pixel) {
						continue;
					}

					for(int modX = strokeWidth * (-1); modX < strokeWidth; modX++) {
						int newX = x + modX;
						if(newX > 0 && newX < image.Width) {
							newImage.setPixel(newX, y, pixel);
						}
					}

					for(int modY = strokeWidth * (-1); modY < strokeWidth; modY++) {
						int newY = y + modY;
						if(newY > 0 && newY < image.Height) {
							newImage.setPixel(x, newY, pixel);
						}
					}
				}
			}

			return newImage;
		}
	}
}

