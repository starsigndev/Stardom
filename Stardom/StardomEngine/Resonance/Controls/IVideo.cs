using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;
using Vivid.Video;

namespace StardomEngine.Resonance.Controls
{
    public class IVideo : IControl
    {

        IntPtr Decoder;

        public IVideo()
        {
            //Decoder = Vivid.Video.VideoCPP.createDecoder();
            int b = 5;
        }

        public void PlayVideo(string video)
        {
            vPath = video;
            Decoder = Vivid.Video.VideoCPP.createDecoder();
            Vivid.Video.VideoCPP.playVideo(Decoder, video);
        }
        Texture2D tex = null;
        int ltick = 0;
        string vPath = "";
        public override void Update()
        {
            if (Vivid.Video.VideoCPP.visDone(Decoder))
            {

                PlayVideo(vPath);
            }

            //base.OnUpdate();
            Vivid.Video.VideoCPP.updateDecoder(Decoder);

            IntPtr frame = Vivid.Video.VideoCPP.vgetFrame(Decoder);

            if (frame != IntPtr.Zero)
            {

                int w, h;
                w = VideoCPP.vgetFrameWidth(frame);
                h = VideoCPP.vgetFrameHeight(frame);
                IntPtr buf = VideoCPP.vgetFramebuf(frame);
                int buf_size = w * h * 4;
                byte[] dat = new byte[buf_size];
                Marshal.Copy(buf, dat, 0, buf_size);
                if (tex != null)
                {
                    tex.Delete();
                }
                tex = new Texture2D(dat, w, h);
                Console.WriteLine("Frame: W:" + w + " H:" + h);

            }

        }
        public override void Render()
        {
            //base.OnRender();
            if (tex != null)
            {
                //Draw(tex);
                GameUI.This.DrawRect(tex, RenderPosition, Size, Color);
            }
        }

    }
}
