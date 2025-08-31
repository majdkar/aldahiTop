using FirstCall.Application.Requests;

namespace FirstCall.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}