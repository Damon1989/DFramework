using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITest : IGrainWithIntegerKey
    {
        Task AddCount(string taskName);
    }
}