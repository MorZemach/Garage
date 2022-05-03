namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        protected const float k_MinCapacityOfEnergy = 0;
        protected readonly float r_MaxCapacityOfEnergy;
        protected float m_CurrentAmountOfEnergy;

        public Engine(float i_MaxCapacityOfEnergy)
        {
            r_MaxCapacityOfEnergy = i_MaxCapacityOfEnergy;
        }

        public float MaxCapacityOfEnergy { get { return r_MaxCapacityOfEnergy; } }

        public float CurrentStatusOfEnergy
        {
            get { return m_CurrentAmountOfEnergy; }
            set
            {
                if (r_MaxCapacityOfEnergy >= value)
                {
                    m_CurrentAmountOfEnergy = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(0, r_MaxCapacityOfEnergy, "Energy level");
                }
            }
        }

        public virtual void GetEnergy(float i_Energy, FuelEngine.eFuelType i_FuelType = 0) 
        {
            CurrentStatusOfEnergy += i_Energy;
        }

        public virtual Vehicle.QuestionForVehicleInformation AskForDataToEngine()
        {
            Vehicle.QuestionForVehicleInformation questionForEngineInfo = new Vehicle.QuestionForVehicleInformation
                ("current amount of energy", Vehicle.QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                (int)k_MinCapacityOfEnergy, (int)r_MaxCapacityOfEnergy);

            return questionForEngineInfo;
        }

        public override string ToString()
        {
            return string.Format("Maximum energy capacity is: {0}. ", MaxCapacityOfEnergy);
        }
    }
}
