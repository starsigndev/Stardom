using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Shader;
using StardomEngine.Texture;
using StardomEngine.App;
using OpenTK.Graphics.OpenGL;
using StardomEngine.Helper;
using System.Runtime.InteropServices;

namespace StardomEngine.Draw
{
    public class SmartDraw
    {
        public List<DrawList> Lists { get; set; }
        public Dictionary<Texture2D, DrawList> ListMap { get; set; }
        public float CurrentZ = 0.0f;
        public ShaderModule DrawNormal { get; set; }

        private int VertexArray = 0;
        private int Buffer;
        private int IndexBuffer;


        public SmartDraw()
        {

            Lists = new List<DrawList>();
            ListMap = new Dictionary<Texture2D, DrawList>();

            DrawNormal = new ShaderModule("data/shader/drawNormalVS.glsl", "data/shader/drawNormalFS.glsl");


        }
        public void Begin()
        {

            CurrentZ = 0.1f;
            Lists.Clear();
            ListMap.Clear();
            //plist = null;

        }
       // DrawList plist = null;
        public DrawList FindList(Texture2D image)
        {
            //if (plist != null)
            {
         //       if (plist.Calls[0].Image == image)
           //     {
             //       return plist;
              //  }
            }
            //if (ListMap.ContainsKey(image))
            //{
            //    return ListMap[image];
            //}

            foreach(var list in Lists)
            {
                if (list.Calls[0].Image == image)
                {
                    //plist = list;
                    return list;
                }
            }

            DrawList new_list = new DrawList();

            ListMap.Add(image, new_list);
            Lists.Add(new_list);
          //  plist = new_list;
            return new_list;

        }

        
        public DrawCall DrawSprite(Texture2D image,Vector2 position,Vector2 size,float rotation,float scale,Vector4 color,Vector4 ext)
        {

            var list = FindList(image);
            

            DrawCall call = new DrawCall();

            float x0 = - size.X / 2;
            float y0 = - size.Y / 2;
            float x1 =  size.X / 2;
            float y1 = - size.Y / 2;
            float x2 =  size.X / 2;
            float y2 =  size.Y / 2;
            float x3 =  - size.X / 2;
            float y3 = size.Y / 2;


            var v1 = GameMaths.RotateAndScale(new Vector2(x0, y0), rotation, scale);

            call.X[0] = position.X+v1.X;
            call.Y[0] = position.Y+v1.Y;

            var v2 = GameMaths.RotateAndScale(new Vector2(x1, y1), rotation, scale);


            call.X[1] = position.X+v2.X;
            call.Y[1] = position.Y+v2.Y;

            var v3 = GameMaths.RotateAndScale(new Vector2(x2, y2), rotation, scale);

            call.X[2] = position.X + v3.X;
            call.Y[2] = position.Y + v3.Y;

            var v4 = GameMaths.RotateAndScale(new Vector2(x3, y3), rotation, scale);

            call.X[3] = position.X + v4.X;
            call.Y[3] = position.Y + v4.Y;

            call.Z = CurrentZ;

            CurrentZ += 0.0001f;

            call.Image = image;

            call.Ext = ext;

            
            list.AddCall(call);

            return call;

        }

        public DrawCall DrawQuad(Texture2D image,Vector2 position,Vector2 size,Vector4 color)
        {

            var list = FindList(image);

            DrawCall call = new DrawCall();

            call.X[0] = position.X;
            call.Y[0] = position.Y;

            call.X[1] = position.X + size.X;
            call.Y[1] = call.Y[0];

            call.X[2] = call.X[1];
            call.Y[2] = position.Y + size.Y;

            call.X[3] = call.X[0];
            call.Y[3] = call.Y[2];

            call.Z = CurrentZ;

            CurrentZ += 0.001f;

            call.Color = color;

            call.Image = image;

            list.AddCall(call);

            return call;
        }

        float[] v_data = null;
        uint[] i_data = null;
        int v_len = 0;
        int i_len = 0;
        public void End2()
        {
            int w = StarApp.FrameWidth;
            int h = StarApp.FrameHeight;
            //int b = 0;
            var se_Projection = Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1.0f, 1.0f);

            // DrawNormal.Bind();

            DrawNormal.SetMat("se_Projection", se_Projection);

            GL.Disable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, w, h);

            uint[] indices = new uint[4];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;

            if (VertexArray == 0)
            {
                VertexArray = GL.GenVertexArray();
                Buffer = GL.GenBuffer();
               // GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
                //GL.BufferData(BufferTarget.ArrayBuffer, 200, IntPtr.Zero, BufferUsage.StaticDraw);
                //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                IndexBuffer = GL.GenBuffer();
              //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
             //   GL.BufferData(BufferTarget.ElementArrayBuffer, 1024 * 1000, IntPtr.Zero, BufferUsage.StaticDraw);
            //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            float[] v_data = new float[3 * 4 * 2 * 4 * 4];
            foreach (var list in Lists)
            {
                
                foreach(var call in list.Calls)
                {
                    
                    int ix = 0;

                    v_data[ix++] = call.X[0];
                    v_data[ix++] = call.Y[0];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 0;
                    v_data[ix++] = 0;

                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v1
                    v_data[ix++] = call.X[1];
                    v_data[ix++] = call.Y[1];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 1;
                    v_data[ix++] = 0;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v2
                    v_data[ix++] = call.X[2];
                    v_data[ix++] = call.Y[2];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 1;
                    v_data[ix++] = 1;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v3 
                    v_data[ix++] = call.X[3];
                    v_data[ix++] = call.Y[3];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 0;
                    v_data[ix++] = 1;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    list.Calls[0].Image.Bind(0);
                    list.Calls[0].Normals.Bind(1);

                    DrawNormal.SetInt("se_ColorTexture", 0);
                    DrawNormal.SetInt("se_NormalMap", 1);

                    GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, v_data, BufferUsage.DynamicDraw);

                    //ReadOnlySpan<float> floats = new ReadOnlySpan<float>(v_data, 0, nlen);
                    //GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
                    //GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, v_data);




                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
                    //GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)0, i_data);
                    GL.BufferData(BufferTarget.ElementArrayBuffer,indices, BufferUsage.DynamicDraw);





                
                        GL.EnableVertexAttribArray(0);
                        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 13 * 4, 0);

                        GL.EnableVertexAttribArray(1);
                        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 13 * 4, 3 * 4);

                        GL.EnableVertexAttribArray(2);
                        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 13 * 4, 7 * 4);

                        GL.EnableVertexAttribArray(3);
                        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 13 * 4, 9 * 4);

                   
                 //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);


                    GL.DrawElements(PrimitiveType.Quads,4, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawElementsInstanced(PrimitiveType.Triangles, list.Calls.Count * 6, DrawElementsType.UnsignedInt,0,1);



                }
            }

        }
        public void End()
        {

            //End2();
            //return;
           
            int w = StarApp.FrameWidth;
            int h = StarApp.FrameHeight;
            //int b = 0;
            var se_Projection = Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1.0f, 1.0f);

           // DrawNormal.Bind();

            DrawNormal.SetMat("se_Projection",se_Projection);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, w, h);

            if (VertexArray == 0)
            {
                VertexArray = GL.GenVertexArray();
                Buffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, 1024 * 5000, IntPtr.Zero, BufferUsage.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                IndexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, 1024 * 5000, IntPtr.Zero, BufferUsage.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }

            foreach(var list in Lists)
            {

                GL.BindVertexArray(VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer,Buffer);


                int nlen = list.Calls.Count * 3 * 4 * 2 * 4 * 4;

                if (nlen > v_len)
                {
                    
                    v_len = nlen;
                    v_data = new float[v_len];

                }
                int ix = 0;
                //v_data = new float[nlen];

                foreach (var call in list.Calls)
                {

                    //v0
                    v_data[ix++] = call.X[0];
                    v_data[ix++] = call.Y[0];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 0;
                    v_data[ix++] = 0;

                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v1
                    v_data[ix++] = call.X[1];
                    v_data[ix++] = call.Y[1];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 1;
                    v_data[ix++] = 0;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v2
                    v_data[ix++] = call.X[2];
                    v_data[ix++] = call.Y[2];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 1;
                    v_data[ix++] = 1;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;

                    //v3 
                    v_data[ix++] = call.X[3];
                    v_data[ix++] = call.Y[3];
                    v_data[ix++] = call.Z;

                    v_data[ix++] = call.Color.X;
                    v_data[ix++] = call.Color.Y;
                    v_data[ix++] = call.Color.Z;
                    v_data[ix++] = call.Color.Z;

                    v_data[ix++] = 0;
                    v_data[ix++] = 1;


                    v_data[ix++] = call.Ext.X;
                    v_data[ix++] = call.Ext.Y;
                    v_data[ix++] = call.Ext.Z;
                    v_data[ix++] = call.Ext.W;


                }

                int nilen = list.Calls.Count * 6;


                if (nilen > i_len)
                {

                     i_data = new uint[list.Calls.Count * 6];
                    i_len = nilen;
                }

                ix = 0;

                uint vi = 0;


                for(int i = 0; i < list.Calls.Count; i++)
                {

                    i_data[ix++] = vi;
                    i_data[ix++] = vi + 1;
                    i_data[ix++] = vi + 2;
                    i_data[ix++] = vi + 3;

                    //i_data[ix++] = vi + 3;
                    //i_data[ix++] = vi;
                    vi = vi + 4;

                }


                list.Calls[0].Image.Bind(0);
                list.Calls[0].Normals.Bind(1);

                DrawNormal.SetInt("se_ColorTexture", 0);
                DrawNormal.SetInt("se_NormalMap", 1);


                //GL.BufferData(BufferTarget.ArrayBuffer,v_data, BufferUsage.DynamicDraw);
                //GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, nlen, v_data);
                GCHandle handle = GCHandle.Alloc(v_data, GCHandleType.Pinned);
                IntPtr pointer = handle.AddrOfPinnedObject();
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, nlen* sizeof(float), pointer);

                //ReadOnlySpan<float> floats = new ReadOnlySpan<float>(v_data, 0, nlen);
                //GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
                //    GL.BufferSubData(BufferTarget.ArrayBuffer,(IntPtr)0,v_data);


                GCHandle handle1 = GCHandle.Alloc(i_data, GCHandleType.Pinned);
                IntPtr pointer1 = handle1.AddrOfPinnedObject();



                //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (int)vi * sizeof(uint), pointer1);
                //GL.BufferData(BufferTarget.ElementArrayBuffer, i_data, BufferUsage.DynamicDraw);





                if (vcc==false)
                {
                    vcc = true;
                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 13 * 4, 0);

                    GL.EnableVertexAttribArray(1);
                    GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 13 * 4, 3 * 4);

                    GL.EnableVertexAttribArray(2);
                    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 13 * 4, 7 * 4);

                    GL.EnableVertexAttribArray(3);
                    GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 13 * 4, 9 * 4);

                }

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

                GL.DrawElements(PrimitiveType.Quads, list.Calls.Count * 4, DrawElementsType.UnsignedInt, 0);
                //GL.DrawElementsInstanced(PrimitiveType.Triangles, list.Calls.Count * 6, DrawElementsType.UnsignedInt,0,1);


               //GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
               // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
              //  GL.BindVertexArray(0);
              //  GL.DisableVertexAttribArray(0);
              //  GL.DisableVertexAttribArray(1);
              //  GL.DisableVertexAttribArray(2);



                //list.Calls[0].Image.Release(0);
               // list.Calls[0].Normals.Release(1);






            }



        }
        bool vcc = false;

    }
}
