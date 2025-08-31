using System.Collections.Generic;

namespace FirstCall.Client
{
    public  class TextEditorConfig
    {
        public Dictionary<string, object> tinymc { get; set; }

        public TextEditorConfig(string _selector)
        {
             tinymc = new Dictionary<string, object>{
            {"toolbar", "undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | styles |  alignleft aligncenter alignright alignjustify | ltr rtl | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak  | charmap emoticons | fullscreen  preview save print  | a11ycheck ltr rtl | insertfile image media pageembed template link anchor codesample | link image | showcomments addcomment | help"},
            { "selector", _selector},
            { "height",  300},
            {"plugins", "advlist, autolink, link, image, lists, charmap, preview, anchor, pagebreak, searchreplace, wordcount, visualblocks, code, fullscreen, insertdatetime, media, table, emoticons, template, help"},
            { "menubar", "favs file edit view insert format tools table help"},
            { "menu", "{favs: { title: 'My Favorites', items: 'code visualaid | searchreplace | emoticons' } }"}
        };
        }

    }
}
