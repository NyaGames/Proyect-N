using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DownSampling
{
	public static DownSampledImage CompressImage(Color[] uncompressedData)
	{
		//int referenceSide = 10000;
		//byte compressionRate = (byte) ((uncompressedData.Length <= referenceSide) ? 1 : uncompressedData.Length / referenceSide);

		byte compressionRate = 2;
		int imageRowSize = (int)Mathf.Sqrt(uncompressedData.Length);	

		byte[] compressedData = new byte[uncompressedData.Length / (compressionRate * compressionRate)];

		for (int i = 0, index = 0; i < uncompressedData.Length; i += ((i + compressionRate) % imageRowSize == 0) ? imageRowSize * (compressionRate - 1) + compressionRate : compressionRate, index++)
		{
			float meanColor = 0f;
			for (int j = 0; j < compressionRate; j++)
			{
				for (int k = 0; k < compressionRate; k++)
				{
					float color = (uncompressedData[i + j + k * imageRowSize].r + uncompressedData[i + j + k * imageRowSize].b + uncompressedData[i + j + k * imageRowSize].g) / 3;
					meanColor += (byte) Mathf.RoundToInt(color * 255);
				}
			}

			compressedData[index] = (byte)(Mathf.RoundToInt(meanColor) / (compressionRate * compressionRate));			
		}

		return new DownSampledImage(compressedData, compressionRate);
	}

	public static byte[] DecompressImage(DownSampledImage downSampledImage)
	{
		byte[] data = downSampledImage.data;
		byte compressionRate = downSampledImage.comppresionRate;

		int imageRowSize = (int)Mathf.Sqrt(data.Length) * compressionRate;

		byte[] decompressedData = new byte[data.Length * compressionRate * compressionRate * 4];

		for (int i = 0, index = 0; i < decompressedData.Length; i+=4, index++)
		{
			int row = index / imageRowSize;
			int col = (index - imageRowSize * row);

			int pixelIndex = (row / compressionRate) * imageRowSize / compressionRate + (col / compressionRate);

			decompressedData[i + 0] = data[pixelIndex];
			decompressedData[i + 1] = data[pixelIndex];
			decompressedData[i + 2] = data[pixelIndex];
			decompressedData[i + 3] = 255;
		}

		return decompressedData;
	}
}

public class DownSampledImage
{
	public byte[] data;
	public byte comppresionRate;

	public DownSampledImage(byte[] data, byte comppresionRate)
	{
		this.data = data;
		this.comppresionRate = comppresionRate;
	}
}
