using Java.Util;
using System;

namespace WearGames
{

    public class GameUpdateTask : TimerTask
    {
        private static GameUpdateTask _instance = new GameUpdateTask();
        public static GameUpdateTask Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameUpdateTask();
                return _instance;
            }
        }

        public static event Action EarlyUpdate;
        public static event Action Update;
        public static event Action LateUpdate;


        private GameUpdateTask()
        { }

        public override void Run()
        {
            GameActivity.Instance.RunOnUiThread(() =>
            {
                this.OnEarlyUpdate();
                this.OnUpdate();
                this.OnLateUpdate();
            });
        }

        public static void Pause()
        {
            GameUpdateTask.Instance.Cancel();
            _instance = null;
        }

        private void OnEarlyUpdate()
        {
            if (EarlyUpdate != null)
                EarlyUpdate.Invoke();
        }
        private void OnUpdate()
        {
            if (Update != null)
                Update.Invoke();
        }
        private void OnLateUpdate()
        {
            if (LateUpdate != null)
                LateUpdate.Invoke();
        }

    }

}