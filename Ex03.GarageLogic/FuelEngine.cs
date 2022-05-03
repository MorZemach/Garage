namespace Ex03.GarageLogic
{
    using System;
    using System.Text;

    public class FuelEngine : Engine
    {
        private readonly eFuelType r_FualType;

        public enum eFuelType
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98
        }
        
        public FuelEngine(float i_MaxCapacityOfEnergy, eFuelType i_FualType) : 
            base(i_MaxCapacityOfEnergy)
        {
            r_FualType = i_FualType;
        }

        public eFuelType FualType
        {
            get { return r_FualType; }
        }

        public override string ToString()
        {
            StringBuilder FuelEngineDetails = new StringBuilder();

            FuelEngineDetails.Append(string.Format("Fuel engine details:{0}",
                Environment.NewLine));
            FuelEngineDetails.Append(base.ToString());
            FuelEngineDetails.Append(string.Format("Fuel type is: {0}, current fuel amount is {1} liters.",
                r_FualType, m_CurrentAmountOfEnergy));

            return FuelEngineDetails.ToString();
        }

        public override void GetEnergy(float i_Energy, FuelEngine.eFuelType i_FuelType = 0)
        {
            if (i_FuelType == FualType)
            {
                base.GetEnergy(i_Energy);
            }
            else
            {
                throw new ArgumentException(string.Format("{0} does not the right fual type.", i_FuelType));
            }

        }

    }
}
