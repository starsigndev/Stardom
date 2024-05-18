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

        public ShaderModule(string vertex_shader,string fragment_shader)
        {

            VertexShader = CompileShader(ShaderType.VertexShader,vertex_shader);
            FragShader = CompileShader(ShaderType.FragmentShader, fragment_shader);


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

            }
            else
            {
                Console.WriteLine("Compiled shader:" + path);
            }

            return shader;


        }


    }
}
