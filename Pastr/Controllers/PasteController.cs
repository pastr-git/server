using Microsoft.AspNetCore.Mvc;
using Pastr.MongoDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasteController : ControllerBase
    {
        public PasteController()
        {
            EditPasteBodyData.BodyData = PasteBodyData.BodyData.Concat(EditData.BodyData).ToList();
        }

        public BodyDataHandler PasteBodyData { get; } = new BodyDataHandler(new List<RequestBodyData>
        {
            new RequestBodyData("title", true, 2, 64),
            new RequestBodyData("content", true, 8, 16384)
        });

        public BodyDataHandler EditPasteBodyData { get; } = new BodyDataHandler();

        public BodyDataHandler EditData { get; } = new BodyDataHandler(new List<RequestBodyData>
        {
            new RequestBodyData("editCode", true, 32, 32)
        });

        // GET api/paste/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Paste>> Get(string id)
        {
            // dry coded
            if (string.IsNullOrWhiteSpace(id) || id.Length > 32)
                return StatusCode(400, "Data for field 'id' is invalid. (max: 32)");
            var paste = await Program.Database.Pastes.FindSingleAsync(x => x.ID == id);
            if (paste == null)
                return StatusCode(404, $"Paste not found with the id '{id}'.");
            paste.EditCode = null; // Remove edit code for GET request
            return paste;
        }

        // GET api/paste/{id}/raw
        [HttpGet("{id}/raw")]
        public async Task<ActionResult<string>> GetRawContent(string id)
        {
            // dry coded
            if (string.IsNullOrWhiteSpace(id) || id.Length > 32)
                return StatusCode(400, "Data for field 'id' is invalid. (max: 32)");
            var paste = await Program.Database.Pastes.FindSingleAsync(x => x.ID == id);
            if (paste == null)
                return StatusCode(404, $"Paste not found with the id '{id}'.");
            return paste.Content;
        }

        // POST api/paste
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            if (Request.ContentType != "application/x-www-form-urlencoded")
                return StatusCode(400, "Incorrect Content-Type. Expected 'application/x-www-form-urlencoded'.");
            var paste = new Paste();
            var result = PasteBodyData.TryValidate(Request.Form, (item, data) =>
            {
                if (item.Name == "title")
                    paste.Title = data;
                else if (item.Name == "content")
                    paste.Content = data;
                return null;
            });
            if (result != null)
                return StatusCode(400, result);
            paste.SetNewEditCode();
            await Program.Database.Pastes.InsertSingleAsync(paste);
            return Created(Url.Content("~/paste/" + paste.ID), paste);
        }

        // PUT api/paste/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id)
        {
            if (Request.ContentType != "application/x-www-form-urlencoded")
                return StatusCode(400, "Incorrect Content-Type. Expected 'application/x-www-form-urlencoded'.");
            // dry coded
            if (string.IsNullOrWhiteSpace(id) || id.Length > 32)
                return StatusCode(400, "Data for field 'id' is invalid. (max: 32)");
            var paste = await Program.Database.Pastes.FindSingleAsync(x => x.ID == id);
            if (paste == null)
                return StatusCode(404, $"Paste not found with the id '{id}'.");


            var result = EditPasteBodyData.TryValidate(Request.Form, (item, data) =>
            {
                if (item.Name == "editCode" && paste.EditCode != data)
                    return "'editCode' did not match the one found linked to the paste.";
                if (item.Name == "title")
                    paste.Title = data;
                else if (item.Name == "content")
                    paste.Content = data;
                return null;
            });
            if (result != null)
                return StatusCode(400, result);
            await paste.UpdateAsync();
            return Ok();
        }

        // DELETE api/paste/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            // dry coded
            if (string.IsNullOrWhiteSpace(id) || id.Length > 32)
                return StatusCode(400, "Data for field 'id' is invalid. (max: 32)");
            var paste = await Program.Database.Pastes.FindSingleAsync(x => x.ID == id);
            if (paste == null)
                return StatusCode(404, $"Paste not found with the id '{id}'.");


            var result = EditData.TryValidate(Request.Form, (item, data) =>
            {
                if (item.Name == "editCode" && paste.EditCode != data)
                    return "'editCode' did not match the one found linked to the paste.";
                return null;
            });
            if (result != null)
                return StatusCode(400, result);
            await paste.DeleteAsync();
            return NoContent();
        }
    }
}
