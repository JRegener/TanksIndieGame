using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksIndieGame.view.models
{
    public class Light : ICloneable
    {
        private vec3 position;
        private vec3 color;

        public vec3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public vec3 Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Light(vec3 position, vec3 color)
        {
            this.position = position;
            this.color = color;
        }

        public object Clone()
        {
            Light light = (Light)this.MemberwiseClone();
            light.Position = new vec3(this.position);
            light.color = new vec3(this.color);
            return light;
        }
    }
}
