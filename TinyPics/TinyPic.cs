using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TinyPic
{
    class TinyPicApi
    {

        private WebClient _client;

        private string _key = "";

        private string _filename = "tiny-output.png";

        private string _upload_url = "https://api.tinypng.com/shrink";

        private string _savedir;
        private string p;
        private string p_2;

        public TinyPicApi(string key, string savedir)
        {
            this._key = key;
            this._savedir = savedir;
        }

        public void Upload(string path)
        {
            this._filename = Path.GetFileNameWithoutExtension(path) + "-compressed." + Path.GetExtension(path);

            this._client = new WebClient();
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + this._key));
            this._client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + auth);

            try
            {
                Console.WriteLine("Uploading:" + path);
                this._client.UploadData(this._upload_url, File.ReadAllBytes(path));
                /* Compression was successful, retrieve output from Location header. */
                this._client.DownloadFile(this._client.ResponseHeaders["Location"], this._savedir + "/" + this._filename);
                Console.WriteLine("Saving:" + this._savedir + Path.PathSeparator + this._filename);
            }
            catch (WebException)
            {
                /* Something went wrong! You can parse the JSON body for details. */
                Console.WriteLine("Compression failed.");
            }
        }
    }
}
