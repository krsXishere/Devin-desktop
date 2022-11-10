using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Devin.Properties;
using Microsoft.Win32;
using Devin.Models;
using System.Net.NetworkInformation;

namespace Devin
{
    public partial class FormAboutPC : Form
    {
        Engine engine = new Engine();
        private static readonly string startUpKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string startUpValue = "Devin";
        Dictionary<string, string> status = new Dictionary<string, string>();
        public FormAboutPC()
        {
            InitializeComponent();
        }

        private static void SetStartUp()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(startUpKey, true);
            key.SetValue(startUpValue, Application.ExecutablePath.ToString());
        }

        private async void FormAboutPC_Load(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                Hide();
            }));

            RegistryKey registryKey = Registry.CurrentUser;
            RegistryKey subKey = registryKey.OpenSubKey(startUpKey);
            var value = subKey.GetValue(startUpValue);

            if(value == null)
            {
                status.Add("isSaved", "no");
                SetStartUp();
            } else
            {
                status.Add("isSaved", "yes");
            }

            ulong mem = engine.GetPhysicalMemoryCapacity();
            ulong disk = engine.GetLogicalDiskCapacity() + engine.GetDiskDriveCapacity();
            label2.Text = Convert.ToString(engine.GetData("Processor", "Name"));
            label9.Text = $"{mem / Math.Pow(1024, 3):N2} GB";
            label3.Text = $"{disk / Math.Pow(1024, 3):N2} GB";
            label5.Text = Convert.ToString(engine.GetData("OperatingSystem", "Caption"));
            String labAndMeja = System.Environment.MachineName;
            String labSubed = labAndMeja.Substring(0, labAndMeja.LastIndexOf("-") + 1);
            String labRemoved = labSubed.Remove(5, 3);
            String mejaRemoved = labAndMeja.Remove(0, 6);
            String mejaSubbed = mejaRemoved.Substring(0, mejaRemoved.LastIndexOf("-"));

            try
            {
                label10.Text = Convert.ToString(engine.GetData("VideoController", "Name"));
            } catch
            {
                label10.Text = "Kartu Grafik tidak ditemukan.";
            }

            if (CheckInternetConnection() == true)
            {
                if (status["isSaved"] == "no")
                {
                    await SpesificationModel.PostDataSpesification(System.Environment.MachineName, label5.Text, label2.Text, label9.Text, label10.Text, label3.Text, labRemoved, mejaSubbed);

                    if (SpesificationModel.isNull == false)
                    {
                        MessageBox.Show("Devin setup completed successfully.", "Devin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        await SpesificationModel.PostDataSpesification(System.Environment.MachineName, label5.Text, label2.Text, label9.Text, label10.Text, label3.Text, labRemoved, mejaSubbed);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please connect to the internet then restart your computer.", "Devin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckInternetConnection()
        {
            try
            {
                Ping ping = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 5000;
                PingOptions pingOptions = new PingOptions();
                PingReply pingReply = ping.Send(host, timeout, buffer, pingOptions);
                return (pingReply.Status == IPStatus.Success);
            } catch
            {
                return false;
            }
        }
    }
}
