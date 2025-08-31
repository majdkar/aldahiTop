using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared;
using FirstCall.Shared.Constants;
using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Shared.Wrapper;
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
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class BlockDetails
    {
        [Parameter]
        public int Id { get; set; }
        private int CategoryId = 0; // This is a queryString parameter

        private BlockUpdateModel BlockModel { get; set; } = new();
        private IEnumerable<BlockCategoryViewModel> categories;

        private IList<IBrowserFile> _images1 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images2 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images3 = new List<IBrowserFile>();
        private IList<IBrowserFile> _images4 = new List<IBrowserFile>();

        private IList<IBrowserFile> _files = new List<IBrowserFile>();
        public List<BlockTranslationUpdateModel> BlockTranslationUpdateModelList = new List<BlockTranslationUpdateModel>();

        private FileUploadModel fileUploadModel;

        private FileUploadModel imageUploadModel1;
        private FileUploadModel imageUploadModel2;
        private FileUploadModel imageUploadModel3;
        private FileUploadModel imageUploadModel4;

        private string searchString = "";
        private bool showLdatenews { get; set; } = false;
        private bool showdatesevent { get; set; } = false;
        private bool showdatesactivity { get; set; } = false;
        private bool showLarticals { get; set; } = false;
        private bool showAlbum { get; set; } = false;
        private LanguageSelector languageSelector { get; set; }
        private string imageUrlForPreview { get; set; } = "";
        private string imageUrlForPreview2 { get; set; } = "";
        private string imageUrlForPreview3 { get; set; } = "";
        private string imageUrlForPreview4 { get; set; } = "";

        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        private bool disableDeleteImageButton4 { get; set; } = true;

        private bool disableDeleteFileButton { get; set; } = true;

        private TextEditorConfig editorAr = new TextEditorConfig("#editorAr");
        private TextEditorConfig editorEn = new TextEditorConfig("#editorEn");
        private TextEditorConfig editorAr2 = new TextEditorConfig("#editorAr2");
        private TextEditorConfig editorEn2 = new TextEditorConfig("#editorEn2");
        private TextEditorConfig editorAr3 = new TextEditorConfig("#editorAr3");
        private TextEditorConfig editorEn3 = new TextEditorConfig("#editorEn3");
        private TextEditorConfig editorAr4 = new TextEditorConfig("#editorAr4");
        private TextEditorConfig editorEn4 = new TextEditorConfig("#editorEn4");
        private IEnumerable<BlockCategoryViewModel> elements;


        private bool isProcessing = false;

        protected async override Task OnInitializedAsync()
        {
            await LoadCategories();
        }

        protected override async Task OnParametersSetAsync()
        {
            _navigationManager.TryGetQueryString<int>("categoryId", out CategoryId);

            await LoadBlock(Id);
        }
        //private void ShowElements()
        //{
        //    if (BlockModel.BlockType == "News")
        //        showLdatenews = true;
        //    else
        //        showLdatenews = false;
        //    if (BlockModel.BlockType == "Event")
        //        showdatesevent = true;
        //    else
        //        showdatesevent = false;
        //    if (BlockModel.BlockType == "Activity")
        //        showdatesactivity = true;
        //    else
        //        showdatesactivity = false;
        //    if (BlockModel.BlockType == "Article")
        //        showLarticals = true;
        //    else
        //        showLarticals = false;
        //    this.StateHasChanged();
        //}
        private async Task LoadBlock(int id)
        {
            string[] orderings = null;
            //orderings[0] = "RecordOrder";
            int pageNumber = 0;
            int pageSize = 10;

            // var requestUricategory = EndPoints.GetAllPaged(EndPoints.BlockCategories, pageNumber, pageSize, searchString, orderings);
            var requestUricategory = EndPoints.GetAllPagedByCategoryID(EndPoints.BlockCategories, pageNumber, pageSize, searchString, orderings, CategoryId);
            var responsecate = await _httpClient.GetFromJsonAsync<PagedResponse<BlockCategoryViewModel>>(requestUricategory);
            if (responsecate != null)
            {
                elements = responsecate.Items;
                if (elements.Count() > 0)
                {
                    var selectedBlockCategory = elements.FirstOrDefault(c => c.Id == CategoryId);
                    if (selectedBlockCategory != null)
                    {
                        if (selectedBlockCategory.BlockType == "News")
                            showLdatenews = true;
                        else
                            showLdatenews = false;
                        if (selectedBlockCategory.BlockType == "Event")
                            showdatesevent = true;
                        else
                            showdatesevent = false;
                        if (selectedBlockCategory.BlockType == "Activity")
                            showdatesactivity = true;
                        else
                            showdatesactivity = false;
                        if (selectedBlockCategory.BlockType == "Article")
                            showLarticals = true;
                        else
                            showLarticals = false;
                        if (selectedBlockCategory.BlockType == "Photo Gallery" || selectedBlockCategory.BlockType == "Video Gallery")
                            showAlbum = true;
                        else
                            showAlbum = false;
                    }
                }
            }
            if (id != 0)
            {
                var requestUri = EndPoints.Blocks + $"/{id}";
                var response = await _httpClient.GetFromJsonAsync<BlockUpdateModel>(requestUri);
                if (response != null)
                {

                    BlockModel = response;

                    BlockTranslationUpdateModelList.Clear();
                    foreach (var selectedTranslation in BlockModel.Translations)
                    {
                        BlockTranslationUpdateModelList.Add(
                                                    new BlockTranslationUpdateModel
                                                    {
                                                        Id = selectedTranslation.Id,
                                                        Name = selectedTranslation.Name,
                                                        Description = selectedTranslation.Description,
                                                        Language = selectedTranslation.Language,
                                                        Slug = selectedTranslation.Slug,
                                                        BlockId = selectedTranslation.BlockId,
                                                        IsActive = selectedTranslation.IsActive
                                                    }
                            );

                    }



                    if (!String.IsNullOrEmpty(response.Image1))
                    {
                        imageUrlForPreview = response.Image1;
                        disableDeleteImageButton = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image2))
                    {
                        imageUrlForPreview2 = response.Image2;
                        disableDeleteImageButton2 = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image3))
                    {
                        imageUrlForPreview3 = response.Image3;
                        disableDeleteImageButton3 = false;
                    }
                    if (!String.IsNullOrEmpty(response.Image4))
                    {
                        imageUrlForPreview4 = response.Image4;
                        disableDeleteImageButton4 = false;
                    }
                    if (!String.IsNullOrEmpty(response.File))
                    {
                        disableDeleteFileButton = false;
                    }
                    this.StateHasChanged();
                }
            }
            else
            {
                BlockModel.CategoryId = CategoryId;

            }
        }

        private async Task LoadCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BlockCategoryViewModel>>(EndPoints.BlockCategoriesSelect);
            if (response != null)
            {
                categories = response;
            }
            else
            {
                _snackBar.Add("Error retrieving data");
            }
        }

        private async Task SaveAsync()
        {

            isProcessing = true;
            var generatedImageName = await UploadFile(_images1, imageUploadModel1, (int)Enums.UploadType.Image);
            var fullImagePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedImageName);

            var generatedImageName2 = await UploadFile(_images2, imageUploadModel2, (int)Enums.UploadType.Image);
            var fullImagePath2 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedImageName2);

            var generatedImageName3 = await UploadFile(_images3, imageUploadModel3, (int)Enums.UploadType.Image);
            var fullImagePath3 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedImageName3);

            var generatedImageName4 = await UploadFile(_images4, imageUploadModel4, (int)Enums.UploadType.Image);
            var fullImagePath4 = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedImageName4);

            var generatedFileName = await UploadFile(_files, fileUploadModel, (int)Enums.UploadType.File);
            var fullFilePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedFileName);


            if (BlockModel.Id == 0)
            {
                BlockModel.Image1 = !String.IsNullOrEmpty(generatedImageName) ? fullImagePath : "";
                BlockModel.Image2 = !String.IsNullOrEmpty(generatedImageName2) ? fullImagePath2 : "";
                BlockModel.Image3 = !String.IsNullOrEmpty(generatedImageName3) ? fullImagePath3 : "";
                BlockModel.Image4 = !String.IsNullOrEmpty(generatedImageName4) ? fullImagePath4 : "";

                BlockModel.File = !String.IsNullOrEmpty(generatedFileName) ? fullFilePath : "";
            }
            else
            {
                if (!String.IsNullOrEmpty(generatedImageName))
                    BlockModel.Image1 = fullImagePath;
                if (!String.IsNullOrEmpty(generatedImageName2))
                    BlockModel.Image2 = fullImagePath2;
                if (!String.IsNullOrEmpty(generatedImageName3))
                    BlockModel.Image3 = fullImagePath3;
                if (!String.IsNullOrEmpty(generatedImageName4))
                    BlockModel.Image4 = fullImagePath4;
                if (!String.IsNullOrEmpty(generatedFileName))
                    BlockModel.File = fullFilePath;
            }

            var content = HelperMethods.ToJson(BlockModel);
            HttpResponseMessage response;
            if (BlockModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.Blocks, content);

            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.Blocks}/{BlockModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                _navigationManager.NavigateTo($"/blocks/{BlockModel.CategoryId}");
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            isProcessing = false;
        }

        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }

        private async void SelectImage(InputFileChangeEventArgs e1)
        {
            _images1.Clear();
            _images1.Add(e1.File);

            if (_images1.Count > 0)
            {
                imageUploadModel1 = await HelperMethods.Save(e1.File);
                imageUrlForPreview = imageUploadModel1.Url;
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
                imageUrlForPreview2 = imageUploadModel2.Url;
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
                imageUrlForPreview3 = imageUploadModel3.Url;
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
                imageUrlForPreview4 = imageUploadModel4.Url;
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

        private async Task<string> UploadFile(IList<IBrowserFile> files, FileUploadModel fileModel, int uploadFileType)
        {
            if (files.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: fileModel.Content, name: "\"file\"", fileName: fileModel.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.BlocksFiles}/{uploadFileType}", content);
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
            _images1.Clear();
            BlockModel.Image1 = "";
            imageUrlForPreview = "";
            disableDeleteImageButton = true;
            this.StateHasChanged();
        }

        private void DeleteImage2()
        {
            _images2.Clear();
            BlockModel.Image2 = "";
            imageUrlForPreview2 = "";
            disableDeleteImageButton2 = true;
            this.StateHasChanged();
        }

        private void DeleteImage3()
        {
            _images3.Clear();
            BlockModel.Image3 = "";
            imageUrlForPreview3 = "";
            disableDeleteImageButton3 = true;
            this.StateHasChanged();
        }

        private void DeleteImage4()
        {
            _images4.Clear();
            BlockModel.Image4 = "";
            imageUrlForPreview4 = "";
            disableDeleteImageButton4 = true;
            this.StateHasChanged();
        }

        private void DeleteFile()
        {
            _files.Clear();
            BlockModel.File = "";
            disableDeleteFileButton = true;
            this.StateHasChanged();
        }
        private async Task SaveAsync(BlockTranslationUpdateModel BlockTranslationModel)
        {
            BlockTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(BlockTranslationModel);
            HttpResponseMessage response;
            if (BlockTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.BlocksTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.BlocksTranslation}/{BlockTranslationModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }

        }
        private async Task InvokeTranslationModal(int blockId, int transltionId = 0)
        {

            var parameters = new DialogParameters();
            var selectedTranslation = new BlockTranslationUpdateModel();
            parameters.Add(nameof(AddEditBlockTranslationModal.BlockTranslationModel), new BlockTranslationUpdateModel
            {
                BlockId = blockId
            });

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBlockTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadBlock(blockId);
            }
        }


    }
}