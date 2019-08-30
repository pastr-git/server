using Pastr.MongoDB.Models;

namespace Pastr.MongoDB
{
    public class DriverContainer : Driver
    {
        public DriverContainer()
        {
        }

        public void LoadCollections()
        {
            Pastes = new CollectionHolder<Paste>(this);
        }

        public CollectionHolder<Paste> Pastes { get; private set; }
    }
}
