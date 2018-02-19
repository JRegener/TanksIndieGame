using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class CollisionData
    {
        public CollisionData(Model other, vec2 overlap = new vec2(),
            vec2 pos1 = new vec2(), vec2 pos2 = new vec2())
        {
            this.other = other;
            this.overlap = overlap;
            this.pos1 = pos1;
            this.pos2 = pos2;
        }

        public Model other;
        public vec2 overlap;
        public vec2 pos1, pos2;
    }
}
