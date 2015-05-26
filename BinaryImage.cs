namespace UnityColorFilters {
	using UnityEngine;
	
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
		
		public BinaryImage(bool[] pixels, int width, int height) {
			this.pixels = pixels;
			this.width = width;
			this.height = height;
		}
		
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
		
		public bool getPixel(int x, int y) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based
			
			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position

			return pixels [arrayPosition];
		}

		public void setPixel(int x, int y, bool value) {
			// pixels array goes left to right, from bottom to top
			int rowLength = width;
			int skipRows = height - y - 1; // -1 for null-based
			
			int arrayPosition = rowLength * skipRows; // y position: skipped n rows
			arrayPosition += x; // x position

			pixels [arrayPosition] = value;
		}


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