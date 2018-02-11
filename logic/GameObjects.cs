using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksIndieGame.view.models;

namespace TanksIndieGame.logic
{
    public sealed class GameObjects
    {

        private static readonly Lazy<GameObjects> instanceHolder =
            new Lazy<GameObjects>(() => new GameObjects());

        private List<Model> gameModels = new List<Model>();
        private Light lights = SceneSettings.BASE_LIGHT;


        public List<Model> GameModels
        {
            get
            {
                return gameModels;
            }

        }

        public Light Lights
        {
            get
            {
                return lights;
            }

        }

        private GameObjects() { }

        public static GameObjects Instance
        {
            get { return instanceHolder.Value; }
        }

    }
}
