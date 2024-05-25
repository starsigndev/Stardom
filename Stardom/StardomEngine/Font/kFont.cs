﻿using StardomEngine.Draw;
using StardomEngine.Texture;
using OpenTK.Mathematics;
using StardomEngine.Resonance;

namespace Vivid.Font
{
    public class kFont
    {
        public Texture2D[] Chars = new Texture2D[256];
        public float Scale = 1.0f;

        public float GlobalScale
        {
            get
            {
                return _GlobalScale;
            }
            set
            {
                _GlobalScale = value;
            }
        }
        float _GlobalScale = 0.7f;
        public kFont(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            for (int i = 0; i < 255; i++)
            {
                int w = r.ReadInt32();
                int h = r.ReadInt32();

                byte[] data = new byte[w * h * 4];

                int loc = 0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte fr = r.ReadByte();
                        byte fg = r.ReadByte();
                        byte fb = r.ReadByte();
                        byte fa = r.ReadByte();

                        data[loc++] = fr;
                        data[loc++] = fg;
                        data[loc++] = fb;

                        if (fr == 0 && fg == 0 && fb == 0)
                        {
                            fa = 0;
                        }

                        data[loc++] = fa;
                    }
                }

                if (w == 0)
                {
                    Chars[i] = null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream();

                    ms.Write(data, 0, w * h * 4);

                    ms.Position = 0;

                    var tex = new Texture2D(data, w, h);

                    Chars[i] = tex;
                }
            }
            fs.Close();
        }

        public int StringWidth(string text)
        {
            int cc = 0;
            int x = 0;

            while (true)
            {
                if (cc == text.Length) break;
                int cnum = (int)text[cc];

                if (cnum == "\0"[0] || cnum == "\n"[0])
                {
                    break;
                }

                if (cnum >= 0 && cnum <= 255)
                {
                    if (Chars[cnum] != null)
                    {
                        x = x + (int)(Chars[cnum].Width * Scale*GlobalScale) + 2;
                    }
                }

                cc++;
            }

            return x;
        }

        public int StringHeight()
        {
            return (int)(Chars[33].Height * (Scale*GlobalScale));
        }

        public void DrawSmartString(string text, int x, int y, float r, float g, float b, float a, SmartDraw draw)
        {

        }
        public void DrawString(string text, int x, int y, float r, float g, float b, float a, SmartDraw draw)
        {
            int cc = 0;
            int sx = 0;

            while (true)
            {
                if (cc == text.Length) break;
                int cnum = (int)text[cc];

                if (cnum == "\0"[0] || cnum == "\n"[0])
                {
                    break;
                }

                if (cnum >= 0 && cnum <= 255)
                {
                    if (Chars[cnum] == null)
                    {
                        cc++;
                        continue;
                    }


        //            draw.Begin();
                    //draw.DrawTexture(Chars[cnum], x+sx, y, (int)(Chars[cnum].Width * Scale),(int)(Chars[cnum].Height * Scale), r, g, b, a);
                    GameUI.Draw.DrawQuad(Chars[cnum], new Vector2(x + sx, y), new Vector2((int)(Chars[cnum].Width * GlobalScale*Scale), (int)(Chars[cnum].Height * GlobalScale*Scale)), new Vector4(r, g, b, a),new Vector4(0,0,0,0));
                    sx = sx + (int)(Chars[cnum].Width * Scale*GlobalScale) + 2;
          //          draw.End();
                }

                cc++;
            }
        }
    }
}