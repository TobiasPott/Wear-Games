using Java.Util;
using System;

namespace WearGames
{
    public class GameLoop : Timer
    {
        public int Framerate
        { get; private set; } = 30;
        public int DeltaTime
        { get { return 1000 / Framerate; } }
        public float DeltaTimeF
        { get { return DeltaTime / 1000.0f; } }

        public static long Frame
        { get; private set; }
        public static float Time
        { get; private set; }


        private bool _isRunning = false;
        private bool _wasRunning = false;

        public GameLoop(int framerate = 30)
            : base(nameof(GameLoop))
        {
            GameUpdateTask.EarlyUpdate += EarlyUpdate;
            //GameUpdateTask.Update += Update;
            //GameUpdateTask.LateUpdate += LateUpdate;
        }

        private void EarlyUpdate()
        {
            GameLoop.Time += DeltaTime * 0.001f;
            GameLoop.Frame++;
        }
        //private void Update()
        //{
        //}
        //private void LateUpdate()
        //{
        //}


        public void Start(int framerate = -1)
        {
            if (!_isRunning)
            {
                if (framerate > 0)
                    this.Framerate = framerate;
                this.ScheduleAtFixedRate(GameUpdateTask.Instance, 0, this.DeltaTime);
                _isRunning = true;
            }
        }
        public void Resume()
        {
            if (_wasRunning)
                this.Start();
        }
        public void Pause()
        {
            if (_isRunning)
            {
                this._wasRunning = _isRunning;
                this._isRunning = false;
                GameUpdateTask.Pause();
            }
        }
        public void Stop()
        {
            this.Pause();
            GameLoop.Frame = 0;
            _wasRunning = false;
        }

    }

}