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
	 * Class to scale an image down to a specified size
	 **/
	public class ImageScaler {
		public float ScaleRatio {
			get;
			set;
		}

		public ImageScaler(float ratio) {
			this.ScaleRatio = ratio;
		}

		public Image Process(Image image) {
			int newWidth = (int)(image.Width * ScaleRatio);
			int newHeight = (int)(image.Height * ScaleRatio);
			Image newImage = new Image (new Color32[newWidth * newHeight], newWidth, newHeight);

			int xChunkSize = image.Width / newWidth;
			int yChunkSize = image.Height / newHeight;

			for (int x = 0; x < newWidth; x++) {
				for (int y = 0; y < newHeight; y++) {
					Color32 averageColor = getAverageColor(x * xChunkSize, x * xChunkSize + xChunkSize, y * yChunkSize, y * yChunkSize + yChunkSize, image); // TODO: get average color for given chunk
					newImage.setPixel(x, y, averageColor);
				}
			}

			return newImage;
		}

		public Color32 getAverageColor(int xFrom, int xTo, int yFrom, int yTo, Image image) {
			int r = 0;
			int g = 0;
			int b = 0;
			int a = 0;

			int pixelnum = 0;
			for (int x = xFrom; x < xTo; x++) {
				for (int y = yFrom; y < yTo; y++) {
					Color32 color = image.getPixel(x, y);
					r += color.r;
					g += color.g;
					b += color.b;
					a += color.a;
					pixelnum++;
				}
			}

			byte rB = (byte)(r / pixelnum);
			byte gB = (byte)(g / pixelnum);
			byte bB = (byte)(b / pixelnum);
			byte aB = (byte)(a / pixelnum);

			return new Color32(rB, gB, bB, aB);
		}
	}
}
