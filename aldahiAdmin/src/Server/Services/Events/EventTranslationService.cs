using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Events;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public class EventTranslationService : IEventTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public EventTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<EventTranslationViewModel>> GetTranslationByEventId(int eventId)
        {
            var translationEntities = await uow.Query<EventTranslation>().Where(x => x.EventId == eventId).ToListAsync();
            var translationsVM = mapper.Map<List<EventTranslation>, List<EventTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<EventTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<EventTranslation>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<EventTranslation, EventTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<EventTranslationViewModel> AddTranslation(EventTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<EventTranslationInsertModel, EventTranslation>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<EventTranslation, EventTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EventTranslationViewModel> UpdateTranslation(EventTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<EventTranslation>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.Name = translationUpdateModel.Name;
                    translationEntity.Place = translationUpdateModel.Place;
                    translationEntity.Description = translationUpdateModel.Description;
                    translationEntity.Slug = translationUpdateModel.Slug;
                    translationEntity.Language = translationUpdateModel.Language;
                    translationEntity.IsActive = translationUpdateModel.IsActive;
                    translationEntity.EventId = translationUpdateModel.EventId;

                    uow.Update(translationEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<EventTranslation, EventTranslationViewModel>(translationEntity);
                    return resultVM;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SoftDeleteTranslation(int translationId)
        {
            try
            {
                var translationEntity = uow.Query<EventTranslation>().Where(x => x.Id == translationId).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.IsActive = !translationEntity.IsActive;
                    uow.Update(translationEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SaveAsync()
        {
            await uow.CommitAsync();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
