using Microsoft.ProjectOxford.Face.Contract;
using System.Collections.Generic;

namespace FaceApiLab.Models
{
    public class PersonModel
    {
        public PersonModel()
        {
            Faces = new List<PersonFaceModel>();
        }
        public string Name { get; set; }
        public object PersonId { get; set; }
        public List<PersonFaceModel> Faces { get; set; }
    }
}