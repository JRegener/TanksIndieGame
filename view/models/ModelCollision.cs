using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class ModelCollision
    {
        private float length; // Z
        private float weight; // X
        private float height; // Y


        public ModelCollision(ModelCollision modelCollision)
        {
            this.length = modelCollision.Length;
            this.weight = modelCollision.Weight;
            this.height = modelCollision.Height;
        }

        public ModelCollision(float length, float weight, float height)
        {
            this.length = length;
            this.weight = weight;
            this.height = height;
        }


        #region properties
        public float Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }
        #endregion

    }
}
