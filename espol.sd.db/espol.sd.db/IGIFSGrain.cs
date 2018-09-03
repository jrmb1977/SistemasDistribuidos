using System.Threading.Tasks;
using Orleans;
using System.Data;

namespace espol.sd.db
{
    public interface IGIFSGrain : IGrainWithIntegerKey
    {
        Task CargarGIFSALaCache();

        Task<DataSet> LeerGIFSDesdeLaCache();
    }
}
