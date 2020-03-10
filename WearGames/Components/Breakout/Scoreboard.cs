using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace WearGames
{
    public enum ScoraboadViews
    {
        Success,
        Failure,
        List
    }

    [Serializable()]
    public class Scoreboard
    {

        public List<Score> Scores
        { get; set; } = new List<Score>();


        public event Action ScoreSubmitted;


        [XmlIgnore()]
        private ViewGroup _mainLayout = null;
        [XmlIgnore()]
        private LinearLayout _gameOverView = null;
        [XmlIgnore()]
        private LinearLayout _listView = null;
        [XmlIgnore()]
        private LinearLayout _inputView = null;

        [XmlIgnore()]
        private LinearLayout _listContent = null;
        [XmlIgnore()]
        private EditText _nameInput = null;
        [XmlIgnore()]
        private TextView _timeLabel = null;
        [XmlIgnore()]
        private View _submitButton = null;


        public Scoreboard()
        { }

        public Scoreboard(ViewGroup layout)
        {
            this.SetLayout(layout);
        }

        public void SetLayout(ViewGroup layout)
        {
            if (_mainLayout == null)
            {
                _mainLayout = layout;
                _listView = layout.FindViewById<LinearLayout>(Resource.Id.scoreboardList);
                _gameOverView = layout.FindViewById<LinearLayout>(Resource.Id.scoreboardGameOver);
                _inputView = layout.FindViewById<LinearLayout>(Resource.Id.scoreboardInput);
                _nameInput = layout.FindViewById<EditText>(Resource.Id.scoreboardUsernameEditText);
                _timeLabel = layout.FindViewById<TextView>(Resource.Id.scoreboardTimeLabel);
                _submitButton = layout.FindViewById<View>(Resource.Id.scoreboardSubmitButton);
                _listContent = layout.FindViewById<LinearLayout>(Resource.Id.scoreboardListContent);
                _submitButton.Click += SubmitButton_Click;
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_nameInput.Text))
            {
                this.Scores.Add(new Score(_nameInput.Text, GameState.Time, GameState.Credits, 0));
                _nameInput.Text = string.Empty;

                this.Serialize();
                this.ShowView(ScoraboadViews.List);
                this.OnScoreSubmitted();
            }
        }

        public void ShowView(ScoraboadViews view)
        {
            _timeLabel.Text = GameState.FormattedTime;
            if (view == ScoraboadViews.Success)
            {
                _mainLayout.Visibility = ViewStates.Visible;
                _inputView.Visibility = ViewStates.Visible;
                _listView.Visibility = ViewStates.Gone;
                _gameOverView.Visibility = ViewStates.Gone;
            }
            else if (view == ScoraboadViews.Failure)
            {
                _mainLayout.Visibility = ViewStates.Visible;
                _inputView.Visibility = ViewStates.Gone;
                _listView.Visibility = ViewStates.Gone;
                _gameOverView.Visibility = ViewStates.Visible;
            }
            else if (view == ScoraboadViews.List)
            {
                this.CreateListItems();
                _mainLayout.Visibility = ViewStates.Visible;
                _inputView.Visibility = ViewStates.Gone;
                _listView.Visibility = ViewStates.Visible;
                _gameOverView.Visibility = ViewStates.Gone;
            }
        }
        public void Show(bool show)
        {
            _mainLayout.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }
        private void OnScoreSubmitted()
        {
            if (this.ScoreSubmitted != null)
                this.ScoreSubmitted.Invoke();
        }


        public void Clear()
        {
            this.Scores.Clear();
            this.Serialize();
            // invalidate to force redrawing and relayout
            _listContent.Invalidate();
        }
        private void CreateListItems()
        {
            // clear scoreboard content view group
            _listContent.RemoveAllViews();

            // add a spacer to the top of the list
            AddSpace(_listContent, 10);
            // sort current scores by time and add items to the content view group
            IEnumerable<Score> scores = this.Scores.OrderByDescending(x => x.Credits).ThenBy(x => x.Time);
            foreach (Score score in scores)
            {
                TextView tv = new TextView(_listContent.Context);
                tv.Text = score.ToString();
                tv.SetTextColor(Color.White);
                tv.Gravity = GravityFlags.Center;
                tv.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                _listContent.AddView(tv);
            }
            // add a spacer to the end of the list
            AddSpace(_listContent, 10);
            // invalidate to force redrawing and relayout
            _listContent.Invalidate();
        }
        private static void AddSpace(ViewGroup parent, int height)
        {
            Space space = new Space(parent.Context);
            space.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, height);
            parent.AddView(space);
        }



        public void Serialize()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File scoreboardFile = new Java.IO.File(path, "weargames.breakout.scoreboard.xml");

            if (scoreboardFile.Exists() || scoreboardFile.CreateNewFile())
            {
                using (StreamWriter writer = File.CreateText(scoreboardFile.AbsolutePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Scoreboard));
                    this.Scores.Sort((x, y) => x.Time.CompareTo(y.Time));
                    serializer.Serialize(writer, this);
                }
            }
        }
        public void Deserialize()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File scoreboardFile = new Java.IO.File(path, "weargames.breakout.scoreboard.xml");

            if (!scoreboardFile.Exists())
                return;

            using (StreamReader reader = new StreamReader(scoreboardFile.AbsolutePath, true))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Scoreboard));
                try
                {
                    Scoreboard deserialized = (Scoreboard)serializer.Deserialize(reader);
                    if (deserialized != null)
                    {
                        this.Scores.Clear();
                        this.Scores.AddRange(deserialized.Scores);
                    }
                }
                catch (Exception ex)
                {
                    scoreboardFile.Delete();
                    scoreboardFile.CreateNewFile(); // create file again as empty file
                }
            }

        }

    }

}