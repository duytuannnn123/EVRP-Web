
namespace EVRP_Web.Options
{
    public class ALNSOptions
    {
        public int MaxIterations { get; set; } = 5000;
        public double InitialTemperature { get; set; } = 100.0;
        public double CoolingRate { get; set; } = 0.995;
        public int EarlyStopping { get; set; } = 1000;
    }
}
