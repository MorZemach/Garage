namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    class Truck : Vehicle
    {
        private const float k_LogicalPossibleMinCarryingWeight = 0;
        private const float k_LogicalPossibleMaxCarryingWeight = 20000;
        private const float k_TruckWheelsMaxAirPressure = 26;
        private float m_MaxCarryingWeight;
        private bool m_IsCarrierDangerousMaterials;

        public Truck(string i_LicenseNumber, Engine i_Engine) :
            base(i_LicenseNumber, eWheelsNumber.Sixteen, k_TruckWheelsMaxAirPressure, i_Engine)
        {}

        public float MaxCarryingWeight 
        { 
            get { return m_MaxCarryingWeight; } 
            set
            {
                if(value > 0)
                {
                    m_MaxCarryingWeight = value;
                }
                else
                {
                    throw new ArgumentException("Maximum carrying Weight must be bigger then 0.");
                }
            }
        }

        public bool IsCarrierDangerousMaterials
        {
            get { return m_IsCarrierDangerousMaterials; }
            set { m_IsCarrierDangerousMaterials = value; }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForTruckInfo = new List<QuestionForVehicleInformation>();

            questionsForTruckInfo.AddRange(base.AskForDataToVehicle());
            questionsForTruckInfo.Add(new QuestionForVehicleInformation("is carrier dangerous materials",
                QuestionForVehicleInformation.eValidationCheckType.ValidBooleanCheck));
            questionsForTruckInfo.Add(new QuestionForVehicleInformation("maximum carrying weight",
                QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                (int)k_LogicalPossibleMinCarryingWeight, (int)k_LogicalPossibleMaxCarryingWeight));

            return questionsForTruckInfo;
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool truckDitailsSetSuccessfully = true;
            bool isCarrierDangerousMaterialsInput;
            float maxCarryingWeightInput; 
            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);

            if (bool.TryParse(i_CurrentInfoToVehicle[4], out isCarrierDangerousMaterialsInput) == true &&
                float.TryParse(i_CurrentInfoToVehicle[5], out maxCarryingWeightInput) == true)
            {
                IsCarrierDangerousMaterials = isCarrierDangerousMaterialsInput;
                MaxCarryingWeight = maxCarryingWeightInput;
            }
            else
            {
                truckDitailsSetSuccessfully = false;
            }

            return truckDitailsSetSuccessfully;
        }

        public override string ToString()
        {
            StringBuilder truckDitails = new StringBuilder();

            truckDitails.Append(base.ToString());
            truckDitails.Append(string.Format(
                "Truck's maximum carrying weight is {0} kg, is it carrying dangerous materials: {1}.",
                MaxCarryingWeight, IsCarrierDangerousMaterials));

            return truckDitails.ToString();
        }
    }
}
