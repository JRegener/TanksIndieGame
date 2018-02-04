using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic
{
    public sealed class GameControl
    {

        private static readonly Lazy<GameControl> instanceHolder =
            new Lazy<GameControl>(() => new GameControl());

        private List<Model> renderModels = new List<Model>();
        private Light lights = SceneSettings.BASE_LIGHT;


        public List<Model> RenderModels
        {
            get
            {
                return renderModels;
            }

        }

        public Light Lights
        {
            get
            {
                return lights;
            }

        }

        private GameControl() { }

        public static GameControl Instance
        {
            get { return instanceHolder.Value; }
        }

    }
}
