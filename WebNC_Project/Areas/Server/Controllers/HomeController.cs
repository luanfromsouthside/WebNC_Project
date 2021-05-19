using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class HomeController : Controller
    {
        private static string Bucket = "imageresort-e3879.appspot.com";
        private static string ApiKey = "AIzaSyAP7K_WNBLe_8vLfW0_gIpazjc5VNRDQjg";
        private static string email = "resortmanagement@gmail.com";
        private static string pass = "resortmanagementproject";
        // GET: Server/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(HttpPostedFileBase editor1)
        {
            FileStream stream;
            if (editor1.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Content/img/"), editor1.FileName);
                editor1.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                return Content(await Upload(stream, editor1.FileName, path));
            }
            return Content("Nope");
        }

        public async Task<string> Upload(FileStream stream, string fileName, string path)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, pass);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }).Child("testimgs").Child(fileName).PutAsync(stream, cancellation.Token);
            try
            {
                if(await task != null)
                {
                    System.IO.File.Delete(path);
                    return task.TargetUrl;
                }
                return null;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}