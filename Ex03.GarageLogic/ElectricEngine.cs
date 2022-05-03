namespace Ex03.GarageLogic
{
    using System;
    using System.Text;

    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_MaxCapacityOfEnergy) : base(i_MaxCapacityOfEnergy) { }

        public override void GetEnergy(float i_Energy, FuelEngine.eFuelType i_FuelType = 0)
        {
            base.GetEnergy(i_Energy);
        }

        public override string ToString()
        {
            StringBuilder electricEngineDetails = new StringBuilder();

            electricEngineDetails.Append(string.Format("Electric engine details:{0}",
                Environment.NewLine));
            electricEngineDetails.Append(base.ToString());
            electricEngineDetails.Append(string.Format("{0} hours of battery left.",
                CurrentStatusOfEnergy));

            return electricEngineDetails.ToString();
        }
    }
}
