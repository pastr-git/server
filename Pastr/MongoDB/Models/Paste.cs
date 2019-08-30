using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastr.MongoDB.Models
{
    public class Paste : IModel
    {
        [JsonIgnore]
        [BsonIgnore]
        public string CollectionName { get; set; } = "pastes";

        [BsonId]
        public string ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EditCode { get; set; }

        public Paste()
        {
            Date = DateTime.UtcNow;
            ID = IdGenerator.Instance.CreateNew();
        }

        public Paste(string title, string content) : this()
        {
            Title = title;
            Content = content;
        }

        public void SetNewEditCode() => EditCode = IdGenerator.Instance.CreateNew();

        public async Task UpdateAsync()
        {
            await Program.Database.Pastes.ReplaceSingleAsync(x => x.ID == ID, this);
        }

        public async Task DeleteAsync()
        {
            await Program.Database.Pastes.RemoveOneAsync(x => x.ID == ID);
        }
    }
}
