using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.render;
using TanksIndieGame.view.shaders;
using TanksIndieGame.view.shaders.objects;

namespace TanksIndieGame.view.models
{
    public class Model : ICloneable
    {
        private OpenGL gl;

        private Loader loader;

        private string tag;

        private BaseModel baseModel = null;

        private ModelView modelView = null;

        private BaseShader baseShader = null;

        private ModelCollision modelCollision = null;

        public Model(OpenGL gl, Loader loader, string tag, BaseModel baseModel, 
            ModelView modelView, BaseShader baseShader, ModelCollision modelCollision)
        {
            this.gl = gl;
            this.loader = loader;
            this.tag = tag;
            this.baseModel = baseModel;
            this.modelView = modelView;
            this.baseShader = baseShader;
            this.modelCollision = modelCollision;
        }

        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public BaseModel BaseModel
        {
            get
            {
                return baseModel;
            }

            set
            {
                baseModel = value;
            }
        }

        public ModelView ModelView
        {
            get
            {
                return modelView;
            }

            set
            {
                modelView = value;
            }
        }

        public BaseShader BaseShader
        {
            get
            {
                return baseShader;
            }

            set
            {
                baseShader = value;
            }
        }

        public ModelCollision ModelCollision
        {
            get
            {
                return modelCollision;
            }

            set
            {
                modelCollision = value;
            }
        }


        public object Clone()
        {
            Model model = loader.LoadModel(String.Copy(this.tag), gl, 0, 0, 0, 0, 0, 0, 0.1f,
                (float[])this.modelView.Vertices.Clone(), (uint[])this.modelView.Indices.Clone(), 
                (float[])this.modelView.Uv.Clone(), (float[])this.modelView.Normals.Clone(),
                (Image)this.modelView.Texture.Clone(), this.baseShader.VertexShaderCode, 
                this.baseShader.FragmentShaderCode, this.baseShader.Lights);
            return model;
        }
    }
}
