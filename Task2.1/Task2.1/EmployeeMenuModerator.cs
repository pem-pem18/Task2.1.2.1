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

namespace Task2._1
{
    public partial class EmployeeMenuModerator : Form
    {
        public EmployeeMenuModerator()
        {
            InitializeComponent();
            ImportNames();
        }

        // импорт имен из базы сотрудников в боксы
        private void ImportNames()
        {
            {
                List<string> ls = new List<string>();

                XDocument doc = XDocument.Load("EmployeeBase.xml");

                foreach (XElement employee in doc.Descendants("NewDataSet"))
                {
                    foreach (XElement name in doc.Descendants("Employee"))
                    {
                        ls.Add((string)name.Attribute("Name"));
                    }
                }

                NameBox.DataSource = ls;
            }
        }

        // смена имени пользователя -> смена значений боксов
        private void NameBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            XDocument doc = XDocument.Load("EmployeeBase.xml");

            foreach (XElement employee in doc.Descendants("NewDataSet"))
            {
                foreach (XElement item in doc.Descendants("Employee"))
                {
                    if (NameBox.Text == (string)item.Attribute("Name"))
                    {
                        //Email = (string)item.Element("Email");
                        //Login = (string)item.Element("Login");
                        //Password = (string)item.Element("Password");
                        //Position = (string)item.Element("Position");

                        EmailBox.Text = (string)item.Element("Email");
                        LoginBox.Text = (string)item.Element("Login");
                        PasswordBox.Text = (string)item.Element("Password");
                        PositionBox.Text = (string)item.Element("Position");
                    }
                }
            }
        }

        // клик по кнопке <сохранить учетные данные пользователя>
        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            if (MainMenu.IsValidEmail(EmailBox.Text))
            {
                try
                {
                    // удаление устаревшего профиля для перезаписи
                    XDocument doc = XDocument.Load("EmployeeBase.xml");
                    var q = from node in doc.Descendants("Employee")
                            let attr = node.Attribute("Name")
                            where attr != null && attr.Value == NameBox.Text
                            select node;
                    q.ToList().ForEach(x => x.Remove());
                    doc.Save("EmployeeBase.xml");
                    // конец

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("EmployeeBase.xml");

                    XmlElement xRoot = xDoc.DocumentElement;
                    XmlElement EmployeeElem = xDoc.CreateElement("Employee");
                    XmlAttribute NameAttr = xDoc.CreateAttribute("Name");
                    XmlElement EmailElem = xDoc.CreateElement("Email");
                    XmlElement LoginELem = xDoc.CreateElement("Login");
                    XmlElement PasswordElem = xDoc.CreateElement("Password");
                    XmlElement PositionElem = xDoc.CreateElement("Position");

                    XmlText Name = xDoc.CreateTextNode(NameBox.Text);
                    XmlText Email = xDoc.CreateTextNode(EmailBox.Text);
                    XmlText Login = xDoc.CreateTextNode(LoginBox.Text);
                    XmlText Password = xDoc.CreateTextNode(PasswordBox.Text);
                    XmlText Position = xDoc.CreateTextNode(PositionBox.Text);

                    NameAttr.AppendChild(Name);
                    EmailElem.AppendChild(Email);
                    LoginELem.AppendChild(Login);
                    PasswordElem.AppendChild(Password);
                    PositionElem.AppendChild(Position);

                    EmployeeElem.Attributes.Append(NameAttr);
                    EmployeeElem.AppendChild(EmailElem);
                    EmployeeElem.AppendChild(LoginELem);
                    EmployeeElem.AppendChild(PasswordElem);
                    EmployeeElem.AppendChild(PositionElem);

                    xRoot.AppendChild(EmployeeElem);
                    xDoc.Save("EmployeeBase.xml");

                    MessageBox.Show("Сохранение выполнено успешно.", "Выполнено");
                }
                catch
                {
                    MessageBox.Show("Ошибка сохранения.", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Неверный формат электронной почты.", "Ошибка");
            }
        }
    }

    
}
