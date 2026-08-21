namespace InventoryManager.WebApi.Mvc.Mappers
{
    using AutoMapper;
    using InventoryManager.WebApi.Business.Models;
    using InventoryManager.WebApi.Mvc.Models;

    internal class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<AddAsyncRequest.Item, Item>()
                .ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<AddAsyncRequest, Item>()
                .IncludeMembers(s => s.Data)
                .ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<Item, AddAsyncResponse.Item>();
            CreateMap<Item, AddAsyncResponse>()
                .ForPath(d => d.Data, opt => opt.MapFrom(s => s));

            CreateMap<Item, GetAsyncResponse.Item>();
            CreateMap<Item, GetAsyncResponse>()
                .ForPath(d => d.Data, opt => opt.MapFrom(s => s));

            CreateMap<Item, DeleteAsyncResponse.Item>();
            CreateMap<Item, DeleteAsyncResponse>()
                .ForPath(d => d.Data, opt => opt.MapFrom(s => s));
        }
    }
}
