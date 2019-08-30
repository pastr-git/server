using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pastr.MongoDB
{
    public class DbStack<T> where T : IModel, new()
    {
        public DbStack(CollectionHolder<T> collectionHolder) : this(collectionHolder, TimeSpan.FromMinutes(15))
        {
        }

        public DbStack(CollectionHolder<T> collectionHolder, TimeSpan insertInterval)
        {
            Collection = collectionHolder;
            InsertInterval = insertInterval;
        }

        public CollectionHolder<T> Collection { get; }

        public List<T> Quene { get; set; } = new List<T>();

        public Timer Timer { get; private set; }

        public TimeSpan InsertInterval { get; set; }

        public delegate void InsertEventHandler();

        public event InsertEventHandler OnInsert;

        public void Add(T item) => Quene.Add(item);

        public void StartTimer()
        {
            if (Timer != null)
                return;
            Timer = new Timer(async (e) => await ForceInsert(), null, InsertInterval, InsertInterval);
        }

        public async Task ForceInsert()
        {
            if (Quene.Count <= 0)
                return;
            OnInsert?.Invoke();
            await Collection.InsertManyAsync(Quene).ConfigureAwait(false);
            Quene.Clear();
        }
    }
}
