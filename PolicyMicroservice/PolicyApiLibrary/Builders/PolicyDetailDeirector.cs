using PolicyApiLibrary.DbModels;

namespace PolicyApiLibrary.Builders
{
    public class PolicyDetailDeirector : IDirector
    {
        public PolicyDetailDeirector()
        {
        }

        public void Construct(IPolicyDetailBuilder builder, Policy policy)
        {
            builder.SetupBuilder(policy);
            builder.BuildMainBodySection();
            builder.BuildPolicyTypeSection();
            builder.BuildUserTypeSection();
            builder.BuildMaturityAmountSection();
        }
    }
}
