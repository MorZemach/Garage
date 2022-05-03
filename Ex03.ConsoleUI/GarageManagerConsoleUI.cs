namespace Ex03.ConsoleUI
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System.Text;
    using Ex03.GarageLogic;

    public class GarageManagerConsoleUI
    {
        private const byte k_InitValue = 0;
        private const byte k_MinLicenseNumberLength = 7;
        private const byte k_PhoneNumberLength = 10;
        private static Garage m_Garage;

        public static Garage Garage
        {
            get { return m_Garage; }
            set { m_Garage = value; }
        }

        private enum eSystemMenuOption
        {
            InsertNewVehicle = 1,
            DisplayLicenseNumbers,
            ChangeVehicleStatus,
            InflateAirWheelsToMaximum,
            RefuelingRegularVehicle,
            RechargeElectricVehicle,
            DisplayVehicleDetailsByLicenseNumbers,
            Exit
        }

        public static void ActivateGarageManagementSystem()
        {
            Garage = new GarageLogic.Garage();

            Console.WriteLine(string.Format(@"Welcome to our garage!

Please follow the following menu:"));

            garageManagementSystemFunctionalityManu();
        }

        private static void garageManagementSystemFunctionalityManu()
        {
            string systemManu;
            eSystemMenuOption userCurrentChoice = k_InitValue;

            systemManu = string.Format(@"===========================================================================
1. Insert a new car to the garage.
2. Display garage vehicle license numbers.
3. Change vehicle status.
4. Inflate air in a vehicle's wheels to the maximum.
5. Refuel a vehicle that is powered by fuel.
6. Charge an electric vehicle.
7. Display Vehicle Details By License Number.
8. Exit
===========================================================================");
            string invalidOperationMsg = string.Format("As a result, the operation you attempted was not successful. Please try again!");
            while (userCurrentChoice != eSystemMenuOption.Exit)
            {
                try
                {
                    Console.WriteLine(systemManu);
                    userCurrentChoice = getUserManuChoice();
                    executionOfUserChoice(userCurrentChoice);
                }
                catch(FormatException i_FormatException)
                {
                    Console.WriteLine(string.Format(@"{0}
{1}", i_FormatException.Message, invalidOperationMsg));
                }
                catch(ArgumentException i_ArgumentException)
                {
                    Console.WriteLine(string.Format(@"{0}
{1}", i_ArgumentException.Message, invalidOperationMsg));
                }
                catch(ValueOutOfRangeException i_ValueOutOfRangeException)
                {
                    Console.WriteLine(string.Format(@"{0}
{1}", i_ValueOutOfRangeException.Message, invalidOperationMsg));
                }
                catch(Exception i_Exception)
                {
                    Console.WriteLine(string.Format(@"{0}
{1}", i_Exception.Message, invalidOperationMsg));
                }
                finally
                {
                    Thread.Sleep(5000);
                    Console.Clear();
                }
            }
        }

        private static eSystemMenuOption getUserManuChoice()
        {
            string msg;

            bool isByteInput = false;
            bool vaildInput = false;
            eSystemMenuOption userCurrentChoice = k_InitValue;

            msg = string.Format(@"Please select number from 1 to 8 that represents the action you want to perform in the garage: ");
            Console.Write(msg);
            while (vaildInput == false)
            {
                isByteInput = Enum.TryParse(Console.ReadLine(), out userCurrentChoice);
                if (isByteInput == true)
                {
                    vaildInput = Enum.IsDefined(typeof(eSystemMenuOption), userCurrentChoice);
                }

                if (vaildInput == false)
                {
                    Console.Write("Invaild input, Let's try again: ");
                }
            }

            return userCurrentChoice;
        }

        private static void executionOfUserChoice(eSystemMenuOption i_UserCurrentChoice)
        {
            switch (i_UserCurrentChoice)
            {
                case eSystemMenuOption.InsertNewVehicle:
                    {
                        InsertVehicleToTheGarage();
                        break;
                    }

                case eSystemMenuOption.DisplayLicenseNumbers:
                    {
                        displayLicenseNumbers();
                        break;
                    }

                case eSystemMenuOption.ChangeVehicleStatus:
                    {
                        changeVehicleStatus();
                        break;
                    }

                case eSystemMenuOption.InflateAirWheelsToMaximum:
                    {
                        inflateAirWheelsToMaximum();
                        break;
                    }

                case eSystemMenuOption.RefuelingRegularVehicle:
                    {
                        refuelingRegularVehicle();
                        break;
                    }

                case eSystemMenuOption.RechargeElectricVehicle:
                    {
                        rechargeElectricVehicle();
                        break;
                    }

                case eSystemMenuOption.DisplayVehicleDetailsByLicenseNumbers:
                    {
                        displayVehicleDetailsByLicenseNumbers();
                        break;
                    }

                case eSystemMenuOption.Exit:
                    {
                        Console.WriteLine("You decided to get out of the garage, have a wonderful day!");
                        break;
                    }

            }
        }
        // $G$ DSN-006 (-5) This method should have been private.
        public static void InsertVehicleToTheGarage()
        {
            string licenseNumber = getLicenseNumber();
            bool isLicenseExist = Garage.VehicleIsAlreadyAtTheGarage(licenseNumber);

            if(isLicenseExist == true)
            {
                Console.WriteLine("Vehicle is already at the garag! It is in repair.");
            }
            else
            {
                createNewVehicle(licenseNumber);
            }
        }

        // $G$ CSS-013 (-3) Bad variable name (should be in the form of: i_CamelCase).
        private static void createNewVehicle(string io_LicenseNumber)
        {
            Vehicle newVehicleToCreate = null;
            List<Vehicle.QuestionForVehicleInformation> vehicleInformationQuestion;
            List<string> vehicleInformationAnswers = new List<string>();
            string clientName, clientPhoneNumber, vehicleType;
            bool isVaildAnswers = false;
            bool vehicleDitailsSetSuccessfully = false;

            vehicleType = getClientVehicleType();
            newVehicleToCreate = VehicleCreator.CreateVehicle(vehicleType, io_LicenseNumber);
            clientDetailsForNewVehicle(out clientName, out clientPhoneNumber);
            vehicleInformationQuestion = newVehicleToCreate.AskForDataToVehicle();

            foreach (Vehicle.QuestionForVehicleInformation question in vehicleInformationQuestion)
            {
                vehicleInformationAnswers.Add(getAnswerToVehicleInformationQuestion(question, out isVaildAnswers));
            }

            vehicleDitailsSetSuccessfully = newVehicleToCreate.SetRemainingVehicleDetails(vehicleInformationAnswers);

            if(vehicleDitailsSetSuccessfully != false)
            {
                Garage.InsertNewVehicleClient(clientName, clientPhoneNumber, io_LicenseNumber, newVehicleToCreate);
                Console.WriteLine("Vehicle was successfully insert to the garage!");
            }
        }
        // $G$ CSS-014 (-3) Bad variable name (should be in the form of: o_CamelCase).
        private static string getAnswerToVehicleInformationQuestion(Vehicle.QuestionForVehicleInformation i_question,
                                                                    out bool io_IsVaildAnswer)
        {
            string informationAnswer;
            string selectFromEnumMsg = string.Format(@"Please select type of {0} from the following list:", i_question.r_AskForData);
            string defaultMsg = string.Format("Please enter vehicle {0}:", i_question.r_AskForData);

            switch (i_question.r_AskForData)
            {
                case "car's color":
                    {
                        Console.Write(string.Format(@"{0}
{1}(insert your chosen number):", selectFromEnumMsg, Garage.GetStringOfEnumValues(typeof(Car.eCarColor))));
                        break;
                    }
                case "car's doors number":
                    {
                        Console.Write(string.Format(@"{0}
{1}(insert your chosen number):", selectFromEnumMsg, Garage.GetStringOfEnumValues(typeof(Car.eCarDoorsNumber))));
                        break;
                    }
                case "license type":
                    {
                        Console.Write(string.Format(@"{0}
{1}(insert your chosen number):", selectFromEnumMsg, Garage.GetStringOfEnumValues(typeof(Motorcycle.eMotorcycleLicenseType))));
                        break;
                    }
                case "is carrier dangerous materials":
                    {
                        Console.Write("Does your vehicle {0}? answer true or false: ", i_question.r_AskForData);
                        break;
                    }
                default:
                    {
                        Console.Write(defaultMsg);
                        break;
                    }
            }

            informationAnswer = Console.ReadLine();
            io_IsVaildAnswer = i_question.AnswerValidationCheck(informationAnswer);

            return informationAnswer;
        }

        private static string getClientVehicleType()
        {
            string inputVehicleType = null;
            bool vehicleTypeInRange = false;

            Console.Write(string.Format(@"Please select the type of vehicle you would like to create from the following list:
{0}(insert your chosen number):", Garage.GetStringOfEnumValues(typeof(VehicleCreator.eVehicleType))));

            while(vehicleTypeInRange == false)
            {
                inputVehicleType = Console.ReadLine();
                vehicleTypeInRange = VehicleCreator.IsVehicleTypeInRange(inputVehicleType);

                if(vehicleTypeInRange == false)
                {
                    Console.Write("Wrong input choice. Let't try again: ");
                }
            }

            return inputVehicleType;
        }

        // $G$ CSS-013 (-3) Bad variable name (should be in the form of: i_CamelCase).
        private static Vehicle createVehicle(string io_LicenseNumber)
        {
            string chosenVehicleType;
            Vehicle vehicleToCreate = null;

            while (vehicleToCreate == null)
            {
                Console.WriteLine(string.Format
                ("Please choose vehicle type (enter from the following  indexes): {0}{1}",
                Environment.NewLine, typeof(VehicleCreator.eVehicleType)));
                chosenVehicleType = Console.ReadLine();
                vehicleToCreate = VehicleCreator.CreateVehicle(chosenVehicleType, io_LicenseNumber);
            }

            return vehicleToCreate;
        }

        private static void clientDetailsForNewVehicle(out string io_ClientName, out string io_ClientPhoneNumber)
        {
            io_ClientName = getClientName();
            io_ClientPhoneNumber = getClientPhoneNumber();
        }

        private static string getClientName()
        {
            string clientName;

            Console.Write("Please enter vehicle's client name: ");
            clientName = Console.ReadLine();
            ValidationCheck.ValidationCheckForName(clientName);

            return clientName;
        }

        private static string getClientPhoneNumber()
        {
            string clientPhoneNumber;
            String errorMsg = string.Format("Phone number must be {0} digits.", k_PhoneNumberLength);

            Console.Write("Please enter vehicle's client phone number: ");
            clientPhoneNumber = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(clientPhoneNumber, k_PhoneNumberLength, errorMsg);

            return clientPhoneNumber;
        }

        private static string getLicenseNumber()
        {
            string licenseNumber;
            string invalidLicenseNumberMsg = string.Format(@"License number is illegal. Must be minimum {0} digits but only digits.",
                k_MinLicenseNumberLength);

            Console.Write(string.Format(@"Please enter the license number of your vehicle (minimum {0} digits): ", k_MinLicenseNumberLength));
            licenseNumber = Console.ReadLine();
            ValidationCheck.ValidationCheckForStringNumbers(licenseNumber, k_MinLicenseNumberLength, invalidLicenseNumberMsg);

            return licenseNumber;
        }

        private static void displayLicenseNumbers()
        {
            Garage.eVehicleStatus selectedVehiclesStatusToDisplay = getVehicleStatusForDisplayLicenseListBy();
            List<string> licensesToDisplay = Garage.PresentVehiclesLicenseNumbersList(selectedVehiclesStatusToDisplay);

            if(licensesToDisplay.Count != 0)
            {
                printLicensesList(licensesToDisplay);
            }
            else
            {
                Console.WriteLine("There is no license list to display.");
            }
        }

        private static void printLicensesList(List<string> i_LicensesList)
        {
            StringBuilder licensesListSring = new StringBuilder();
            byte index = 1;

            Console.WriteLine("List of requested license numbers: ");

            foreach(string licenses in i_LicensesList)
            {
                licensesListSring.Append(index++).Append(".").Append(licenses).Append(".").Append(Environment.NewLine);
            }

            Console.WriteLine(licensesListSring);
        }

        private static Garage.eVehicleStatus getVehicleStatusForDisplayLicenseListBy()
        {
            Garage.eVehicleStatus selectedVehiclesStatus = Garage.eVehicleStatus.General;
            string userChoice, optionManu;
            bool isVaildInput = false;

            Console.WriteLine("You have decided to present the list of vehicle license numbers in the garage!");
            optionManu = string.Format(@"Please select the type of license number list you would like to see:
1. List of vehicles currently under repair in the garage.
2. List of vehicles repaired in the garage.
3. List of vehicles that paid up for their repair.
4. List of vehicles in the garage.
(insert your chosen number):");
            Console.Write(optionManu);

            while(isVaildInput == false)
            {
                userChoice = Console.ReadLine();
                if (Enum.TryParse(userChoice, out selectedVehiclesStatus) == true &&
                    Enum.IsDefined(typeof(Garage.eVehicleStatus), selectedVehiclesStatus))
                {
                    isVaildInput = true;
                }
                else
                {
                    Console.Write("Invaild input, Let's try again: ");
                }
            }

            return selectedVehiclesStatus;
        }

        private static void changeVehicleStatus()
        {
            string userChoice;
            string licenseNumberForChangingStatus = getLicenseNumber();
            bool isLicenseExist = Garage.VehicleIsAlreadyAtTheGarage(licenseNumberForChangingStatus);
            Garage.eVehicleStatus selectedVehiclesStatus = Garage.eVehicleStatus.General;
            bool isVaildInput = false;

            if(isLicenseExist == true)
            {
                Console.Write((string.Format(@"Please enter the new vehicle statue you would like to change to:
1.Repair
2.Repaired
3.PayUp
(insert your chosen number): ")));

                while (isVaildInput == false)
                {
                    userChoice = Console.ReadLine();
                    if (Enum.TryParse(userChoice, out selectedVehiclesStatus) == true &&
                      Enum.GetValues(typeof(Garage.eVehicleStatus)).Cast<Garage.eVehicleStatus>().Max() >= selectedVehiclesStatus &&
                      Enum.GetValues(typeof(Garage.eVehicleStatus)).Cast<Garage.eVehicleStatus>().Min() < selectedVehiclesStatus)
                    {
                        isVaildInput = true;
                    }
                    else
                    {
                        Console.Write("Invaild input, Let's try again: ");
                    }
                }
                Garage.ChangeVehicleStatus(licenseNumberForChangingStatus, selectedVehiclesStatus);
                Console.WriteLine(String.Format("The vehicle status has been successfully changed, and is now: {0}.", selectedVehiclesStatus));
            }
            else
            {
                Console.WriteLine("There is no license number in the garage that matches to the given license number.");
            }
        }

        private static void inflateAirWheelsToMaximum()
        {
            string licenseNumberToInflateAirWheel= getLicenseNumber();
            bool isLicenseExist = Garage.VehicleIsAlreadyAtTheGarage(licenseNumberToInflateAirWheel);

            if (isLicenseExist == true)
            {
                if(Garage.InflatingVehicleWheels(licenseNumberToInflateAirWheel) == true)
                {
                    Console.WriteLine("The air in the car wheels is inflated successfully to the maximum.");
                }
            }
            else
            {
                Console.WriteLine("There is no license number in the garage that matches to the given license number.");
            }
        }

        private static void refuelingRegularVehicle()
        {
            string licenseNumberToRefuel, fuelTypeInput;
            FuelEngine.eFuelType fuelType = k_InitValue;
            float amountFuelToRefuel;
            licenseNumberToRefuel = getLicenseNumber();
            bool isLicenseExist = Garage.VehicleIsAlreadyAtTheGarage(licenseNumberToRefuel);
            bool isVaildInput = false;

            if (isLicenseExist == true)
            {
                if(Garage.Clients[licenseNumberToRefuel].ClientVehicle.Engine is ElectricEngine)
                {
                    throw new ArgumentException("You tried to refuel a fuel-powered electric vehicle.");
                }
                else
                {
                    Console.Write(string.Format(@"Please insert the type of fuel you would like to refuel:
{0}(insert your chosen number): ", Garage.GetStringOfEnumValues(typeof(FuelEngine.eFuelType))));

                    while (isVaildInput == false)
                    {
                        fuelTypeInput = Console.ReadLine();
                        if (Enum.TryParse(fuelTypeInput, out fuelType) == true &&
                            Enum.IsDefined(typeof(FuelEngine.eFuelType), fuelType))
                        {
                            isVaildInput = true;
                        }
                        else
                        {
                            Console.Write("Invaild input, Let's try again: ");
                        }
                    }

                    Console.Write(string.Format(@"Please insert the amount of {0} fuel you would to refuel: ", fuelType));
                    ValidationCheck.ValidationCheckForAmount(Console.ReadLine(), out amountFuelToRefuel);
                    if (Garage.Refuel(licenseNumberToRefuel, amountFuelToRefuel, fuelType) == true)
                    {
                        Console.WriteLine("The refueling operation was performed successfully.");
                    }
                }
            }
            else
            {
                Console.WriteLine("There is no license number in the garage that matches to the given license number.");
            }
        }

        private static void rechargeElectricVehicle()
        {
            string licenseNumberToRefuel;
            float amountMinutesToCharge;
            licenseNumberToRefuel = getLicenseNumber();
            bool isLicenseExist = Garage.VehicleIsAlreadyAtTheGarage(licenseNumberToRefuel);

            if (isLicenseExist == true)
            {
                if (Garage.Clients[licenseNumberToRefuel].ClientVehicle.Engine is FuelEngine)
                {
                    throw new ArgumentException("You tried to charge a fuel-powered vehicle.");
                }
                else
                {
                    Console.Write(string.Format(@"Please insert the the amount of minutes you would like to charge: "));
                    ValidationCheck.ValidationCheckForAmount(Console.ReadLine(), out amountMinutesToCharge);
                    if (Garage.ChargingElectricVehicle(licenseNumberToRefuel, amountMinutesToCharge) == true)
                    {
                        Console.WriteLine("The refueling operation was performed successfully.");
                    }
                }
            }
            else
            {
                Console.WriteLine("There is no license number in the garage that matches to the given license number.");
            }
        }

        private static void displayVehicleDetailsByLicenseNumbers()
        {
            string licenseToDisplayDetails = getLicenseNumber();
            bool islicenseNumberExist = Garage.VehicleIsAlreadyAtTheGarage(licenseToDisplayDetails);

            if (islicenseNumberExist == true)
            {
                Console.WriteLine(Garage.PresentSpecificVehicleDetails(licenseToDisplayDetails));
                Thread.Sleep(4000);
            }
            else
            {
                Console.WriteLine("There is no license number in the garage that matches to the given license number.");
            }
        }
    }
}
