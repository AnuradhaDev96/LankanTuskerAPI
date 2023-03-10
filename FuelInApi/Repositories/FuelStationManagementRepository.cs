using FuelInApi.Data;
using FuelInApi.Interfaces;
using FuelInApi.Models;

namespace FuelInApi.Repositories
{
    public class FuelStationManagementRepository : IFuelStationManagementInterface
    {
        private DataContext _context;
        public FuelStationManagementRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckFuelStationExistByLicenseId(string licenseId)
        {
            return _context.FuelStations.Any(f => f.LicenseId == licenseId);
        }

        public bool CheckManagerExistForFuelStationByLicenseId(string licenseId)
        {
            var fuelStation = _context.FuelStations.Where(s => s.LicenseId == licenseId).FirstOrDefault();
            if (fuelStation == null)
                return false;

            if (fuelStation.ManagerUserId == null || fuelStation.ManagerUserId <= 0)
                return false;

            return true;
        }

        public bool CreateFuelOrderByFuelStationId(FuelOrder fuelOrder)
        {
            _context.FuelOrders.Add(fuelOrder);
            return Save();
        }

        public bool CreateFuelStationByAdmin(FuelStation data)
        {
            _context.FuelStations.Add(data);
            return Save();

        }

        public bool CreateFuelTokenRequestByDriverId(FuelTokenRequest token)
        {
            _context.FuelTokenRequests.Add(token);
            return Save();
        }

        public ICollection<FuelTokenRequest> FuelTokenRequestsByDriverId(int driverId)
        {
            return _context.FuelTokenRequests.Where(f => f.DriverId == driverId).ToList();
        }

        public FuelOrder? GetFuelOrderById(int id)
        {
            return _context.FuelOrders.Where(f => f.Id == id).FirstOrDefault();
        }

        public FuelOrder? GetFuelOrderExistForGivenExpectedFillingDateByStationId(DateTime expectedFillingDate, int fillingStationId)
        {
            return _context.FuelOrders.Where(fo => fo.FuelStationId == fillingStationId && fo.ExpectedDeliveryDate.Date == expectedFillingDate.Date).FirstOrDefault();
        }

        public ICollection<FuelOrder> GetFuelOrders()
        {
            return _context.FuelOrders.ToList();
        }

        public FuelStation? GetFuelStationById(int id)
        {
            return _context.FuelStations.Where(x => x.Id == id).FirstOrDefault();
        }

        public FuelStation? GetFuelStationByLicenseId(string licenseId)
        {
            return _context.FuelStations.Where(f => f.LicenseId == licenseId).FirstOrDefault();
        }

        public FuelStation? GetFuelStationByManagerId(int managerUserId)
        {
            return _context.FuelStations.Where(f => f.ManagerUserId == managerUserId).FirstOrDefault();
        }

        public ICollection<FuelStation> GetFuelStations()
        {
            return _context.FuelStations.ToList();
        }

        public bool IsFuelOrderExistForGivenExpectedFillingDateByStationId(DateTime expectedFillingDate, int fillingStationId)
        {
            return _context.FuelOrders.Any(fo => fo.FuelStationId == fillingStationId && fo.ExpectedDeliveryDate.Date == expectedFillingDate.Date);
        }

        public bool UpdateFuelOrder(FuelOrder fuelOrder)
        {
            _context.FuelOrders.Update(fuelOrder);
            return Save();
        }

        public bool UpdateFuelStation(FuelStation data)
        {
            _context.FuelStations.Update(data);
            return Save();
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
