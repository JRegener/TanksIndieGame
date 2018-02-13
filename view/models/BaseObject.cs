using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class BaseObject
    {
        protected float posX, posY, posZ;

        protected float rotX, rotY, rotZ;

        protected float scale;

        private vec3 viewPosition;

        private vec3 viewRotation;

        public BaseObject(float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float scale)
        {
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;

            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;

            this.scale = scale;

            this.viewPosition = new vec3(posX, posY, posZ);
            this.viewRotation = new vec3(rotX, rotY, rotZ);
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
            set
            {
                posX = value.x;
                posY = value.y;
                posZ = value.z;

                viewPosition = value;
            }
        }

        public vec3 Rotation
        {
            get { return new vec3(rotX, rotY, rotZ); }
            set
            {
                rotX = value.x;
                rotY = value.y;
                rotZ = value.z;

                viewRotation = value;
            }
        }

        public vec3 ViewPosition
        {
            get
            {
                return viewPosition;
            }

            set
            {
                viewPosition = value;
            }
        }

        public vec3 ViewRotation
        {
            get
            {
                return viewRotation;
            }

            set
            {
                viewRotation = value;
            }
        }


        #endregion


    }
}
