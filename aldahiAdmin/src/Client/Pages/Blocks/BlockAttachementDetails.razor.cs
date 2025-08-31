using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared;
using FirstCall.Shared.Constants;
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


namespace FirstCall.Client.Pages.Blocks
{
    public partial class BlockAttachementDetails
    {
        [Parameter]
        public string? Id { get; set; }
        private BlockUpdateModel BlockModel { get; set; } = new();
        private IList<IBrowserFile> _blockAttachements = new List<IBrowserFile>();
        public List<BlockAttachementUpdateModel> BlockAttachementUpdateModelList = new List<BlockAttachementUpdateModel>();
        private List<FileUploadModel> blockAttachementUploadModelList = new List<FileUploadModel>();

        private bool disableDeleteBlockAttachement { get; set; } = true;


        protected override void OnInitialized()
        {

        }

        protected override async Task OnParametersSetAsync()
        {
            Id = Int32.TryParse(Id, out var idInteger) ? Id : "0";
            await LoadBlock(Id);
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
                    BlockAttachementUpdateModelList.Clear();
                    foreach (var selectedBlockAttachement in BlockModel.BlockAttachements)
                    {
                        BlockAttachementUpdateModelList.Add(
                                                    new BlockAttachementUpdateModel
                                                    {
                                                        Id = selectedBlockAttachement.Id,
                                                        File = selectedBlockAttachement.File,
                                                        Name = selectedBlockAttachement.Name,
                                                        BlockId = selectedBlockAttachement.BlockId,

                                                    }
                            );
                        if (!String.IsNullOrEmpty(selectedBlockAttachement.File))
                        {

                            disableDeleteBlockAttachement = false;
                        }


                    }

                    this.StateHasChanged();
                }
            }
        }
        private void BlocksAsync()
        {
            _navigationManager.NavigateTo($"/blocks/{@BlockModel.CategoryId}");
        }

        private async Task SaveAsync()
        {

            foreach (var fileUploadModel in blockAttachementUploadModelList)
            {
                var generatedFileName = "";
                if (fileUploadModel.Type.StartsWith("video"))
                {
                    generatedFileName = await UploadFile(_blockAttachements, fileUploadModel, (int)Enums.UploadType.Video);
                }
                else
                {
                    generatedFileName = await UploadFile(_blockAttachements, fileUploadModel, (int)Enums.UploadType.File);
                }
                if (generatedFileName != String.Empty)
                {
                    var fullFilePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.BlocksFiles.ToString(), generatedFileName);
                    var BlockAttachementInsertModel = new BlockAttachementInsertModel()
                    {
                        BlockId = int.Parse(Id),
                        File = !String.IsNullOrEmpty(generatedFileName) ? fullFilePath : "",
                        Name = fileUploadModel.Name,
                    };
                    var content = HelperMethods.ToJson(BlockAttachementInsertModel);
                    HttpResponseMessage response;
                    response = await _httpClient.PostAsync(EndPoints.BlocksAttachement, content);
                    if (response.IsSuccessStatusCode)
                    {
                        _snackBar.Add("Completed Successful!", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Something went wrong!", Severity.Error);
                    }

                }
                else
                {
                    _snackBar.Add("Extension Not  Support!", Severity.Error);
                }
            }

        }

        public void Cancel()
        {
            //_navigationManager.NavigateTo("/blocks");
            _navigationManager.NavigateTo("/blocks/" + BlockModel.CategoryId);

        }


        private async void UploadFiles(InputFileChangeEventArgs e)
        {
            _blockAttachements.Clear();
            foreach (var file in e.GetMultipleFiles())
            {
                this._blockAttachements.Add(file);

                if (_blockAttachements.Count > 0)
                {
                    var blockAttachementUploadModel = await HelperMethods.Save(file);
                    blockAttachementUploadModelList.Add(blockAttachementUploadModel);
                    disableDeleteBlockAttachement = false;

                }


            }
            await SaveAsync();
            _blockAttachements.Clear();
            blockAttachementUploadModelList.Clear();
            await LoadBlock(Id);
            this.StateHasChanged();
            // _navigationManager.NavigateTo($"/block-Attachement-details/{Id}",true);
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

        private async void DeleteAllFiles()
        {
            string deleteContent = localizer["DeleteAllFiles"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["PCDelete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                _blockAttachements.Clear();
                disableDeleteBlockAttachement = true;
                foreach (var delblockAttachement in BlockAttachementUpdateModelList)
                {
                    var response = await _httpClient.DeleteAsync($"{EndPoints.BlocksAttachement}/{delblockAttachement.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        _snackBar.Add("Deleted Successful!", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Something went wrong!", Severity.Error);
                    }
                }

                blockAttachementUploadModelList.Clear();
                BlockAttachementUpdateModelList.Clear();

                this.StateHasChanged();
            }
        }
        private async void DeleteFile(BlockAttachementUpdateModel fileForPreview)
        {
            string deleteContent = localizer["DeleteFile"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["PCDelete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var delblockAttachement = BlockAttachementUpdateModelList.Find(x => x.Id == fileForPreview.Id);
                BlockAttachementUpdateModelList.Remove(delblockAttachement);
                var respnse = await _httpClient.DeleteAsync($"{EndPoints.BlocksAttachement}/{delblockAttachement.Id}");
                if (respnse.IsSuccessStatusCode)
                {
                    if (BlockAttachementUpdateModelList.Count == 0)
                    { disableDeleteBlockAttachement = true; }
                    _snackBar.Add("Deleted Successful!", Severity.Success);
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