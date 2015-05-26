namespace UnityColorFilters {
	using UnityEngine;
	using System.Collections.Generic;
	using System;

	class Coordinate : IEquatable<Coordinate> {
		private int x;
		private int y;

		public int X {
			get {
				return x;
			}

			set {
				x = value;
			}
		}

		public int Y {
			get {
				return y;
			}

			set {
				y = value;
			}
		}

		public Coordinate(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public bool Equals(Coordinate coordinate) {
			if (coordinate.X == x && coordinate.Y == y) {
				return true;
			}

			return false;
		}
	}

	public class BlobFinder {
		private int minWidth = 0;
		private int minHeight = 0;

		public int MinWidth {
			get {
				return minWidth;
			}

			set {
				minWidth = value;
			}
		}

		public int MinHeight {
			get {
				return minHeight;
			}

			set {
				minHeight = value;
			}
		}

		public Rectangle[] Process(Image image, CameraView cameraView) {
			List<Rectangle> rectangles = new List<Rectangle> ();

			// go over all rows
			for(int row = 0; row < image.Height; row++) {
				// go over all pixels
				for(int xPos = 0; xPos < image.Width; xPos++) {
					Coordinate coordinate = new Coordinate(xPos, row);

					// do not check coordinates that are inside a discovered rectangle
					// -> as a consequence, we do not detect blobs that are inside another blob's rectangle
					if(rectangles.Exists(rect => rect.ContainsCoordinate(coordinate.X, coordinate.Y))) {
						continue;
					}

					// skip black pixels, expand from non-black pixels
					Color32 color = image.getPixel(coordinate.X, coordinate.Y);
					if(color.r != 0 || color.g != 0 || color.b != 0) {
						// breadth first search expansion
						Debug.Log ("Expanding " + coordinate.X + " " + coordinate.Y);
						Rectangle blob = expandBlob(image, coordinate, cameraView);
						if(blob != null) {
							rectangles.Add(blob);
						}
						Debug.Log ("Found blob " + blob.TopLeftX + " " + blob.TopLeftY + " " + blob.BottomRightX + " " + blob.BottomRightY + " Size: " + blob.GetWidth + "x" + blob.GetHeight + " checked coordinate " + coordinate.X + " " + coordinate.Y);
					}
				}
			}

			// check if it has minHeight and minWidth
	//		Debug.Log ("Length before pruning: " + rectangles.Count);
			rectangles.RemoveAll (blob => blob.GetHeight < minHeight || blob.GetWidth < minWidth);
	//		Debug.Log ("Length after: " + rectangles.Count);
			return rectangles.ToArray ();
		}

		private Rectangle expandBlob(Image image, Coordinate startingPoint, CameraView cameraView) {
			Color32[] debImgP = new Color32[image.Pixels.Length];
			for (int i = 0; i < debImgP.Length; i++) {
				debImgP [i] = new Color32 (0, 0, 0, 1);
			}
			Image debImg = new Image (debImgP, image.Width, image.Height);

			bool[] bin = new bool[image.Pixels.Length];
			for (int i = 0; i < bin.Length; i++) {
				bin [i] = false;
			}

			BinaryImage binaryMap = new BinaryImage (bin, image.Width, image.Height);
			Rectangle blob = new Rectangle (startingPoint.X, startingPoint.Y, startingPoint.X, startingPoint.Y);
			List<Coordinate> pointsToCheck = new List<Coordinate> ();

			pointsToCheck = getUnseenWhiteAdjacentPixels (startingPoint, image, binaryMap);

			while (pointsToCheck.Count > 0) {
				List<Coordinate> newPointsToCheck = new List<Coordinate> ();

				foreach(Coordinate pointToCheck in pointsToCheck) {
					if (pointToCheck.X < blob.TopLeftX) {
						blob.TopLeftX = pointToCheck.X;
					} else if(pointToCheck.X > blob.BottomRightX) {
						blob.BottomRightX = pointToCheck.X;
					}
					if(pointToCheck.Y < blob.TopLeftY) {
						blob.TopLeftY = pointToCheck.Y;
					} else if(pointToCheck.Y > blob.BottomRightY) {
						blob.BottomRightY = pointToCheck.Y;
					}

					debImg.setPixel(pointToCheck.X, pointToCheck.Y, new Color32(255, 0, 0, 1));
					List<Coordinate> adjacent = getUnseenWhiteAdjacentPixels (pointToCheck, image, binaryMap);
					newPointsToCheck.AddRange (adjacent);
				}

				pointsToCheck.Clear ();
				pointsToCheck = newPointsToCheck;
				cameraView.processedImage = debImg;
			}

			return blob;
		}

		private List<Coordinate> getUnseenWhiteAdjacentPixels(Coordinate coordinate, Image image, BinaryImage seenMap) {
			int[][] modifiers = {
				new int[]{0, -1}, // up
				new int[]{1, -1}, // up-right
				new int[]{1, 0}, // right
				new int[]{1, 1}, // down-right
				new int[]{0, 1}, // down
				new int[]{-1, 1}, // down-left
				new int[]{-1, 0}, // left
				new int[]{-1, -1} // up-left
			};
			List<Coordinate> adjacentCoordinates = new List<Coordinate> ();

			foreach (int[] modifier in modifiers) {
				Coordinate modCoord = new Coordinate(coordinate.X + modifier[0], coordinate.Y + modifier[1]);
				if(modCoord.X < 0 || modCoord.X > image.Width -1) {
					continue;
				}

				if(modCoord.Y < 0 || modCoord.Y > image.Height - 1) {
					continue;
				}

				if(!adjacentCoordinates.Contains(modCoord) && !seenMap.getPixel(modCoord.X, modCoord.Y) && !isBlackPixel(modCoord, image)) {
					adjacentCoordinates.Add (modCoord);
					seenMap.setPixel(modCoord.X, modCoord.Y, true);
				}
			}

			return adjacentCoordinates;
		}

		private List<Coordinate> getAdjacentPixels(Coordinate coordinate, Image image, List<Coordinate> seen) {
			int[][] modifiers = {
				new int[]{0, -1}, // up
				new int[]{1, -1}, // up-right
				new int[]{1, 0}, // right
				new int[]{1, 1}, // down-right
				new int[]{0, 1}, // down
				new int[]{-1, 1}, // down-left
				new int[]{-1, 0}, // left
				new int[]{-1, -1} // up-left
			};
			List<Coordinate> adjacentCoordinates = new List<Coordinate> ();
			
			foreach (int[] modifier in modifiers) {
				Coordinate modCoord = new Coordinate(coordinate.X + modifier[0], coordinate.Y + modifier[1]);
				if(modCoord.X < 0 || modCoord.X > image.Width -1) {
					continue;
				}
				
				if(modCoord.Y < 0 || modCoord.Y > image.Height - 1) {
					continue;
				}
			}
			
			return adjacentCoordinates;
		}


		private bool isBlackPixel(Coordinate coordinate, Image image) {
			Color32 color = image.getPixel(coordinate.X, coordinate.Y);

			return color.r == 0 && color.g == 0 && color.b == 0;
		}
	}
}