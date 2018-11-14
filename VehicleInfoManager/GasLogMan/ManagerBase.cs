using DomainEntity.Misc;

namespace VehicleInfoManager.GasLogMan
{
    public abstract class ManagerBase<T>
    {
        protected abstract void SetEntityContent(T entity);

        protected abstract T GetEntityFromNote(HmmNote note);
    }
}