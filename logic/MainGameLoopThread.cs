using SharpGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace TanksIndieGame.logic
{
    public class MainGameLoopThread
    {

        //желательный fps
        private const int MAX_FPS = 50;
        //максимальное число кадров, которые можно пропустить
        private const int MAX_FRAME_SKIPS = 5;
        //период, который занимает кадр(последовательность обновление-рисование)
        private const int FRAME_PERIOD = 1000 / MAX_FPS;

        private Thread thread;

        private GameObjects gameObjects = null;
        private Stopwatch stopwatch = null;
        private OpenGLControl glControl = null;

        public MainGameLoopThread(OpenGLControl glControl)
        {
            this.glControl = glControl;
            this.gameObjects = GameObjects.Instance;

            this.thread = new Thread(this.Run);
            this.thread.IsBackground = true;
        }

        public void Start()
        {
            this.thread.Start();
            this.stopwatch = Stopwatch.StartNew();
        }

        private void Run()
        {
            long currentTime;     // время начала цикла
            float interpolation = 0;
            int loops;
            currentTime = stopwatch.ElapsedMilliseconds;


            while (true)
            {
                loops = 0;
                while (stopwatch.ElapsedMilliseconds > currentTime && loops < MAX_FRAME_SKIPS)
                {
                    UpdateGame();

                    currentTime += FRAME_PERIOD;
                    loops++;
                }

                interpolation = (stopwatch.ElapsedMilliseconds + FRAME_PERIOD - currentTime)
                                / (float)(FRAME_PERIOD);
                UpdateDisplay(interpolation);

            }
        }


        private void UpdateGame()
        {

            // Update object logic
            for (int i = 0; i < gameObjects.GameModels.Count; i++)
            {
                if (gameObjects.GameModels[i].ObjectBehaviour != null)
                    gameObjects.GameModels[i].ObjectBehaviour.FixedUpdate();
            }

        }

        private void UpdateDisplay(float interpolation)
        {
            for (int i = 0; i < gameObjects.GameModels.Count; i++)
            {
                if (gameObjects.GameModels[i].ObjectBehaviour != null)
                    gameObjects.GameModels[i].ObjectBehaviour.Update(interpolation);
            }

            glControl.Invoke(new Action(() => { glControl.DoRender(); }));
        }


    }
}
