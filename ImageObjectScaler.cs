/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2015 Keno Schwalb
 */

namespace UnityImageProcessing
{
	using System;
	using UnityEngine;

	/**
	 * This filter will scale all pixels found in a BinaryImage <b>up</b>
	 * by a given amount by simply adding more pixels around every white pixel in the image
	 **/
	public class ImageObjectScaler
	{
		private int halfStrokeWidth;

		public int StrokeWidth {
			get {
				return halfStrokeWidth * 2;
			}

			set {
				halfStrokeWidth = (int)Math.Round ((double)Math.Abs (value) / 2, MidpointRounding.AwayFromZero);
			}
		}

		public ImageObjectScaler (int strokeWidth)
		{
			StrokeWidth = strokeWidth;
		}

		/**
		 * Apply the filter to a given BinaryImage.
		 * All transformations will be done on a copy of the image and the original image will not be altered.
		 **/
		public BinaryImage Apply(BinaryImage image) {
			BinaryImage newImage = new BinaryImage (new bool[image.Pixels.Length], image.Width, image.Height);

			for(int y = 0; y < image.Height; y++) {
				for(int x = 0; x < image.Width; x++) {
					bool pixel = image.getPixel(x, y);
					newImage.setPixel (x, y, pixel);

					if(!pixel) {
						continue;
					}

					for(int modX = halfStrokeWidth * (-1); modX < halfStrokeWidth; modX++) {
						int newX = x + modX;
						if(newX > 0 && newX < image.Width) {
							newImage.setPixel(newX, y, pixel);
						}
					}

					for(int modY = halfStrokeWidth * (-1); modY < halfStrokeWidth; modY++) {
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

