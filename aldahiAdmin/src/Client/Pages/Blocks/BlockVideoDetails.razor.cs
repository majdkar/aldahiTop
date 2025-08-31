using FirstCall.Client.Helpers;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class BlockVideoDetails
    {
        [Parameter]
        public string? Id { get; set; }
        private BlockUpdateModel BlockModel { get; set; } = new();
        private IList<IBrowserFile> _blockVideos = new List<IBrowserFile>();
        public List<BlockVideoUpdateModel> BlockVideoUpdateModelList = new List<BlockVideoUpdateModel>();
        public BlockVideoInsertModel blockVideoInsertModel { get; set; } = new();

        private bool disableDeleteBlockVideo { get; set; } = true;

        private bool isProcessing = false;

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
                    BlockVideoUpdateModelList.Clear();
                    foreach (var selectedBlockVideo in BlockModel.BlockVideos)
                    {
                        BlockVideoUpdateModelList.Add(
                                                    new BlockVideoUpdateModel
                                                    {
                                                        Id = selectedBlockVideo.Id,
                                                        Url = selectedBlockVideo.Url,
                                                        BlockId = selectedBlockVideo.BlockId,

                                                    }
                            );
                        if (!String.IsNullOrEmpty(selectedBlockVideo.Url))
                        {

                            disableDeleteBlockVideo = false;
                        }


                    }

                    this.StateHasChanged();
                }
            }
        }
        private List<string> values = new List<string>();

        private void AddValue() => values.Add("");
        private void UpdateValue(int i, string value)
        {
            values[i] = value;

        }
        private async Task BlocksAsync()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }
        //private async Task RemoveValue(int i)
        //{
        //    values.RemoveAt(i);
        //    i = 2;
        //    bool error = false;
        //    var result = await _httpClient.DeleteAsync($"{EndPoints.BlocksVideo}/{i}");
        //    if (result.IsSuccessStatusCode)
        //    {
        //        _snackBar.Add("Complete Successful!", Severity.Success);
        //    }
        //    else
        //    {
        //        _snackBar.Add("Something went wrong!", Severity.Error);
        //        error = true;
        //    }
        //}
        private async Task HandleSubmit()
        {
            isProcessing = true;
            blockVideoInsertModel.BlockId = int.Parse(Id);

            var content = HelperMethods.ToJson(blockVideoInsertModel);
            HttpResponseMessage response;
            response = await _httpClient.PostAsync(EndPoints.BlocksVideo, content);
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            isProcessing = false;
            await LoadBlock(Id);
            this.StateHasChanged();
        }

        private async void DeleteFile(BlockVideoUpdateModel fileForPreview)
        {

            var delblockAttachement = BlockVideoUpdateModelList.Find(x => x.Id == fileForPreview.Id);
            BlockVideoUpdateModelList.Remove(delblockAttachement);
            var result = await _httpClient.DeleteAsync($"{EndPoints.BlocksVideo}/{delblockAttachement.Id}");
            if (result.IsSuccessStatusCode)
            {
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