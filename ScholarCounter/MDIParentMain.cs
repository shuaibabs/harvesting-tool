using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System.Security.Policy;
using System.Threading;

namespace ScholarCounter
{
    public partial class MDIParentMain : Form
    {
        private int childFormNumber = 0;
        // Add A Header CheckBox
        CheckBox HeaderCheckBox = null;
        bool IsHeaderCheckBoxClicked = false;

        public MDIParentMain()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        private void onLoad()
        {
            MySqlDao mySqlDao = new MySqlDao();
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT " +
                            "'' AS `S.No`," +
                            "Account_id AS `AccountID`," +
                            "platform_code AS `Platform`," +
                            "platform_name AS `Platform Name`," +
                            "customer_id AS `Customer ID`," +
                            "customer_type AS `Account Type`," +
                            "api_key," +
                            "sushi_url," +
                            "requestor_id," +
                            "`account_code`," +
                            "`platform_id`" +
                "FROM" +
                            "`auth_account`" +
                "INNER JOIN" +
                            "`auth_account_platform` USING(`account_id`);";

                dt = mySqlDao.ExecuteSelectQuery(query);
                dgvAccountPlatform.Rows.Clear();
                dgvAccountPlatform.Columns.Clear();
                dgvAccountPlatform.DataSource = dt;
                //Add a CheckBox Column to the DataGridView at the first position.
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.Width = 50;
                checkBoxColumn.Name = "checkBoxColumn";
                dgvAccountPlatform.Columns.Insert(0, checkBoxColumn);
            }
            catch (Exception ex)
            {
                MessageBox.Show("OnLoad: " + ex.Message);
            }
        }
        private void MDIParentMain_Load(object sender, EventArgs e)
        {
            onLoad();
            AddHeaderCheckBox();
            DataFolderPath.Text = "E:\\SC-Dummy-Data";
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
        }
        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();
            HeaderCheckBox.Size = new Size(15, 15);
            try
            {
                // Add The CheckBox Into The DataGridView
                dgvAccountPlatform.Controls.Add(HeaderCheckBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddHeaderCheckBox: " + ex.Message);
            }
        }
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;
            try
            {
                foreach (DataGridViewRow Row in dgvAccountPlatform.Rows)
                    ((DataGridViewCheckBoxCell)Row.Cells["checkBoxColumn"]).Value = HCheckBox.Checked;

                dgvAccountPlatform.RefreshEdit();
                IsHeaderCheckBoxClicked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HeaderCheckBoxClick: " + ex.Message);
            }
        }
        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                HeaderCheckBoxClick((CheckBox)sender);
            }
            catch (Exception ex)
            {
                MessageBox.Show("HeaderCheckBox_MouseClick: " + ex.Message);
            }
        }
        private async void download_Click(object sender, EventArgs e)
        {
            HTTPService HTTP = new HTTPService();
            DataTable dt = new DataTable();
            WebClient wc = new WebClient();
            MySqlDao MySql = new MySqlDao();
            GTUtility utility = new GTUtility();

            string platform = "";
            string customerID = "";
            string apiKey = "";
            string sushiURl = "";
            string requestorID = "";
            string accountCode = "";
            string platformID = "";
            string accountID = "";
            string BDate = "";
            string EDate = "";
            string reportUrl = "";
            string FilePath = "";
            string BasePath = "";
            string FileName = "";
            string options = "";
            string query = "";
            string status = "";
            string start_DT = "";
            string end_DT = "";
            string processing_log = "";
            string database = "";
            string table = "";
            int state = 0;
            int records_processed = 0;
            int records_affected = 0;
            int records_error = 0;
            int records_total = 0;
            int records_time = 0;
            int successCount = 0;
            int errorCount = 0;
            int processedCount = 0;
            int overriteCount = 0;
            long FileSizeInKB = 0;
            long FileSize = 0;
            try
            {
                // First And Last Day Date of the Selected Date
                DateTime bd = beginDate.Value;
                var bdDay = new DateTime(bd.Year, bd.Month, 1);
                DateTime ed = endDate.Value;
                var edDay = new DateTime(ed.Year, ed.Month, 1);
                var lastDayOfMonth = edDay.AddMonths(1).AddDays(-1);
                BDate = bdDay.ToString("yyyy-MM-dd");
                EDate = lastDayOfMonth.ToString("yyyy-MM-dd");
                // Base Path
                BasePath = DataFolderPath.Text;
                successCount = 0;
                errorCount = 0;
                processedCount = 0;
                overriteCount = 0;
                // Details will Show in Message
                for (int i = 0; i <= dgvAccountPlatform.RowCount -1; i++)
                {
                    if (Convert.ToBoolean(dgvAccountPlatform.Rows[i].Cells["checkBoxColumn"].Value) == true)
                    {
                        processing_log = "Started"; 
                        // Initializing Some Variables
                        accountID    = dgvAccountPlatform.Rows[i].Cells[2].Value.ToString();
                        customerID   = dgvAccountPlatform.Rows[i].Cells[5].Value.ToString();
                        platform     = dgvAccountPlatform.Rows[i].Cells[3].Value.ToString();
                        sushiURl     = dgvAccountPlatform.Rows[i].Cells[8].Value.ToString();
                        apiKey       = dgvAccountPlatform.Rows[i].Cells[7].Value.ToString();
                        requestorID  = dgvAccountPlatform.Rows[i].Cells[9].Value.ToString();
                        accountCode  = dgvAccountPlatform.Rows[i].Cells[10].Value.ToString();
                        platformID   = dgvAccountPlatform.Rows[i].Cells[11].Value.ToString();
                        // fill reports aaray
                        String[] reports = {"tr", "dr", "ir", "pr"};
                        for (int reportNo = 0; reportNo < reports.Length; reportNo++) 
                        {
                            start_DT = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                            var LetStartDate = DateTime.Now;
                            FilePath = BasePath + "\\" +
                                       "Data" + "\\" +
                                       accountCode + "\\" +
                                       accountCode + "_" + platformID + "\\" +
                                       accountCode + "_" + platformID + "_" + EDate.Substring(0,4)  + "\\" +
                                       accountCode + "_" + platformID + "_" + EDate.Substring(0,4) + "_" + EDate.Substring(5).Substring(0, 2);

                            FileName = accountCode + "#" + accountID + 
                                "_#" + platformID + "_" + 
                                "BD#" + BDate + "_ED#" + EDate + "_" + reports[reportNo] + ".json";


                            reportUrl = sushiURl + "/reports/" + reports[reportNo] + "?" +
                            "customer_id=" + customerID +
                            "&requestor_id=" + requestorID +
                            "&platform=" + platform +
                            "&api_key=" + apiKey +
                            "&begin_date=" + BDate +
                            "&end_date=" + EDate +
                            "";
                            // Download Data
                            options = "application/json";
                            var data = await HTTP.Get(reportUrl, options);
                            if (data == "Error")
                            {
                                errorCount = errorCount + 1;
                                errorFiles.Text = errorCount.ToString();
                                records_error = 1;
                                status = "Get Error";
                                state = -3;
                                records_affected = 0;
                                records_processed = 0;
                                processing_log = "Error in Get Data from HTTP Class";
                            }
                            else
                            {
                                status = "OK";
                                state = 1;
                                records_error = 0;
                                // Save Data into File
                                string isOverrite = utility.SaveDataToFile(FilePath, FileName, data);

                                string path = System.IO.Path.Combine(FilePath, FileName);
                                FileSize = new System.IO.FileInfo(path).Length;
                                FileSizeInKB = FileSize / 1024;
                                FileSizeInKB = FileSizeInKB + 1;
                                
                                if (isOverrite == "Overrited")
                                {
                                    overriteCount = overriteCount + 1;
                                    overrite.Text = overriteCount.ToString();
                                }
                                // Progression
                                AccountProgress.Text = accountCode;
                                PlatformProgress.Text = platform;
                                ReportProgress.Text = reports[reportNo];
                                FileNameProgress.Text = FileName;
                                successCount = successCount + 1;
                                successesFiles.Text = successCount.ToString();
                                records_affected = 1;
                                processing_log = "Successfully Saved Files into HDD";
                            }
                            processedCount = errorCount + successCount;
                            ProcessedFiles.Text = processedCount.ToString();
                            records_processed = 1;
                            end_DT = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                            var LetEndDate = DateTime.Now;
                            TimeSpan ts = LetEndDate.Subtract(LetStartDate);
                            records_time = ts.Seconds;

                            FilePath = FilePath.Replace("\\", "\\\\");
                            query = "INSERT INTO" +
                                                "`harvesting_processing`" +
                                                "(" +
                                                    "`accountt_id`," +
                                                    "`platform_id`," +
                                                    "`report_id`," +
                                                    "`account`," +
                                                    "`platform`," +
                                                    "`begin_date`," +
                                                    "`end_date`," +
                                                    "`folder_path`," +
                                                    "`file_name`," +
                                                    "`sushi_url`," +
                                                    "`status`," +
                                                    "`start_DT`," +
                                                    "`end_DT`," +
                                                    "`state`," +
                                                    "`processing_log`," +
                                                    "`database`," +
                                                    "`table`," +
                                                    "`records_processed`," +
                                                    "`records_affected`," +
                                                    "`records_error`," +
                                                    "`records_total`," +
                                                    "`records_time`," +
                                                    "`file_size_kb`" +
                                                    ")" +
                                          "VALUES" +
                                                    " (" +
                                                    " '" + accountID + "'," +
                                                    " '" + platformID + "'," +
                                                    " '" + reports[reportNo] + "'," +
                                                    " '" + accountCode + "'," +
                                                    " '" + platform + "'," +
                                                    " '" + BDate + "'," +
                                                    " '" + EDate + "'," +
                                                    " '" + FilePath + "'," +
                                                    " '" + FileName + "'," +
                                                    " '" + sushiURl + "'," +
                                                    " '" + status + "'," +
                                                    " '" + start_DT + "'," +
                                                    " '" + end_DT + "'," +
                                                    " '" + state + "'," +
                                                    " '" + processing_log + "'," +
                                                    " '" + database + "'," +
                                                    " '" + table + "'," +
                                                    " '" + records_processed + "'," +
                                                    " '" + records_affected + "'," +
                                                    " '" + records_error + "'," +
                                                    " '" + records_total + "'," +
                                                    " '" + records_time + "'," +
                                                    " '" + FileSizeInKB + "'" +
                                                    " );";
                            MySql.ExecuteInsertUpdateQuery(query);
                        }
                    }
                }
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("download_Click: " + ex.Message);
            }
            finally
            {
                MessageBox.Show(
                    "Processed Files:  " + processedCount + "\n" +
                    "Success Files:    " + successCount + "\n" +
                    "Error Files:      " + errorCount + "\n" +
                    "Overrited Files:  " + overriteCount);

                processedCount = 0;
                successCount = 0;
                errorCount = 0;
                overriteCount = 0;
                AccountProgress.Text = "?";
                PlatformProgress.Text = "?";
                ReportProgress.Text = "?";
                FileNameProgress.Text = "?";
                ProcessedFiles.Text = processedCount.ToString();
                successesFiles.Text = successCount.ToString();
                errorFiles.Text = errorCount.ToString();
                overrite.Text = overriteCount.ToString();
            }
        }
        public void wc_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }
        private void dgvAccountPlatform_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = new DataTable();
            string accountID = "";
            string platform = "";
            string customerID = "";
            string apiKey = "";
            string sushiURl = "";
            string requestorID = "";
            string BDate = "";
            string EDate = "";
            string reportUrl = "";
            try
            {
                // First And Last Day Date of the Selected Date
                DateTime bd = beginDate.Value;
                var bdDay = new DateTime(bd.Year, bd.Month, 1);
                DateTime ed = endDate.Value;
                var edDay = new DateTime(ed.Year, ed.Month, 1);
                var lastDayOfMonth = edDay.AddMonths(1).AddDays(-1);
                BDate = bdDay.ToString("dd-MM-yyyy");
                EDate = lastDayOfMonth.ToString("dd-MM-yyyy");
                // MessageBox.Show("BeginDate: " + BDate + "\n\n" + "EndDate: " + EDate);
                dt.Columns.Add("PLlatform");
                dt.Columns.Add("Customer ID");
                dt.Columns.Add("Reports");
                dt.Columns.Add("URL");
                if (dgvAccountPlatform.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvAccountPlatform.CurrentRow.Selected = true;
                    accountID = dgvAccountPlatform.Rows[e.RowIndex].Cells["AccountID"].FormattedValue.ToString();
                    platform = dgvAccountPlatform.Rows[e.RowIndex].Cells["PLatform"].FormattedValue.ToString();
                    customerID = dgvAccountPlatform.Rows[e.RowIndex].Cells["Customer ID"].FormattedValue.ToString();
                    apiKey = dgvAccountPlatform.Rows[e.RowIndex].Cells["api_key"].FormattedValue.ToString();
                    sushiURl = dgvAccountPlatform.Rows[e.RowIndex].Cells["sushi_url"].FormattedValue.ToString();
                    requestorID = dgvAccountPlatform.Rows[e.RowIndex].Cells["requestor_id"].FormattedValue.ToString();
                    // fill reports aaray
                    String[] reports = { "tr", "dr", "ir", "pr" };
                    for (int reportNo = 0; reportNo < reports.Length; reportNo++)
                    {
                        reportUrl = sushiURl + "reports/" + reports[reportNo] + "?" +
                        "customer_id=" + customerID +
                        "&requestor_id=" + requestorID +
                        "&platform=" + platform +
                        "&api_key=" + apiKey +
                        "&begin_date=" + BDate +
                        "&end_date=" + EDate +
                        "";
                        dt.Rows.Add(platform, customerID, reports[reportNo], reportUrl);
                    }
                    dgv2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("dgvAccountPlatform_CellClick: " + ex.Message);
            }
        }
        private void choose_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Choose Path For Saving Reports";
                if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MessageBox.Show(fbd.SelectedPath);
                    DataFolderPath.Text = fbd.SelectedPath;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show("choose_CLick: " + except.Message);
            }
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 50 milliseconds.  
                Thread.Sleep(50);
                // Report progress.  
                BackgroundWorker.ReportProgress(i);
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar
            progress.Value = e.ProgressPercentage;
            // Set the text.
            this.Text = "Progress: " + e.ProgressPercentage.ToString() + "%";
        }
    }
}