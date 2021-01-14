using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeSample
{
    public abstract class AbstractFile
    {
        public abstract void Add(AbstractFile file);
        public abstract void Remove(AbstractFile file);
        public abstract AbstractFile GetChild(int i);
        public abstract void KillVirus();
    }
}
