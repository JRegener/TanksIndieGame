using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.shaders;
using TanksIndieGame.view.shaders.objects;

namespace TanksIndieGame.view.models
{
    public class Model : ICloneable
    {
        private string tag;

        private BaseModel baseModel = null;

        private ModelView modelView = null;

        private BaseShader baseShader = null;

        private ModelCollision modelCollision = null;

        public Model(string tag, BaseModel baseModel, ModelView modelView, BaseShader baseShader, ModelCollision modelCollision)
        {
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
            Model model = (Model)this.MemberwiseClone();
            model.Tag = String.Copy(this.tag);
            model.BaseModel = (BaseModel)this.baseModel.Clone();
            model.ModelView = (ModelView)this.modelView.Clone();
            model.BaseShader = (ModelShader)(baseShader as ModelShader).Clone();
            model.ModelCollision = (ModelCollision)this.modelCollision.Clone();

            return model;
        }
    }
}
