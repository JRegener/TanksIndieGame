using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class BaseModel : ICloneable
    {
        private float posX, posY, posZ;

        private float rotX, rotY, rotZ;

        private float scale;

        public BaseModel(float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float scale)
        {
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;

            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;

            this.scale = scale;
        }
        #region properties
        public float PosX
        {
            get
            {
                return posX;
            }

            set
            {
                posX = value;
            }
        }

        public float PosY
        {
            get
            {
                return posY;
            }

            set
            {
                posY = value;
            }
        }

        public float PosZ
        {
            get
            {
                return posZ;
            }

            set
            {
                posZ = value;
            }
        }

        public float RotX
        {
            get
            {
                return rotX;
            }

            set
            {
                rotX = value;
            }
        }

        public float RotY
        {
            get
            {
                return rotY;
            }

            set
            {
                rotY = value;
            }
        }

        public float RotZ
        {
            get
            {
                return rotZ;
            }

            set
            {
                rotZ = value;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public vec3 Position
        {
            get { return new vec3(posX, posY, posZ); }
        }

        public vec3 Rotation
        {
            get { return new vec3(rotX, rotY, rotZ); }
        }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
