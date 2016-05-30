namespace Eurofurence.Companion.DataStore.Abstractions
{
    public interface INavigationResolver
    {
        void Resolve(IDataContext dataContext);
    }
}