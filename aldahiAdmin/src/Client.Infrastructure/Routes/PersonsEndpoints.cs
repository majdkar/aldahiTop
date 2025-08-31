using System;
using System.Linq;
namespace FirstCall.Client.Infrastructure.Routes
{
    public static class PersonsEndpoints
    {
        public static string GetAllPaged(string personName, string email, string phoneNumber, int countryId,int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/persons?personName={personName}&email={email}&phoneNumber={phoneNumber}&countryId={countryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }
        public static string GetByClientId(int clientId)
        {
            return $"api/v1/persons/GetByClientId/{clientId}";
        }
        public static string GetCount = "api/v1/persons/count";

       

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int personId)
        {
            return $"api/v1/persons/{personId}";
        }

        public static string GetAll = "api/v1/persons/GetAll";
        public static string Save = "api/v1/persons";
        public static string Delete = "api/v1/persons";
        public static string Export = "api/v1/persons/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";



        //Persons images
        public static string GetAllPersonImages(int PersonId,bool FilterActive)
        {
            return $"api/v1/PersonImages/GetAllByPerson/{PersonId}/{FilterActive}";
        }

  

        public static string SaveImage = "api/v1/PersonImages";

        public static string SaveVideo = "api/v1/PersonVideos";

        public static string DeleteImage = "api/v1/PersonImages";


        public static string SetDefaultImage(int PersonId, int imageId)
        {
            return $"api/v1/PersonImages/SetDefault/{PersonId}/{imageId}";
        }
    }
}
