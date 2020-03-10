using System;
using System.Xml.Serialization;

namespace WearGames
{

    [Serializable()]
    public class Score
    {

        [XmlAttribute("name")]
        public string Name
        { get; set; }
        [XmlAttribute("stage")]
        public int Stage
        { get; set; }
        [XmlAttribute("credits")]
        public int Credits
        { get; set; }
        [XmlAttribute("time")]
        public float Time
        { get; set; }

        [XmlAttribute("timestamp")]
        public long Timestamp
        { get; set; }


        public Score()
        { }
        public Score(string name, float time, int credits, int stage)
        {
            this.Name = name;
            this.Time = time;
            this.Stage = stage;
            this.Credits = credits;
            this.Timestamp = DateTime.Now.Ticks;
        }

        public override string ToString()
        {
            return $"{this.Name} in {this.Time.ToStringAsMinutesAndSeconds()} with {this.Credits}";
        }
    }
}