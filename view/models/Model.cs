﻿using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.logic;
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

        private bool isKinematic;

        private CollisionObjects collisionObjects = null;

        private ObjectBehaviour objectBehaviour = null;

        private BaseObject baseObject = null;

        private ModelView modelView = null;

        private BaseShader baseShader = null;

        private ModelCollision modelCollision = null;

        public Model(OpenGL gl, Loader loader, string tag, bool isKinematic, BaseObject baseObject,
            ModelView modelView, BaseShader baseShader, ModelCollision modelCollision)
        {
            this.gl = gl;
            this.loader = loader;
            this.tag = tag;
            this.isKinematic = isKinematic;
            this.baseObject = baseObject;
            this.modelView = modelView;
            this.baseShader = baseShader;
            this.modelCollision = modelCollision;
            this.collisionObjects = new CollisionObjects();
        }
        #region properties
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

        public BaseObject BaseObject
        {
            get
            {
                return baseObject;
            }

            set
            {
                baseObject = value;
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

        public ObjectBehaviour ObjectBehaviour
        {
            get
            {
                return objectBehaviour;
            }

            set
            {
                objectBehaviour = value;
            }
        }

        public CollisionObjects CollisionObjects
        {
            get
            {
                return collisionObjects;
            }

            set
            {
                collisionObjects = value;
            }
        }

        public bool IsKinematic
        {
            get
            {
                return isKinematic;
            }

            set
            {
                isKinematic = value;
            }
        }

        #endregion

        public object Clone()
        {
            Model model = loader.LoadModel(String.Copy(this.tag), this.isKinematic, gl, 0, 0, 0, 0, 0, 0, 1f,
                (float[])this.modelView.Vertices.Clone(), (uint[])this.modelView.Indices.Clone(),
                (float[])this.modelView.Uv.Clone(), (float[])this.modelView.Normals.Clone(),
                (Image)this.modelView.Texture.Clone(), this.ModelCollision.Radius,
                this.baseShader.VertexShaderCode, this.baseShader.FragmentShaderCode, this.baseShader.Lights);
            return model;
        }

        public void Destroy()
        {

            //free video memory
            loader.FreeModel(gl, this);

            collisionObjects.Destroy();
            collisionObjects = null;

            baseObject = null;

            modelView.Destroy();
            modelView = null;

            baseShader = null;

            modelCollision = null;
        }
    }
}
