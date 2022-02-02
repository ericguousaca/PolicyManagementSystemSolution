using PolicyApiLibrary.DbModels;

namespace PolicyApiLibrary.Builders
{
    public interface IDirector
    {
        void Construct(IPolicyDetailBuilder builder, Policy policy);
    }
}
