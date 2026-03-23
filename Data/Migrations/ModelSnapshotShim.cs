using Microsoft.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore.Migrations
{
    // Local shim for ModelSnapshot so generated snapshots can compile in environments
    // lacking EF design-time assemblies. Safe to remove if EF packages are available.
    public abstract class ModelSnapshot
    {
        protected virtual void BuildModel(ModelBuilder modelBuilder) { }
    }
}
