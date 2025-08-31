using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Shared.Wrapper;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClientNameSpace = FirstCall.Domain.Entities.Clients;
using FirstCall.Application.Interfaces.Services.Identity;
using FirstCall.Application.Requests.Identity;
using FirstCall.Application.Interfaces.Services.Account;
using FirstCall.Application.Requests.Mail;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Http;
using System.Text;
using System.Text.Json;


namespace FirstCall.Application.Features.Clients.Persons.Commands.AddEdit
{
    public class AddEditPersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }



        public int CountryId { get; set; } = 0;

        public string CityName { get; set; }




        public string PersomImageUrl { get; set; }
        public UploadRequest PersomImageUploadRequest { get; set; }



        public string FullName { get; set; }


        public string Email { get; set; }
        public string Address { get; set; }


        public string AdditionalInfo { get; set; }

        public string UserId { get; set; }

        public string Phone { get; set; }


    }
    internal class AddEditPersonCommandHandler : IRequestHandler<AddEditPersonCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditPersonCommandHandler> _localizer;
        private readonly IUserService _userService;
        private readonly IAccountService _AccountService;
        private readonly MailSettings _mailSettings;
        private readonly HttpClient _httpClient;

        //private readonly string _apiKey = "c8ad5d4adbc2581dff0a8a63da9db18450fb8923"; // من CometChat Dashboard
        //private readonly string _appId = "2794344958d04ee3";


        public AddEditPersonCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditPersonCommandHandler> localizer, IUserService userService, IAccountService AccountService, IOptions<MailSettings> mailSettings, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _userService = userService;
            _AccountService = AccountService;
            _mailSettings = mailSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<Result<int>> Handle(AddEditPersonCommand command, CancellationToken cancellationToken)
        {
            var PersomImageUploadRequest = command.PersomImageUploadRequest;



            if (PersomImageUploadRequest != null)
            {
                PersomImageUploadRequest.FileName = $"{Path.GetRandomFileName()}{PersomImageUploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                try
                {

                    //create client
                    var client = new Client
                    {
                        Status = ClientStatusEnum.Accepted.ToString(),
                        Type = ClientTypesEnum.Person.ToString(),
                        UserId = command.UserId,

                    };

                    await _unitOfWork.Repository<ClientNameSpace.Client>().AddAsync(client);
                    await _unitOfWork.Commit(cancellationToken);


                    //create person
                    var person = _mapper.Map<Person>(command);
                    person.ClientId = client.Id;
                    person.CountryId = person.CountryId == 0 ? null : person.CountryId;

                    if (PersomImageUploadRequest != null)
                    {
                        person.PersomImageUrl = _uploadService.UploadAsync(PersomImageUploadRequest);
                    }




                    await _unitOfWork.Repository<Person>().AddAsync(person);
                    await _unitOfWork.Commit(cancellationToken);
                    if (person.Id > 0)
                    {
                        return await Result<int>.SuccessAsync(person.Id, _localizer["Person Saved"]);
                    }
                    else
                    {
                        ToggleUserStatusRequest toggleUserStatusRequest = new ToggleUserStatusRequest();
                        toggleUserStatusRequest.ActivateUser = false;
                        toggleUserStatusRequest.UserId = command.UserId;
                        await _userService.ToggleUserStatusAsync(toggleUserStatusRequest);
                        return await Result<int>.FailAsync("Person Not saved and user inactivated");
                    }

                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);

                }
            }
            else
            {
                var person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
                if (person != null)
                {
                    person.FullName = command.FullName ?? person.FullName;
                    person.CountryId = (command.CountryId == 0) ? person.CountryId : command.CountryId;


                    person.CityName = command.CityName ?? person.CityName;
                    person.Address = command.Address ?? person.Address;
                    person.AdditionalInfo = command.AdditionalInfo ?? person.AdditionalInfo;

                    person.Phone = command.Phone ?? person.Phone;



                    if (PersomImageUploadRequest != null)
                    {
                        person.PersomImageUrl = _uploadService.UploadAsync(PersomImageUploadRequest);

                    }
                        await _unitOfWork.Repository<Person>().UpdateAsync(person);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(person.Id, _localizer["Person Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
                    }
                }
            }

        }
    }
