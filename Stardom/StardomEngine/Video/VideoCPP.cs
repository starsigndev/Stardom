using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Video
{
    public class VideoCPP
    {

        [DllImport("vividcpp.dll")]
        public static extern int initVivid(int p);

        [DllImport("vividcpp.dll")]
        public static extern IntPtr createDecoder();
        [DllImport("vividcpp.dll")]
        public static extern void playVideo(IntPtr decoder,string path);

        [DllImport("vividcpp.dll")]
        public static extern void updateDecoder(IntPtr decoder);

        [DllImport("vividcpp.dll")]
        public static extern IntPtr vgetFrame(IntPtr decoder);
        [DllImport("vividcpp.dll")]
        public static extern int vgetFrameWidth(IntPtr f);
        [DllImport("vividcpp.dll")]
        public static extern int vgetFrameHeight(IntPtr f);
        [DllImport("vividcpp.dll")]
        public static extern IntPtr vgetFramebuf(IntPtr f);
        [DllImport("vividcpp.dll")]
        public static extern bool visDone(IntPtr d);

        
    }

}
