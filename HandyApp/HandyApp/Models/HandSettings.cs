namespace HandyApp.Models
{
    public class HandSettings
    {
        public string HoldTime { get; set; }
        public string PeakThreshold { get; set; }
        public int CurrentHand { get; set; }
        public bool MotorsEnabled { get; set; }
        public bool DemoModeEnabled { get; set; }
        public int MuscleMode { get; set; }
    }

    public enum SelectedHand : int
    {
        Left = 1,
        Right = 2
    }

    public enum MuscleMode : int
    {
        Off = 1,
        Simple = 2,
        Proportional = 3
    }
}
