using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared
{
    public static class Enums
    {
        public enum FileLocation
        {
            BlocksFiles = 1,
            MenusFiles = 2,
            PagesFiles = 3,
            EventsFiles = 4,
            ArticlesFiles = 5,
            SharedFiles = 6,
            ProductFiles = 7,
            BrandsFiles = 8
        }

        public enum UploadType
        {
            Image = 1,
            File = 2,
            Video = 3,
        }
    }
}
