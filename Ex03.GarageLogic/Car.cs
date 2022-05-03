namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Car : Vehicle
    {
        private const byte k_CarColorMinValue = 1;
        private const byte k_CarColorMaxValue = 4;
        private const byte k_CarDoorsNumberMinValue = 1;
        private const byte k_CarDoorsNumberMaxValue = 4;
        private const float k_CarWheelsMaxAirPressure = 32;
        private eCarColor m_Color;
        private eCarDoorsNumber m_DoorsNumber;

        public enum eCarColor
        {
            Red = 1,
            Silver,
            White,
            Black
        }

        public enum eCarDoorsNumber
        {
            Two = 1,
            Three,
            Four,
            Five
        }

        public Car(string i_LicenseNumber, Engine i_Engine) :
            base(i_LicenseNumber, eWheelsNumber.Four, k_CarWheelsMaxAirPressure, i_Engine)
        {}

        public eCarColor Color
        {
            get { return m_Color; }
            set
            {
                if(Enum.IsDefined(typeof(eCarColor), value) == true)
                {
                    m_Color = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_CarColorMinValue, k_CarColorMaxValue, "Car's color");
                }
            }
        }

        public eCarDoorsNumber DoorsNumber 
        { 
            get { return m_DoorsNumber; }
            set
            {
                if (Enum.IsDefined(typeof(eCarDoorsNumber), value) == true)
                {
                    m_DoorsNumber = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(k_CarDoorsNumberMinValue, k_CarDoorsNumberMaxValue,
                        "Car's doors number");
                }
            }
        }

        public override List<QuestionForVehicleInformation> AskForDataToVehicle()
        {
            List<QuestionForVehicleInformation> questionsForCarInfo = new List<QuestionForVehicleInformation>();

            questionsForCarInfo.AddRange(base.AskForDataToVehicle());
            questionsForCarInfo.Add(new QuestionForVehicleInformation
                ("car's color", QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                k_CarColorMinValue, k_CarColorMaxValue));
            questionsForCarInfo.Add(new QuestionForVehicleInformation
                ("car's doors number", QuestionForVehicleInformation.eValidationCheckType.ValidRangeCheck,
                k_CarDoorsNumberMinValue, k_CarDoorsNumberMaxValue));

            return questionsForCarInfo;
        }

        public override bool SetRemainingVehicleDetails(List<string> i_CurrentInfoToVehicle)
        {
            bool carDitailsSetSuccessfully = true;

            base.SetRemainingVehicleDetails(i_CurrentInfoToVehicle);
            eCarColor carColorInput = (eCarColor)(Enum.Parse(typeof(eCarColor), i_CurrentInfoToVehicle[4]));
            eCarDoorsNumber carDoorsNumberInput = (eCarDoorsNumber)(Enum.Parse(typeof(eCarColor), i_CurrentInfoToVehicle[5]));

            if (Enum.IsDefined(typeof(eCarColor), carColorInput) == true &&
                Enum.IsDefined(typeof(eCarDoorsNumber), carDoorsNumberInput) == true)
            {
                Color = carColorInput;
                DoorsNumber = carDoorsNumberInput;
            }
            else
            {
                throw new Exception(string.Format(@"Worng input. You must enter number between {0} to {1}."
, k_CarColorMinValue, k_CarColorMaxValue));
            }

            return carDitailsSetSuccessfully;
        }

        public override string ToString()
        {
            StringBuilder carDitails = new StringBuilder();

            carDitails.Append(base.ToString());
            carDitails.Append(string.Format(@"Car's color is '{0}', there is {1} doors.", Color.ToString(), (int)(DoorsNumber)+1));

            return carDitails.ToString();
        }
    }
}
