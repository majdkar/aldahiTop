using System.Collections.Generic;
using System.IO;

namespace FirstCall.Shared.Constants
{
    public static class Constants
    {
        public static int MaxFileSizeInByte = 1024 * 1024 * 60;

        public static string UploadFolderName = "Files/UploadFiles";

        public static string NOImagePath = Path.Combine(UploadFolderName, Enums.FileLocation.SharedFiles.ToString(), "noimage.png");
        public static string ReadyToUploadPath = Path.Combine(UploadFolderName, Enums.FileLocation.SharedFiles.ToString(), "rtu.jpg");


        public static string TinyMceApiKey = "wfvkrqv4k0bbmixix5c7kaz2u2mo1etjwv9eh0n5fb1utxgf";//"cg9l6ots0mmn3h7rr3asbg6m2ephkvpcfxjn5o1d54fau1rz";


        public static Dictionary<string, object> editorConf = new Dictionary<string, object>{
           { "menubar", true },
           { "plugins", "link image table" },
           { "toolbar", "undo redo | styleselect | forecolor | bold italic | alignleft aligncenter alignright alignjustify | outdent indent | link image | code" }
        };
    }
}
