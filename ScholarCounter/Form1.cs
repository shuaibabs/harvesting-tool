using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ScholarCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT *FROM `auth_account_platform`;";
                DataTable dataTable = new DataTable();
                MySqlDao mySqlDao = new MySqlDao();
                dataTable = mySqlDao.ExecuteSelectQuery(query);
                dataGrid.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            MySqlDao mySqlDao = new MySqlDao();
            string query = "INSERT INTO `processing_auth_account` " + 
                "(`configuration_name`, `sushi_service_url`, " + 
                "`customer_id`, `requester_id`, `platform`, " +
                "`api_key`, `remarks`) VALUES('" +
                configurationName.Text + "', '" + 
                sushiServiceUrl.Text + "', '" + 
                customerId.Text + "', '" + 
                RequestorId.Text + "', '" + 
                platform.Text + "', '" + 
                apiKey.Text + "', '" + 
                remarks.Text + "'); ";
            dataTable = mySqlDao.ExecuteInsertUpdateQuery(query);
            MessageBox.Show("Saved Data Into MySql");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 50 milliseconds.  
                //read.Sleep(50);
                System.Threading.Thread.Sleep(50);
                // Report progress.  
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        private void backgroundWorker1_ProgressChanged(object sender,
        ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar   
            progressBar1.Value = e.ProgressPercentage;
            // Set the text.  
            this.Text = e.ProgressPercentage.ToString();
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
