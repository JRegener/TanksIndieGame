using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic
{
    public class ObjectBehaviour
    {
        protected BaseObject baseObject;
        protected ModelCollision modelCollision;

        public ObjectBehaviour(BaseObject baseObject, ModelCollision modelCollision)
        {
            this.baseObject = baseObject;
            this.modelCollision = modelCollision;
        }

        #region properties
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
        #endregion

        public virtual void FixedUpdate() { }

        public virtual void Update(float interpolation) { }
    }
}
