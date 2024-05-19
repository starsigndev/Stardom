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

        }

        public DrawList FindList(Texture2D image)
        {

            if (ListMap.ContainsKey(image))
            {
                return ListMap[image];
            }

            DrawList new_list = new DrawList();

            ListMap.Add(image, new_list);
            Lists.Add(new_list);

            return new_list;

        }


        public void DrawSprite(Texture2D image,Vector2 position,Vector2 size,float rotation,float scale,Vector4 color)
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

            CurrentZ += 0.01f;

            call.Image = image;

            list.AddCall(call);

        }

        public void DrawQuad(Texture2D image,Vector2 position,Vector2 size,Vector4 color)
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

            CurrentZ += 0.01f;

            call.Color = color;

            call.Image = image;

            list.AddCall(call);


        }

        public void End()
        {

            int w = StarApp.FrameWidth;
            int h = StarApp.FrameHeight;
            //int b = 0;
            var se_Projection = Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1.0f, 1.0f);

            DrawNormal.Bind();

            DrawNormal.SetMat("se_Projection",se_Projection);

            GL.Disable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, w, h);

            VertexArray = GL.GenVertexArray();
            Buffer = GL.GenBuffer();
            IndexBuffer = GL.GenBuffer();

            foreach(var list in Lists)
            {

                GL.BindVertexArray(VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer,Buffer);

                float[] v_data = new float[list.Calls.Count * 3 * 4 * 2*4];

                int ix = 0;

                foreach(var call in list.Calls)
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

                }

                uint[] i_data = new uint[list.Calls.Count * 6];

                ix = 0;

                uint vi = 0;


                for(int i = 0; i < list.Calls.Count; i++)
                {

                    i_data[ix++] = vi;
                    i_data[ix++] = vi + 1;
                    i_data[ix++] = vi + 2;
                    i_data[ix++] = vi + 2;
                    i_data[ix++] = vi + 3;
                    i_data[ix++] = vi;
                    vi = vi + 4;

                }


                list.Calls[0].Image.Bind(0);

                DrawNormal.SetInt("se_ColorTexture", 0);

                GL.BufferData(BufferTarget.ArrayBuffer, v_data, BufferUsage.DynamicDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, i_data, BufferUsage.DynamicDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 9 * 4, 7 * 4);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);

                GL.DrawElements(PrimitiveType.Triangles, list.Calls.Count * 6, DrawElementsType.UnsignedInt, 0);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                list.Calls[0].Image.Release(0);






            }



        }

    }
}
