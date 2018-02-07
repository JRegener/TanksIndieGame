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
        private Thread thread;
        //желательный fps
        private static int MAX_FPS = 50;
        //максимальное число кадров, которые можно пропустить
        private static int MAX_FRAME_SKIPS = 5;
        //период, который занимает кадр(последовательность обновление-рисование)
        private static int FRAME_PERIOD = 1000 / MAX_FPS;
        private Stopwatch stopwatch = null;
        private OpenGLControl glControl = null;

        private bool isRunning;
        public bool IsRunning
        {
            set { this.isRunning = value; }
        }

        public MainGameLoopThread(bool isRunning, OpenGLControl glControl)
        {
            this.isRunning = isRunning;
            this.glControl = glControl;

            this.thread = new Thread(this.Run);
            this.thread.IsBackground = true;
            this.thread.Start();
            this.stopwatch = Stopwatch.StartNew();
        }

        private void Run()
        {

            long beginTime;     // время начала цикла
            long timeDiff;      // время выполнения шага цикла
            int sleepTime;      // сколько мс можно спать (<0 если выполнение опаздывает)
            int framesSkipped;  // число кадров у которых не выполнялась операция вывода графики на экран

            sleepTime = 0;

            while (isRunning)
            {
                try
                {
                    beginTime = stopwatch.ElapsedMilliseconds;
                    framesSkipped = 0;  // обнуляем счетчик пропущенных кадро
                                        // обновляем состояние игры
                    UpdateGame();
                    // формируем новый кадр
                    UpdateDisplay(); //Вызываем метод для рисования
                                     // вычисляем время, которое прошло с момента запуска цикла
                    timeDiff = stopwatch.ElapsedMilliseconds - beginTime;
                    // вычисляем время, которое можно спать
                    sleepTime = (int)(FRAME_PERIOD - timeDiff);

                    if (sleepTime > 0)
                    {
                        // если sleepTime > 0 все хорошо, мы идем с опережением
                        try
                        {
                            // отправляем поток в сон на период sleepTime
                            // такой ход экономит к тому же заряд батареи
                            Thread.Sleep(sleepTime);
                        }
                        catch (Exception e) { }
                    }

                    while (sleepTime < 0 && framesSkipped < MAX_FRAME_SKIPS)
                    {
                        // если sleepTime < 0 нам нужно обновлять игровую
                        // ситуацию и не тратить время на вывод кадра
                        UpdateGame();
                        // добавляем смещение FRAME_PERIOD, чтобы иметь
                        // время границы следующего кадра
                        sleepTime += FRAME_PERIOD;
                        framesSkipped++;
                    }

                }
                finally { }
            }
        }


        private void UpdateGame()
        {

        }

        private void UpdateDisplay()
        {
            glControl.Invoke(new Action(() => { glControl.DoRender(); })); 
        }
    }
}
