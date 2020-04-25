using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                Authorization(textBox2.Text, textBox3.Text);
            }
            else
            {
                MessageBox.Show("Insert login/password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        static void Authorization(string login, string password)
        {
            if (File.Exists("response.txt"))
            {
                File.Delete("response.txt");
            }

            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                var values = new Dictionary<String, String>
                {
                    { "nick", login },
                    { "pass", password }
                };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync("https://www.bestseller.md/", content)
                   .GetAwaiter()
                   .GetResult();
                var responseString = response.Content.ReadAsStringAsync();
                Uri uri = new Uri("https://www.bestseller.md/");
                var c = cookieContainer.GetCookies(uri);

                for (int i = 0; i < c.Count; i++)
                {
                    File.AppendAllText("response.txt", c[i].Name, Encoding.UTF8);
                    File.AppendAllText("response.txt", "=", Encoding.UTF8);
                    File.AppendAllText("response.txt", c[i].Value + "\n", Encoding.UTF8);


                }
                if (File.Exists("response.txt"))
                    Process.Start("response.txt");
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            var cookieContainer = new CookieContainer();
            if (File.Exists("response.txt"))
            {
                string[] cookies = File.ReadAllLines(@"response.txt", System.Text.Encoding.Default);

                foreach (string cookie1 in cookies)
                    cookieContainer.SetCookies(new Uri("https://www.bestseller.md/"), cookie1);

            }
            Uri uri = new Uri("https://www.bestseller.md/");
            var values1 = new Dictionary<String, String>
            {

            };
            var content1 = new FormUrlEncodedContent(values1);
            using (var handler1 = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client1 = new HttpClient(handler1))
            {

                using (HttpResponseMessage response = await client1.GetAsync(uri))
                {

                    var s = response.Headers.ToString();
                    richTextBox1.Text = s;
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();

                        webBrowser1.DocumentText = mycontent;
                    }

                }

            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var cookieContainer = new CookieContainer();
            if (File.Exists("response.txt"))
            {
                string[] cookies = File.ReadAllLines(@"response.txt", System.Text.Encoding.Default);

                foreach (string cookie1 in cookies)
                    cookieContainer.SetCookies(new Uri("https://www.bestseller.md/"), cookie1);

            }
            Uri uri = new Uri("https://www.bestseller.md/");
            var values1 = new Dictionary<String, String>
            {
                { "usearch", textBox4.Text.ToString()},
                { "serch", "2" }

            };
            var content1 = new FormUrlEncodedContent(values1);
            using (var handler1 = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client1 = new HttpClient(handler1))
            {

                using (HttpResponseMessage response = await client1.PostAsync(uri, content1))
                {

                    var s = response.Headers.ToString();
                    richTextBox1.Text = s;
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();

                        webBrowser1.DocumentText = mycontent;
                    }

                }

            }

        }



    private async void button4_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("https://developer.mozilla.org");
                using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, url)))
                {
                    richTextBox1.Text = response.ToString();
                }


            }

        }

        private async void  button3_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("https://www.bestseller.md/");
                using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)))
                {
                    richTextBox1.Text = response.ToString();
                }


            }


        }
    }
}
