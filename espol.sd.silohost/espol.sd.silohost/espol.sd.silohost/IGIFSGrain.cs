using System.Threading.Tasks;
using Orleans;
using System.Data;

namespace espol.sd.silohost
{
    /// <summary>
    /// Grain interface IGIFSGrain
    /// </summary>
    public interface IGIFSGrain : IGrainWithIntegerKey
    {
        Task CargarGIFSALaCache();

        Task<DataSet> LeerGIFSDesdeLaCache();
    }
}
