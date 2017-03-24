using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FaceApiLab.Models;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceApiLab.Controllers
{
    /// <summary>
    /// https://github.com/madukapai/maduka-FaceAccessControlSystem
    /// </summary>
    public class UsersController : Controller
    {
        Config Config;
        FaceServiceClient Client;
        public UsersController(IOptions<Config> config)
        {
            this.Config = config.Value;
            this.Client = new FaceServiceClient(this.Config.FaceApiKey);
        }

        public async Task<IActionResult> Index()
        {
            List<PersonGroupModel> models = new List<PersonGroupModel>();
            PersonGroup[] objPersonGroups = await Client.ListPersonGroupsAsync();
            for (int i = 0; i < objPersonGroups.Length; i++)
            {
                models.Add(
                    new Models.PersonGroupModel
                    {
                        Name = objPersonGroups[i].Name,
                        PersonGroupId = objPersonGroups[i].PersonGroupId,
                    });
            }

            return View(models);
        }


        public async Task<IActionResult> Users(string personGroupId)
        {
            ViewBag.PersonGroupId = personGroupId;
            List<PersonModel> models = new List<PersonModel>();
            Person[] objPersons = await Client.GetPersonsAsync(personGroupId);
            for (int i = 0; i < objPersons.Length; i++)
            {
                models.Add(
                    new Models.PersonModel
                    {
                        Name = objPersons[i].Name,
                        PersonId = objPersons[i].PersonId,
                    });
            }

            return View(models);
        }

        public async Task<IActionResult> UserFaces(string personGroupId, string personId)
        {
            Person person = await Client.GetPersonAsync(personGroupId, Guid.Parse(personId));
            PersonModel model = new PersonModel();
            model.PersonId = person.PersonId.ToString();
            model.Name = person.Name;
            //ps:取不到圖片，圖片上傳前要先自行儲存
            for (int i = 0; i < person.PersistedFaceIds.Length; i++)
            {
                model.Faces.Add(
                    new Models.PersonFaceModel
                    {
                        FaceId = person.PersistedFaceIds[i].ToString(),
                    });
            }
            return View(model);
        }
    }
}