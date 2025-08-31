using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FirstCall.Shared.Constants;
using Microsoft.JSInterop;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class BlockPhotoDetails
    {
        [Parameter]
        public string? Id { get; set; }
        private int CategoryId = 0; // This is a queryString parameter
        private BlockUpdateModel BlockModel { get; set; } = new();
        private IList<IBrowserFile> _blockphotos = new List<IBrowserFile>();
        public List<BlockPhotoUpdateModel> BlockPhotoUpdateModelList = new List<BlockPhotoUpdateModel>();

        private List<FileUploadModel> blockphotoUploadModelList = new List<FileUploadModel>();

        private List<string> blockphotoUrlForPreviewList { get; set; } = new();
        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteBlockPhoto { get; set; } = true;


    
        protected override async Task OnParametersSetAsync()
        {
            Id = Int32.TryParse(Id, out var idInteger) ? Id : "0";
            await LoadBlock(Id);
            _navigationManager.TryGetQueryString<int>("categoryId", out CategoryId);

        }

        private async Task LoadBlock(string id)
        {
            if (id != "0")
            {
                var requestUri = EndPoints.Blocks + $"/{id}";
                var response = await _httpClient.GetFromJsonAsync<BlockUpdateModel>(requestUri);
                if (response != null)
                {
                    BlockModel = response;
                    BlockPhotoUpdateModelList.Clear();
                    foreach (var selectedBlockPhoto in BlockModel.BlockPhotos)
                    {
                        BlockPhotoUpdateModelList.Add(
                                                    new BlockPhotoUpdateModel
                                                    {
                                                        Id = selectedBlockPhoto.Id,
                                                        Image = selectedBlockPhoto.Image,
                                                        BlockId = selectedBlockPhoto.BlockId,

                                                    }
                            );
                        if (!String.IsNullOrEmpty(selectedBlockPhoto.Image))
                        {
                            blockphotoUrlForPreviewList.Add(selectedBlockPhoto.Image);
                            disableDeleteBlockPhoto = false;
                        }


                    }

                    this.StateHasChanged();
                }
            }
        }
        private async Task BlocksAsync()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);

        }

        private async Task SaveAsync()
        {
            bool error = false;

            foreach (var imageUploadModel in blockphotoUploadModelList)
            {
                var generatedImageName = await UploadFile(_blockphotos, imageUploadModel, (int)Enums.UploadType.Image);
                var fullImagePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedImageName);
                var BlockPhotoInsertModel = new BlockPhotoInsertModel()
                {
                    BlockId = int.Parse(Id),
                    Image = !String.IsNullOrEmpty(generatedImageName) ? fullImagePath : "",
                };
                var content = HelperMethods.ToJson(BlockPhotoInsertModel);
                HttpResponseMessage response;
                response = await _httpClient.PostAsync(EndPoints.BlocksPhoto, content);
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Completed Successful!", Severity.Success);
                }
                else
                {
                    _snackBar.Add("Something went wrong!", Severity.Error);
                    error = true;
                }

            }
            if (!error)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                // _navigationManager.NavigateTo("/blocks");
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }



        }

        public void Cancel()
        {
                _navigationManager.NavigateTo($"/blocks?categoryId={CategoryId}");
        }

        private async void SelectImage(InputFileChangeEventArgs e1)
        {
            //_blockphotos.Clear();
            //blockphotoUploadModelList.Clear();
            _blockphotos.Add(e1.File);

            if (_blockphotos.Count > 0)
            {
                var blockphotoUploadModel = await HelperMethods.Save(e1.File);
                blockphotoUploadModelList.Add(blockphotoUploadModel);
                blockphotoUrlForPreviewList.Add(blockphotoUploadModel.Url);
                disableDeleteBlockPhoto = false;
                this.StateHasChanged();
            }
        }


        private async void UploadFiles(InputFileChangeEventArgs e)
        {
            _blockphotos.Clear();
            foreach (var file in e.GetMultipleFiles())
            {
                this._blockphotos.Add(file);

                if (_blockphotos.Count > 0)
                {
                    var blockphotoUploadModel = await HelperMethods.Save(file);
                    blockphotoUploadModelList.Add(blockphotoUploadModel);
                    blockphotoUrlForPreviewList.Add(blockphotoUploadModel.Url);
                    disableDeleteBlockPhoto = false;

                }


            }
            await SaveAsync();
            _blockphotos.Clear();
            blockphotoUrlForPreviewList.Clear();
            blockphotoUploadModelList.Clear();
            await LoadBlock(Id);
            this.StateHasChanged();
            // _navigationManager.NavigateTo($"/block-photo-details/{Id}",true);
            //TODO upload the files to the server
        }


        private async Task<string> UploadFile(IList<IBrowserFile> files, FileUploadModel fileModel, int uploadFileType)
        {
            if (files.Count > 0)
            {
                var content = new MultipartFormDataContent();
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

        private async void DeleteAllImages()
        {

            string deleteContent = localizer["Delete All Image"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["PCDelete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                _blockphotos.Clear();

                blockphotoUrlForPreviewList.Clear();
                blockphotoUploadModelList.Clear();
                disableDeleteBlockPhoto = true;
                bool error = false;
                foreach (var delblockphoto in BlockPhotoUpdateModelList)
                {
                    var repsonse = await _httpClient.DeleteAsync($"{EndPoints.BlocksPhoto}/{delblockphoto.Id}");
                    if (repsonse.IsSuccessStatusCode)
                    {

                        _snackBar.Add("Complete Successful!", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Something went wrong!", Severity.Error);
                        error = true;
                    }

                }
                if (!error)
                {
                    _snackBar.Add("Deleted Successful!", Severity.Success);
                }
                else
                {
                    _snackBar.Add("Something went wrong!", Severity.Error);
                }

                this.StateHasChanged();
            }
        }

        private async void DeleteImage(string imageUrlForPreview)
        {

            string deleteContent = localizer["Delete Image"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["PCDelete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {

                blockphotoUrlForPreviewList.Remove(imageUrlForPreview);
                var delblockphoto = BlockPhotoUpdateModelList.Find(x => x.Image == imageUrlForPreview);
                var respponse = await _httpClient.DeleteAsync($"{EndPoints.BlocksPhoto}/{delblockphoto.Id}");
                if (respponse.IsSuccessStatusCode)
                {
                    if (blockphotoUrlForPreviewList.Count == 0) { disableDeleteBlockPhoto = true; }

                    _snackBar.Add("Complete Successful!", Severity.Success);
                }
                else
                {
                    _snackBar.Add("Something went wrong!", Severity.Error);
                }

                this.StateHasChanged();
            }
        }

    }
}