using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic.scripts
{
    public class PlayerTankBehaviour : ObjectBehaviour
    {
        private float velocity = 0.1f;
        private vec3 direction = new vec3(-1, 0, 0);

        private bool isMove = false;

        public PlayerTankBehaviour(Model parentModel) : base(parentModel)
        {}

        public override void FixedUpdate()
        {
            if (isMove)
            {
                Console.WriteLine("before update tank position x = {0} y = {1} z = {2}",
                    parentModel.BaseObject.Position.x, 
                    parentModel.BaseObject.Position.y, 
                    parentModel.BaseObject.Position.z);

                parentModel.BaseObject.Position += velocity * direction;

            }
        }

        public override void Update(float interpolation)
        {
            if (isMove)
            {
                parentModel.BaseObject.ViewPosition = parentModel.BaseObject.Position + velocity * direction * interpolation;
            }
        }

        public void Move(bool isMove)
        {
            this.isMove = isMove;
        }
    }
}
