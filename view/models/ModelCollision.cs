using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class ModelCollision
    {
        private float radius;

        private float halfRadius;



        public ModelCollision(ModelCollision modelCollision)
        {
            this.radius = modelCollision.Radius;
            this.halfRadius = this.radius / 2;
        }

        public ModelCollision(float radius)
        {
            this.radius = radius;
            this.halfRadius = this.radius / 2;
        }


        #region properties

        public float Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public float HalfRadius
        {
            get
            {
                return halfRadius;
            }

            set
            {
                halfRadius = value;
            }
        }
        #endregion

    }
}
