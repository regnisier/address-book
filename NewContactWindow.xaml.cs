using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AddressBook
{
    public partial class newContactWindow : Window
    {
        string fileName = "";

        public newContactWindow(string fileName_)
        {
            InitializeComponent();

            fileName = fileName_;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if ((newFirstNameEntry.Text.Length == 0) && (newSecondNameEntry.Text.Length == 0))
            {
                MessageBox.Show("Please specify at least a first name or a second name before saving.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((newTelephoneNumberEntry.Text.Length == 0) && (newEMailAddressEntry.Text.Length == 0))
            {
                MessageBox.Show("A corresponding telephone number or e-mail address is needed in order " +
                    "to create a meaningful entry.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ContactInformation newContactInformation = new ContactInformation(
                newFirstNameEntry.Text,
                newSecondNameEntry.Text,
                newTelephoneNumberEntry.Text,
                newEMailAddressEntry.Text
                );

            try
            {
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    file.WriteLine(newContactInformation.ToString());

                    MessageBox.Show("The new entry was saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.None);

                    newFirstNameEntry.Text = "";
                    newSecondNameEntry.Text = "";
                    newEMailAddressEntry.Text = "";
                    newTelephoneNumberEntry.Text = "";
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("File not found!", "Error!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Something went wrong!", "Error!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
