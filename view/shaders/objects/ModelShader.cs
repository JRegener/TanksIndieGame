using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using TanksIndieGame.view.models;

namespace TanksIndieGame.view.shaders.objects
{
    public class ModelShader : BaseShader, ICloneable
    {
        public ModelShader(OpenGL gl, string vertexShaderCode, string fragmentShaderCode, Light lights)
            : base(gl, vertexShaderCode, fragmentShaderCode, lights)
        {
        }



        public override void LoadVariables()
        {
            LoadLight();
        }

        public object Clone()
        {
            ModelShader modelShader = (ModelShader)this.MemberwiseClone();
            modelShader.gl = gl;
            modelShader.attributeLocation = new Dictionary<uint, string>();
            modelShader.shaderProgram = new ShaderProgram();
            modelShader.lights = (Light)this.lights.Clone();
            modelShader.CreateShaders();
            
            return modelShader;
        }
    }
}
