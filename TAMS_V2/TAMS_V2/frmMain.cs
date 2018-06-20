using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using TAMS_V2.EF;
using TAMS_V2.Services.Services;
using TAMS_V2.AVLTree;
using System.Data.Entity.Validation;

namespace TAMS_V2
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        AVL tree = new AVL();
        public frmMain()
        {
            InitializeComponent();
            LoadGvCheckDoc();
            LoadGvDoc();
            BuildTree(tree);

        }

        private void btnAddDoc_ItemClick(object sender, ItemClickEventArgs e)
        {

            var checkDoc = OpenNewDoc(0);
            var sentencesAfter = ReadWord(checkDoc, 0);
            CheckSentence(tree, sentencesAfter, checkDoc, 0);

            LoadGvCheckDoc();
        }
        public CHECKING_DOCUMENT OpenNewDoc(int type)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Docx Files| *.doc| Doc Files| *.docx";
            file.Title = "Word File";
            file.RestoreDirectory = true;
            F_CHECKING_DOCUMENT fCheckDoc = new F_CHECKING_DOCUMENT();
            CHECKING_DOCUMENT checkDoc = new CHECKING_DOCUMENT();
            if (file.ShowDialog() == DialogResult.OK)
            {
                string FileName = file.FileName;
                checkDoc.Name = FileName;
                if (type == 0)
                {
                    fCheckDoc.Add(checkDoc);
                    fCheckDoc.Save();
                }
            }
            return checkDoc;
        }
        public List<SENTENCE> ReadWord(CHECKING_DOCUMENT checkDoc, int type)
        {
            List<SENTENCE> sentencesAfter = new List<SENTENCE>();
            string filePath = "";
            filePath = checkDoc.Name;
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = filePath;
            object readOnly = false;
            Microsoft.Office.Interop.Word.Document docs = new Microsoft.Office.Interop.Word.Document();
            try
            {
                docs = word.Documents.Open(ref path, ref miss,
                ref readOnly, ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss, ref miss);

                docs.ActiveWindow.Selection.WholeStory();
                docs.ActiveWindow.Selection.Copy();

                IDataObject data = Clipboard.GetDataObject();
                string TextContent = data.GetData(DataFormats.UnicodeText).ToString();

                //for (int i = 0; i < docs.Paragraphs.Count; i++)
                //{
                //    tmp += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
                //}

                char[] DauCau = new char[] { '!', '?', '.', '\t', '\n', '\r' };
                //string pattern = @"[\t \. \! \; \? \r \n] \s+" ;
                //string pattern = @"[\t \. \! \; \? \n] \s+";

                //string[] sentences = Regex.Split(TextContent, pattern);
                // MessageBox.Show("Đọc file Word xong");

                //Lưu file vừa mở vào CHECKING_DOCUMENT



                //Thêm content và Hash_Value
                string[] sentences = TextContent.Split(DauCau, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < sentences.Length; i++)
                {
                    SENTENCE nSentence = new SENTENCE();
                    nSentence.Sentence_Content = sentences[i].Trim();
                    sentencesAfter.Add(nSentence);

                }
                //MessageBox.Show("Tách câu xong, Bỏ khoảng trắng thừa xong");
                for (int i = 0; i < sentencesAfter.Count; i++)
                {
                    if (sentences[i].Length > 30)
                    {
                        string tmp1 = sentencesAfter[i].Sentence_Content;
                        sentencesAfter[i].Hash_Value = tmp1.GetHashCode();
                    }
                }
                // MessageBox.Show("Hash xong");
                return sentencesAfter;


                //Check SENTENCE và lưu RESULT trong CSDL
                //MessageBox.Show("Xong");
                // richTextBoxt = data.GetData(DataFormats.UnicodeText).ToString();               
            }
            catch (Exception ex)
            {
                docs.Close();
                word.Quit(false);
                word = null;
                GC.Collect();
                MessageBox.Show("Chương trình gặp lỗi: " + ex.ToString());
                return sentencesAfter;
            }
            finally
            {
                docs.Close();
                word.Quit(false);
                word = null;
                GC.Collect();
            }
        }

        //BuilTree
        public void BuildTree(AVL tree)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            F_SENTENCE fsentence = new F_SENTENCE();
            var list = fsentence.GetAll().OrderBy(x => x.Hash_Value.Value).ToList();
            List<long> sortArray = new List<long>();
            foreach (var item in list)
            {
                sortArray.Add(item.Hash_Value.Value);
            }
            tree.SortedArray(sortArray);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show("Thời gian build cây: " + elapsedMs.ToString() + "ms");
        }
        //Check SENTENCE trong CSDL và Thêm RESULT
        public float CheckSentence(AVL tree, List<SENTENCE> sentencesAfter, CHECKING_DOCUMENT checkDoc, int type)
        {
            float Similaryty = 0;
            try
            {
                if (type == 0)//kiểm tra CHECKING_DOCUMENT với tree
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    int Count = 0;
                    List<long> listIdDoc = new List<long>();
                    for (int i = 0; i < sentencesAfter.Count; i++)
                    {
                        //nếu câu trùng trong CSDL  thì thêm RESULT
                        if (sentencesAfter[i].Hash_Value != null && tree.Find(sentencesAfter[i].Hash_Value.Value) == true)
                        {
                            //Tìm SENTENCE trùng
                            F_SENTENCE fsentence = new F_SENTENCE();
                            long hash = sentencesAfter[i].Hash_Value.Value;
                            SENTENCE sentence = fsentence.GetSingleByCondition(x => x.Hash_Value == hash);
                            //Thêm RESULT
                            RESULT res = new RESULT();
                            res.Sentence_ID = sentence.Sentence_ID;
                            res.Checking_Document_ID = checkDoc.Document_ID;
                            res.Checking_Sentence_Position = i + 1;
                            F_SENTENCE_DOCUMENT fsd = new F_SENTENCE_DOCUMENT();
                            var listFsd = fsd.GetMany(x => x.Sentence_ID == sentence.Sentence_ID, null);
                            foreach (var item in listFsd)
                            {
                                res.Document_IDs = res.Document_IDs + item.Document_ID.ToString() + ",";//thêm DOCUMENT có câu trùng
                                var find = listIdDoc.Where(x => x == item.Document_ID);
                                if(find == null)
                                {
                                    listIdDoc.Add(item.Document_ID.Value);
                                }
                            }
                            F_RESULT fRes = new F_RESULT();
                            fRes.Add(res);
                            fRes.Save();
                            Count++;
                        }
                    }
                    float count = 0;
                    foreach (var item in sentencesAfter)
                    {
                        if (item.Hash_Value != null)
                        {
                            count++;
                        }
                    }
                    Similaryty = (float)Count / count * 100;
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    MessageBox.Show("Độ tương đồng là " + Similaryty.ToString() + "%\n" + "Thời gian xử lý: " + elapsedMs.ToString() + "ms");
                }
                if (type == 1)//xác nhận CHECKING_DOCUMENT vào DOCUMENT
                {
                    //tạo DOCUMENT mới
                    F_DOCUMENT fDoc = new F_DOCUMENT();
                    DOCUMENT doc = new DOCUMENT();
                    doc.Name = checkDoc.Name;
                    fDoc.Add(doc);
                    fDoc.Save();
                    // Xóa hết RESULT ở CHECKING_DOCUMENT cũ


                    for (int i = 0; i < sentencesAfter.Count; i++)
                    {
                        //nếu câu trùng với câu trong tree
                        if (sentencesAfter[i].Hash_Value != null && tree.Find(sentencesAfter[i].Hash_Value.Value) == true)
                        {
                            //Tìm SENTENCE trong CSDL
                            F_SENTENCE fSentence = new F_SENTENCE();
                            long hashCode = sentencesAfter[i].Hash_Value.Value;
                            var sentence = fSentence.GetSingleByCondition(x => x.Hash_Value == hashCode);
                            //Thêm SENTENCE_DOCUMENT
                            F_SENTENCE_DOCUMENT fSenDoc = new F_SENTENCE_DOCUMENT();
                            SENTENCE_DOCUMENT senDoc = new SENTENCE_DOCUMENT();
                            senDoc.Document_ID = doc.Document_ID;
                            senDoc.Sentence_ID = sentence.Sentence_ID;
                            senDoc.Position = i.ToString();
                            fSenDoc.Add(senDoc);
                            fSenDoc.Save();
                            //Xóa RESULT
                            F_RESULT fRes = new F_RESULT();
                            fRes.DeleteMulti(x => x.Checking_Document_ID == checkDoc.Document_ID && x.Sentence_ID == sentence.Sentence_ID);
                            fRes.Save();
                        }
                        //nếu câu không trùng với câu trong tree
                        if (sentencesAfter[i].Hash_Value != null && tree.Find(sentencesAfter[i].Hash_Value.Value) == false)
                        {
                            //Thêm SENTENCE mới
                            SENTENCE sentence = new SENTENCE();
                            sentence.Sentence_Content = sentencesAfter[i].Sentence_Content;
                            sentence.Hash_Value = sentencesAfter[i].Hash_Value;
                            F_SENTENCE fSentence = new F_SENTENCE();
                            fSentence.Add(sentence);
                            fSentence.Save();
                            //thêm node vào tree
                            tree.Add(sentence.Hash_Value.Value);
                            //Thêm SENT
                            F_SENTENCE_DOCUMENT fSenDoc = new F_SENTENCE_DOCUMENT();
                            SENTENCE_DOCUMENT senDoc = new SENTENCE_DOCUMENT();
                            senDoc.Document_ID = doc.Document_ID;
                            senDoc.Sentence_ID = sentence.Sentence_ID;
                            senDoc.Position = i.ToString();
                            fSenDoc.Add(senDoc);
                            fSenDoc.Save();
                        }
                    }
                    //Xóa CHECKING_DOCUMENT  
                    F_CHECKING_DOCUMENT fCheckDoc = new F_CHECKING_DOCUMENT();
                    CHECKING_DOCUMENT deleteCheckDoc = new CHECKING_DOCUMENT();
                    deleteCheckDoc = fCheckDoc.GetSingleById(checkDoc.Document_ID);
                    fCheckDoc.Delete(deleteCheckDoc);
                    fCheckDoc.Save();
                }
                return Similaryty;
            }
            catch (DbEntityValidationException dbEx)
            {
                string ex = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ex = ex + "Property: " + validationError.PropertyName + "Error: " + validationError.ErrorMessage + "\n";

                    }
                }
                MessageBox.Show(ex);
                return 0;
            }
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            long IDCheckDoc = Convert.ToInt64(gvCheckDoc.GetFocusedRowCellValue(ID).ToString());
            F_CHECKING_DOCUMENT fCheckDoc = new F_CHECKING_DOCUMENT();
            var checkDoc = fCheckDoc.GetSingleById(IDCheckDoc);
            var sentencesAfter = ReadWord(checkDoc, 1);
            CheckSentence(tree, sentencesAfter, checkDoc, 1);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            LoadGvCheckDoc();
            LoadGvDoc();
            MessageBox.Show("Xong trong " + elapsedMs.ToString() + "ms");
        }
        public void LoadGvCheckDoc()
        {
            F_CHECKING_DOCUMENT fCheckDoc = new F_CHECKING_DOCUMENT();
            var list = fCheckDoc.GetAll().ToList();
            gcCheckDoc.DataSource = list;
        }

        public void LoadGvDoc()
        {
            F_DOCUMENT fDoc = new F_DOCUMENT();
            var list = fDoc.GetAll().ToList();
            gcDoc.DataSource = list;
        }
    }
}