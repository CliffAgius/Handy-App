namespace HandyApp.Models
{
    public class HandSettings
    {
        public string HoldTime { get; set; }
        public string PeakThreshold { get; set; }
        public SelectedHand CurrentHand { get; set; }
        public bool MotorsEnabled { get; set; }
        public bool DemoModeEnabled { get; set; }
        public MuscleMode MuscleMode { get; set; }
    }

    public enum SelectedHand : int
    {
        UnSet = 0,
        Left = 1,
        Right = 2
    }

    public enum MuscleMode : int
    {
        UnSet = 0,
        Off = 1,
        Simple = 2,
        Proportional = 3
    }
}
