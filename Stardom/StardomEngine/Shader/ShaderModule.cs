using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace StardomEngine.Shader
{
    public class ShaderModule
    {

        public int VertexShader { get; set; }
        public int FragShader { get; set; }

        public int Program { get; set; }

        public Dictionary<string,int> Cache
        {
            get;
            set;
        }

        public ShaderModule(string vertex_shader,string fragment_shader)
        {

            Cache = new Dictionary<string, int>(128);
            VertexShader = CompileShader(ShaderType.VertexShader,vertex_shader);
            FragShader = CompileShader(ShaderType.FragmentShader, fragment_shader);

            Program = GL.CreateProgram();
            GL.AttachShader(Program, VertexShader);
            GL.AttachShader(Program, FragShader);

            GL.LinkProgram(Program);

            int stat = GL.GetProgrami(Program, ProgramProperty.LinkStatus);

            if (stat == 0)
            {

                string status = "";

                GL.GetProgramInfoLog(Program, out status);

                Console.WriteLine("Program Link Faliure.\n" + status);

            }

            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragShader);

        }

        public int CompileShader(ShaderType type,string path)
        {

            int shader = GL.CreateShader(type);

            GL.ShaderSource(shader, File.ReadAllText(path));

            GL.CompileShader(shader);

            int id = GL.GetShaderi(shader, ShaderParameterName.CompileStatus);

            if (id==0)
            {

                string info = "None";
                GL.GetShaderInfoLog(shader, out info);

                Console.WriteLine("Shader Compile Error\n" + info);

                Environment.Exit(1);



            }
            else
            {
                Console.WriteLine("Compiled shader:" + path);
            }

            return shader;



        }

        public int GetLoc(string name)
        {

            //int hash = Helper.Helpers.GenerateHash(name);

            if (Cache.ContainsKey(name))
            {
                return Cache[name];
            }

            int loc = GL.GetUniformLocation(Program, name);
            Cache.Add(name, loc);
           

            return loc;
        }

        public void SetInt(string name,int value)
        {

            GL.Uniform1i(GetLoc(name), value);

        }

        public void SetFloat(string name,float value)
        {

            GL.Uniform1f(GetLoc(name), value);

        }

        public void SetVec2(string name,Vector2 value)
        {

            GL.Uniform2f(GetLoc(name),value.X,value.Y);

        }

        public void SetVec3(string name,Vector3 value)
        {

            GL.Uniform3f(GetLoc(name), value.X, value.Y, value.Z);

        }

        public void SetVec4(string name,Vector4 value)
        {

            GL.Uniform4f(GetLoc(name), value.X, value.Y, value.Z, value.W);

        }

        public void SetMat(string name,Matrix4 value)
        {

            GL.UniformMatrix4f(GetLoc(name), 1,false, in value);

        }



        public void Bind()
        {

            GL.UseProgram(Program);

        }

        public void Release()
        {

            GL.UseProgram(0);

        }

    }
}
