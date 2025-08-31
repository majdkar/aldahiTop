using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared;
using FirstCall.Shared.Constants;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Shared.ViewModels.Menus;
using FirstCall.Shared.ViewModels.Pages;
using FirstCall.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Pages
{
    public partial class AddEditPageModal
    {
        [Parameter]

        public string? Id { get; set; }
        public PageUpdateModel PageModel { get; set; } = new();

        public List<PageTranslationUpdateModel> BlockTranslationUpdateModelList = new List<PageTranslationUpdateModel>();

        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton { get; set; } = true;
        private bool disableDeleteImage1Button { get; set; } = true;
        private bool disableDeleteImage2Button { get; set; } = true;
        private bool disableDeleteImage3Button { get; set; } = true;

        private bool disableDeleteImage4Button { get; set; } = true;
        private bool showLocation { get; set; } = false;

        private IList<IBrowserFile> _images = new List<IBrowserFile>();
        private IList<IBrowserFile> _images1 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images2 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images3 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images4 = new List<IBrowserFile>();

        private IEnumerable<MenuViewModel> _menus;


        private IEnumerable<MenuCategoryViewModel> MenuTypes;

        private FileUploadModel imageUploadModel;
        private FileUploadModel imageUploadModel1;
        private FileUploadModel imageUploadModel2;
        private FileUploadModel imageUploadModel3;
        private FileUploadModel imageUploadModel4;
        private TextEditorConfig editorAr1 = new TextEditorConfig("#editorAr1");
        private TextEditorConfig editorEn1 = new TextEditorConfig("#editorEn1");
        private TextEditorConfig editorAr2 = new TextEditorConfig("#editorAr2");
        private TextEditorConfig editorEn2 = new TextEditorConfig("#editorEn2");

        private bool isProcessing = false;
        private List<string> PageTypes = new List<string> {
            "Basic Pages" ,
            "Contact Us" ,
        };

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWebSiteManagement;

        //List<string> MenuTypes = new List<string> {
        //    "Home" ,
        //    "Footer" ,
        //};

        protected async override Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.Create)).Succeeded;
            Id = Int32.TryParse(Id, out var _) ? Id : "0";
            await LoadMenus();
            await LoadCategoryMenus();
            await LoadBlock(Id);
        }
        protected override async Task OnParametersSetAsync()
        {
            //Id = Int32.TryParse(Id, out var idInteger) ? Id : "0";
            //await LoadBlock(Id);
        }

        private async Task LoadBlock(string id)
        {
            if (id != "0")
            {
                var requestUri = EndPoints.Pages + $"/{id}";
                var response = await _httpClient.GetFromJsonAsync<PageUpdateModel>(requestUri);
                if (response != null)
                {
                    PageModel = response;
                    if (PageModel.Type == "Contact Us")
                        showLocation = true;
                    BlockTranslationUpdateModelList.Clear();
                    foreach (var selectedTranslation in PageModel.Translations)
                    {
                        BlockTranslationUpdateModelList.Add(
                        new PageTranslationUpdateModel
                        {
                            Id = selectedTranslation.Id,
                            Name = selectedTranslation.Name,
                            Description = selectedTranslation.Description,
                            File = selectedTranslation.File,
                            Link1 = selectedTranslation.Link1,
                            Link2 = selectedTranslation.Link2,
                            Language = selectedTranslation.Language,
                            Slug = selectedTranslation.Slug,
                            PageId = selectedTranslation.PageId,
                            IsActive = selectedTranslation.IsActive,

                        }
                            );

                    }
                    if (!String.IsNullOrEmpty(response.Image))
                    {
                        PageModel.Image = response.Image;
                        disableDeleteImageButton = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image1))
                    {
                        PageModel.Image1 = response.Image1;
                        disableDeleteImage1Button = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image2))
                    {
                        PageModel.Image2 = response.Image2;
                        disableDeleteImage2Button = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image3))
                    {
                        PageModel.Image3 = response.Image3;
                        disableDeleteImage3Button = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image4))
                    {
                        PageModel.Image4 = response.Image4;
                        disableDeleteImage4Button = false;
                    }


                    this.StateHasChanged();
                }
            }
        }

        private async Task LoadMenus()
        {
            PagedResponse<MenuViewModel> response = null;
            try
            {
                var requestUri = EndPoints.GetAll(EndPoints.MenusNoCategory, "", null);
                response = await _httpClient.GetFromJsonAsync<PagedResponse<MenuViewModel>>(requestUri);
                if (response != null)
                {
                    _menus = response.Items;
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

        private async Task LoadCategoryMenus()
        {
            try
            {
                var requestUri = EndPoints.GetAll(EndPoints.MenuCategories, "", null);
                var response = await _httpClient.GetFromJsonAsync<PagedResponse<MenuCategoryViewModel>>(requestUri);
                if (response != null)
                {
                    MenuTypes = response.Items;
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

        public void Cancel()
        {
            //MudDialog.Cancel();
            _navigationManager.NavigateTo("/pages");
        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            var generatedImageName = await UploadImage();
            var generatedImage1Name = await UploadImage1();
            var generatedImage2Name = await UploadImage2();
            var generatedImage3Name = await UploadImage3();
            var generatedImage4Name = await UploadImage4();

            var fullImagePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedImageName);
            var fullImagePath1 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedImage1Name);
            var fullImagePath2 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedImage2Name);
            var fullImagePath3 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedImage3Name);
            var fullImagePath4 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedImage4Name);


            if (PageModel.Id == 0)
            {
                PageModel.Image = !String.IsNullOrEmpty(generatedImageName) ? fullImagePath : "";
                PageModel.Image1 = !String.IsNullOrEmpty(generatedImage1Name) ? fullImagePath1 : "";
                PageModel.Image2 = !String.IsNullOrEmpty(generatedImage2Name) ? fullImagePath2 : "";
                PageModel.Image3 = !String.IsNullOrEmpty(generatedImage3Name) ? fullImagePath3 : "";
                PageModel.Image4 = !String.IsNullOrEmpty(generatedImage4Name) ? fullImagePath4 : "";

            }
            else
            {
                if (!string.IsNullOrEmpty(generatedImageName))
                    PageModel.Image = fullImagePath;
                if (!string.IsNullOrEmpty(generatedImage1Name))
                    PageModel.Image1 = fullImagePath1;
                if (!string.IsNullOrEmpty(generatedImage2Name))
                    PageModel.Image2 = fullImagePath2;
                if (!string.IsNullOrEmpty(generatedImage3Name))
                    PageModel.Image3 = fullImagePath3;
                if (!string.IsNullOrEmpty(generatedImage4Name))
                    PageModel.Image4 = fullImagePath4;
            }


            ////prepare content
            //PageModel.Description = await RichTextEditorA.GetContent();
            //PageModel.EnglishDescription = await RichTextEditorE.GetContent();

            var content = HelperMethods.ToJson(PageModel);
            HttpResponseMessage response;
            if (PageModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.Pages, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.Pages}/{PageModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                _navigationManager.NavigateTo("/pages");

            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            isProcessing = false;
        }


        private async void SelectImage(InputFileChangeEventArgs e)
        {
            _images.Clear();
            _images.Add(e.File);

            if (_images.Count > 0)
            {
                imageUploadModel = await HelperMethods.Save(e.File);
                PageModel.Image = imageUploadModel.Url;
                disableDeleteImageButton = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage1(InputFileChangeEventArgs e)
        {
            _images1.Clear();
            _images1.Add(e.File);

            if (_images1.Count > 0)
            {
                imageUploadModel1 = await HelperMethods.Save(e.File);
                PageModel.Image1 = imageUploadModel1.Url;
                disableDeleteImage1Button = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage2(InputFileChangeEventArgs e)
        {
            _images2.Clear();
            _images2.Add(e.File);

            if (_images2.Count > 0)
            {
                imageUploadModel2 = await HelperMethods.Save(e.File);
                PageModel.Image2 = imageUploadModel2.Url;
                disableDeleteImage2Button = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage3(InputFileChangeEventArgs e)
        {
            _images3.Clear();
            _images3.Add(e.File);

            if (_images3.Count > 0)
            {
                imageUploadModel3 = await HelperMethods.Save(e.File);
                PageModel.Image3 = imageUploadModel3.Url;
                disableDeleteImage3Button = false;
                this.StateHasChanged();
            }
        }

        private async void SelectImage4(InputFileChangeEventArgs e)
        {
            _images4.Clear();
            _images4.Add(e.File);

            if (_images4.Count > 0)
            {
                imageUploadModel4 = await HelperMethods.Save(e.File);
                PageModel.Image4 = imageUploadModel4.Url;
                disableDeleteImage4Button = false;
                this.StateHasChanged();
            }
        }

        private async Task<string> UploadImage()
        {
            if (_images.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: imageUploadModel.Content, name: "\"file\"", fileName: imageUploadModel.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.Image}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        private async Task<string> UploadImage1()
        {
            if (_images1.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: imageUploadModel1.Content, name: "\"file\"", fileName: imageUploadModel1.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.Image}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        private async Task<string> UploadImage2()
        {
            if (_images2.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: imageUploadModel2.Content, name: "\"file\"", fileName: imageUploadModel2.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.Image}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        private async Task<string> UploadImage3()
        {
            if (_images3.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: imageUploadModel3.Content, name: "\"file\"", fileName: imageUploadModel3.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.Image}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        private async Task<string> UploadImage4()
        {
            if (_images4.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: imageUploadModel4.Content, name: "\"file\"", fileName: imageUploadModel4.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.Image}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }



        private void DeleteImage()
        {
            _images.Clear();
            PageModel.Image = "";
            disableDeleteImageButton = true;
            this.StateHasChanged();
        }
        private void DeleteImage1()
        {
            _images1.Clear();
            PageModel.Image1 = "";
            disableDeleteImage1Button = true;
            this.StateHasChanged();
        }
        private void DeleteImage2()
        {
            _images2.Clear();
            PageModel.Image2 = "";
            disableDeleteImage2Button = true;
            this.StateHasChanged();
        }
        private void DeleteImage3()
        {
            _images3.Clear();
            PageModel.Image3 = "";
            disableDeleteImage3Button = true;
            this.StateHasChanged();
        }
        private void DeleteImage4()
        {
            _images4.Clear();
            PageModel.Image4 = "";
            disableDeleteImage4Button = true;
            this.StateHasChanged();
        }
        private void ShowLocation()
        {
            if (PageModel.Type == "Contact Us")
                showLocation = true;
            else
                showLocation = false;
            this.StateHasChanged();
        }
        private void SetInHome()
        {
            PageModel.IsHome = true;
            PageModel.IsFooter = false;
            PageModel.IsHomeFooter = false;
            this.StateHasChanged();
        }
        private void SetInFooter()
        {
            PageModel.IsHome = false;
            PageModel.IsFooter = true;
            PageModel.IsHomeFooter = false;
            this.StateHasChanged();
        }
        private void SetInHomeFooter()
        {
            PageModel.IsHome = false;
            PageModel.IsFooter = false;
            PageModel.IsHomeFooter = true;
            this.StateHasChanged();
        }


    }
}

