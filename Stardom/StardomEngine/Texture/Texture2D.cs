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
using System.Threading.Channels;
using StardomEngine.App;
using System.Data;
namespace StardomEngine.Texture
{
    public class Texture2D
    {

        public int Handle { get; set; }
        public byte[] Data { get; set; }
        public float[] DataFloat { get; set; }

        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Channels { get; set; }

        public static Dictionary<string, Texture2D> Cache = new Dictionary<string, Texture2D>();

        public void Delete()
        {

            Data = null;
            DataFloat = null;
            GL.DeleteTexture(Handle);

        }
        public Texture2D(byte[] data,int w,int h)
        {

            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            // Data = LoadPngToByteArray(path);
            Width = w;
            Height = h;
            Channels = 4;

            //Data = new byte[Width * Height * Channels];
            //DataFloat = new float[Width * Height * Channels];
            Data = data;

            if (Channels == 4)
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb32f, Width, Height, 0, PixelFormat.Rgb, PixelType.Float, DataFloat);
            }
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            //GL.GenerateMipmap(TextureTarget.Texture2d);

            GL.BindTexture(TextureTarget.Texture2d, 0);

        }
        public Texture2D(int width,int height,int channels,bool fp = true)
        {

            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            // Data = LoadPngToByteArray(path);
            Width = width;
            Height = height;
            Channels = channels;

            //Data = new byte[Width * Height * Channels];
            DataFloat = new float[Width * Height * Channels];
            Data = new byte[Width * Height * Channels];


            if (fp)
            {
                if (Channels == 4)
                {
                    GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, DataFloat);
                }
                else
                {
                    GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb32f, Width, Height, 0, PixelFormat.Rgb, PixelType.Float, DataFloat);
                }
            }
            else
            {
                if (Channels == 4)
                {
                    GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
                }
                else
                {
                    GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, Data);
                }
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
            Path = path;
            if (Cache.ContainsKey(path))
            {

                var cache = Cache[path];

               // Data = cache.Data;
              //  DataFloat = cache.DataFloat;
                Width = cache.Width;
                Height = cache.Height;
                Channels = cache.Channels;
                Handle = cache.Handle;
                //  Console.WriteLine("Cached!!!");
                return;

            }
            Cache.Add(path, this);
            //Console.WriteLine("ADDED!");



            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, Handle);

            if (File.Exists(path + ".cache"))
            {

                FileInfo fil2 = new FileInfo(path);
                var time2 = fil2.LastWriteTime;

                FileStream ctime = new FileStream(path + ".time", FileMode.Open, FileAccess.Read);
                BinaryReader r1 = new BinaryReader(ctime);
                long ticks = r1.ReadInt64();
                ctime.Close();

                if (ticks != time2.Ticks)
                {

                }
                else
                {
                    FileStream fs = new FileStream(path + ".cache", FileMode.Open, FileAccess.Read);
                    BinaryReader r2 = new BinaryReader(fs);

                    Width = r2.ReadInt32();
                    Height = r2.ReadInt32();
                    Channels = r2.ReadInt32();
                    Data = r2.ReadBytes(Width * Height * Channels);


                    fs.Close();
                    MakeTexture();
                    return;
                }

            }

            Data = LoadPngToByteArray(path);

            FileInfo fil = new FileInfo(path);
            var time = fil.LastWriteTime;

            FileStream tfile = new FileStream(path + ".time", FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(tfile);
            w.Write(time.Ticks);
            w.Flush();
            tfile.Close();


            FileStream file = new FileStream(path + ".cache", FileMode.Create, FileAccess.Write);
            BinaryWriter w1 = new BinaryWriter(file);
            w1.Write(Width);
            w1.Write(Height);
            w1.Write(Channels);
            w1.Write(Data);
            w1.Flush();
            file.Flush();
            file.Close();

            MakeTexture();

        }

        private void MakeTexture()
        {
            if (Channels == 4)
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, Data);
            }
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(TextureTarget.Texture2d);

            GL.BindTexture(TextureTarget.Texture2d, 0);
            //Data = null;
            DataFloat = null;

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

        public void Grab(Vector2 position)
        {
            int adjustedY = StarApp.FrameHeight - (int)position.Y - Height;

            GL.BindTexture(TextureTarget.Texture2d, Handle);
            GL.CopyTexImage2D(TextureTarget.Texture2d, 0,InternalFormat.Rgb, (int)position.X, adjustedY, Width, Height, 0);
        }

    }
}
