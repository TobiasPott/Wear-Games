using System;

namespace WearGames
{
    public class GameState
    {
        public static float Time
        { get; private set; }

        public static string FormattedTime
        { get => Time.ToStringAsMinutesAndSeconds(); }


        public static int Credits
        { get; private set; }

        public static void Reset(int credits = 3)
        {
            Time = 0.0f;
            Credits = credits;
        }


        public static void DecreaseLifes(int amount = 1)
        {
            Credits = Math.Clamp(Credits - amount, 0, int.MaxValue);
        }

        public static void AdvanceTime(float deltaTime)
        {
            Time += deltaTime;
        }


    }


}