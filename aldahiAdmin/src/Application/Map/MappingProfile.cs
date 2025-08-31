using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities;
//using FirstCall.Shared.ViewModels.Articles;
using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Shared.ViewModels.Events;
using FirstCall.Shared.ViewModels.Menus;
using FirstCall.Shared.ViewModels.Pages;
using FirstCall.Shared.ViewModels.Settings.Languages;

namespace FirstCall.Application.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Blocks
            CreateMap<BlockCategory, BlockCategoryViewModel>();
            CreateMap<BlockCategoryInsertModel, BlockCategory>();
            CreateMap<BlockCategoryUpdateModel, BlockCategory>();

           



            CreateMap<BlockCategoryTranslation, BlockCategoryTranslationViewModel>();
            CreateMap<BlockCategoryTranslationInsertModel, BlockCategoryTranslation>();
            CreateMap<BlockCategoryTranslationUpdateModel, BlockCategoryTranslation>();



          



            CreateMap<Block, BlockViewModel>();
            // .ForMember(dest => dest.FileUrl, opt => opt.MapFrom<CustomBlockFileResolver>());
            CreateMap<BlockInsertModel, Block>();
            CreateMap<BlockUpdateModel, Block>();

           

            CreateMap<BlockTranslation, BlockTranslationViewModel>();
            CreateMap<BlockTranslationInsertModel, BlockTranslation>();
            CreateMap<BlockTranslationUpdateModel, BlockTranslation>();


           

            CreateMap<BlockPhoto, BlockPhotoViewModel>();
            CreateMap<BlockPhotoInsertModel, BlockPhoto>();
            CreateMap<BlockPhotoUpdateModel, BlockPhoto>();

            CreateMap<BlockVideo, BlockVideoViewModel>();
            CreateMap<BlockVideoInsertModel, BlockVideo>();
            CreateMap<BlockVideoUpdateModel, BlockVideo>();

            

            CreateMap<BlockAttachement, BlockAttachementViewModel>();
            CreateMap<BlockAttachementInsertModel, BlockAttachement>();
            CreateMap<BlockAttachementUpdateModel, BlockAttachement>();

            
            //Menus
            CreateMap<MenuCategory, MenuCategoryViewModel>();
            CreateMap<MenuCategoryInsertModel, MenuCategory>();
            CreateMap<MenuCategoryUpdateModel, MenuCategory>();

            CreateMap<MenuCategoryTranslation, MenuCategoryTranslationViewModel>();
            CreateMap<MenuCategoryTranslationInsertModel, MenuCategoryTranslation>();
            CreateMap<MenuCategoryTranslationUpdateModel, MenuCategoryTranslation>();

            CreateMap<Menu, MenuViewModel>();
            CreateMap<MenuInsertModel, Menu>();
            CreateMap<MenuUpdateModel, Menu>();

            CreateMap<MenueTranslate, MenuTranslationViewModel>();
            CreateMap<MenuTranslationInsertModel, MenueTranslate>();
            CreateMap<MenuTranslationUpdateModel, MenueTranslate>();

            //Pages
            CreateMap<Page, PageViewModel>();
            CreateMap<PageInsertModel, Page>();
            CreateMap<PageUpdateModel, Page>();

            CreateMap<PageTranslation, PageTranslationViewModel>();
            CreateMap<PageTranslationInsertModel, PageTranslation>();
            CreateMap<PageTranslationUpdateModel, PageTranslation>();

            CreateMap<PagePhoto, PagePhotoViewModel>();
            CreateMap<PagePhotoInsertModel, PagePhoto>();
            CreateMap<PagePhotoUpdateModel, PagePhoto>();
            CreateMap<PageAttachement, PageAttachementViewModel>();
            CreateMap<PageAttachementInsertModel, PageAttachement>();
            CreateMap<PageAttachementUpdateModel, PageAttachement>();

            //Events
            CreateMap<EventCategory, EventCategoryViewModel>();
            CreateMap<EventCategoryInsertModel, EventCategory>();
            CreateMap<EventCategoryUpdateModel, EventCategory>();

            CreateMap<EventCategoryTranslation, EventCategoryTranslationViewModel>();
            CreateMap<EventCategoryTranslationInsertModel, EventCategoryTranslation>();
            CreateMap<EventCategoryTranslationUpdateModel, EventCategoryTranslation>();

            CreateMap<Event, EventViewModel>();
            CreateMap<EventInsertModel, Event>();
            CreateMap<EventUpdateModel, Event>();

            CreateMap<EventTranslation, EventTranslationViewModel>();
            CreateMap<EventTranslationInsertModel, EventTranslation>();
            CreateMap<EventTranslationUpdateModel, EventTranslation>();

            CreateMap<EventPhoto, EventPhotoViewModel>();
            CreateMap<EventPhotoInsertModel, EventPhoto>();
            CreateMap<EventPhotoUpdateModel, EventPhoto>();

            CreateMap<EventAttachement, EventAttachementViewModel>();
            CreateMap<EventAttachementInsertModel, EventAttachement>();
            CreateMap<EventAttachementUpdateModel, EventAttachement>();

           

            //General Settings
            //Language
            CreateMap<Language, LanguageViewModel>();
            CreateMap<LanguageInsertModel, Language>();
            CreateMap<LanguageUpdateModel, Language>();

            

        }

    }


    //public class CustomBlockImageResolver : IValueResolver<Block, BlockViewModel, string>
    //{
    //    public string Resolve(Block source, BlockViewModel destination, string member, ResolutionContext context)
    //    {
    //        return  source.Url + Path.DirectorySeparatorChar.ToString() + source.Image;
    //    }
    //}

    //public class CustomBlockFileResolver : IValueResolver<Block, BlockViewModel, string>
    //{
    //    public string Resolve(Block source, BlockViewModel destination, string member, ResolutionContext context)
    //    {
    //        return   source.Url + Path.DirectorySeparatorChar.ToString() + source.File;
    //    }
    //}

}


