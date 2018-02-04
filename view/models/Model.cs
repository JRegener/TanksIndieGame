using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.shaders;

namespace TanksIndieGame.view.models
{
    public class Model
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
    }
}
