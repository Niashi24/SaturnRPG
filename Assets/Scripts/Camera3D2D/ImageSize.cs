using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace SaturnRPG.Camera3D2D
{
	[System.Serializable]
	public class ImageSize : ISize
	{
		[SerializeField]
		private Image image;

		public Vector2 Size
		{
			get
			{
				if (image == null) return Vector2.one;
				if (image.rectTransform == null) return Vector2.one;
				
				return image.rectTransform.rect.size;
			}
		}
	}
}