using DevExpress.Internal;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            JsConvertAndLoad();
            flowLayoutPanel1.Controls.Clear();

        }
   
        int startingIndex = 0;
        int groupSize = 10;
        private async void JsConvertAndLoad()  
        {
            using (StreamReader file = File.OpenText("MOCK_DATAm.json"))  //İşimize yarayacak kadar bir veri seti ile çalışıyoruz lazy loading ile
            using (JsonTextReader reader = new JsonTextReader(file))      //verileri bellek maliyeti minimum ve hızlı bir şekilde işliyoruz.
            {
                int userCount = 0;

                JsonSerializer serializer = new JsonSerializer();
                while (reader.Read())
                {
            
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        UserData user = serializer.Deserialize<UserData>(reader);

                        if (userCount >= startingIndex && userCount < startingIndex + groupSize) //Grup boyutuna verdiğimiz değer kadar 
                        {                                                                        //döngünün devam etmesini sağlıyoruz.
                            await LoadUser(user);
                        }

                        userCount++;

                        if (userCount >= startingIndex + groupSize)
                        {
                            startingIndex += groupSize; // Son kullanıcımızın takibini yapıyoruz.
                            break;
                        }
                    }
                    
                }
            }
        }

        private async Task LoadUser(UserData userData)
        {
            PersonelView personelView = new PersonelView();
            await Task.Run(() =>
            {
                personelView.SetTextValues(userData);
                personelView.SetImage(userData);
                personelView.SetMap(userData);
            });

            flowLayoutPanel1.Invoke(new Action(() =>
            {
                flowLayoutPanel1.Controls.Add(personelView);
            }));
        }

        private bool isProcessingScroll = false;
        private async void flowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.Height >= flowLayoutPanel1.VerticalScroll.Maximum)
            {
                if (isProcessingScroll) return; // Kaydırma işlemi varken tetiklemeyi engelledik.

                if (flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.Height >= flowLayoutPanel1.VerticalScroll.Maximum)
                {
                    isProcessingScroll = true;  // Program thread yapıları ile çalıştığı için tetiklemeyi süre kısıtlı
                    await Task.Delay(800);     // ve kontrol altında tutmamız gerekiyor askı halde bir kullanıcı birden fazla sefer yüklenebiliyor.
                    JsConvertAndLoad();
                    isProcessingScroll = false; 
                }
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string searchText = textEdit1.Text.ToLower();

            foreach(Control control in flowLayoutPanel1.Controls)  //bütün değerler ile arama yapabiliyoruz.
            {
                if (control is PersonelView personelView)
                {
                    var id = personelView.TextBox1.Text.ToString().Contains(searchText);
                    var firstName = personelView.TextBox2.Text.ToLower().Contains(searchText);
                    var lastName = personelView.TextBox3.Text.ToLower().Contains(searchText);
                    var email= personelView.TextBox4.Text.ToLower().Contains(searchText);
                    var gender= personelView.TextBox5.Text.ToLower().Contains(searchText);

                    if (firstName || lastName || email || gender)
                    {
                        personelView.Visible = true;
                    }
                    else
                    {
                        personelView.Visible = false;
                    }

                }
            }
        }
    }
}
