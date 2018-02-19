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

        private float halfLength;
        private float halfWeight;

        public ModelCollision(ModelCollision modelCollision)
        {
            this.length = modelCollision.Length;
            this.weight = modelCollision.Weight;
            this.halfLength = this.length / 2;
            this.halfWeight = this.weight / 2;
        }

        public ModelCollision(float weight, float length)
        {
            this.length = length;
            this.weight = weight;
            this.halfLength = this.length / 2;
            this.halfWeight = this.weight / 2;
        }


        #region properties
        /// <summary>
        /// Z axis
        /// </summary>
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

        /// <summary>
        /// X Axis
        /// </summary>
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

        /// <summary>
        /// Z axis
        /// </summary>
        public float HalfLength
        {
            get
            {
                return halfLength;
            }

            set
            {
                halfLength = value;
            }
        }

        /// <summary>
        /// X Axis
        /// </summary>
        public float HalfWeight
        {
            get
            {
                return halfWeight;
            }

            set
            {
                halfWeight = value;
            }
        }

        #endregion

    }
}
