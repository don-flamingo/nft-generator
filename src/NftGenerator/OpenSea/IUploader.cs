using System.Threading;
using System.Threading.Tasks;
using NftGenerator.Models;

namespace NftGenerator.OpenSea
{
    public interface IUploader
    {
        Task Upload(Nft nft, CancellationToken cancellationToken);
    }
}
