using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Task2._1
{
    public partial class MainMenu : Form
    {
        public static string AccessLevel = "Модератор"; // строка, хранящая наименование уровня доступа, значение по умолчанию
        public static string BasesDirectory = "..\\Debug\\Contract Bases\\"; // директория хранения баз контрактов
        public static string NewBaseName = null; // имя новой создаваемой базы
        public static string BaseName; // имя отображаемой в dgv базы
        public static List<string> BasesNames; // список названий баз
        //public static string BaseNameBuffer = null; // буфер для удержания значения имени базы во время "сохранить как"

        public static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MainMenu()
        {
            InitializeComponent();
            ImportBasesNames(); // импорт имен баз в коллекцию BaseChangeBox
            ImportXML(); // импорт базы по умолчанию призапуске
        }

        // импорт имен баз в combobox
        private void ImportBasesNames()
        {
            string[] Bases = Directory.GetFiles(BasesDirectory);
            string buffer;
            BasesNames = new List<string>();

            foreach (string b in Bases)
            {
                buffer = b.Substring(b.LastIndexOf('\\') + 1);
                BasesNames.Add(buffer);
            }

            BaseChangeBox.DataSource = BasesNames;
        }

        // функция импорта из xml
        private void ImportXML()
        {
            try
            {
                // если первая загрузка базы после запуска
                if (BaseName == null)
                    BaseName = BaseChangeBox.Text;

                // загрузка файла в объект
                XDocument xDoc = XDocument.Load(BasesDirectory + BaseName);

                // обход всех узлов в корневом элементе
                foreach (XElement root in xDoc.Descendants("NewDataSet"))
                {
                    if (root.HasElements)
                    {
                        dataGridView.Rows.Clear();

                        // обходим все дочерние узлы элемента Contract
                        foreach (XElement elm in xDoc.Descendants("Contract"))
                        {
                            // чтение элементов
                            if (elm.Element("ID") != null && elm.Element("Client") != null && elm.Element("PhoneNumber") != null && elm.Element("Email") != null)
                                dataGridView.Rows.Add(elm.Element("ID").Value, elm.Element("Client").Value, elm.Element("PhoneNumber").Value, elm.Element("Email").Value);
                        }
                    }
                }
                xDoc = null;
            }
            catch
            {
                // очистка таблицы
                dataGridView.Rows.Clear();
            }
        }

        // проверка на корректность введенного символа (только цифры)
        private void IDBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // попытка автовставки текста
            if (e.KeyChar == 22)
            {
                e.Handled = true;

                MessageBox.Show("Вставка запрещена.", "Ошибка");
            }
            else if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 0 && e.KeyChar <= 31))
            {
                e.Handled = true;

                MessageBox.Show("Недопустимый символ.", "Ошибка");
            }
        }

        // проверка на корректность введенного символа (только цифры)
        private void PhoneNumberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 22)
            {
                e.Handled = true;

                MessageBox.Show("Вставка запрещена.", "Ошибка");
            }
            else if (PhoneNumberBox.TextLength == 11 && !(e.KeyChar >= 0 && e.KeyChar <= 31))
            {
                e.Handled = true;

                MessageBox.Show("Достигнута максимальная длина номера телефона.", "Ошибка");
            }
            else if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 0 && e.KeyChar <= 31))
            {
                e.Handled = true;

                MessageBox.Show("Недопустимый символ.", "Ошибка");
            }

            //    if (e.KeyChar == 8 && (PhoneNumberBox.TextLength == 2 || PhoneNumberBox.TextLength == 6 ||
            //           PhoneNumberBox.TextLength == 10 || PhoneNumberBox.TextLength == 13))
            //    {
            //        e.Handled = true; // перехватывание символа, вместо него выполняется следующая функция
            //        PhoneNumberBox.Text = PhoneNumberBox.Text.Remove(PhoneNumberBox.TextLength - 2, 2); // программное удаление, чтобы не сработал метод TextChanged
            //        PhoneNumberBox.SelectionStart = PhoneNumberBox.TextLength; // установка курсора в конец строки
            //    }
            //    else if (PhoneNumberBox.TextLength == 15 && e.KeyChar != 8)
            //    {
            //        e.Handled = true;

            //        MessageBox.Show("Достигнуто максимальное количество символов", "Ошибка");
            //    }
            //    else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < 0 || e.KeyChar > 31))
            //    {
            //        e.Handled = true;

            //        MessageBox.Show("Недопустимый символ.", "Ошибка");
            //    }
        }

        // проверка на корректность введенного символа (+ проверка формата)
        private void EmailBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //// объявление строки = введенный email
            //string email = EmailBox.ToString();
            //int startEmail = email.LastIndexOf(' ') + 1;
            //email = email.Substring(startEmail);

            //if (e.KeyChar == '.' && email.LastIndexOf('@') != -1 && email.LastIndexOf('.') != -1 && EmailBox.SelectionStart > email.LastIndexOf('@'))
            //{
            //    e.Handled = true;

            //    MessageBox.Show("Недопустимый символ.", "Ошибка");
            //}
            //else if (e.KeyChar == '@' && email.Contains('@'))
            //{
            //    e.Handled = true;

            //    MessageBox.Show("Недопустимый символ.", "Ошибка");
            //}
            //else if (e.KeyChar == 22 ||
            //    !(e.KeyChar >= 0 && e.KeyChar <= 31 || e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == '_' || e.KeyChar == '-' || e.KeyChar == '.' || e.KeyChar == '@'))
            //{
            //    e.Handled = true;

            //    MessageBox.Show("Недопустимый символ.", "Ошибка");
            //}
        }

        // клик по кнопке <добавить значения боксов в соответствующие столбцы datagridview>
        private void AddButton_Click(object sender, EventArgs e)
        {
            //// объявление строки = введенный email
            //string email = EmailBox.ToString();
            //int startEmail = email.LastIndexOf(' ') + 1;
            //email = email.Substring(startEmail);

            // переменные для установки, какое из полей неверного формата
            bool ID_isInvalid = false;
            bool Client_isInvalid = false;
            bool PhoneNumber_isInvalid = false;
            //bool Email_isInvalid = false;

            if (IDBox.Text == "")
                ID_isInvalid = true;
            if (ClientBox.Text == "")
                Client_isInvalid = true;
            if (PhoneNumberBox.Text == "" || PhoneNumberBox.TextLength < 11)
                PhoneNumber_isInvalid = true;
            //if (EmailBox.Text == "" || !email.Contains('@') || email.LastIndexOf('.') < email.LastIndexOf('@'))
            //    Email_isInvalid = true;

            if (ID_isInvalid)
            {
                MessageBox.Show("Недопустимое значение поля ID.", "Ошибка");
            }
            else if (Client_isInvalid)
            {
                MessageBox.Show("Недопустимое значение поля Client.", "Ошибка");
            }
            else if (PhoneNumber_isInvalid)
            {
                MessageBox.Show("Недопустимое значение поля Phone Number.", "Ошибка");
            }
            else if (!IsValidEmail(EmailBox.Text))
            {
                MessageBox.Show("Недопустимое значение поля Email.", "Ошибка");
            }
            else // перенос инфы в datagrid
            {
                int n = dataGridView.Rows.Add(); // взятие индекса каждой новой строки

                dataGridView.Rows[n].Cells[0].Value = IDBox.Text;
                dataGridView.Rows[n].Cells[1].Value = ClientBox.Text;
                dataGridView.Rows[n].Cells[2].Value = PhoneNumberBox.Text;
                dataGridView.Rows[n].Cells[3].Value = EmailBox.Text;

                // очистка боксов для последующего ввода
                IDBox.Clear();
                ClientBox.Clear();
                PhoneNumberBox.Clear();
                EmailBox.Clear();
            }
        }

        // клик по кнопке <очиститть строки datagridview>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
        }

        // клик по кнопке <сохранить данные в XML>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet(); // создаем пустой кэш данных
                DataTable dt = new DataTable(); // создаем пустую таблицу данных

                dt.TableName = "Contract";
                dt.Columns.Add("ID");
                dt.Columns.Add("Client");
                dt.Columns.Add("PhoneNumber");
                dt.Columns.Add("Email");

                ds.Tables.Add(dt);

                foreach (DataGridViewRow r in dataGridView.Rows) // пока в datagridview есть строки
                {
                    DataRow row = ds.Tables["Contract"].NewRow(); // создаем новую строку в таблице, занесенной в ds

                    // в столбец каждой из строк заносим значение соответствующего стоблца datagridview
                    row["ID"] = r.Cells[0].Value;
                    row["Client"] = r.Cells[1].Value;
                    row["PhoneNumber"] = r.Cells[2].Value;
                    row["Email"] = r.Cells[3].Value;

                    ds.Tables["Contract"].Rows.Add(row);
                }
                ds.WriteXml(BasesDirectory + BaseName);
                ds = null;
                dt = null;
                MessageBox.Show("XML файл успешно сохранен.", "Выполнено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        // клик по кнопке <войти в меню сотрудников>
        private void EmployeeMenuButton_Click(object sender, EventArgs e)
        {
            if (AccessLevel == "Администратор")
            {
                EmployeeMenuAdministrator f = new EmployeeMenuAdministrator();
                f.ShowDialog();
            }
            else if (AccessLevel == "Модератор")
            {
                EmployeeMenuModerator f = new EmployeeMenuModerator();
                f.ShowDialog();
            }
        }

        // клик по кнопке <сменить уровень доступа>
        private void AccessLevelChangeButton_Click(object sender, EventArgs e)
        {
            AccessLevelMenu f = new AccessLevelMenu();

            f.ShowDialog();
        }

        // смена базы -> импорт указанной в BaseChangeBox
        private void BaseChangeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BaseName = BaseChangeBox.Text;
            ImportXML();
        }

        // клик по кнопке <удаление указанной в BaseChangeBox базы> 
        private void DeleteBaseButton_Click(object sender, EventArgs e)
        {
            string BaseToDelete = BasesDirectory + BaseChangeBox.Text;

            File.Delete(BaseToDelete);

            ImportBasesNames();
            ImportXML();
        }

        // клик по кнопке <создать новую базу>
        private void CreateBaseButton_Click(object sender, EventArgs e)
        {
            // именование
            NewBaseNaming f = new NewBaseNaming();
            f.ShowDialog();

            // импорт имен баз
            ImportBasesNames();

            // вывод пустой новой базы в таблицу
            if (NewBaseName != null)
            {
                BaseName = NewBaseName;
            }

            BaseChangeBox.Text = BaseName;

            ImportXML();
        }

        // клик по кнопке <сохранить как>
        //private void SaveAsButton_Click(object sender, EventArgs e)
        //{
        //    // именование
        //    NewBaseNaming f = new NewBaseNaming();
        //    f.ShowDialog();

        //    // импорт имен баз
        //    ImportBasesNames();

        //    // вывод пустой новой базы в таблицу
        //    BaseName = NewBaseName;
        //    BaseChangeBox.SelectedText = BaseName;
        //    ImportXML();
        //}
    }
}
