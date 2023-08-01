
namespace Team121GBCapstoneProject.Services.Abstract;

public interface IWhisperService
{
    ///<summary>
    /// This method takes in a byte array of an mp3 file and returns a string of the text that was spoken in the mp3 file.
    ///</summary>
    ///<param name="file">The IFormFile containing the mp3 file</param>
    public Task<string> GetTextFromSpeech(IFormFile file);  

}