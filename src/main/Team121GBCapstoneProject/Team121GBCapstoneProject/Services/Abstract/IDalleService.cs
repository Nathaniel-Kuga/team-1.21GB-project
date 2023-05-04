namespace Team121GBCapstoneProject.Services.Abstract
{
    public interface IDalleService
    {
        public Task<string> GetImages(string prompt);
        //public byte[] SetImageToProfilePicure(byte[] imageArray);
        public Task<byte[]> TurnImageUrlIntoByteArray(string imageURL);
    }
}
