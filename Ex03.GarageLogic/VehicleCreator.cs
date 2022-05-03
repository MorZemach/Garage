namespace Ex03.GarageLogic
{
    using System;
    public class VehicleCreator
    {
        public const byte k_MinVehicleType = 1;
        public const byte k_MaxVehicleType = 5;

        public enum eVehicleType
        {
            Electric_Motorcycle = 1,
            Regular_Motorcycle,
            Electric_Car,
            Regular_Car,
            Truck
        }

        public static bool IsVehicleTypeInRange(string i_TypeNumber)
        {
            bool typeIsInRange = false;

            if(Enum.IsDefined(typeof(eVehicleType), Enum.Parse(typeof(eVehicleType), i_TypeNumber)) == true)
            {
                typeIsInRange = true;
            }

            return typeIsInRange;
        }

        public static Vehicle CreateVehicle(string i_VehicleType, string i_LicenseNumer)
        {
            Vehicle vehicleToCreate = null;

            if(IsVehicleTypeInRange(i_VehicleType) == true)
            {
                eVehicleType vehicleType = (eVehicleType)Enum.Parse(typeof(eVehicleType),
                    i_VehicleType);

                switch (vehicleType)
                {
                    case eVehicleType.Electric_Motorcycle:
                        {
                            vehicleToCreate = createElectricMotorcycle(i_LicenseNumer);
                            break;
                        }
                    case eVehicleType.Regular_Motorcycle:
                        {
                            vehicleToCreate = createRegularMotocycle(i_LicenseNumer);
                            break;
                        }
                    case eVehicleType.Electric_Car:
                        {
                            vehicleToCreate = createElectricCar(i_LicenseNumer);
                            break;
                        }
                    case eVehicleType.Regular_Car:
                        {
                            vehicleToCreate = createRegularCar(i_LicenseNumer);
                            break;
                        }
                    case eVehicleType.Truck:
                        {
                            vehicleToCreate = createTruck(i_LicenseNumer);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException();
                        }
                }
            }

            return vehicleToCreate;
        }

        private static Motorcycle createElectricMotorcycle(string i_LicenseNumer)
        {
            return new Motorcycle(i_LicenseNumer, new ElectricEngine(1.8f));
        }

        private static Motorcycle createRegularMotocycle(string i_LicenseNumber)
        {
            return new Motorcycle(i_LicenseNumber, new FuelEngine(6f, FuelEngine.eFuelType.Octan98));
        }

        private static Car createElectricCar(string i_LicenseNumer)
        {
            return new Car(i_LicenseNumer, new ElectricEngine(3.2f));
        }

        private static Car createRegularCar(string i_LicenseNumer)
        {
            return new Car(i_LicenseNumer, new FuelEngine(45f, FuelEngine.eFuelType.Octan95));
        }

        private static Truck createTruck(string i_LicenseNumer)
        {
            return new Truck(i_LicenseNumer, new FuelEngine(120f, FuelEngine.eFuelType.Soler));
        }
    }
}
