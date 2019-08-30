using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pastr.MongoDB
{
    public class CollectionHolder<T> where T : IModel, new()
    {
        public IMongoCollection<T> Collection { get; set; }

        public Driver Driver { get; set; }

        public CollectionHolder(Driver driver)
        {
            Collection = driver.GetCollection<T>();
        }

        public async Task<List<T>> FindManyAsync(Expression<Func<T, bool>> filter)
        {
            var list = new List<T>();
            using (var cursor = await Collection.FindAsync(filter).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync().ConfigureAwait(false))
                {
                    var batch = cursor.Current;
                    list.AddRange(batch);
                }
            }
            return list;
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> filter)
        {
            using (var cursor = await Collection.FindAsync(filter).ConfigureAwait(false))
            {
                return await cursor.FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }

        public async Task InsertManyAsync(IEnumerable<T> items) => await Collection.InsertManyAsync(items);

        public async Task InsertSingleAsync(T item) => await Collection.InsertOneAsync(item).ConfigureAwait(false);

        public async Task<DeleteResult> RemoveOneAsync(Expression<Func<T, bool>> filter) => await Collection.DeleteOneAsync(filter).ConfigureAwait(false);

        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update) => await Collection.UpdateManyAsync(filter, update).ConfigureAwait(false);

        public async Task<UpdateResult> UpdateSingleAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update) => await Collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        public async Task<ReplaceOneResult> ReplaceSingleAsync(Expression<Func<T, bool>> filter, T update) => await Collection.ReplaceOneAsync(filter, update);

        public T FindSingle(Expression<Func<T, bool>> filter)
        {
            using (var cursor = Collection.FindSync(filter))
            {
                return cursor.FirstOrDefault();
            }
        }

        public T RemoveOne(Expression<Func<T, bool>> filter) => Collection.FindOneAndDelete(filter);

        public ReplaceOneResult ReplaceSingle(Expression<Func<T, bool>> filter, T update) => Collection.ReplaceOne(filter, update);

        public void InsertSingle(T item) => Collection.InsertOne(item);
    }
}
