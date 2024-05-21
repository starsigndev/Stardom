using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK.Mathematics;
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
        public float[] DataFloat { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int Channels { get; set; }

        public Texture2D(int width,int height,int channels)
        {

            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            // Data = LoadPngToByteArray(path);
            Width = width;
            Height = height;
            Channels = channels;

            //Data = new byte[Width * Height * Channels];
            DataFloat = new float[Width * Height * Channels];

            if (Channels == 4)
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, DataFloat);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb32f, Width, Height, 0, PixelFormat.Rgb, PixelType.Float,DataFloat);
            }
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            //GL.GenerateMipmap(TextureTarget.Texture2d);

            GL.BindTexture(TextureTarget.Texture2d, 0);



        }
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


        public void SetPixelFloat(int x,int y,Vector4 color)
        {

            switch (Channels)
            {
                case 3:
                    {
                        int loc = (y * Width * 3) + x * 3;
                        DataFloat[loc++] = color.X;
                        DataFloat[loc++] = color.Y;
                        DataFloat[loc] = color.Z;
                    }
                    break;
                case 4:
                    {
                        int loc = (y * Width * 4) + x * 4;
                     //   int loc = (y * Width * 3) + x * 3;
                        Data[loc++] = (byte)(color.X * 255.0f);
                        Data[loc++] = (byte)(color.Y * 255.0f);
                        Data[loc++] = (byte)(color.Z * 255.0f);
                        Data[loc] = (byte)(color.W * 255.0f);
                    }
                    break;
            }

        }

        public Vector4 GetPixel(int x,int y)
        {
            if(x<0 || y<0 || x>=Width || y >= Height)
            {
                return new Vector4(0,0,0,0);
            }

            switch (Channels)
            {
                case 3:
                    {
                        int loc = (y * Width * 3) + x * 3;
                        float r, g, b;
                        r = ((float)Data[loc++]) / 255.0f;
                        g = ((float)Data[loc++]) / 255.0f;
                        b = ((float)Data[loc++]) / 255.0f;
                        return new Vector4(r, g, b, 1.0f);
                    }
                    break;
                case 4:
                    {
                        int loc = (y * Width * 4) + x * 4;
                        float r, g, b,a;
                        r = ((float)Data[loc++]) / 255.0f;
                        g = ((float)Data[loc++]) / 255.0f;
                        b = ((float)Data[loc++]) / 255.0f;
                        a = ((float)Data[loc++]) / 255.0f;
                        return new Vector4(r, g, b, a);
                    }
                    break;
            }

            return new Vector4(0, 0, 0, 0);
        }

        public void Upload()
        {

            GL.BindTexture(TextureTarget.Texture2d, Handle);

            if (Channels == 3)
            {
                GL.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, Width, Height, PixelFormat.Red, PixelType.Float, DataFloat);
            }

            GL.BindTexture(TextureTarget.Texture2d, 0);

        }

        public void Resize(int new_w,int new_h)
        {
            if (new_w == Width && new_h == Height) return; 

            float sx = (float)Width / (float)new_w;
            float sy = (float)Height / (float)new_h;

            byte[] ndat = new byte[new_w * new_h * Channels];

            for(int y = 0; y < new_h; y++)
            {
                for(int x = 0; x < new_w; x++)
                {

                    float gx = x * sx;
                    float gy = y * sy;

                    int loc1 = (y * new_w * Channels) + (x * Channels);
                    int loc2 = ((int)gy * Width * Channels) + ((int)gx * Channels);

                    ndat[loc1++] = Data[loc2++];
                    ndat[loc1++] = Data[loc2++];
                    ndat[loc1++] = Data[loc2++];
                    if(Channels>3)
                    {
                        ndat[loc1++] = Data[loc2++];
                    }


                }
            }
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            Data = ndat;
            Width = new_w;
            Height = new_h;

            if (Channels == 4)
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
                
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, Data);
            }
            GL.GenerateMipmap(TextureTarget.Texture2d);
            //Bind();
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
