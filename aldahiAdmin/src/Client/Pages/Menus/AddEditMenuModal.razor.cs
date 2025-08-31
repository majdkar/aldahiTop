using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Client.Pages.Pages;
using FirstCall.Shared;
using FirstCall.Shared.Constants;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Menus
{
    public partial class AddEditMenuModal
    {
        [Parameter] public string? Id { get; set; }
        [Parameter] public int? CategoryId { get; set; }


        public MenuUpdateModel MenuModel { get; set; } = new();

        private IEnumerable<MenuCategoryViewModel> categories;
        private IEnumerable<MenuViewModel> _menus;

        public List<MenuTranslationUpdateModel> BlockTranslationUpdateModelList = new List<MenuTranslationUpdateModel>();
        //public BlazoredRishTextEditor RichTextEditorE { get; set; }
        private bool disableDeleteImageButton { get; set; } = true;
        private bool disableDeleteFileButton1 { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        private bool disableDeleteImageButton4 { get; set; } = true;

        private bool disableDeleteFileButton { get; set; } = true;

        //public BlazoredRishTextEditor RichTextEditorA { get; set; }

        private IList<IBrowserFile> _images1 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images2 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images3 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images4 = new List<IBrowserFile>();
        private IList<IBrowserFile> _files = new List<IBrowserFile>();
        private IList<IBrowserFile> _files1 = new List<IBrowserFile>();

        private bool isProcessing = false;

        private string noImageUrl = Constants.NOImagePath;
        private TextEditorConfig editorAr = new TextEditorConfig("#editorAr");
        private TextEditorConfig editorEn = new TextEditorConfig("#editorEn");
        private TextEditorConfig editorAr2 = new TextEditorConfig("#editorAr2");
        private TextEditorConfig editorEn2 = new TextEditorConfig("#editorEn2");
        private TextEditorConfig editorAr3 = new TextEditorConfig("#editorAr3");
        private TextEditorConfig editorEn3 = new TextEditorConfig("#editorEn3");
        private TextEditorConfig editorAr4 = new TextEditorConfig("#editorAr4");
        private TextEditorConfig editorEn4 = new TextEditorConfig("#editorEn4");
        private List<string> MenuTypes = new List<string> {
            "Pages" ,
            "Drop Down Menu" ,
            "Internal Link" ,
            "External Link" ,
            "Downloaded File" ,
        };
        private FileUploadModel fileUploadModel;
        private FileUploadModel fileUploadModel1;

        private FileUploadModel imageUploadModel1;
        private FileUploadModel imageUploadModel2;
        private FileUploadModel imageUploadModel3;
        private FileUploadModel imageUploadModel4;

        public int parentId { get; set; } = 0;

        public long maxFileSize = Constants.MaxFileSizeInByte;
        private ClaimsPrincipal _currentUser;
        private bool _canEditMenu;




        protected async override Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditMenu = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Menues.Edit)).Succeeded;

            Id = Int32.TryParse(Id, out var _) ? Id : "0";

            await LoadCategories();
            await LoadMenus();
            await LoadBlock(Id);

        }
        private async Task LoadBlock(string id)
        {
            if (id != "0")
            {
                var requestUri = EndPoints.Menus + $"/{id}";
                var response = await _httpClient.GetFromJsonAsync<MenuUpdateModel>(requestUri);
                if (response != null)
                {
                    MenuModel = response;

                    BlockTranslationUpdateModelList.Clear();
                    foreach (var selectedTranslation in MenuModel.Translations)
                    {
                        BlockTranslationUpdateModelList.Add(
                        new MenuTranslationUpdateModel
                        {
                            Id = selectedTranslation.Id,
                            Name = selectedTranslation.Name,
                            HtmlText = selectedTranslation.HtmlText,
                            MenueId = selectedTranslation.MenueId,
                            Language = selectedTranslation.Language,
                            IsActive = selectedTranslation.IsActive,
                            CategoryId = selectedTranslation.CategoryId

                        }
                            );

                    }
                    if (!String.IsNullOrEmpty(response.Image1))
                    {
                        MenuModel.Image1 = response.Image1;
                        disableDeleteImageButton = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image2))
                    {
                        MenuModel.Image2 = response.Image2;
                        disableDeleteImageButton2 = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image3))
                    {
                        MenuModel.Image3 = response.Image3;
                        disableDeleteImageButton3 = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image4))
                    {
                        MenuModel.Image4 = response.Image4;
                        disableDeleteImageButton4 = false;
                    }
                    if (!String.IsNullOrEmpty(response.File))
                    {

                        MenuModel.File = response.File;
                        disableDeleteFileButton = false;

                    }
                    if (!String.IsNullOrEmpty(response.FileEnglish))
                    {

                        MenuModel.FileEnglish = response.FileEnglish;
                        disableDeleteFileButton1 = false;

                    }
                    parentId = MenuModel.ParentId ?? 0;
                    this.StateHasChanged();
                }
            }
        }


        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
            // _navigationManager.NavigateTo($"/menus/{MenuModel.CategoryId}");
        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            if (MenuModel.LevelOrder < 0)
            {
                _snackBar.Add("Order Must Be More Or Equal 0", Severity.Error);
                return;
            }
            if (parentId == 0)
                MenuModel.ParentId = null;
            else
                MenuModel.ParentId = parentId;

            var generatedImageName = await UploadFile(_images1, imageUploadModel1, (int)Enums.UploadType.Image);
            var fullImagePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedImageName);

            var generatedImageName2 = await UploadFile(_images2, imageUploadModel2, (int)Enums.UploadType.Image);
            var fullImagePath2 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedImageName2);

            var generatedImageName3 = await UploadFile(_images3, imageUploadModel3, (int)Enums.UploadType.Image);
            var fullImagePath3 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedImageName3);

            var generatedImageName4 = await UploadFile(_images4, imageUploadModel4, (int)Enums.UploadType.Image);
            var fullImagePath4 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedImageName4);

            var generatedFileName = await UploadFile(_files, fileUploadModel, (int)Enums.UploadType.File);
            var fullFilePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedFileName);

            var generatedFileName1 = await UploadFile(_files1, fileUploadModel1, (int)Enums.UploadType.File);
            var fullFilePath1 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedFileName1);

            if (MenuModel.Id == 0)
            {
                MenuModel.Image1 = !String.IsNullOrEmpty(generatedImageName) ? fullImagePath : "";
                MenuModel.Image2 = !String.IsNullOrEmpty(generatedImageName2) ? fullImagePath2 : "";
                MenuModel.Image3 = !String.IsNullOrEmpty(generatedImageName3) ? fullImagePath3 : "";
                MenuModel.Image4 = !String.IsNullOrEmpty(generatedImageName4) ? fullImagePath4 : "";

                MenuModel.File = !String.IsNullOrEmpty(generatedFileName) ? fullFilePath : "";
                MenuModel.FileEnglish = !String.IsNullOrEmpty(generatedFileName1) ? fullFilePath1 : "";

            }
            else
            {
                if (!String.IsNullOrEmpty(generatedImageName))
                    MenuModel.Image1 = fullImagePath;

                if (!String.IsNullOrEmpty(generatedImageName2))
                    MenuModel.Image2 = fullImagePath2;

                if (!String.IsNullOrEmpty(generatedImageName3))
                    MenuModel.Image3 = fullImagePath3;

                if (!String.IsNullOrEmpty(generatedImageName4))
                    MenuModel.Image4 = fullImagePath4;

                if (!String.IsNullOrEmpty(generatedFileName))
                    MenuModel.File = fullFilePath;

                if (!String.IsNullOrEmpty(generatedFileName1))
                    MenuModel.FileEnglish = fullFilePath1;
            }


            ////prepare content
            //MenuModel.Description = await RichTextEditorA.GetContent();
            //MenuModel.EnglishDescription = await RichTextEditorE.GetContent();

            var content = HelperMethods.ToJson(MenuModel);
            HttpResponseMessage response;

            if (MenuModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.Menus, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.Menus}/{MenuModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                _navigationManager.NavigateTo($"/menus/{MenuModel.CategoryId}");

            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            isProcessing = false;
        }

        private async Task LoadCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<MenuCategoryViewModel>>(EndPoints.MenuCategoriesSelect);
            if (response != null)
            {
                categories = response;
            }
            else
            {
                _snackBar.Add("Error retrieving data");
            }
        }

        private async Task LoadMenus()
        {
            try
            {

                var response = await _httpClient.GetFromJsonAsync<IEnumerable<MenuViewModel>>(EndPoints.MenuSelect);

                if (response != null)
                {
                    _menus = response.Where(x => x.Id != MenuModel.Id);
                }
                else
                {
                    _snackBar.Add("Error retrieving data");
                }
            }
            catch (Exception)
            {
                _snackBar.Add("Error retrieving data: ");
            }
            finally
            {

            }
        }


        private void DeleteImage()
        {
            _images1.Clear();
            MenuModel.Image1 = "";

            disableDeleteImageButton = true;
            this.StateHasChanged();
        }
        private void DeleteImage2()
        {
            _images2.Clear();
            MenuModel.Image2 = "";

            disableDeleteImageButton2 = true;
            this.StateHasChanged();
        }
        private void DeleteImage3()
        {
            _images3.Clear();
            MenuModel.Image3 = "";

            disableDeleteImageButton3 = true;
            this.StateHasChanged();
        }
        private void DeleteImage4()
        {
            _images4.Clear();
            MenuModel.Image4 = "";

            disableDeleteImageButton4 = true;
            this.StateHasChanged();
        }

        private void DeleteFile()
        {
            _files.Clear();
            MenuModel.File = "";
            disableDeleteFileButton = true;
            this.StateHasChanged();
        }

        private void DeleteFile1()
        {
            _files1.Clear();
            MenuModel.FileEnglish = "";
            disableDeleteFileButton1 = true;
            this.StateHasChanged();
        }




        private async Task InvokeAddEditPage(string value)
        {

            MenuModel.Type = value;
            if (value == "Pages")
            {
                var parameters = new DialogParameters();

                //add operation
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = false, DisableBackdropClick = true };
                var dialog = _dialogService.Show<AddEditPageModal>("Create", parameters, options);
                var result = await dialog.Result;
                if (!result.Cancelled)
                {

                }
            }
        }



        private async void SelectImage(InputFileChangeEventArgs e1)
        {
            _images1.Clear();
            _images1.Add(e1.File);

            if (_images1.Count > 0)
            {
                imageUploadModel1 = await HelperMethods.Save(e1.File);
                MenuModel.Image1 = imageUploadModel1.Url;
                disableDeleteImageButton = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage2(InputFileChangeEventArgs e1)
        {
            _images2.Clear();
            _images2.Add(e1.File);

            if (_images2.Count > 0)
            {
                imageUploadModel2 = await HelperMethods.Save(e1.File);
                MenuModel.Image2 = imageUploadModel2.Url;
                disableDeleteImageButton2 = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage3(InputFileChangeEventArgs e1)
        {
            _images3.Clear();
            _images3.Add(e1.File);

            if (_images3.Count > 0)
            {
                imageUploadModel3 = await HelperMethods.Save(e1.File);
                MenuModel.Image3 = imageUploadModel3.Url;
                disableDeleteImageButton3 = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage4(InputFileChangeEventArgs e1)
        {
            _images4.Clear();
            _images4.Add(e1.File);

            if (_images4.Count > 0)
            {
                imageUploadModel4 = await HelperMethods.Save(e1.File);
                MenuModel.Image4 = imageUploadModel4.Url;
                disableDeleteImageButton4 = false;
                this.StateHasChanged();
            }
        }

        private async void SelectFile(InputFileChangeEventArgs e2)
        {
            _files.Clear();
            _files.Add(e2.File);

            if (_files.Count > 0)
            {
                fileUploadModel = await HelperMethods.Save(e2.File);
                disableDeleteFileButton = false;
                this.StateHasChanged();
            }
        }


        private async void SelectFile1(InputFileChangeEventArgs e2)
        {
            _files1.Clear();
            _files1.Add(e2.File);

            if (_files1.Count > 0)
            {
                fileUploadModel1 = await HelperMethods.Save(e2.File);
                disableDeleteFileButton1 = false;
                this.StateHasChanged();
            }
        }


        private async Task<string> UploadFile(IList<IBrowserFile> files, FileUploadModel fileModel, int uploadFileType)
        {
            if (files.Count > 0)
            {
                // Provides a container for content encoded using multipart/form-data MIME type.
                using var content = new MultipartFormDataContent();
                content.Add
                (content: fileModel.Content, name: "\"file\"", fileName: fileModel.Name);
                try
                {
                    var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.MenusFiles}/{uploadFileType}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.getMessage();
                    }
                    else
                    {
                        return String.Empty;
                    }

                }
                catch (Exception)
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }
    }
}

