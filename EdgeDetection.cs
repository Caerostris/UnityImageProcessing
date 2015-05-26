namespace UnityColorFilters
{
	/**
	 * Edge detection algorithm as described in "Edge detection of binary images using the method of masks", Sept. 2000, Bahaa-Eldin et. al
	*/
	public class EdgeDetection
	{
		public static BinaryImage Apply(BinaryImage image) {
			BinaryImage mask = image.invert ();
			BinaryImage result;

			// initalise result to 0's
			bool[] bRes = new bool[image.Pixels.Length];
			for (int i = 0; i < image.Pixels.Length; i++) {
				bRes[i] = false;
			}
			result = new BinaryImage (bRes, image.Width, image.Height);

			for (int y = 0; y < image.Height; y++) {
				for(int x = 0; x < image.Width; x++) {
					bool res;

					if(y+1 < image.Height) {
						// shift the image down, AND it with the mask
						res = mask.getPixel(x, y) && image.getPixel(x, y+1);
						// OR this result with the result array
						result.setPixel(x, y, result.getPixel(x, y) || res);
					}

					if(y-1 > 0) {
						// shift the image up, AND it with the mask
						res = mask.getPixel(x, y) && image.getPixel(x, y-1);
						// OR this result with the result array
						result.setPixel(x, y, result.getPixel(x, y) || res);
					}

					if(x-1 > 0) {
						// shift the image left, AND it with the mask
						res = mask.getPixel(x, y) && image.getPixel(x-1, y);
						// OR this result with the result array
						result.setPixel(x, y, result.getPixel(x, y) || res);
					}

					if(x+1 < image.Width) {
						// shift the image right, AND it with the mask
						res = mask.getPixel(x, y) && image.getPixel(x+1, y);
						// OR this result with the result array
						result.setPixel(x, y, result.getPixel(x, y) || res);
					}
				}
			}

			return result;
		}
	}
}

