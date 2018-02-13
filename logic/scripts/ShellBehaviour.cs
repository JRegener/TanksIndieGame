using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic.scripts
{
    public class ShellBehaviour : ObjectBehaviour
    {
        private float velocity = 0.5f;

        //


        public ShellBehaviour(Model parentModel) : base(parentModel)
        { }

        public override void FixedUpdate()
        {
            if (parentModel != null)
                parentModel.BaseObject.Position += velocity * parentModel.BaseObject.Rotation;
        }

        public override void Update(float interpolation)
        {
            if (parentModel != null)
                parentModel.BaseObject.ViewPosition = parentModel.BaseObject.Position
                + velocity * parentModel.BaseObject.Rotation * interpolation;
        }
    }
}
