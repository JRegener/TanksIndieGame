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
        protected Model parentModel;

        protected float startRotationAngle;

        public ObjectBehaviour(Model parentModel)
        {
            this.parentModel = parentModel;
            this.startRotationAngle = this.parentModel.BaseObject.RotY;
        }

        #region properties
        public Model ParentModel
        {
            get { return this.parentModel; }
            set { this.parentModel = value; }
        }
        #endregion

        public virtual void FixedUpdate() { }

        public virtual void Update(float interpolation) { }
    }
}
