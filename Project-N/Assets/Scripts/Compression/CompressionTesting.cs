using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CompressionTesting : MonoBehaviour
{
	[SerializeField] private RawImage sourceImage;
	[SerializeField] private RawImage targetImage;

	private void Update()
	{
		Texture2D sourceTexture = (Texture2D) sourceImage.texture;		

		Color[] data = sourceTexture.GetPixels();
		DownSampledImage downSampledImage = DownSampling.CompressImage(data);

		byte[] uncompressedData = DownSampling.DecompressImage(downSampledImage);
		Texture2D tex = new Texture2D(sourceImage.texture.width, sourceImage.texture.height);
		tex.LoadRawTextureData(uncompressedData);
		tex.Apply();

		targetImage.texture = tex;
	}
}
