using DevExpress.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp4
{
    public partial class PersonelView : UserControl
    {
        public PersonelView()
        {
            InitializeComponent();
        }

        public void SetTextValues(UserData userData)
        {
            textBox1.Text = userData.id.ToString();
            textBox2.Text = userData.first_name.ToString();
            textBox3.Text = userData.last_name.ToString();
            textBox4.Text = userData.email.ToString();
            textBox5.Text = userData.gender.ToString();
            textBox6.Text = userData.ip_address.ToString();
        }

        public void SetImage(UserData userData)
        {
            var imageUrl = userData.image;

            using (WebClient webClient = new WebClient())
            {
                byte[] imageData = webClient.DownloadData(imageUrl);

                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    pictureBox1.Image = Image.FromStream(stream);
                }
            }
        }
        public void SetMap(UserData userData) //Bing Map Static API ile harita konum resmini işliyoruz.
        {
            double latitude = userData.latitude;
            double longitude =userData.longitude; 

            using (HttpClient client = new HttpClient()) { 
                var map2Url = string.Format(CultureInfo.InvariantCulture, "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{0},{1}/18?mapSize=300,300&pp={0},{1};66&mapLayer=Basemap,Buildings&key=wbh0nJRsK3LjsQm2Ymn3~XRnt4DLa4F2fNFTvQpq0kw~ApbSXWA8n9ScNqziMOyXxYUvHXjik-wBQw61VAmcQG0VSSIMgpKN3UWinILqXSDl", latitude,longitude);
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(map2Url);

                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        pictureBox2.Image = Image.FromStream(stream);
                    }
                }
            }
            
        }


        //Search işlemlerinde kullanmak için textbox geri dönüş değerleri.

        public System.Windows.Forms.TextBox TextBox1
        {
            get { return textBox1; }
        }
        public System.Windows.Forms.TextBox TextBox2 
        {
            get { return textBox2; }
        }
        public System.Windows.Forms.TextBox TextBox3 
        {
            get { return textBox3; }
        }
        public System.Windows.Forms.TextBox TextBox4 
        {
            get { return textBox4; }
        }
        public System.Windows.Forms.TextBox TextBox5
        {
            get { return textBox5; }
        }


    }
}
