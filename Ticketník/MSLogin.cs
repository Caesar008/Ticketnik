using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class MSLogin : Form
    {
        public MSLogin(HttpClient terpLoaderClient, Form1 form)
        {
            InitializeComponent();
            Login(terpLoaderClient);
        }

        private async void LoginV2(HttpClient terpLoaderClient)
        {
            HttpContent formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("loginfmt", "-----@-----"),
                new KeyValuePair<string, string>("passwd", "-----"),
                new KeyValuePair<string, string>("flowToken", "AQABAAEAAAD--DLA3VO7QrddgJg7WevrG1OYdmOJyOh4AStRMLdYry3rk7YN4X1jPt3q3kKkzpo28utOnuqmzEDNJ-GjagzonbwUeCp8jgjC6aX8MoH9T37NtOAgA-igsWcOb6Fy1SW6LpY6HJqYqrzOyQAKn-XNIrpGzZ552UZKzluoau7y2T3pAKfYOVwVpzqRhiJ-6A4P-LV48uO-V8K4wpNaqD6l8aPF--iTgJKKPH4JPM2hli8QAFPEcvvbwt35WFmlgeKOdJfjVCCBXNvSI1OJqFkuQ-TvO7g-hmbL8oYetOs3MSOPpA_Uilc8V_fpY09_hzv8mXhl_dmVOiGCc7xBY64545H3aD4YaNVoj1EmcorwHzJ1CE6eLGYfPf6LXonS7WWVFYD0zWy9-hKhwM1BWhcyLs1raj2Ul5GLRfI7L6JrIyLdSBfSbEWrSzlWOji-JH0zzAcF7DrdquqQyd3OCVsttKP_gk9Of2a8Zq0ta8NUQ6IlXb4ZG15pZndIoZBkDMvdnpE20Kr16Kkh6lRP5UIJyKLtzlEGCMQtU3fZv3fwFA2HWCQDZyjpmLQAoGt8YorboWzWp3uACIbQXWUbPL4oIAA")
                
                
            });

            string getstring = await terpLoaderClient.GetStringAsync("https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/authorize?response_type=code&client_id=c8d8a940-d031-4ec1-85f3-79180e45d53f&scope=openid&redirect_uri=https%3a%2f%2fmytime.tietoevry.com%2f&response_mode=form_post").ConfigureAwait(false);
            var post = await terpLoaderClient.PostAsync("https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/login", formContent).ConfigureAwait(false);
            string stringContent = await post.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async void Login(HttpClient terpLoaderClient)
        {
            //podle tohodle to zkusit https://joonasw.net/view/device-code-flow 
            //tohle taky čeknout https://learn.microsoft.com/en-us/azure/active-directory-b2c/openid-connect
            HttpContent formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("scope", "openid"),
                new KeyValuePair<string, string>("redirect_uri", "https%3a%2f%2fmytime.tietoevry.com%2f"),
                new KeyValuePair<string, string>("client_id", "b78e3b3a-b11c-44ad-9edc-b76f7ed1cd7c")
                
            });
            //https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/token
            //https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/v2.0/token
            //https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/devicecode
            var post = await terpLoaderClient.PostAsync("https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/v2.0/devicecode", formContent).ConfigureAwait(false);
            string stringContent = await post.Content.ReadAsStringAsync().ConfigureAwait(false);

            JsonTextReader reader = new JsonTextReader(new StringReader(stringContent));
            string expires_in = "", device_code = "", verification_url = "", user_code = "", interval = "";

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (expires_in == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "expires_in")
                    {
                        reader.Read();
                        expires_in = reader.Value.ToString();
                    }
                    else if (device_code == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "device_code")
                    {
                        reader.Read();
                        device_code = ((string)reader.Value);
                    }
                    else if (verification_url == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "verification_url")
                    {
                        reader.Read();
                        verification_url = ((string)reader.Value);
                    }
                    else if (user_code == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "user_code")
                    {
                        reader.Read();
                        user_code = ((string)reader.Value);
                    }
                    else if (interval == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "interval")
                    {
                        reader.Read();
                        interval = reader.Value.ToString();//((string)reader.Value);
                    }
                }
            }

            TimeSpan pollingInterval = TimeSpan.FromSeconds(int.Parse(interval));
            DateTimeOffset codeExpiresOn = DateTimeOffset.UtcNow.AddSeconds(int.Parse(expires_in));
            TimeSpan timeRemaining = codeExpiresOn - DateTimeOffset.UtcNow;
            //TokenResponse tokenResponse = null;

            while (timeRemaining.TotalSeconds > 0)
            {
                HttpContent postContent = new FormUrlEncodedContent(new[]
                {
                    //AQABAAEAAAD--DLA3VO7QrddgJg7WevrG1OYdmOJyOh4AStRMLdYry3rk7YN4X1jPt3q3kKkzpo28utOnuqmzEDNJ-GjagzonbwUeCp8jgjC6aX8MoH9T37NtOAgA-igsWcOb6Fy1SW6LpY6HJqYqrzOyQAKn-XNIrpGzZ552UZKzluoau7y2T3pAKfYOVwVpzqRhiJ-6A4P-LV48uO-V8K4wpNaqD6l8aPF--iTgJKKPH4JPM2hli8QAFPEcvvbwt35WFmlgeKOdJfjVCCBXNvSI1OJqFkuQ-TvO7g-hmbL8oYetOs3MSOPpA_Uilc8V_fpY09_hzv8mXhl_dmVOiGCc7xBY64545H3aD4YaNVoj1EmcorwHzJ1CE6eLGYfPf6LXonS7WWVFYD0zWy9-hKhwM1BWhcyLs1raj2Ul5GLRfI7L6JrIyLdSBfSbEWrSzlWOji-JH0zzAcF7DrdquqQyd3OCVsttKP_gk9Of2a8Zq0ta8NUQ6IlXb4ZG15pZndIoZBkDMvdnpE20Kr16Kkh6lRP5UIJyKLtzlEGCMQtU3fZv3fwFA2HWCQDZyjpmLQAoGt8YorboWzWp3uACIbQXWUbPL4oIAA
                    //b78e3b3a-b11c-44ad-9edc-b76f7ed1cd7c ticketník
                    //c8d8a940-d031-4ec1-85f3-79180e45d53f mytime

                    new KeyValuePair<string, string>("device_code", device_code),
                    new KeyValuePair<string, string>("client_id", "b78e3b3a-b11c-44ad-9edc-b76f7ed1cd7c"),
                    new KeyValuePair<string, string>("grant_type", "device_code")
                });

                post = await terpLoaderClient.PostAsync("https://login.microsoftonline.com/cbede638-a3d9-459f-8f4e-24ced73b4e5e/oauth2/v2.0/token", postContent).ConfigureAwait(false);
                stringContent = await post.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            this.BeginInvoke(new Action(() => richTextBox1.Text = stringContent + "\r\n\r\n" + user_code + "\r\n" + device_code));
        }

    }
}
