using System.ComponentModel;

namespace FirstCall.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images\Products")]
        Product,
        [Description(@"Images\ProductBrands")]
        ProductBrand,
        /*s0018s*/
        [Description(@"Images\Brands")]
        Brand,
       
        [Description(@"Images\Owners")]
         Owner,
      

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document,

        [Description(@"Images\Clients\Companies")]
        Company,

        [Description(@"Images\Clients\Individuals")]        Person,
    }
}