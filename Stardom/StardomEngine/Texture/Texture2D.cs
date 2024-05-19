using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Core.Platform;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace StardomEngine.Texture
{
    public class Texture2D
    {

        public int Handle { get; set; }
        public byte[] Data { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int Channels { get; set; }

        public Texture2D(string path)
        {

            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            Data = LoadPngToByteArray(path);

            if (Channels == 4)
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, Data);
            }
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,(int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(TextureTarget.Texture2d);

            GL.BindTexture(TextureTarget.Texture2d, 0);


        }

        public void Bind(int unit)
        {

            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + unit));
            GL.BindTexture(TextureTarget.Texture2d,Handle);

        }

        public void Release(int unit)
        {

            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + unit));
            GL.BindTexture(TextureTarget.Texture2d, 0);

        }

        public byte[] LoadPngToByteArray(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Path cannot be null or empty", nameof(imagePath));
            }

            using (var image = Image.Load(imagePath))
            {
                byte[] byteArray;
                if (image is Image<Rgba32> rgbaImage)
                {
                    int width = rgbaImage.Width;
                    int height = rgbaImage.Height;
                    byteArray = new byte[width * height * 4]; // 4 bytes per pixel (R, G, B, A)

                    rgbaImage.CopyPixelDataTo(byteArray);

                    Width = width;
                    Height = height;
                    Channels = 4;
                }
                else if (image is Image<Rgb24> rgbImage)
                {
                    int width = rgbImage.Width;
                    int height = rgbImage.Height;
                    byteArray = new byte[width * height * 3]; // 3 bytes per pixel (R, G, B)

                    rgbImage.CopyPixelDataTo(byteArray);

                    Width = width;
                    Height = height;
                    Channels = 3;
                }
                else
                {
                    throw new NotSupportedException("Unsupported image format");
                }

                return byteArray;
            }
        }



    }
}
