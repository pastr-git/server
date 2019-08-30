using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pastr.MongoDB
{
    public interface IModel
    {
        string CollectionName { get; set; }

        string ID { get; set; }
    }
}