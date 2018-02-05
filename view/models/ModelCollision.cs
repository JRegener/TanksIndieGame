using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class ModelCollision : ICloneable
    {
        private float length; // Z
        private float weight; // X
        private float height; // Y


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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
