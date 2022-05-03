namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Garage
    {
        private const byte k_VehicleStatusMinValue = 0;
        private const byte k_VehicleStatusMaxValue = 3;
        private Dictionary<string, ClientCard> m_Clients;

        public enum eVehicleStatus
        {
            Repair = 1,
            Repaired,
            PayUp,
            General
        }

        public Garage()
        {
            m_Clients = new Dictionary<string, ClientCard>();
        }

        public Dictionary<string, ClientCard> Clients
        {
            get { return m_Clients; }
            set { m_Clients = value; }
        }

        public static StringBuilder GetStringOfEnumValues(Type i_EnumValuesTypes)
        {
            StringBuilder listOfEnumValues = new StringBuilder();

            foreach(object valueTypes in Enum.GetValues(i_EnumValuesTypes))
            {
                listOfEnumValues.Append(string.Format(
                    "{0}. {1}{2}", (int)(valueTypes), valueTypes, Environment.NewLine));
            }

            return listOfEnumValues;
        }

        public bool InsertNewVehicleClient(string i_ClientName, string i_ClientPhoneNumber,
            string i_LicenseNumber, Vehicle i_ClientVehicle)
        {
            bool vehicleClientInsertSuccessfully = false;
            
            if(ValidationCheck.ValidationCheckForName(i_ClientName) == true &&
               ValidationCheck.ValidationCheckForStringNumbers(i_ClientPhoneNumber, ClientCard.k_DesiredAmountOfPhoneNumberDigits, null) == true)
            {
                vehicleClientInsertSuccessfully = true;
                Clients.Add(i_LicenseNumber, new ClientCard(i_ClientName, i_ClientPhoneNumber, i_ClientVehicle));
            }

            return vehicleClientInsertSuccessfully;
        }

        public bool VehicleIsAlreadyAtTheGarage(string i_LicenseNumber)
        {
            return Clients.ContainsKey(i_LicenseNumber);
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleStatus i_VehicleStatus)
        {
            Clients[i_LicenseNumber].ClientVehicleStatus = i_VehicleStatus;
        }

        public bool InflatingVehicleWheels(string i_LicenseNumber)
        {
            bool vehicleWheelsInflatingSuccessfully = false;
            float airPressure;

            if (Clients[i_LicenseNumber].ClientVehicle.Wheels[0].CurrentAirPressure ==
                Clients[i_LicenseNumber].ClientVehicle.Wheels[0].MaxAirPressure)
            {
                Console.WriteLine("The air pressure in the car wheels, is already the maximum air pressure.");
            }
            else
            {
                airPressure = airPressureToAdd(Clients[i_LicenseNumber].ClientVehicle.Wheels[0]);

                foreach (Vehicle.Wheel wheel in Clients[i_LicenseNumber].ClientVehicle.Wheels)
                {
                    wheel.Inflating(airPressure);
                    vehicleWheelsInflatingSuccessfully = true;
                }
            }

            return vehicleWheelsInflatingSuccessfully;
        }

        public bool Refuel(string i_LicenseNumber ,float i_LitersOfFuel, FuelEngine.eFuelType i_FualType)
        {
            bool vehicleRefuelSuccessful = false;
            bool isTheRightTypeOfFuelToRefuelVehicle;
            //FuelEngine engineToCheck;

            if (Clients[i_LicenseNumber].ClientVehicle.Engine.CurrentStatusOfEnergy ==
                  Clients[i_LicenseNumber].ClientVehicle.Engine.MaxCapacityOfEnergy)
            {
                Console.WriteLine("The vehicle is fully refueled, therefore it does not need further refueling.");
            }
            else
            {
                //engineToCheck = Clients[i_LicenseNumber].ClientVehicle.Engine as FuelEngine;
                Clients[i_LicenseNumber].ClientVehicle.UpdateEnergyAmount(i_LitersOfFuel, i_FualType);
                vehicleRefuelSuccessful = true;
                ////isTheRightTypeOfFuelToRefuelVehicle = checkIfIsTheRightTypeOfFuelToRefuelVehicle(i_FualType, engineToCheck);
                //if (isTheRightTypeOfFuelToRefuelVehicle == true)
                //{
                //    Clients[i_LicenseNumber].ClientVehicle.UpdateEnergyAmount(i_LitersOfFuel);
                //    vehicleRefuelSuccessful = true;
                //}
            }

            return vehicleRefuelSuccessful;
        }

        //private bool checkIfIsTheRightTypeOfFuelToRefuelVehicle(FuelEngine.eFuelType i_TestedFualType, FuelEngine i_EngineToCheck)
        //{
        //    bool isTheRightTypeOfFuelToRefuelVehicle = false;

        //    if(i_TestedFualType == i_EngineToCheck.FualType)
        //    {
        //        isTheRightTypeOfFuelToRefuelVehicle = true;
        //    }
        //    else
        //    {
        //        throw new ArgumentException(string.Format("{0} does not the right fual type.", i_TestedFualType));
        //    }

        //    return isTheRightTypeOfFuelToRefuelVehicle;
        //}

        public bool ChargingElectricVehicle(string i_LicenseNumber, float i_MinutesToCharge)
        {
            bool vehicleChargeSuccessful = false;

            if (Clients[i_LicenseNumber].ClientVehicle.Engine.CurrentStatusOfEnergy ==
                  Clients[i_LicenseNumber].ClientVehicle.Engine.MaxCapacityOfEnergy)
            {
                Console.WriteLine("The vehicle is fully charged, therefore it does not need further charging.");
            }
            else
            {
                Clients[i_LicenseNumber].ClientVehicle.UpdateEnergyAmount(minutesToHoursConverter(i_MinutesToCharge));
                vehicleChargeSuccessful = true;
            }

            return vehicleChargeSuccessful;
        }

        public List<string> PresentVehiclesLicenseNumbersList(eVehicleStatus i_VehicleStatus)
        {
            List<string> vehiclesLicenseNumbersList;

            if (i_VehicleStatus == eVehicleStatus.General)
            {
                vehiclesLicenseNumbersList = makeGeneralLicenseNumbersList();
            }
            else
            {
                vehiclesLicenseNumbersList = makeLicenseNumbersListAccordingToVehicleStatus(i_VehicleStatus);
            }

            return vehiclesLicenseNumbersList;
        }

        private List<string> makeGeneralLicenseNumbersList()
        {
            List<string> vehiclesLicenseNumbersList = new List<string>();

            foreach(string licenseNumberKey in Clients.Keys)
            {
                vehiclesLicenseNumbersList.Add(licenseNumberKey);
            }

            return vehiclesLicenseNumbersList;
        }

        private List<string> makeLicenseNumbersListAccordingToVehicleStatus(eVehicleStatus i_VehicleStatus)
        {
            List<string> vehiclesLicenseNumbersList = new List<string>();

            foreach(KeyValuePair<string, ClientCard> client in Clients)
            {
                if(client.Value.ClientVehicleStatus == i_VehicleStatus)
                {
                    vehiclesLicenseNumbersList.Add(client.Key);
                }
            }

            return vehiclesLicenseNumbersList;
        }

        public string PresentSpecificVehicleDetails(string i_LicenseNumber)
        {
            StringBuilder clientVehicleDetailsToPresent = new StringBuilder();

            clientVehicleDetailsToPresent.Append(Clients[i_LicenseNumber].ToString());
            clientVehicleDetailsToPresent.Append(Clients[i_LicenseNumber].ClientVehicle.ToString());

            return clientVehicleDetailsToPresent.ToString();
        }

        private float minutesToHoursConverter(float i_Minutes)
        {
            return (float)TimeSpan.FromMinutes(i_Minutes).TotalHours;
        }

        private float airPressureToAdd(Vehicle.Wheel wheel)
        {
            return wheel.MaxAirPressure - wheel.CurrentAirPressure;
        }

        public class ClientCard
        {
            public const byte k_DesiredAmountOfPhoneNumberDigits = 10;
            private string m_ClientName;
            private string m_ClientPhoneNumber;
            private eVehicleStatus m_ClientVehicleStatus;
            private Vehicle m_ClientVehicle;

            public ClientCard(string i_ClientName, string i_ClientPhoneNumber, Vehicle i_ClientVehicle)
            {
                m_ClientName = i_ClientName;
                m_ClientPhoneNumber = i_ClientPhoneNumber;
                m_ClientVehicleStatus = eVehicleStatus.Repair;
                m_ClientVehicle = i_ClientVehicle;
            }

            public string ClientName
            {
                get { return m_ClientName; }
                set 
                { 
                    if(ValidationCheck.ValidationCheckForName(value) == true)
                    {
                        m_ClientName = value;
                    }
                }
            }

            public string ClientPhoneNumber
            {
                get { return m_ClientPhoneNumber; }
                set 
                { 
                    if(ValidationCheck.ValidationCheckForStringNumbers(value, k_DesiredAmountOfPhoneNumberDigits, null) == true)
                    {
                        m_ClientPhoneNumber = value;
                    }
                }
            }

            public eVehicleStatus ClientVehicleStatus
            {
                get { return m_ClientVehicleStatus; }
                set 
                {
                    if (Enum.IsDefined(typeof(eVehicleStatus), value) == true)
                    {
                        m_ClientVehicleStatus = value;
                    }
                    else
                    {
                        throw new ValueOutOfRangeException(k_VehicleStatusMinValue, k_VehicleStatusMaxValue,
                            "Vehicle status");
                    }
                }
            }

            public Vehicle ClientVehicle
            {
                get { return m_ClientVehicle; }
                set { m_ClientVehicle = value; }
            }

            public override string ToString()
            {
                string clientDetails = string.Format(
  "Client's name: {0}, phone number: {1}. Vehicle status is: {2}.{3}",
                    ClientName, ClientPhoneNumber, ClientVehicleStatus.ToString(), Environment.NewLine);

                return clientDetails;
            }
        }
    }
}
