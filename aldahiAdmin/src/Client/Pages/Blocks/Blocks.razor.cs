using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Shared.Wrapper;
using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class Blocks
    {
        //public string categoryId = "0"; // This is a queryString parameter

        [Parameter] public int CategoryId { get; set; } = 0; // This is a queryString parameter
        [Parameter] public BlockCategoryViewModel _blockCategory { get; set; } = new();

        private IEnumerable<BlockViewModel> elements = new List<BlockViewModel>();
        private IEnumerable<BlockCategoryViewModel> categories = new List<BlockCategoryViewModel>();
        private MudTable<BlockViewModel> _table;
        private int totalItems;
        private string searchString = "";
        private bool loaded;
        private int clickedRowId = 0;
        private int selectedRowForTranslation = 0;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateBlocks;
        private bool _canEditBlocks;
        private bool _canDeleteBlocks;
        private bool _canSearchBlocks;
        private bool _canViewBlocks;
        private IEnumerable<BlockCategoryViewModel> categoryelements;
        private bool showAlbum { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {


            await Task.WhenAll(LoadCategories());
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBlocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Blocks.Create)).Succeeded;
            _canEditBlocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Blocks.Edit)).Succeeded;
            _canDeleteBlocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Blocks.Delete)).Succeeded;
            _canSearchBlocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Blocks.Search)).Succeeded;
            _canViewBlocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Blocks.View)).Succeeded;

            loaded = true;


        }
        private async Task LoadCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BlockCategoryViewModel>>(EndPoints.BlockCategoriesSelect);
            if (response != null)
            {
                categories = response;
                _blockCategory = response.Where(e => e.Id.Equals(CategoryId)).FirstOrDefault();
                StateHasChanged();

            }
            else
            {
                _snackBar.Add("Error retrieving data");
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            await Task.WhenAll(LoadCategories());
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }

        private async Task<TableData<BlockViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            if (_canViewBlocks)
            {
                await LoadData(state, state.Page, state.PageSize);

            }
            return new TableData<BlockViewModel>() { Items = elements, TotalItems = totalItems };
        }

        private async Task LoadData(TableState state, int pageNumber, int pageSize)
        {
            string[] orderings = null;
            state.SortLabel = "RecordOrder";
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAllPaged(EndPoints.Blocks, pageNumber, pageSize, searchString, orderings) + $"&categoryId={CategoryId}";

            var response = await _httpClient.GetFromJsonAsync<PagedResponse<BlockViewModel>>(requestUri);
            if (response != null)
            {
                totalItems = response.TotalRecords;
                
                elements = response.Items;
                if (elements.Count() > 0)
                { 
                    if (elements.FirstOrDefault(x => x.Id == selectedRowForTranslation) != null)
                    elements.FirstOrDefault(x => x.Id == selectedRowForTranslation).ShowTranslation = true;
                }

                //var requestUricategory = EndPoints.GetAllPaged(EndPoints.BlockCategories, pageNumber, pageSize, searchString, orderings);
                var requestUricategory = EndPoints.GetAllPagedByCategoryID(EndPoints.BlockCategories, pageNumber, pageSize, searchString, orderings, CategoryId);
                var responsecate = await _httpClient.GetFromJsonAsync<PagedResponse<BlockCategoryViewModel>>(requestUricategory);
                if (responsecate != null)
                {
                    categoryelements = responsecate.Items;
                    if (categoryelements.Count() > 0)
                    {
                        var selectedBlockCategory = categoryelements.FirstOrDefault(c => c.Id == CategoryId);
                        if (selectedBlockCategory != null)
                        {
                            if (selectedBlockCategory.BlockType == "Photo Gallery" || selectedBlockCategory.BlockType == "Video Gallery")
                                showAlbum = true;
                            else
                                showAlbum = false;
                        }
                    }
                }
            }
            else
            {
                _snackBar.Add("Error retrieving data");
            }
        }

        private void OnSearch(string text)
        {
            if (_canSearchBlocks)
            {
                searchString = text;
                _table.ReloadServerData();
            }
        }



        private void InvokeAddEditBlock(int id = 0)
        {
            if ((id == 0 && _canCreateBlocks) || (id != 0 && _canEditBlocks))
            {
                _navigationManager.NavigateTo($"/block-details/{id}?categoryId={CategoryId}");
            }
        }

        private void InvokeAddEditBlockPhotos(int id = 0)
        {
            if ((id == 0 && _canCreateBlocks) || (id != 0 && _canEditBlocks))
            {
                _navigationManager.NavigateTo($"/block-photo-details/{id}?categoryId={CategoryId}");
            }
        }

        private void InvokeAddEditBlockAttachements(int id = 0)
        {
            if ((id == 0 && _canCreateBlocks) || (id != 0 && _canEditBlocks))
            {
                _navigationManager.NavigateTo($"/block-attachement-details/{id}");
            }
        }
        private void InvokeAddEditBlockVideos(int id = 0)
        {
            if ((id == 0 && _canCreateBlocks) || (id != 0 && _canEditBlocks))
            {
                _navigationManager.NavigateTo($"/block-video-details/{id}");
            }
        }
        private async Task InvokeTranslationModal(int blockId, int transltionId = 0)
        {
            selectedRowForTranslation = blockId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == blockId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditBlockTranslationModal.BlockTranslationModel), new BlockTranslationUpdateModel
                    {
                        Id = selectedTranslation.Id,
                        Name = selectedTranslation.Name,
                        Description = selectedTranslation.Description,
                        Language = selectedTranslation.Language,
                        Slug = selectedTranslation.Slug,
                        BlockId = selectedTranslation.BlockId,
                        IsActive = selectedTranslation.IsActive
                    });
                }
            }
            else//add operation
            {
                var selectedTranslation = new BlockTranslationUpdateModel();
                parameters.Add(nameof(AddEditBlockTranslationModal.BlockTranslationModel), new BlockTranslationUpdateModel
                {
                    BlockId = blockId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBlockTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
                loaded = false;
            }
        }

        protected async Task SoftDeleteBlock(int id)
        {
            string deleteContent = localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response  = await _httpClient.DeleteAsync($"{EndPoints.Blocks}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Complete Successful!", Severity.Success);


                }
                else
                {
                    _snackBar.Add("Something went wrong!", Severity.Error);
                }
            }
            await _table.ReloadServerData();



          
        }

        public void PageChanged()
        {
            _table.ReloadServerData();
        }

        private void RowClickEvent(TableRowClickEventArgs<BlockViewModel> e)
        {
            if (!e.Item.ShowTranslation)
            {
                if (clickedRowId != 0)
                    elements.FirstOrDefault(x => x.Id == clickedRowId).ShowTranslation = false;
                e.Item.ShowTranslation = true;
                clickedRowId = e.Item.Id;
            }
            else
            {
                e.Item.ShowTranslation = false;
            }
            loaded = false;
        }
    }

}
