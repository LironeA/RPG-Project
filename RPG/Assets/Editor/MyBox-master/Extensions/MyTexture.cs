using UnityEngine;

namespace MyBox
{
    public static class MyTexture
    {
        /// <summary>
        /// Create new sprite out of Texture
        /// </summary>
        public static Sprite AsSprite(this Texture2D texture)
        {
            var rect = new Rect(0, 0, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture, rect, pivot);
        }

        /// <summary>
        /// Change texture size (and scale accordingly)
        /// </summary>
        public static Texture2D Resample(this Texture2D source, int targetWidth, int targetHeight)
        {
            var sourceWidth = source.width;
            var sourceHeight = source.height;
            var sourceAspect = (float) sourceWidth / sourceHeight;
            var targetAspect = (float) targetWidth / targetHeight;

            var xOffset = 0;
            var yOffset = 0;
            float factor;

            if (sourceAspect > targetAspect)
            {
                // crop width
                factor = (float) targetHeight / sourceHeight;
                xOffset = (int) ((sourceWidth - sourceHeight * targetAspect) * 0.5f);
            }
            else
            {
                // crop height
                factor = (float) targetWidth / sourceWidth;
                yOffset = (int) ((sourceHeight - sourceWidth / targetAspect) * 0.5f);
            }

            var data = source.GetPixels32();
            var data2 = new Color32[targetWidth * targetHeight];
            for (var y = 0; y < targetHeight; y++)
            for (var x = 0; x < targetWidth; x++)
            {
                var p = new Vector2(Mathf.Clamp(xOffset + x / factor, 0, sourceWidth - 1),
                    Mathf.Clamp(yOffset + y / factor, 0, sourceHeight - 1));
                // bilinear filtering
                var c11 = data[Mathf.FloorToInt(p.x) + sourceWidth * Mathf.FloorToInt(p.y)];
                var c12 = data[Mathf.FloorToInt(p.x) + sourceWidth * Mathf.CeilToInt(p.y)];
                var c21 = data[Mathf.CeilToInt(p.x) + sourceWidth * Mathf.FloorToInt(p.y)];
                var c22 = data[Mathf.CeilToInt(p.x) + sourceWidth * Mathf.CeilToInt(p.y)];

                data2[x + y * targetWidth] = Color.Lerp(Color.Lerp(c11, c12, p.y), Color.Lerp(c21, c22, p.y), p.x);
            }

            var tex = new Texture2D(targetWidth, targetHeight);
            tex.SetPixels32(data2);
            tex.Apply(true);
            return tex;
        }

        /// <summary>
        /// Crop texture to desired size.
        /// Somehow cropped image seemed darker, brightness offset may fix this
        /// </summary>
        public static Texture2D Crop(this Texture2D original, int left, int right, int top, int down,
            float brightnessOffset = 0)
        {
            var x = left + right;
            var y = top + down;
            var resW = original.width - x;
            var resH = original.height - y;
            var pixels = original.GetPixels(left, down, resW, resH);

            if (!Mathf.Approximately(brightnessOffset, 0))
                for (var i = 0; i < pixels.Length; i++)
                    pixels[i] = pixels[i].BrightnessOffset(brightnessOffset);

            var result = new Texture2D(resW, resH, TextureFormat.RGB24, false);
            result.SetPixels(pixels);
            result.Apply();

            return result;
        }

        /// <summary>
        /// Will texture with solid color
        /// </summary>
        public static Texture2D WithSolidColor(this Texture2D original, Color color)
        {
            var target = new Texture2D(original.width, original.height);
            for (var i = 0; i < target.width; i++)
            for (var j = 0; j < target.height; j++)
                target.SetPixel(i, j, color);

            target.Apply();

            return target;
        }
    }
}