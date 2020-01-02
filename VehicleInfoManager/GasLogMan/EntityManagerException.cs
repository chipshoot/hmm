using System;

namespace VehicleInfoManager.GasLogMan
{
    public class EntityManagerException: Exception
    {
        public EntityManagerException(string message) : base(message)
        {
        } 
    }
}