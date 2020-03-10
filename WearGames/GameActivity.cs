using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Wearable.Activity;
using Android.Views;
using Android.Widget;
using System;
using WearGames.Components.Breakout;

namespace WearGames
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class GameActivity : WearableActivity
    {
        public static GameActivity Instance
        { get; private set; }


        private GameLoop _gameLoop = null;
        private Countdown _countdown = null;
        private Scoreboard _scoreboard = null;

        private bool _isInitialized = false;
        private RelativeLayout _mainLayout = null;
        private RelativeLayout _mainMenu = null;
        private RelativeLayout _blocksLayout = null;
        private PaddleView _paddle = null;
        private TextView _timerLabel = null;

        private CheckBox[] _rectryCheckboxes = new CheckBox[3];


        #region Android - Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            GameActivity.Instance = this;

            SetContentView(Resource.Layout.GameActivity);

            _gameLoop = new GameLoop(30);
            _gameLoop.Stop();
            // hook up game loop events
            //GameUpdateTask.Instance.EarlyUpdate += EarlyUpdate;
            GameUpdateTask.Update += Update;
            //GameUpdateTask.Instance.LateUpdate += LateUpdate;

            _rectryCheckboxes[0] = this.FindViewById<CheckBox>(Resource.Id.retryCheckBox0);
            _rectryCheckboxes[1] = this.FindViewById<CheckBox>(Resource.Id.retryCheckBox1);
            _rectryCheckboxes[2] = this.FindViewById<CheckBox>(Resource.Id.retryCheckBox2);

            _mainLayout = this.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
            _mainMenu = this.FindViewById<RelativeLayout>(Resource.Id.mainMenu);
            _mainLayout.ViewTreeObserver.GlobalLayout += Initialize;

            _blocksLayout = this.FindViewById<RelativeLayout>(Resource.Id.blocksLayout);

            _countdown = new Countdown(this.FindViewById<TextView>(Resource.Id.countdownLabel));
            _scoreboard = new Scoreboard(this.FindViewById<RelativeLayout>(Resource.Id.scoreboard));
            _scoreboard.ScoreSubmitted += Scoreboard_ScoreSubmitted;
            _scoreboard.Deserialize();

            _timerLabel = this.FindViewById<TextView>(Resource.Id.timerLabel);

            _paddle = this.FindViewById<PaddleView>(Resource.Id.paddle);
            _paddle.RequestFocus();
            _isInitialized = false;

        }


        protected override void OnPause()
        {
            base.OnPause();
            _gameLoop.Pause();
            _scoreboard.Serialize();
        }
        protected override void OnResume()
        {
            base.OnResume();
            _scoreboard.Deserialize();
            _gameLoop.Resume();
        }
        protected override void OnDestroy()
        {
            _gameLoop.Stop();
            _scoreboard.Serialize();
            base.OnDestroy();
        }
        #endregion

        public override bool OnGenericMotionEvent(MotionEvent ev)
        {
            return _paddle.OnGenericMotionEvent(ev);
        }

        private void Initialize(object sender, System.EventArgs e)
        {
            if (!_isInitialized)
            {
                _mainLayout = this.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
                BlockView.BlockDestroyed += BlockView_BlockDestroyed;
                BallView.BallDestroyed += BallView_BallDestroyed;

                this.ResetGame();
                _isInitialized = true;
            }
        }

        //private void EarlyUpdate()
        //{ }
        private void Update()
        {
            GameState.AdvanceTime(_gameLoop.DeltaTimeF);
            this.UpdateTimer();
        }
        //private void LateUpdate()
        //{ }


        [Java.Interop.Export("StartGameCountdown")]
        public void StartGameCountdown(View v)
        {
            this.ResetGame();
            this.SpawnBall(_mainLayout.Width / 2, _mainLayout.Height - 40);
            // add grid of blocks
            SimpleBlockGrid blockGrid = null;
            blockGrid = new SimpleBlockGrid() { X = 7, Y = 5, Width = 25, Height = 16, Structure = 1 };
            //blockGrid = new SimpleBlockGrid() { X = 1, Y = 1, Width = 25, Height = 16, Structure = 3 };
            blockGrid.Create(this._blocksLayout, 0, 40, 5);

            _mainMenu.Visibility = ViewStates.Gone;
            _countdown.Run(3, StartGame);
        }

        [Java.Interop.Export("QuitApp")]
        public void QuitApp(View v)
        {
            this.EndGame(false);
            // finish the current running activity
            this.FinishAffinity();
        }
        private void StartGame()
        {
            _gameLoop.Start();
        }
        private void EndGame(bool success)
        {
            _gameLoop.Stop();
            _scoreboard.Show(true);
            if (success)
                _scoreboard.ShowView(ScoraboadViews.Success);
            else
                _scoreboard.ShowView(ScoraboadViews.Failure);
        }
        private void PauseGame(bool runResumeCountdown = false)
        {
            _gameLoop.Pause();
            if (runResumeCountdown)
                _countdown.Run(3, this.ResumeGame);
        }
        private void ResumeGame()
        {
            _gameLoop.Resume();
        }
        [Java.Interop.Export("BackToMenu")]
        public void BackToMenu(View v)
        {
            _scoreboard.Show(false);
            _mainMenu.Visibility = ViewStates.Visible;
            this.ResetGame();
        }
        [Java.Interop.Export("ShowScoreboard")]
        public void ShowScoreboard(View v)
        {
            _mainMenu.Visibility = ViewStates.Gone;
            _scoreboard.Show(true);
            _scoreboard.ShowView(ScoraboadViews.List);
        }
        [Java.Interop.Export("ClearScoreboard")]
        public void ClearScoreboard(View v)
        {
            _scoreboard.Clear();
            _scoreboard.ShowView(ScoraboadViews.List);
        }

        private void ResetGame()
        {
            BlockView.Clear();
            BallView.Clear();
            GameState.Reset(3);
            // rest UI elements
            _paddle.Rotation = 0.0f;
            this.UpdateTimer();
            this.UpdateCredits();
            _mainLayout.Invalidate();
        }

        public void SpawnBall(float x, float y)
        {
            BallView ball = BallView.Create(_mainLayout, x, y);
            ball.Size = 14;
        }
        private void UpdateTimer()
        {
            _timerLabel.Text = GameState.FormattedTime;
        }
        private void UpdateCredits()
        {
            for (int i = 0; i < _rectryCheckboxes.Length; i++)
            { _rectryCheckboxes[i].Checked = i < GameState.Credits; }
        }

        private void Scoreboard_ScoreSubmitted()
        {
            _paddle.RequestFocus();
        }
        private void BlockView_BlockDestroyed()
        {
            if (BlockView.Instances.Count == 0)
                this.EndGame(true);
        }
        private void BallView_BallDestroyed()
        {
            GameState.DecreaseLifes();
            this.UpdateCredits();
            if (GameState.Credits <= 0)
            {
                this.EndGame(false);
            }
            else
            {
                // ! ! ! !
                //  pause game and continue with new countdown to allow player reorientation
                this.PauseGame(true);
                PaddleView.Instance.Rotation = 0;
                // ! ! ! !
                // maybe determine ball position based on the current position of the paddle
                this.SpawnBall(_mainLayout.Width / 2, _mainLayout.Height - 40);
            }
        }

    }

}