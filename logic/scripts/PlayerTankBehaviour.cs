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

        public PlayerTankBehaviour(BaseObject baseObject, ModelCollision modelCollision)
            : base(baseObject, modelCollision)
        { }

        public override void FixedUpdate()
        {
            if (isMove)
            {
                Console.WriteLine("before update tank position x = {0} y = {1} z = {2}", baseObject.Position.x, baseObject.Position.y, baseObject.Position.z);
                baseObject.Position += velocity * direction;

            }
        }

        public override void Update(float interpolation)
        {
            if (isMove)
            {
                baseObject.ViewPosition = baseObject.Position + velocity * direction * interpolation;
            }
        }

        public void Move(bool isMove)
        {
            this.isMove = isMove;
        }
    }
}
