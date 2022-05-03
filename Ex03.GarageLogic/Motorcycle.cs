namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Motorcycle : Vehicle
    {
        private const byte k_MinLicenseTypeValue = 1;
        private const byte k_MaxLicenseTypeValue = 4;
        private const int k_MinMotorcycleEngineCapacity = 125;
        private const int k_MaxMotorcycleEngineCapacity = 2000;
        private const float k_MotorcycleWheelsMaxAirPressure = 30;
        private eMotorcycleLicenseType m_LicenseType;
        private int m_EngineCapacity;

        public enum eMotorcycleLicenseType
        {
            A = 1,
            B1,
            AA,
            BB
        }

        public Motorcycle(string i_LicenseNumber, Engine i_Engine) :
            base(i_LicenseNumber, eWheelsNumber.Two, k_MotorcycleWheelsMaxAirPressure, i_Engine)
        {}

        public int EngineCapacity 
        { 
            get { return m_EngineCapacity; } 
            set
            {
                if(value >= k_MinMotorcycleEngineCapacity && value <= k_MaxMotorcycleEngineCapacity)
                {
                    m_EngineCapacity = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_MinMotorcycleEngineCapacity, k_MaxMotorcycleEngineCapacity,
                        "Engine capacity");
                }
            }
        }

        public eMotorcycleLicenseType LicenseType 
        { 
            get { return m_LicenseType; }
            set
            {
                if(Enum.IsDefined(typeof(eMotorcycleLicenseType), value) == true)
                {
                    m_LicenseType = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_MinLicenseTypeValue, k_MaxLicenseTypeValue,
                        "License type");
                }
            }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForMotorcycleInfo = new List<QuestionForVehicleInformation>();
            
            questionsForMotorcycleInfo.AddRange(base.AskForDataToVehicle());
            questionsForMotorcycleInfo.Add(new QuestionForVehicleInformation
                ("license type", QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                k_MinLicenseTypeValue, k_MaxLicenseTypeValue));
            questionsForMotorcycleInfo.Add(new QuestionForVehicleInformation("engine capacity",
                QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                k_MinMotorcycleEngineCapacity, k_MaxMotorcycleEngineCapacity));

            return questionsForMotorcycleInfo; 
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool motorcycleDitailsSetSuccessfully = true;
            eMotorcycleLicenseType licenseTypeInput =
                (eMotorcycleLicenseType)(Enum.Parse(typeof(eMotorcycleLicenseType), i_CurrentInfoToVehicle[4]));
            int engineCapacityInput; 

            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);

            if (Enum.IsDefined(typeof(eMotorcycleLicenseType), licenseTypeInput) == true &&
                int.TryParse(i_CurrentInfoToVehicle[5], out engineCapacityInput) == true)
            {
                LicenseType = licenseTypeInput;
                EngineCapacity = engineCapacityInput;
            }
            else
            {
                throw new Exception(string.Format(@"Worng input. You must enter number between {0} to {1}."
, k_MinLicenseTypeValue, k_MaxLicenseTypeValue));
            }

            return motorcycleDitailsSetSuccessfully;
        }
  
        public override string ToString()
        {
            StringBuilder motorcycleDitails = new StringBuilder();

            motorcycleDitails.Append(base.ToString());
            motorcycleDitails.Append(string.Format("Motorcycle engine capacity: {0} cc, license type: {1}.",
                EngineCapacity, LicenseType.ToString()));

            return motorcycleDitails.ToString();
        }
    }
}
