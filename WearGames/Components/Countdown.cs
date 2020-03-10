using Android.Views;
using Android.Widget;
using Java.Lang;
using System;

namespace WearGames
{
    public class Countdown
    {

        private TextView _label = null;

        public Countdown(TextView label)
        {
            _label = label;
        }


        public void Run(int duration, Action onFinished)
        {
            this.RestoreVisuals();
            this.RunCountdown(duration, onFinished);
        }


        private void RunCountdown(int countdownTime, Action onFinished)
        {
            UpdateCountdownLabel(countdownTime);
            ViewPropertyAnimator animator = _label.Animate().ScaleX(0).ScaleY(0).Alpha(0.0f).SetDuration(1000);
            if (countdownTime > 0)
                animator.WithEndAction(new Runnable(() => RunCountdown(countdownTime - 1, onFinished)));
            else
            {
                animator.WithStartAction(new Runnable(onFinished));
                animator.WithEndAction(new Runnable(() => _label.Visibility = ViewStates.Visible));
            }
            animator.Start();
        }
        private void UpdateCountdownLabel(int countdownTime)
        {
            _label.Text = countdownTime > 0 ? countdownTime.ToString() : "GO";
            this.RestoreVisuals();
        }

        private void RestoreVisuals()
        {
            _label.Visibility = ViewStates.Visible;
            _label.ScaleX = 1.0f;
            _label.ScaleY = 1.0f;
            _label.Alpha = 1.0f;
        }

    }

}