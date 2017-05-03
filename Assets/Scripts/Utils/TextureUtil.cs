using UnityEngine;

public static class TextureUtil {
	
	
    /*public void LoadImage(string filePath)
	{
		byte[] fileData = File.ReadAllBytes(filePath);
		var tex = new Texture2D(2, 2);
		tex.LoadImage(fileData);
		GetComponent<RawImage>().texture = tex;
	}*/
    

	/*public static Texture2D PrepareTexture(Texture2D tex, CameraShot.ImageOrientation orientation)
	{
		int size = Mathf.Min(tex.width, tex.height);

		Texture2D target = new Texture2D(size, size, tex.format, false);

		Color32[] pixels = tex.GetPixels32(0);

		pixels = CropTexture(pixels, tex.width, tex.height);

		pixels = RotateTexture(pixels, size, size, orientation);

		target.SetPixels32(pixels);
		target.Apply();

		return target;
	}*/
}
