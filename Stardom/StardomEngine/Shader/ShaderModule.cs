using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace StardomEngine.Shader
{
    public class ShaderModule
    {

        public int VertexShader { get; set; }
        public int FragShader { get; set; }

        public int Program { get; set; }

        public ShaderModule(string vertex_shader,string fragment_shader)
        {

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


    }
}
