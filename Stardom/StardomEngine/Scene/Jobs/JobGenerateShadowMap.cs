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
using StardomEngine.Scene.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        byte[] spriteBuffer = null;
        int[] spriteIndex = null;
        bool first = true;

        List<SceneSprite> _Casters = new List<SceneSprite>();
        bool CreateBuffers = true;
        bool AssignParametrs = true;


        ComputeBuffer<float> LightPosBuffer;
        ComputeBuffer<float> ShadowBuffer;
        ComputeBuffer<float> SpritePosBuffer;
        ComputeBuffer<byte> SpriteDataBuffer;
        ComputeBuffer<int> SpriteIndexBuffer;
        ComputeBuffer<float> SpriteSizeBuffer;
        ComputeBuffer<float> SpriteRotBuffer;


        public void AssignSpriteData(int size)
        {

            spriteBuffer = new byte[size];
            spriteIndex = new int[Scene.GetShadowCasters().Count];


            int si = 0;
            int css = 0;
            foreach (var sc2 in Scene.GetShadowCasters())
            {

                for (int y = 0; y < sc2.Size.Y; y++)
                {
                    for (int x = 0; x < sc2.Size.X; x++)
                    {
                        int loc = y * (int)sc2.Size.X + x;

                        int loc2 = (y * sc2.Image.Width * sc2.Image.Channels) + (x * sc2.Image.Channels);
                        if (sc2.Image.Data[loc2 + 3] > 0)
                        {
                            spriteBuffer[si + loc] = 255;
                        }
                    }
                }
                spriteIndex[css] = si;
                si = si + (int)(sc2.Size.X * sc2.Size.Y);
                css++;
            }


        }

        public void Exec()
        {
            
            
            int spriteCount = Scene.GetShadowCasters().Count;
            

            if (CreateBuffers)
            {

                int spriteBufCount = 0;

                foreach (var sc2 in Scene.GetShadowCasters())
                {

                    spriteBufCount += (int)(sc2.Size.X * sc2.Size.Y);

                }


                ComputeMemoryFlags flag = ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.AllocateHostPointer;

                LightPosBuffer = new ComputeBuffer<float>(context,flag,2);

               ShadowBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly ,360);
            
                SpritePosBuffer = new ComputeBuffer<float>(context, flag ,spriteCount*2);

                SpriteDataBuffer = new ComputeBuffer<byte>(context, flag ,spriteBufCount);

                SpriteIndexBuffer = new ComputeBuffer<int>(context, flag ,spriteCount);

                SpriteSizeBuffer = new ComputeBuffer<float>(context, flag ,spriteCount*2);

                SpriteRotBuffer = new ComputeBuffer<float>(context, flag, spriteCount);

              //  ShadowBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, 360);

                AssignSpriteData(spriteBufCount);

                CreateBuffers = false;
            }

            var light = Scene.Lights[0];

           

            if (AssignParametrs)
            {
                AssignParametrs = false;
                AssignLightData(light);
                AssignSpriteData();
                AssignPars();
            }
            AssignSpriteRot();

            CallKernel();

        }

        void CallKernel()
        {
       //     queue.Flush();
            //queue.Finish();

            queue.Execute(genShadows, null, new long[] { 360 }, null, null);
            //       queue.Flush();
            //return;
            //queue.Finish();

            UploadShadow(Scene.Lights[0]);


        }
        float[] res = new float[360];

        void UploadShadow(SceneLight light)
        {


            //float[] res = new float[360];
            queue.ReadFromBuffer(ShadowBuffer, ref res, true, null);
            light.ShadowMap.DataFloat = res;
            light.ShadowMap.Upload();
        }

        void AssignPars()
        {

            genShadows.SetMemoryArgument(0, LightPosBuffer);
            genShadows.SetMemoryArgument(1, SpritePosBuffer);
            genShadows.SetValueArgument(2, Scene.GetShadowCasters().Count);
            genShadows.SetMemoryArgument(3, SpriteDataBuffer);
            genShadows.SetMemoryArgument(4, SpriteIndexBuffer);
            genShadows.SetMemoryArgument(5, SpriteSizeBuffer);
            genShadows.SetMemoryArgument(6, SpriteRotBuffer);
            genShadows.SetValueArgument(7, Scene.Lights[0].Range);
            genShadows.SetMemoryArgument(8, ShadowBuffer);

        }


        void AssignLightData(SceneLight light)
        {

            Vector2 lightPosition = new Vector2(light.Position.X, light.Position.Y);
            queue.WriteToBuffer(new float[] { lightPosition.X, lightPosition.Y }, LightPosBuffer,false, null);


        }
        void AssignSpriteRot()
        {
            float[] spriteRots = new float[Scene.GetShadowCasters().Count];
            int idx = 0;
            foreach (var spr in Scene.GetShadowCasters())
            {
                spriteRots[idx] = spr.Rotation;
                idx++;
            }
            queue.WriteToBuffer(spriteRots, SpriteRotBuffer,true, null);

        }
        void AssignSpriteData()
        {

            float[] spriteRots = new float[Scene.GetShadowCasters().Count];
            float[] spriteSize = new float[Scene.GetShadowCasters().Count * 2];
            float[] spritePos = new float[Scene.GetShadowCasters().Count * 2];

            int idx = 0;
            int jdx = 0;
            foreach(var spr in Scene.GetShadowCasters())
            {
                spriteRots[idx] = spr.Rotation;
                idx++;
                spriteSize[jdx] = spr.Size.X;
                spriteSize[jdx + 1] = spr.Size.Y;
                spritePos[jdx] = spr.Position.X;
                spritePos[jdx + 1] = spr.Position.Y;
                jdx = jdx + 2;
             }

            queue.WriteToBuffer(spritePos, SpritePosBuffer, false, null);
            queue.WriteToBuffer(spriteBuffer, SpriteDataBuffer, false, null);
            queue.WriteToBuffer(spriteIndex, SpriteIndexBuffer, false, null);
            queue.WriteToBuffer(spriteRots, SpriteRotBuffer, false, null);
            queue.WriteToBuffer(spriteSize, SpriteSizeBuffer, false, null);


        }

        public override void Execute()
        {

            Exec();
            return;

            //var sprs = Scene.GetShadowCasters();

            if (_Casters.Count == 0)
            {
                _Casters = Scene.GetShadowCasters();
            }

            if (lightPosBuffer == null)
            {
                lightPosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, new float[] { 0, 0 });
              //  spritePosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, new float[] { 0, 0 });
                //ComputeBuffer<byte> spriteBufferCL = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly, 0);
                int outputBufferSize = 380;

                // Create the output buffer with write-only flag
                closeBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, outputBufferSize);
            }
            //    float[] carray = new float[360];



          //  base.Execute();
            foreach(var light in Scene.Lights)
            {

           //     float[] lightMap = new float[360];

                Vector2 lightPosition = new Vector2(light.Position.X, light.Position.Y);
                queue.WriteToBuffer(new float[] { lightPosition.X, lightPosition.Y }, lightPosBuffer, true, null);

                if (first)
                {
                    genShadows.SetMemoryArgument(0, lightPosBuffer);
                    //gen
                }


                int siz = 0;


                if (spriteBuffer == null)
                {

                    foreach (var sc2 in _Casters)
                    {

                        siz = siz + (int)(sc2.Size.X * sc2.Size.Y);

                    }

                    spriteBuffer = new byte[siz];
                    int[] spriteIndex = new int[_Casters.Count];


                    int si = 0;
                    int css = 0;
                    foreach (var sc2 in _Casters)
                    {

                        for (int y = 0; y < sc2.Size.Y; y++)
                        {
                            for (int x = 0; x < sc2.Size.X; x++)
                            {
                                int loc = y * (int)sc2.Size.X + x;

                                int loc2 = (y * sc2.Image.Width * sc2.Image.Channels) + (x * sc2.Image.Channels);
                                if (sc2.Image.Data[loc2 + 3] > 0)
                                {
                                    spriteBuffer[si + loc] = 255;
                                }
                            }
                        }
                        spriteIndex[css] = si;
                        si = si + (int)(sc2.Size.X * sc2.Size.Y);
                        css++;
                    }


                    //var sc = sprs[0];

                    {

                        float[] spritePos = new float[_Casters.Count * 2];
                        int fi = 0;
                        foreach (var sp in _Casters)
                        {
                            spritePos[fi++] = sp.Position.X;
                            spritePos[fi++] = sp.Position.Y;
                        }

                        spritePosBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, spritePos);
                        //queue.WriteToBuffer(new float[] { sc.Position.X, sc.Position.Y }, spritePosBuffer, true, null);
                        genShadows.SetMemoryArgument(1, spritePosBuffer);

                        //      byte[] spriteBuffer = new byte[sc.Image.Width*sc.Image.Height]; // Initially null


                        genShadows.SetValueArgument(2, _Casters.Count);

                        var spriteBufferCL = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, spriteBuffer);
                        genShadows.SetMemoryArgument(3, spriteBufferCL);

                        var indexBuf = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, spriteIndex);

                        genShadows.SetMemoryArgument(4, indexBuf);

                        float[] spriteSizes = new float[_Casters.Count * 2];

                        si = 0;
                        foreach (var s in _Casters)
                        {
                            spriteSizes[si++] = s.Size.X;
                            spriteSizes[si++] = s.Size.Y;
                        }

                        var sizeBuf = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, spriteSizes);



                        genShadows.SetMemoryArgument(5, sizeBuf);



                        //genShadows.SetValueArgument(3, (int)sc.Size.X);
                        //genShadows.SetValueArgument(4, (int)sc.Size.Y);
                        float[] spriteRots = new float[_Casters.Count];

                        si = 0;
                        foreach (var s3 in _Casters)
                        {
                            spriteRots[si] = s3.Rotation;
                            si++;
                        }

                        var rotBuf = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, spriteRots);



                        genShadows.SetMemoryArgument(6, rotBuf);
                    }

                    Console.WriteLine("Processed");
                }
                if (first)
                {
                    genShadows.SetValueArgument(7, light.Range);
                    genShadows.SetMemoryArgument(8, closeBuffer);
                }
                first = false;
                queue.Flush();
                try { 

                queue.Execute(genShadows, null, new long[] { 360 }, null, null);

                //
                //commandQueue.Execute(kernel, null, new long[] { data.Length }, null, null);
                //commandQueue.Finish();
            }
catch (Cloo.InvalidKernelArgumentsComputeException ex)
{
                Console.WriteLine("Invalid kernel arguments: " + ex.Message);
                // Additional logging or handling
            }


            //

            //queue.Finish();
         //   queue.Flush();

                //queue.Finish();
                
                return;

                    float[] res = new float[360];
                    queue.ReadFromBuffer(closeBuffer, ref res, true, null);
                  //  queue.Finish();
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
        public override void Release()
        {
            base.Release();
        }

    }
}
