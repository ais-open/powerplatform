using System.Collections.Generic;

namespace PluginOperations.Builder
{
    public interface IBuilder<T1,T2>
    {
        T1 BuildModel(T2 entity);
        List<T2> BuildEntity(List<T1> instances);
    }
}
