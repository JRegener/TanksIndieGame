using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.view.shaders
{
    public abstract class BaseShader
    {
        public abstract void LoadVariables();

        protected OpenGL gl;
        protected string vertexShaderCode = "";
        protected string fragmentShaderCode = "";
        protected Dictionary<uint, string> attributeLocation = new Dictionary<uint, string>();
        protected ShaderProgram shaderProgram = new ShaderProgram();

        protected int locationTransformationMatrix;
        protected int locationProjectionMatrix;
        protected int locationViewMatrix;

        private Light lights;

        public string VertexShaderCode
        {
            get { return vertexShaderCode; }
            set { vertexShaderCode = value; }
        }

        public string FragmentShaderCode
        {
            get { return fragmentShaderCode; }
            set { fragmentShaderCode = value; }
        }

        public BaseShader(OpenGL gl, string vertexShaderCode, string fragmentShaderCode, Light lights)
        {
            this.vertexShaderCode = vertexShaderCode;
            this.fragmentShaderCode = fragmentShaderCode;
            this.gl = gl;
            this.lights = lights;
            CreateShaders();
        }

        private void CreateShaders()
        {
            AddAttributeLocation(0, "position");
            AddAttributeLocation(1, "textureCoordinates");
            AddAttributeLocation(2, "normal");

            try
            {
                shaderProgram.Create(gl, vertexShaderCode, fragmentShaderCode,
                    attributeLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine("SHADER ERROR");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("SHADER ERROR");
            }
        }

        private void AddAttributeLocation(uint key, string value)
        {
            attributeLocation.Add(key, value);
        }

        /// <summary>
        /// Open shader connection 
        /// </summary>
        public void Start()
        {
            shaderProgram.Bind(gl);
        }

        /// <summary>
        /// Close shader connection 
        /// </summary>
        public void Stop()
        {
            shaderProgram.Unbind(gl);
        }

        public void Delete()
        {
            shaderProgram.Delete(gl);
        }

        public void LoadTransformationMatrix(mat4 matrix)
        {
            shaderProgram.SetUniformMatrix4(gl, "transformationMatrix", matrix.to_array());
        }

        public void LoadViewMatrix(mat4 matrix)
        {
            shaderProgram.SetUniformMatrix4(gl, "viewMatrix", matrix.to_array());
        }

        public void LoadProjectionMatrix(mat4 matrix)
        {
            shaderProgram.SetUniformMatrix4(gl, "projectionMatrix", matrix.to_array());
        }

        public void LoadLight()
        {
            shaderProgram.SetUniform3(gl, "lightPosition", lights.Position.x, lights.Position.y, lights.Position.z);
            shaderProgram.SetUniform3(gl, "lightColor", lights.Color.x, lights.Color.y, lights.Color.z);
        }



        //not used
        //private void GetAllUniformLocations(OpenGL gl)
        //{
        //    _locationTransformationMatrix = _shaderProgram.
        //        GetUniformLocation(gl, "transformationMatrix");
        //    _locationProjectionMatrix = _shaderProgram.
        //        GetUniformLocation(gl, "projectionMatrix");
        //    _locationViewMatrix = _shaderProgram.
        //        GetUniformLocation(gl, "viewMatrix");
        //}

        //private int GetUniformLocation(OpenGL gl, string uniformName)
        //{
        //    return gl.GetUniformLocation(_shaderProgram.ShaderProgramObject, uniformName);
        //}


    }


}
