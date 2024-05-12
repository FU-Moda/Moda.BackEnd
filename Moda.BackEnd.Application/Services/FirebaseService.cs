using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Response;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.Services
{
    public class FirebaseService : GenericBackendService, IFirebaseService
    {
        private readonly IConfiguration _configuration;
        public FirebaseService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
        {
              _configuration = configuration;
        }

        public async Task<AppActionResult> DeleteImageFromFirebase(string pathFileName)
        {
            var result = new AppActionResult();
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));

                var account = await auth.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"],
                    _configuration["Firebase:AuthPassword"]);
                var cancellation = new CancellationTokenSource();
                var storage = new FirebaseStorage(
              _configuration["Firebase:Bucket"],
              new FirebaseStorageOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                  ThrowOnCancel = true
              });
                await storage
                    .Child(pathFileName)
                    .DeleteAsync();
                result.Messages.Add("Delete image successful");
            }
            catch (FirebaseStorageException ex)
            {
                result.Messages.Add($"Error deleting image: {ex.Message}");
            }
            return result;
        }

        public async Task<string> GetUrlImageFromFirebase(string pathFileName)
        {
            var a = pathFileName.Split("/");
            pathFileName = $"{a[0]}%2F{a[1]}";
            var api = $"https://firebasestorage.googleapis.com/v0/b/yogacenter-44949.appspot.com/o?name={pathFileName}";
            if (string.IsNullOrEmpty(pathFileName))
            {
                return string.Empty;
            }

            var client = new RestClient();
            var request = new RestRequest(api);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jmessage = JObject.Parse(response.Content);
                var downloadToken = jmessage.GetValue("downloadTokens").ToString();
                return
                    $"https://firebasestorage.googleapis.com/v0/b/{_configuration["Firebase:Bucket"]}/o/{pathFileName}?alt=media&token={downloadToken}";
            }

            return string.Empty;
        }

        public async Task<AppActionResult> UploadImageToFirebase(IFormFile file, string pathFileName)
        {
            var result = new AppActionResult();
            try
            {
                var isValid = true;
                if (file == null || file.Length == 0)
                {
                    isValid = false;
                    result.Messages.Add("The file is empty");
                }
                if (isValid)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file!.CopyToAsync(memoryStream);

                        var stream = new MemoryStream(memoryStream.ToArray());
                        var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));

                        var account = await auth.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"],
                           _configuration["Firebase:AuthPassword"]);
                        var cancellation = new CancellationTokenSource();

                        var destinationPath = $"{pathFileName}";

                        var task = new FirebaseStorage(
                             _configuration["Firebase:Bucket"],
                             new FirebaseStorageOptions
                             {
                                 AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                                 ThrowOnCancel = true
                             })
                         .Child(destinationPath)
                         .PutAsync(stream, cancellation.Token);
                        if (task != null)
                        {
                            result.Result = await GetUrlImageFromFirebase(pathFileName);
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Messages.Add("Upload failed");
                        }
                    }
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
