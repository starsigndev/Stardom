using StardomEngine.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cloo;
using OpenTK.Compute.OpenCL;
using OpenTK.Mathematics;
namespace StardomEngine.Scene.Jobs
{
    public class JobGenerateShadowMap : CLJob
    {

        public Scene2D Scene;
        //float[] lightPos = new (10.0f, 10.0f);
        //byte[] spriteMask = new byte[360];
        //float[] spritePos = new float[360];
        // float[] spriteRot = new float[360];
        //float[] output = new float[360];
        private ComputeBuffer<float> lightPosBuf;
        private ComputeBuffer<byte> spriteMask;
        private ComputeBuffer<float> spritePos;
        ComputeKernel genShadows;


        public JobGenerateShadowMap() : base("data/cl/genShadowMap.cl")
        {

           genShadows= GetKernel("generateShadows");


        }
        public override void Bind()
        {
            base.Bind();
        }

        ComputeBuffer<float> lightPosBuffer=null;
        ComputeBuffer<float> spritePosBuffer;
        ComputeBuffer<float> closeBuffer;
        public override void Execute()
        {

            var sprs = Scene.GetShadowCasters();

            if (lightPosBuffer == null)
            {
                lightPosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, new float[] { 0, 0 });
              //  spritePosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, new float[] { 0, 0 });
                //ComputeBuffer<byte> spriteBufferCL = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly, 0);
                int outputBufferSize = 360;

                // Create the output buffer with write-only flag
                closeBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, outputBufferSize);
            }
                float[] carray = new float[360];


            base.Execute();
            foreach(var light in Scene.Lights)
            {

                float[] lightMap = new float[360];
                for(int i = 0; i < 360; i++)
                {
                    lightMap[i] = 1.0f;
                }


                Vector2 lightPosition = new Vector2(light.Position.X, light.Position.Y);
                queue.WriteToBuffer(new float[] { lightPosition.X, lightPosition.Y }, lightPosBuffer, true, null);
                
                genShadows.SetMemoryArgument(0, lightPosBuffer);
                //gen


                int siz = 0;

                foreach(var sc2 in sprs)
                {

                    siz = siz + (int)(sc2.Size.X * sc2.Size.Y);

                }

                byte[] spriteBuffer = new byte[siz];
                int[] spriteIndex = new int[sprs.Count];


                int si = 0;
                int css = 0;
                foreach (var sc2 in sprs)
                {

                    for (int y = 0; y < sc2.Size.Y; y++)
                    {
                        for (int x = 0; x < sc2.Size.X; x++)
                        {
                            int loc = y * (int)sc2.Size.X + x;

                            int loc2 = (y * sc2.Image.Width * sc2.Image.Channels) + (x * sc2.Image.Channels);
                            if (sc2.Image.Data[loc2 + 3] > 0)
                            {
                                spriteBuffer[si+loc] = 255;
                            }
                        }
                    }
                    spriteIndex[css] = si;
                    si = si + (int)(sc2.Size.X * sc2.Size.Y);
                    css++;
                }

                var sc = sprs[0];
                {

                    float[] spritePos = new float[sprs.Count * 2];
                    int fi = 0;
                    foreach(var sp in sprs)
                    {
                        spritePos[fi++] = sp.Position.X;
                        spritePos[fi++] = sp.Position.Y;
                    }

                    spritePosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, spritePos);
                   //queue.WriteToBuffer(new float[] { sc.Position.X, sc.Position.Y }, spritePosBuffer, true, null);
                   genShadows.SetMemoryArgument(1, spritePosBuffer);

                    //      byte[] spriteBuffer = new byte[sc.Image.Width*sc.Image.Height]; // Initially null


                    genShadows.SetValueArgument(2, sprs.Count);

                    var spriteBufferCL = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, spriteBuffer);
                    genShadows.SetMemoryArgument(3, spriteBufferCL);

                    var indexBuf = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer,spriteIndex);

                    genShadows.SetMemoryArgument(4, indexBuf);

                    float[] spriteSizes = new float[sprs.Count * 2];

                    si = 0;
                    foreach(var s in sprs)
                    {
                        spriteSizes[si++] = s.Size.X;
                        spriteSizes[si++] = s.Size.Y;
                    }

                    var sizeBuf = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, spriteSizes);



                    genShadows.SetMemoryArgument(5, sizeBuf);



                    //genShadows.SetValueArgument(3, (int)sc.Size.X);
                    //genShadows.SetValueArgument(4, (int)sc.Size.Y);
                    float[] spriteRots = new float[sprs.Count];

                    si = 0;
                    foreach(var s3 in sprs)
                    {
                        spriteRots[si] = s3.Rotation;
                        si++;
                    }

                    var rotBuf = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer,spriteRots);



                    genShadows.SetMemoryArgument(6,rotBuf);

                    genShadows.SetValueArgument(7, light.Range);
                    genShadows.SetMemoryArgument(8, closeBuffer);

                    queue.Execute(genShadows, null, new long[] { 360 }, null, null);
                    queue.Finish();

                    float[] res = new float[360];
                    queue.ReadFromBuffer(closeBuffer, ref res, true, null);
                    queue.Finish();
                    for(int i = 0; i < 360; i++)
                    {
                        light.ShadowMap.SetPixelFloat(i, 0, new Vector4(res[i], res[i], res[i],1.0f));
                        //   Console.WriteLine("i:" + i + " R:" + res[i]);
                        if (res[i] < 1)
                        {
                            int b = 5;
                        }
                    }

                    light.ShadowMap.Upload();

                }


            }

        }
        public override void Release()
        {
            base.Release();
        }

    }
}
