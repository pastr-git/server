using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastr.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastr
{
    public class BodyDataHandler
    {
        public BodyDataHandler()
        {
        }

        public BodyDataHandler(List<RequestBodyData> bodyData)
        {
            BodyData = bodyData;
        }

        public List<RequestBodyData> BodyData { get; set; }

        public string TryValidate(IFormCollection form, Func<RequestBodyData, string, string> onIteration = null)
        {
            foreach (var item in BodyData)
            {
                if (!form.ContainsKey(item.Name))
                    return $"Request does not have the field '{item.Name}' available.";
                string data = form[item.Name];
                if (string.IsNullOrWhiteSpace(data) && !item.Required)
                    continue;
                if (data.Length > item.Max || data.Length < item.Min)
                    return $"Data for field '{item.Name}' is invalid. ({(item.Required ? "" : "not ")}required, min: {item.Min} max: {item.Max})";
                var result = onIteration?.Invoke(item, data);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
